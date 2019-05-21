using System;
using System.Collections.Generic;

namespace Data.PizzaBox.DataModel
{
    public partial class Store
    {
        public Store()
        {
            Location = new HashSet<Location>();
            PizzaCrust = new HashSet<PizzaCrust>();
            PizzaPreset = new HashSet<PizzaPreset>();
            PizzaSize = new HashSet<PizzaSize>();
            PizzaTopping = new HashSet<PizzaTopping>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? MaxToppings { get; set; }
        public int? MaxPizza { get; set; }
        public int? MaxPrice { get; set; }
        public int MinHours { get; set; }

        public virtual ICollection<Location> Location { get; set; }
        public virtual ICollection<PizzaCrust> PizzaCrust { get; set; }
        public virtual ICollection<PizzaPreset> PizzaPreset { get; set; }
        public virtual ICollection<PizzaSize> PizzaSize { get; set; }
        public virtual ICollection<PizzaTopping> PizzaTopping { get; set; }
    }
}
