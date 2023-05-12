using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Ordering
    {
        public int OrderNumber { get; set; }
        public int UserIdd { get; set; }
        public int NumberProduct { get; set; }
        public DateTime DateAndTimeReferences { get; set; }
        public string Statuss { get; set; } = null!;
        public int Quantity { get; set; }

        public virtual Product NumberProductNavigation { get; set; } = null!;
        public virtual User UserIddNavigation { get; set; } = null!;
    }
}
