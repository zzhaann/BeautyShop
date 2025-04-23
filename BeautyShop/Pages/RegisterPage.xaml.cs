using BeautyShop.Models;
using BeautyShop.Services;
using Microsoft.Maui.Storage;

namespace BeautyShop.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly DatabaseService _db;

    public RegisterPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;
        string role = RolePicker.SelectedItem?.ToString();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
        {
            await DisplayAlert("Ошибка", "Заполните все поля", "ОК");
            return;
        }

        var existing = await _db.GetUserAsync(username);
        if (existing != null)
        {
            await DisplayAlert("Ошибка", "Пользователь уже существует", "ОК");
            return;
        }

        var newUser = new User { Username = username, Password = password, Role = role };
        await _db.AddUserAsync(newUser);

        await DisplayAlert("Успешно", "Пользователь зарегистрирован", "ОК");
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void OnLoginRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
