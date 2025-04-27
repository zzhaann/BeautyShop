using SQLite;
using BeautyShop.Models;
using System.IO;
using System.Linq;

namespace BeautyShop.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public async Task Init()
        {
            if (_database != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "beautyshop.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<Service>();
            await _database.CreateTableAsync<OrderHistory>();
            await _database.CreateTableAsync<FavoriteService>();
            await _database.CreateTableAsync<Review>();

            // Добавить администратора при первом запуске
            var admin = await _database.Table<User>().FirstOrDefaultAsync(u => u.Username == "admin");
            if (admin == null)
            {
                await _database.InsertAsync(new User
                {
                    Username = "admin",
                    Password = "admin",
                    Role = "admin"
                });
            }
        }

        // ----------- Работа с пользователями -----------
        public async Task<List<User>> GetUsersAsync()
        {
            await Init();
            return await _database.Table<User>().ToListAsync();
        }

        public async Task<User> GetUserAsync(string username)
        {
            await Init();
            return await _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<int> AddUserAsync(User user)
        {
            await Init();
            return await _database.InsertAsync(user);
        }

        public async Task<bool> ValidateUser(string username, string password)
        {
            await Init();
            var user = await _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            return user != null;
        }

        // ----------- Работа с услугами -----------
        public async Task<List<Service>> GetServicesAsync()
        {
            await Init();
            return await _database.Table<Service>().ToListAsync();
        }

        public async Task<int> AddServiceAsync(Service service)
        {
            await Init();
            return await _database.InsertAsync(service);
        }

        public async Task<Service> GetServiceByIdAsync(int id)
        {
            await Init();
            return await _database.Table<Service>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Service>> GetServicesByCreatorAsync(string creator)
        {
            await Init();
            return await _database.Table<Service>().Where(s => s.CreatedBy == creator).ToListAsync();
        }

        public async Task DeleteServiceAsync(int serviceId)
        {
            await Init();
            var service = await _database.Table<Service>().FirstOrDefaultAsync(s => s.Id == serviceId);
            if (service != null)
                await _database.DeleteAsync(service);
        }

        public async Task UpdateServiceAsync(Service service)
        {
            await Init();
            await _database.UpdateAsync(service);
        }

        // ----------- Работа с историей заказов -----------
        public async Task<int> SaveOrderHistoryAsync(OrderHistory order)
        {
            await Init();
            return await _database.InsertAsync(order);
        }

        public async Task<List<OrderHistory>> GetOrderHistoryAsync()
        {
            await Init();
            return await _database.Table<OrderHistory>().OrderByDescending(o => o.CreatedAt).ToListAsync();
        }



        public async Task<List<OrderHistory>> GetOrderHistoryByUserAsync(string username)
        {
            await Init();
            return await _database.Table<OrderHistory>()
                                   .Where(o => o.Username == username)
                                   .OrderByDescending(o => o.CreatedAt)
                                   .ToListAsync();
        }


        // ----------- Работа с отзывами -----------
        public async Task AddReviewAsync(Review review)
        {
            await Init();
            await _database.InsertAsync(review);
        }

        public async Task<List<Review>> GetReviewsByServiceIdAsync(int serviceId)
        {
            await Init();
            return await _database.Table<Review>()
                .Where(r => r.ServiceId == serviceId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(int serviceId)
        {
            await Init();
            var reviews = await _database.Table<Review>()
                .Where(r => r.ServiceId == serviceId)
                .ToListAsync();

            return reviews.Count == 0 ? 0 : reviews.Average(r => r.Rating);
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            await Init();
            var review = await _database.Table<Review>().FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review != null)
                await _database.DeleteAsync(review);
        }

        public async Task<List<ReviewWithService>> GetAllReviewsWithServiceAsync()
        {
            await Init();

            var reviews = await _database.Table<Review>().ToListAsync();
            var services = await _database.Table<Service>().ToListAsync();

            return (from r in reviews
                    join s in services on r.ServiceId equals s.Id
                    select new ReviewWithService
                    {
                        Id = r.Id,
                        Username = r.Username,
                        Comment = r.Comment,
                        Rating = r.Rating,
                        Date = r.Date,
                        ServiceId = r.ServiceId,
                        ServiceTitle = s.Title
                    }).ToList();
        }

        // ----------- Работа с избранным -----------
        public async Task<List<Service>> GetFavoritesAsync()
        {
            await Init();
            var username = Preferences.Get("user_name", "");

            var favoriteIds = (await _database.Table<FavoriteService>()
                .Where(f => f.Username == username)
                .ToListAsync())
                .Select(f => f.ServiceId)
                .ToList();

            var services = await _database.Table<Service>()
                .Where(s => favoriteIds.Contains(s.Id))
                .ToListAsync();

            return services;
        }

        public async Task AddToFavoritesAsync(FavoriteService favorite)
        {
            await Init();
            favorite.Username = Preferences.Get("user_name", ""); // Важно: добавляем владельца
            await _database.InsertAsync(favorite);
        }

        public async Task RemoveFromFavoritesAsync(int serviceId)
        {
            await Init();
            var username = Preferences.Get("user_name", "");
            var existing = await _database.Table<FavoriteService>()
                .FirstOrDefaultAsync(f => f.ServiceId == serviceId && f.Username == username);

            if (existing != null)
                await _database.DeleteAsync(existing);
        }

        public async Task<bool> IsFavoriteAsync(int serviceId)
        {
            await Init();
            var username = Preferences.Get("user_name", "");
            var existing = await _database.Table<FavoriteService>()
                .FirstOrDefaultAsync(f => f.ServiceId == serviceId && f.Username == username);

            return existing != null;
        }


        public async Task<List<User>> GetAllClientsAsync()
        {
            await Init();
            return await _database.Table<User>()
                .Where(u => u.Role == "user")
                .ToListAsync();
        }


        public async Task DeleteUserAsync(int userId)
        {
            await Init();
            var user = await _database.Table<User>().FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
                await _database.DeleteAsync(user);
        }


    }
}
