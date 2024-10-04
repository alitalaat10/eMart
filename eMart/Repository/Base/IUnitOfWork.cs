using eMart.Models;
using System.Collections.Concurrent;

namespace eMart.Repository.Base
{
    public interface IUnitOfWork 
    {
        IRepository<Product> products { get; }
        IRepository<Category> categories { get; }
        IRepository<SubCategory> subCategories { get; }
        IRepository<Brand> brands { get; }
        IRepository<Cart> carts { get; }
        IRepository<Order> orders { get; }
        IRepository<OrderProducts> orderProducts { get; }
        IRepository<CartProducts> cartProducts { get; }
        IRepository<Review> reviews { get; }

    }
}
