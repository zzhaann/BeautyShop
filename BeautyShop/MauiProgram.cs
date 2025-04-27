using BeautyShop.Pages;
using BeautyShop.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;


namespace BeautyShop;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<CartService>();
       
        builder.Services.AddSingleton<OrderHistoryPage>();
        builder.Services.AddSingleton<FavoritesPage>();

        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<RegisterPage>();
        builder.Services.AddSingleton<UserHomePage>();
        builder.Services.AddSingleton<AdminHomePage>();
        builder.Services.AddSingleton<ServicesPage>();
        builder.Services.AddSingleton<AddServicePage>();
        builder.Services.AddSingleton<AddReviewPage>();
        builder.Services.AddSingleton<ReviewsPage>();
        builder.Services.AddSingleton<MyServicesPage>();
        builder.Services.AddSingleton<EditServicePage>();
        builder.Services.AddSingleton<AllReviewsPage>();
        builder.Services.AddSingleton<WelcomePage>();
        builder.Services.AddSingleton<ClientsPage>();

        builder.Services.AddSingleton<CartPage>();
        builder.Services.AddSingleton<AdminOrdersPage>();



#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
