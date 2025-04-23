using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class OrderHistoryPage : ContentPage
{
    private readonly DatabaseService _db;

    public OrderHistoryPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;

        LoadHistory();
    }

    private async void LoadHistory()
    {
        var history = await _db.GetOrderHistoryAsync();
        HistoryList.ItemsSource = history;
    }

    private async void OnOpenPdfClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is OrderHistory item)
        {
            try
            {
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(item.FilePath)
                });
            }
            catch
            {
                await DisplayAlert("Ошибка", "Не удалось открыть файл.", "ОК");
            }
        }
    }
}
