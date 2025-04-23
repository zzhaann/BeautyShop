using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class AdminHomePage : ContentPage
{
    private readonly DatabaseService _db;

    public AdminHomePage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;

        LoadServices();
    }

    private async void LoadServices()
    {
        var services = await _db.GetServicesAsync();
        //AdminServicesList.ItemsSource = services;
    }

    private async void OnAddServiceClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddServicePage");
    }

    private async void OnAdminViewReviewsClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is Service service)
        {
            await Shell.Current.GoToAsync($"ReviewsPage?serviceId={service.Id}");
        }
    }

    private async void OnMyServicesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("MyServicesPage");
    }


    private async void OnGoToAllReviewsPageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AllReviewsPage");
    }

}
