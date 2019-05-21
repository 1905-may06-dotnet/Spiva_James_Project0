using System;
using System.Collections.Generic;

namespace Data.PizzaBox.DataModel
{
    public partial class PizzaPreset : PizzaPart
    {
        public PizzaPreset()
        {
            Pizza = new HashSet<Pizza>();
            PizzaPresetTopping = new HashSet<PizzaPresetTopping>();
        }

        //public int Id { get; set; }
        //public int StoreId { get; set; }
        //public string Name { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<Pizza> Pizza { get; set; }
        public virtual ICollection<PizzaPresetTopping> PizzaPresetTopping { get; set; }
    }
}
