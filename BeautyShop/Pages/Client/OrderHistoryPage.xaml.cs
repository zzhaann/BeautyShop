using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class OrderHistoryPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly string _userRole;
    private readonly string _username;

    public OrderHistoryPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
        _userRole = Preferences.Get("user_role", "user");
        _username = Preferences.Get("user_name", "");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadOrderHistory();
    }

    private async Task LoadOrderHistory()
    {
        List<OrderHistory> orders;

        if (_userRole == "admin")
            orders = await _db.GetOrderHistoryAsync(); // Админ видит всё
        else
            orders = await _db.GetOrderHistoryByUserAsync(_username); // Клиент видит только своё

        OrderHistoryCollectionView.ItemsSource = orders;
    }

    private async void OnOpenOrderClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is OrderHistory order)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, order.FilePath);

            if (File.Exists(filePath))
            {
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
            else
            {
                await DisplayAlert("Ошибка", "Файл не найден.", "ОК");
            }
        }
    }
}
