using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class FavoritesPage : ContentPage
{
    private readonly DatabaseService _db;

    public FavoritesPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadFavorites();
    }

    private async void LoadFavorites()
    {
        var favorites = await _db.GetFavoritesAsync();
        FavoritesCollectionView.ItemsSource = favorites;

        if (favorites.Count == 0)
            await DisplayAlert("Пусто", "Пока ничего в избранном", "ОК");
    }

    private async void OnRemoveFromFavoritesClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is Service service)
        {
            await _db.RemoveFromFavoritesAsync(service.Id);
            await DisplayAlert("Успешно", "Услуга удалена из избранного.", "ОК");
            LoadFavorites(); // Обновить список после удаления
        }
    }
}
