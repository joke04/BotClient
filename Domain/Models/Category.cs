using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
            Filter = new HashSet<Filter>();
        }

        public int IdCategories { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Filter>? Filter { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
