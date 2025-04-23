using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

public partial class AddServicePage : ContentPage
{
    private readonly DatabaseService _db;
    private string _imagePath;


    public AddServicePage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите изображение",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                _imagePath = result.FullPath;
                ServiceImage.Source = ImageSource.FromFile(_imagePath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось выбрать изображение: {ex.Message}", "ОК");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        string title = TitleEntry.Text;
        string description = DescriptionEditor.Text;
        bool priceParsed = decimal.TryParse(PriceEntry.Text, out decimal price);

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || !priceParsed)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля корректно.", "ОК");
            return;
        }

        var service = new Service
        {
            Title = title,
            Description = description,
            Price = price,
            ImagePath = _imagePath,
            CreatedBy = Preferences.Get("user_name", "admin") // 👈 сохраняем автора
        };

        await _db.AddServiceAsync(service);
        await DisplayAlert("Успешно", "Услуга добавлена!", "ОК");
        await Shell.Current.GoToAsync("//AdminHomePage");
    }





}
