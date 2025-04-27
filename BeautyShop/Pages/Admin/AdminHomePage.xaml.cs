using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class AdminHomePage : ContentPage
{
    private readonly DatabaseService _db;
    private CancellationTokenSource _carouselCts;

    public AdminHomePage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;

        LoadServices();
        StartCarouselAutoScroll();
    }

    private async void LoadServices()
    {
        var services = await _db.GetServicesAsync();
        
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

    

    private async void OnClientsTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ClientsPage");
    }

    private async void OnAppointmentsTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AdminOrdersPage");
    }

    private async void OnServicesTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("MyServicesPage");
    }

    private async void OnReviewsTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AllReviewsPage");
    }

    private async void OnAddServiceTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddServicePage");
    }
}
