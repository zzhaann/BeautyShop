using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class AllReviewsPage : ContentPage
{
    private readonly DatabaseService _db;
    private List<ReviewWithService> _allReviews;

    public AllReviewsPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _allReviews = await _db.GetAllReviewsWithServiceAsync();
        AllReviewsList.ItemsSource = _allReviews;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var filtered = _allReviews
            .Where(r => r.ServiceTitle.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase))
            .ToList();

        AllReviewsList.ItemsSource = filtered;
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is ReviewWithService review)
        {
            bool confirm = await DisplayAlert("Удалить отзыв?", $"От пользователя {review.Username}?", "Да", "Нет");
            if (confirm)
            {
                await _db.DeleteReviewAsync(review.Id);
                await DisplayAlert("Удалено", "Отзыв успешно удалён", "ОК");

                _allReviews = await _db.GetAllReviewsWithServiceAsync();
                AllReviewsList.ItemsSource = _allReviews;
            }
        }
    }
}
