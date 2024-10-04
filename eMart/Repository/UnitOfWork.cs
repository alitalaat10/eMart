using eMart.Data;
using eMart.Models;
using eMart.Repository.Base;

namespace eMart.Repository
{
    public class UnitOfWork :IUnitOfWork
    {
        public UnitOfWork(AppDbContext Db) 
        {
            products = new Repository<Product>(Db);
            categories = new Repository<Category>(Db);
            subCategories = new Repository<SubCategory>(Db);
            brands = new Repository<Brand>(Db);
            carts = new Repository<Cart>(Db);
            orders = new Repository<Order>(Db);
            orderProducts = new Repository<OrderProducts>(Db);
            cartProducts = new Repository<CartProducts>(Db);
            cartProducts = new Repository<CartProducts>(Db);
            reviews = new Repository<Review>(Db);
        }
      
        public IRepository<Product> products { get; }

        public IRepository<Category> categories { get; }

        public IRepository<SubCategory> subCategories { get; }

        public IRepository<Brand> brands { get;}

        public IRepository<Cart> carts { get; }

        public IRepository<Order> orders { get; }

        public IRepository<OrderProducts> orderProducts { get; }

        public IRepository<CartProducts> cartProducts { get; }
        public IRepository<Review> reviews { get; }
    }
}
