using SQLite;
using BeautyShop.Models;


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

            
            var admin = await _database.Table<User>().Where(u => u.Username == "admin").FirstOrDefaultAsync();
            if (admin == null)
            {
                await _database.InsertAsync(new User
                {
                    Username = "admin",
                    Password = "admin", 
                    Role = "admin"
                });
            }

            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<Service>();
            await _database.CreateTableAsync<OrderHistory>();
            await _database.CreateTableAsync<FavoriteService>();
            await _database.CreateTableAsync<Review>();




        }

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




        public async Task<List<FavoriteService>> GetFavoritesAsync()
        {
            await Init();
            return await _database.Table<FavoriteService>().ToListAsync();
        }

        public async Task AddToFavoritesAsync(FavoriteService favorite)
        {
            await Init();
            await _database.InsertAsync(favorite);
        }

        public async Task RemoveFromFavoritesAsync(int serviceId)
        {
            await Init();
            var existing = await _database.Table<FavoriteService>().FirstOrDefaultAsync(f => f.ServiceId == serviceId);
            if (existing != null)
                await _database.DeleteAsync(existing);
        }


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

        public async Task<Service> GetServiceByIdAsync(int id)
        {
            await Init();
            return await _database.Table<Service>().FirstOrDefaultAsync(s => s.Id == id);
        }


        public async Task DeleteReviewAsync(int reviewId)
        {
            await Init();
            var review = await _database.Table<Review>().FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review != null)
                await _database.DeleteAsync(review);
        }

     
        public async Task<List<Service>> GetServicesByCreatorAsync(string creator)
        {
            await Init();
            return await _database.Table<Service>()
                .Where(s => s.CreatedBy == creator)
                .ToListAsync();
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


    }
}
