using SQLite;

namespace BeautyShop.Models
{
    public class OrderHistory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
