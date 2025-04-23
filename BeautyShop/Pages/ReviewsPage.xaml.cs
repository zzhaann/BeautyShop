using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

[QueryProperty(nameof(ServiceId), "serviceId")]
public partial class ReviewsPage : ContentPage
{
    private readonly DatabaseService _db;

    public int ServiceId { get; set; }

    public ReviewsPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var service = await _db.GetServiceByIdAsync(ServiceId);
        if (service != null)
        {
            TitleLabel.Text = service.Title;
            DescriptionLabel.Text = service.Description;
        }

        await RefreshReviews();
    }

    private async Task RefreshReviews()
    {
        var reviews = await _db.GetReviewsByServiceIdAsync(ServiceId);
        var reviewModels = reviews.Select(r => new ReviewViewModel
        {
            Id = r.Id,
            Username = r.Username,
            Rating = r.Rating,
            Comment = r.Comment,
            Date = r.Date
        }).ToList();

        ReviewList.ItemsSource = reviewModels;

        if (reviews.Count == 0)
            await DisplayAlert("Нет отзывов", "Для этой услуги пока нет отзывов.", "ОК");
    }

    private async void OnDeleteReviewClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is Review review)
        {
            bool confirm = await DisplayAlert("Удалить отзыв?",
                $"Удалить отзыв пользователя: {review.Username}?", "Да", "Нет");

            if (confirm)
            {
                await _db.DeleteReviewAsync(review.Id);
                await DisplayAlert("Готово", "Отзыв удалён.", "ОК");
                await RefreshReviews();
            }
        }
    }
}
