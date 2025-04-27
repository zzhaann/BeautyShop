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
