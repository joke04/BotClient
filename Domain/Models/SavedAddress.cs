using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class SavedAddress
    {
        public int UserIdd { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public int House { get; set; }
        public int Construction { get; set; }
        public int Flat { get; set; }
        public string AddressName { get; set; } = null!;

        public virtual User UserIddNavigation { get; set; } = null!;
    }
}
