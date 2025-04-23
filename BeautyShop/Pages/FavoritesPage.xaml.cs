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
        LoadFavorites();
    }

    private async void LoadFavorites()
    {
        var favorites = await _db.GetFavoritesAsync();
        FavoritesList.ItemsSource = favorites;
        if (favorites.Count == 0)
            await DisplayAlert("Пусто", "Пока ничего в избранном", "ОК");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadFavorites();
    }



}
