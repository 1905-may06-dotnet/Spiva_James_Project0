using System;
using System.Collections.Generic;

namespace Data.PizzaBox.DataModel
{
    public partial class Address
    {
        public Address()
        {
            Location = new HashSet<Location>();
        }

        public int Id { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }

        public virtual ICollection<Location> Location { get; set; }
    }
}
