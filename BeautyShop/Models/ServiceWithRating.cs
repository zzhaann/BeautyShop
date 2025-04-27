namespace BeautyShop.Models
{
    public class ServiceWithRating
    {
        public Service Service { get; set; }
        public double Rating { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsInCart { get; set; }

    }
}
