using BeautyShop.Services;
using BeautyShop.Models;

namespace BeautyShop.Pages;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _db;

    public LoginPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        bool isValid = await _db.ValidateUser(username, password);

        if (isValid)
        {
            var user = await _db.GetUserAsync(username);

            // ⏺ Сохраняем роль и имя в Preferences
            Preferences.Set("user_role", user.Role);      // "admin" или "user"
            Preferences.Set("user_name", user.Username);  // для отзывов и истории

            // Навигация по роли
            if (user.Role == "admin")
                await Shell.Current.GoToAsync("//AdminHomePage");
            else
                await Shell.Current.GoToAsync("//UserHomePage");
        }
        else
        {
            await DisplayAlert("Ошибка", "Неверный логин или пароль", "ОК");
        }
    }


    private async void OnRegisterRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }
}
