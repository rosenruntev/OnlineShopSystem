using OSS.Models.Entities;
using System;

namespace OSS.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly ShopSystemDbContext dbContext;
        private BaseRepository<User> userRepository;
        private BaseRepository<Product> productRepository;
        private BaseRepository<Order> orderRepository;

        private bool disposed = false;

        public UnitOfWork()
        {
            this.dbContext = new ShopSystemDbContext();
        }

        public BaseRepository<User> UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new BaseRepository<User>(dbContext);
                }

                return userRepository;
            }
        }

        public BaseRepository<Product> ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new BaseRepository<Product>(dbContext);
                }

                return productRepository;
            }
        }

        public BaseRepository<Order> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new BaseRepository<Order>(dbContext);
                }

                return orderRepository;
            }
        }

        public bool Save()
        {
            try
            {
                dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }

                disposed = true;
            }
        }
    }
}
