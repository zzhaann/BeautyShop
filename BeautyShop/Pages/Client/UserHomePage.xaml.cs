using System.Threading; 

namespace BeautyShop.Pages;

public partial class UserHomePage : ContentPage
{
    private CancellationTokenSource _carouselCts;

    public UserHomePage()
    {
        InitializeComponent();
        StartCarouselAutoScroll();
    }

    private async void StartCarouselAutoScroll()
    {
        _carouselCts = new CancellationTokenSource();
        var token = _carouselCts.Token;

        while (!token.IsCancellationRequested)
        {
            await Task.Delay(3000);

            if (PromoCarousel.ItemsSource is System.Collections.IList items && items.Count > 0)
            {
                PromoCarousel.Position = (PromoCarousel.Position + 1) % items.Count;
            }
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _carouselCts?.Cancel();
    }

    private async void OnViewServicesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ServicesPage");
    }

    private async void OnFavoritesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("FavoritesPage");
    }

    private async void OnCardClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CartPage");
    }

    private async void OnHistoryClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("OrderHistoryPage");
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Remove("user_name");
        Preferences.Remove("user_role");

        await Shell.Current.GoToAsync("//WelcomePage");
    }
}
