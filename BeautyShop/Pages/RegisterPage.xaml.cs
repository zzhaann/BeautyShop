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
            await DisplayAlert("������", "��������� ��� ����", "��");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("������", "������ �� ���������", "��");
            return;
        }

        if (password.Length < 6 || !password.Any(char.IsDigit) || !password.Any(char.IsLetter))
        {
            await DisplayAlert("������ ������", "������ ������ ��������� ������� 6 ��������, ������� ����� � �����", "��");
            return;
        }

        var existing = await _db.GetUserAsync(username);
        if (existing != null)
        {
            await DisplayAlert("������", "������������ � ����� ������� ��� ����������", "��");
            return;
        }

        var user = new User
        {
            Username = username,
            Password = password,
            Role = "user"
        };

        await _db.AddUserAsync(user);
        await DisplayAlert("�������", "����������� ������ �������!", "��");
        await Shell.Current.GoToAsync("//LoginPage");
    }




    private async void OnLoginRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
