using BeautyShop.Models;
using BeautyShop.Services;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.IO;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using System.Linq;

namespace BeautyShop.Pages;

public partial class CartPage : ContentPage
{
    private readonly CartService _cart;
    private readonly DatabaseService _db;

    public CartPage(DatabaseService db, CartService cart)
    {
        InitializeComponent();
        _db = db;
        _cart = cart;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadCart();
    }

    private void LoadCart()
    {
        CartCollectionView.ItemsSource = _cart.GetCart();
        TotalLabel.Text = $"Итого: {_cart.GetTotal()} ₸";
    }

    private async void OnCheckoutClicked(object sender, EventArgs e)
    {
        if (_cart.GetCart().Count == 0)
        {
            await DisplayAlert("Ошибка", "Корзина пуста!", "ОК");
            return;
        }

        var document = new PdfDocument();
        var page = document.Pages.Add();
        var graphics = page.Graphics;

        float yPosition = 0;
        graphics.DrawString("Чек заказа", new PdfStandardFont(PdfFontFamily.Helvetica, 20), PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, yPosition));
        yPosition += 40;

        foreach (var item in _cart.GetCart())
        {
            graphics.DrawString($"{item.Title} - {item.Price} ₸",
                new PdfStandardFont(PdfFontFamily.Helvetica, 14),
                PdfBrushes.Black,
                new Syncfusion.Drawing.PointF(0, yPosition));
            yPosition += 20;
        }

        yPosition += 20;
        graphics.DrawString($"Итого: {_cart.GetTotal()} ₸",
            new PdfStandardFont(PdfFontFamily.Helvetica, 16),
            PdfBrushes.Black,
            new Syncfusion.Drawing.PointF(0, yPosition));

        using (var stream = new MemoryStream())
        {
            document.Save(stream);
            stream.Position = 0;

            var fileName = $"Чек_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            using (var fileStream = File.Create(filePath))
            {
                stream.CopyTo(fileStream);
            }

            // Сохраняем заказ в БД вместе с названиями услуг
            await _db.SaveOrderHistoryAsync(new OrderHistory
            {
                Username = Preferences.Get("user_name", ""),
                ServiceTitles = string.Join(", ", _cart.GetCart().Select(x => x.Title)),
                FilePath = fileName,
                CreatedAt = DateTime.Now
            });

            await DisplayAlert("Успех", $"Заказ оформлен!\nФайл сохранен: {fileName}", "ОК");

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }

        document.Close(true);

        _cart.ClearCart();
        LoadCart();
    }

    private async void OnClearCartClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Очистить корзину", "Вы уверены?", "Да", "Нет");
        if (confirm)
        {
            _cart.ClearCart();
            LoadCart();
        }
    }
}
