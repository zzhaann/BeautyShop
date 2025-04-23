using SQLite;

namespace BeautyShop.Models
{
    public class Review
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ServiceId { get; set; }
        public string Username { get; set; } // имя клиента
        public int Rating { get; set; }      // от 1 до 5
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
