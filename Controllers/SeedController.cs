using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication191024_Shop.Data;
using WebApplication191024_Shop.Models;

namespace WebApplication191024_Shop.Controllers
{
    public class SeedController : Controller
    {
        private readonly ApplicationContext _context;

        public SeedController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Count = _context.Products.Count();
            return View(_context.Products.Include(e => e.Category).OrderBy(e => e.Id).Take(20));
        }

        [HttpPost]
        public IActionResult CreateSeedData(int count)
        {
            ClearData();
            if (count > 0)
            {
                _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
                _context.Database.ExecuteSqlRaw("DROP PROCEDURE IF EXISTS CreateSeedData");
                _context.Database.ExecuteSqlRaw($@"
            CREATE PROCEDURE CreateSeedData 
            @RowCount decimal
            AS
            BEGIN
            SET NOCOUNT ON
            DECLARE @i INT = 0;
            DECLARE @catId INT;
            DECLARE @CatCount INT = @RowCount / 10;
            DECLARE @pprice DECIMAL(5,2);
            DECLARE @rprice DECIMAL(5,2);
            BEGIN TRANSACTION
            WHILE @i < @CatCount
            BEGIN 
            INSERT INTO Categories (Name,Description)
            VALUES (CONCAT('Category-',@i),
            'Test Data Category');
            SET @catId = SCOPE_IDENTITY();
            DECLARE @j INT = 1;
            WHILE @j <= 10
            BEGIN
            SET @pprice = RAND() * (500-5+1);
            SET @rprice = (RAND() * @pprice) + @pprice;
            INSERT INTO Products (Name,CategoryId,PurchasePrice,RetailPrice)
            VALUES (CONCAT('Product',@i,'-',@j),@catId,@pprice,@rprice)
            SET @j = @j + 1
            END
            SET @i = @i + 1
            END
            COMMIT
            END");
                _context.Database.BeginTransaction();
                _context.Database.ExecuteSqlRaw($"EXEC CreateSeedData @RowCount = {count}");
                _context.Database.CommitTransaction();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult ClearData()
        {
            _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
            _context.Database.BeginTransaction();
            _context.Database.ExecuteSqlRaw("DELETE FROM Products");
            _context.Database.ExecuteSqlRaw("DELETE FROM Categories");
            _context.Database.CommitTransaction();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult SeedDatabase()
        {
            if (!_context.Categories.Any() && !_context.Products.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Action", Description = "Action games" },
                    new Category { Name = "RPG", Description = "Role-playing games" },
                    new Category { Name = "Sports", Description = "Sports games" },
                    new Category { Name = "Simulation", Description = "Simulation games" },
                    new Category { Name = "Strategy", Description = "Strategy games" }
                };

                var products = new List<Product>
                {
                    new Product { Name = "Cyberpunk 2077", Category = categories[0], PurchasePrice = 1500, RetailPrice = 3000 },
                    new Product { Name = "The Witcher 3", Category = categories[1], PurchasePrice = 1200, RetailPrice = 2500 },
                    new Product { Name = "FIFA 23", Category = categories[2], PurchasePrice = 1800, RetailPrice = 3500 },
                    new Product { Name = "The Sims 4", Category = categories[3], PurchasePrice = 1000, RetailPrice = 2000 },
                    new Product { Name = "Age of Empires IV", Category = categories[4], PurchasePrice = 1300, RetailPrice = 2700 },
                    new Product { Name = "Hades", Category = categories[0], PurchasePrice = 800, RetailPrice = 1500 },
                    new Product { Name = "Dark Souls 3", Category = categories[1], PurchasePrice = 1400, RetailPrice = 2800 },
                    new Product { Name = "NBA 2K23", Category = categories[2], PurchasePrice = 2000, RetailPrice = 4000 },
                    new Product { Name = "SimCity", Category = categories[3], PurchasePrice = 1100, RetailPrice = 2200 },
                    new Product { Name = "Civilization VI", Category = categories[4], PurchasePrice = 1200, RetailPrice = 2400 }
                };

                _context.Categories.AddRange(categories);
                _context.Products.AddRange(products);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
