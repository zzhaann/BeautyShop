using System;

namespace BeautyShop.Models
{
    public class ReviewViewModel : Review
    {
        public bool CanDelete => Preferences.Get("user_role", "user") == "admin";
    }
}
