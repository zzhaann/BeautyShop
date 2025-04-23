using SQLite;

namespace BeautyShop.Models
{
    public class FavoriteService
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
    }
}
