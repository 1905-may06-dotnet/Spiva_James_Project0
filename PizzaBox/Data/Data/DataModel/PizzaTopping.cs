using System;
using System.Collections.Generic;

namespace Data.PizzaBox.DataModel
{
    public partial class PizzaTopping : PizzaPart
    {
        public PizzaTopping()
        {
            PizzaPresetTopping = new HashSet<PizzaPresetTopping>();
            PizzaTopping1 = new HashSet<PizzaTopping1>();
            PizzaToppingInv = new HashSet<PizzaToppingInv>();
        }

        //public int Id { get; set; }
        //public int StoreId { get; set; }
        //public string Name { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<PizzaPresetTopping> PizzaPresetTopping { get; set; }
        public virtual ICollection<PizzaTopping1> PizzaTopping1 { get; set; }
        public virtual ICollection<PizzaToppingInv> PizzaToppingInv { get; set; }
    }
}
