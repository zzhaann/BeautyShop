using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

[QueryProperty(nameof(ServiceId), "serviceId")]
public partial class AddReviewPage : ContentPage
{
    private readonly DatabaseService _db;

    public int ServiceId { get; set; }

    public AddReviewPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(RatingEntry.Text, out int rating) || rating < 1 || rating > 5)
        {
            await DisplayAlert("������", "������ ������ ���� �� 1 �� 5", "��");
            return;
        }

        var review = new Review
        {
            ServiceId = ServiceId,
            Rating = rating,
            Comment = CommentEditor.Text,
            Username = Preferences.Get("user_name", "������"), // ���� ���� ���������� �����
            Date = DateTime.Now
        };

        await _db.AddReviewAsync(review);
        await DisplayAlert("������", "������� �� �����!", "��");

        await Shell.Current.GoToAsync("//UserHomePage");
    }
}

