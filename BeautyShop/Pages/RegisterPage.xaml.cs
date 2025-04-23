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
        string confirmPassword = ConfirmPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Ошибка", "Заполните все поля", "ОК");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Ошибка", "Пароли не совпадают", "ОК");
            return;
        }

        if (password.Length < 6 || !password.Any(char.IsDigit) || !password.Any(char.IsLetter))
        {
            await DisplayAlert("Слабый пароль", "Пароль должен содержать минимум 6 символов, включая буквы и цифры", "ОК");
            return;
        }

        var existing = await _db.GetUserAsync(username);
        if (existing != null)
        {
            await DisplayAlert("Ошибка", "Пользователь с таким логином уже существует", "ОК");
            return;
        }

        var user = new User
        {
            Username = username,
            Password = password,
            Role = "user"
        };

        await _db.AddUserAsync(user);
        await DisplayAlert("Успешно", "Регистрация прошла успешно!", "ОК");
        await Shell.Current.GoToAsync("//LoginPage");
    }




    private async void OnLoginRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
