using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Product
    {
        public Product()
        {
            Baskets = new HashSet<Basket>();
            Orderings = new HashSet<Ordering>();
        }

        public int NumberProduct { get; set; }
        public int IdCategories { get; set; }
        public string Namee { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; } = null!;
        public int Article { get; set; }

        public virtual Category IdCategoriesNavigation { get; set; } = null!;
        public virtual ICollection<Basket> Baskets { get; set; }
        public virtual ICollection<Ordering> Orderings { get; set; }
    }
}
