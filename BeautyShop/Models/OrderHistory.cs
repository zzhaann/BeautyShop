using SQLite;

namespace BeautyShop.Models
{
    public class OrderHistory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; } // Имя клиента
        public string ServiceTitles { get; set; } // Названия услуг через запятую

        public string FilePath { get; set; } // Путь к PDF
        public DateTime CreatedAt { get; set; } // Дата заказа
    }
}
