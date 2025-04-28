using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class ServicesPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly CartService _cart;
    private List<ServiceWithRating> _allServices;

    public ServicesPage(DatabaseService db, CartService cart)
    {
        InitializeComponent();
        _db = db;
        _cart = cart;

        LoadServices();
    }

    private async void LoadServices()
    {
        var rawServices = await _db.GetServicesAsync();
        _allServices = new List<ServiceWithRating>();

        foreach (var service in rawServices)
        {
            var rating = await _db.GetAverageRatingAsync(service.Id);
            bool isFavorite = await _db.IsFavoriteAsync(service.Id);
            bool isInCart = _cart.GetCart().Any(x => x.ServiceId == service.Id);

            _allServices.Add(new ServiceWithRating
            {
                Service = service,
                Rating = rating,
                IsFavorite = isFavorite,
                IsInCart = isInCart
            });
        }

        ServicesList.ItemsSource = _allServices;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = e.NewTextValue?.ToLower() ?? "";
        ServicesList.ItemsSource = _allServices
            .Where(s => s.Service.Title.ToLower().Contains(keyword))
            .ToList();
    }

    private async void OnFavoriteClicked(object sender, EventArgs e)
    {
        if ((sender as ImageButton)?.BindingContext is ServiceWithRating item)
        {
            if (item.IsFavorite)
            {
                await _db.RemoveFromFavoritesAsync(item.Service.Id);
                item.IsFavorite = false;
            }
            else
            {
                await _db.AddToFavoritesAsync(new FavoriteService
                {
                    ServiceId = item.Service.Id,
                    Title = item.Service.Title,
                    Price = item.Service.Price,
                    ImagePath = item.Service.ImagePath
                });
                item.IsFavorite = true;
            }

            ServicesList.ItemsSource = null;
            ServicesList.ItemsSource = _allServices;
        }
    }

    private void OnCartClicked(object sender, EventArgs e)
    {
        if ((sender as ImageButton)?.BindingContext is ServiceWithRating item)
        {
            if (item.IsInCart)
            {
                _cart.RemoveFromCart(item.Service.Id);
                item.IsInCart = false;
            }
            else
            {
                _cart.AddToCart(new AppointmentCartItem
                {
                    ServiceId = item.Service.Id,
                    Title = item.Service.Title,
                    Price = item.Service.Price
                });
                item.IsInCart = true;
            }

            ServicesList.ItemsSource = null;
            ServicesList.ItemsSource = _allServices;
        }
    }

    private async void OnLeaveReviewClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is ServiceWithRating item)
        {
            await Shell.Current.GoToAsync($"AddReviewPage?serviceId={item.Service.Id}");
        }
    }

    private async void OnViewReviewsClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is ServiceWithRating item)
        {
            await Shell.Current.GoToAsync($"ReviewsPage?serviceId={item.Service.Id}");
        }
    }
}
