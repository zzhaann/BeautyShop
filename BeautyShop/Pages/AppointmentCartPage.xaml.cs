using BeautyShop.Models;
using BeautyShop.Services;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.IO;

namespace BeautyShop.Pages;

public partial class AppointmentCartPage : ContentPage
{
    private readonly CartService _cart;
    private readonly DatabaseService _db;

    public AppointmentCartPage(CartService cart, DatabaseService db)
    {
        InitializeComponent();
        _cart = cart;
        _db = db;

        CartListView.ItemsSource = _cart.GetCart();
        TotalLabel.Text = $"Общая сумма: {_cart.GetTotal()} ?";
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        var cartItems = _cart.GetCart();
        if (cartItems.Count == 0)
        {
            await DisplayAlert("Пусто", "Корзина пуста", "ОК");
            return;
        }

        // ?? Генерация PDF
        var doc = new PdfDocument();
        var page = doc.Pages.Add();
        var graphics = page.Graphics;
        var font = new PdfStandardFont(PdfFontFamily.Helvetica, 14);

        graphics.DrawString("Чек BeautyShop", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(10, 10));

        float y = 40;
        foreach (var item in cartItems)
        {
            graphics.DrawString($"- {item.Title} — {item.Price} ?", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(10, y));
            y += 25;
        }

        graphics.DrawString($"ИТОГО: {_cart.GetTotal()} ?", font, PdfBrushes.DarkBlue, new Syncfusion.Drawing.PointF(10, y + 10));

        // ?? Сохраняем PDF в файл
        string fileName = $"check_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
        using (var stream = File.Create(filePath))
        {
            doc.Save(stream);
        }
        doc.Close(true);

        // ?? Сохраняем в историю заказов
        await _db.SaveOrderHistoryAsync(new OrderHistory
        {
            FilePath = filePath,
            CreatedAt = DateTime.Now
        });

        await DisplayAlert("Успешно", $"Чек сохранён:\n{filePath}", "ОК");

        // ?? Очищаем корзину и возвращаемся на главную
        _cart.ClearCart();
        await Shell.Current.GoToAsync("//UserHomePage");
    }
}
