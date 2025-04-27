using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class ClientsPage : ContentPage
{
    private readonly DatabaseService _db;

    public ClientsPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadClients();
    }

    private async Task LoadClients()
    {
        var clients = await _db.GetAllClientsAsync();
        ClientsCollectionView.ItemsSource = clients;
    }

    private async void OnDeleteClientClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is User user)
        {
            bool confirm = await DisplayAlert("Удаление", $"Удалить {user.Username}?", "Да", "Нет");
            if (confirm)
            {
                await _db.DeleteUserAsync(user.Id);
                await LoadClients();
            }
        }
    }
}
