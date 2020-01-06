using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public virtual void AddItem(Product product, int quantity)
        {
            var line = lineCollection
                .Where(x => x.Product.ProductID == product.ProductID)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine()
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) => 
            lineCollection.RemoveAll(x => x.Product.ProductID == product.ProductID);

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;

        public virtual decimal ComputeTotalValue() => lineCollection.Sum(x => x.Product.Price * x.Quantity);
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
