using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Basket
    {
        public int UserIdd { get; set; }
        public int BasketNumber { get; set; }
        public int QuantityOfGoods { get; set; }

        public virtual Product BasketNumberNavigation { get; set; } = null!;
        public virtual User UserIddNavigation { get; set; } = null!;
    }
}
