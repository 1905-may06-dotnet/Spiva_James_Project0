using System;
using System.Collections.Generic;

namespace Data.PizzaBox.DataModel
{
    public partial class Purchase
    {
        public Purchase()
        {
            Pizza = new HashSet<Pizza>();
        }

        public int Id { get; set; }
        public int LocationId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }

        public virtual Location Location { get; set; }
        public virtual Account User { get; set; }
        public virtual ICollection<Pizza> Pizza { get; set; }
    }
}
