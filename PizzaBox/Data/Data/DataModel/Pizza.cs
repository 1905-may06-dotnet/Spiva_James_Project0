using System;
using System.Collections.Generic;

namespace Data.PizzaBox.DataModel
{
    public partial class Pizza
    {
        public Pizza()
        {
            PizzaTopping1 = new HashSet<PizzaTopping1>();
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public int CrustId { get; set; }
        public int SizeId { get; set; }
        public int? PresetId { get; set; }
        public double Price { get; set; }

        public virtual PizzaCrust Crust { get; set; }
        public virtual Purchase Order { get; set; }
        public virtual PizzaPreset Preset { get; set; }
        public virtual PizzaSize Size { get; set; }
        public virtual ICollection<PizzaTopping1> PizzaTopping1 { get; set; }
    }
}
