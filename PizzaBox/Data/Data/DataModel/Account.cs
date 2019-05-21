using System;
using System.Collections.Generic;

namespace Data.PizzaBox.DataModel
{
    public partial class Account
    {
        public Account()
        {
            Purchase = new HashSet<Purchase>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Purchase> Purchase { get; set; }
    }
}
