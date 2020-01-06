using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class FakeProductRepository : IProductRepository
    {
        public IQueryable<Product> Products => new List<Product>
        {
            new Product {Name = "Football", Price = 25},
            new Product {Name = "Volleyball", Price = 50},
            new Product {Name = "Volleyball net", Price = 300},
            new Product {Name = "Volleyball posts", Price = 150}
        }.AsQueryable<Product>();
    }
}
