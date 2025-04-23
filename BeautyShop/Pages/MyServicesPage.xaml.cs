using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class MyServicesPage : ContentPage
{
    private readonly DatabaseService _db;

    public MyServicesPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        string currentAdmin = Preferences.Get("user_name", "admin");
        var myServices = await _db.GetServicesByCreatorAsync(currentAdmin);

        MyServicesList.ItemsSource = myServices;

        if (myServices.Count == 0)
            await DisplayAlert("Пусто", "Вы пока не добавили ни одной услуги.", "ОК");
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is Service service)
        {
            await Shell.Current.GoToAsync($"EditServicePage?serviceId={service.Id}");
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is Service service)
        {
            bool confirm = await DisplayAlert("Удалить?", $"Удалить услугу: {service.Title}?", "Да", "Нет");
            if (confirm)
            {
                await _db.DeleteServiceAsync(service.Id);
                await DisplayAlert("Готово", "Услуга удалена", "ОК");
                await RefreshMyServices();
            }
        }
    }

    private async Task RefreshMyServices()
    {
        string currentAdmin = Preferences.Get("user_name", "admin");
        var myServices = await _db.GetServicesByCreatorAsync(currentAdmin);
        MyServicesList.ItemsSource = myServices;
    }

}
