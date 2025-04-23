namespace BeautyShop.Pages;

public partial class UserHomePage : ContentPage
{
    public UserHomePage()
    {
        InitializeComponent();
    }

    private async void OnViewServicesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ServicesPage");
    }

    private async void OnFavoritesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("FavoritesPage");
    }
}
