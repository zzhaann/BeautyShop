using BeautyShop.Models;
using BeautyShop.Services;

namespace BeautyShop.Pages;

[QueryProperty(nameof(ServiceId), "serviceId")]
public partial class EditServicePage : ContentPage
{
    private readonly DatabaseService _db;
    public int ServiceId { get; set; }
    private string _imagePath;
    private Service _originalService;

    public EditServicePage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _originalService = await _db.GetServiceByIdAsync(ServiceId);

        if (_originalService != null)
        {
            TitleEntry.Text = _originalService.Title;
            DescriptionEditor.Text = _originalService.Description;
            PriceEntry.Text = _originalService.Price.ToString();
            _imagePath = _originalService.ImagePath;
            ServiceImage.Source = ImageSource.FromFile(_imagePath);
        }
    }

    private async void OnPickImageClicked(object sender, EventArgs e)
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

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_originalService == null)
            return;

        string title = TitleEntry.Text;
        string description = DescriptionEditor.Text;
        bool priceParsed = decimal.TryParse(PriceEntry.Text, out decimal price);

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || !priceParsed)
        {
            await DisplayAlert("Ошибка", "Заполните все поля корректно", "ОК");
            return;
        }

        _originalService.Title = title;
        _originalService.Description = description;
        _originalService.Price = price;
        _originalService.ImagePath = _imagePath;

        await _db.UpdateServiceAsync(_originalService);
        await DisplayAlert("Успешно", "Услуга обновлена", "ОК");

        await Shell.Current.GoToAsync("//AdminHomePage");
    }
}
