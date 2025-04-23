using SQLite;

namespace BeautyShop.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        // "user" — клиент, "admin" — админ
        public string Role { get; set; }
    }
}
