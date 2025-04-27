using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class AdminOrdersPage : ContentPage
{
    private readonly DatabaseService _db;

    public AdminOrdersPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var orders = await _db.GetOrderHistoryAsync();
  
        OrderCollectionView.ItemsSource = orders;
    }
}
