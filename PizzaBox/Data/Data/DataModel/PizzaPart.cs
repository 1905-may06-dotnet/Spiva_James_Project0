using System;
using System.Collections.Generic;
using System.Text;

namespace Data.PizzaBox.DataModel
{
    public abstract class PizzaPart
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; }
    }
}
