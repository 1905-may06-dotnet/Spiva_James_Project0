using System;
using System.Collections.Generic;

namespace Data.PizzaBox.DataModel
{
    public partial class Location
    {
        public Location()
        {
            PizzaCrustInv = new HashSet<PizzaCrustInv>();
            PizzaToppingInv = new HashSet<PizzaToppingInv>();
            Purchase = new HashSet<Purchase>();
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public int AddressId { get; set; }
        public int OpenTime { get; set; }
        public int CloseTime { get; set; }

        public virtual Address Address { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<PizzaCrustInv> PizzaCrustInv { get; set; }
        public virtual ICollection<PizzaToppingInv> PizzaToppingInv { get; set; }
        public virtual ICollection<Purchase> Purchase { get; set; }
    }
}
