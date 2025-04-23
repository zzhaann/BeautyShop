using BeautyShop.Pages;

namespace BeautyShop
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("UserHomePage", typeof(UserHomePage));
            Routing.RegisterRoute("AdminHomePage", typeof(AdminHomePage));
            Routing.RegisterRoute("ServicesPage", typeof(ServicesPage));
            Routing.RegisterRoute("AddServicePage", typeof(AddServicePage));
            Routing.RegisterRoute("AppointmentCartPage", typeof(AppointmentCartPage));
            Routing.RegisterRoute("OrderHistoryPage", typeof(OrderHistoryPage));
            Routing.RegisterRoute("FavoritesPage", typeof(FavoritesPage));
            Routing.RegisterRoute("AddReviewPage", typeof(AddReviewPage));

            Routing.RegisterRoute("ReviewsPage", typeof(ReviewsPage));
            Routing.RegisterRoute("MyServicesPage", typeof(MyServicesPage));
            Routing.RegisterRoute("EditServicePage", typeof(EditServicePage));

            Routing.RegisterRoute("AllReviewsPage", typeof(AllReviewsPage));
            Routing.RegisterRoute("WelcomePage", typeof(WelcomePage));


        }
    }
}
