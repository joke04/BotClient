using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class User
    {
        public User()
        {
            Baskets = new HashSet<Basket>();
            Orderings = new HashSet<Ordering>();
            SavedAddresses = new HashSet<SavedAddress>();
        }

        public int UserNumber { get; set; }
        public string Nickname { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Namee { get; set; } = null!;
        public string Patronymic { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime Birthdate { get; set; }

        public virtual ICollection<Basket> Baskets { get; set; }
        public virtual ICollection<Ordering> Orderings { get; set; }
        public virtual ICollection<SavedAddress> SavedAddresses { get; set; }
    }
}
