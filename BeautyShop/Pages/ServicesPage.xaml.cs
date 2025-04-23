using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class ServicesPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly CartService _cart;
    private readonly string _userRole;

    public ServicesPage(DatabaseService db, CartService cart)
    {
        InitializeComponent();
        _db = db;
        _cart = cart;

        _userRole = Preferences.Get("user_role", "user");
        AddServiceBtn.IsVisible = _userRole == "admin";

        LoadServices();
    }

    private async void LoadServices()
    {
        var rawServices = await _db.GetServicesAsync();
        var servicesWithRatings = new List<ServiceWithRating>();

        foreach (var s in rawServices)
        {
            var rating = await _db.GetAverageRatingAsync(s.Id);
            servicesWithRatings.Add(new ServiceWithRating
            {
                Service = s,
                Rating = rating
            });
        }

        ServicesList.ItemsSource = servicesWithRatings;
    }

    private async void OnAddServiceClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddServicePage");
    }

    private async void OnServiceTapped(object sender, EventArgs e)
    {
        if (_userRole != "user")
            return;

        if ((sender as VisualElement)?.BindingContext is ServiceWithRating item)
        {
            var service = item.Service;

            _cart.AddToCart(new AppointmentCartItem
            {
                ServiceId = service.Id,
                Title = service.Title,
                Price = service.Price
            });

            await DisplayAlert("Добавлено", $"Услуга \"{service.Title}\" добавлена в корзину.", "ОК");
        }
    }

    private async void OnAddToFavoritesClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is ServiceWithRating item)
        {
            var service = item.Service;

            var favorite = new FavoriteService
            {
                ServiceId = service.Id,
                Title = service.Title,
                Price = service.Price,
                ImagePath = service.ImagePath
            };

            await _db.AddToFavoritesAsync(favorite);
            await DisplayAlert("Избранное", "Добавлено в избранное!", "ОК");
        }
    }

    private async void OnLeaveReviewClicked(object sender, EventArgs e)
    {
        if (_userRole != "user")
            return;

        if ((sender as Button)?.BindingContext is ServiceWithRating item)
        {
            var service = item.Service;
            await Shell.Current.GoToAsync($"AddReviewPage?serviceId={service.Id}");
        }
    }




    private async void OnViewReviewsClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is ServiceWithRating item)
        {
            var service = item.Service;
            await Shell.Current.GoToAsync($"ReviewsPage?serviceId={service.Id}");
        }
    }


}
