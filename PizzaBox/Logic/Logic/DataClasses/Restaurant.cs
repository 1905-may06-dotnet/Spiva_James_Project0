using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.PizzaBox;

namespace Logic.PizzaBox.DataClasses
{
    public class Restaurant
    {
        public int ID { get; set; }
        public (int ID, string Name) Store { get; set; }
        public (int Open, int Close) Hours { get; set; }

        public Address address;

        public Restaurant(Data.PizzaBox.DataModel.Location restaurant)
        {
            var s = ExternalDB.Find<Data.PizzaBox.DataModel.Store>(restaurant.StoreId);
            ID = restaurant.Id;
            Store = (s.Id, s.Name);
            Hours = (restaurant.OpenTime, restaurant.CloseTime);
            address = new Address(ExternalDB.Find<Data.PizzaBox.DataModel.Address>(restaurant.AddressId));
        }

        public string AsString(bool hours, bool detailed)
        {
            string s = $"{ID}: {Store.Name}: {address.AsString()}";
            if (hours) s += $"\n    {HoursString()}";

            if (detailed)
            {
                s += InventoryString();
                s += OrderString();
            }

            return s;
        }
        public string HoursString()
        {
            int op = Hours.Open;
            int cl = Hours.Close;

            if (cl == 24) cl = 0;

            bool a1 = op < 12;
            bool a2 = cl < 12;

            string s = $"Open: ";
            if (a1) s += $"{op}:00 AM";
            else s += $"{op - 12}:00 PM";
            s += " - ";
            if (a2) s += $"{cl}:00 AM";
            else s += $"{cl}:00 PM";

            return s;
        }
        public string InventoryString ()
        {
            string s = "\nInventory: ";
            s += "\n  Crusts: ";
            foreach (var c in ExternalDB.FindList<Data.PizzaBox.DataModel.PizzaCrustInv>().Where(x => x.LocationId == ID))
            {
                s += $"\n    {ExternalDB.Find<Data.PizzaBox.DataModel.PizzaCrust>(c.CrustId).Name}: {c.Amount}";
            }
            s += "\n  Toppings: ";
            foreach (var c in ExternalDB.FindList<Data.PizzaBox.DataModel.PizzaToppingInv>().Where(x => x.LocationId == ID))
            {
                s += $"\n    {ExternalDB.Find<Data.PizzaBox.DataModel.PizzaTopping>(c.ToppingId).Name}: {c.Amount}";
            }
            return s;
        }
        public string OrderString ()
        {
            string s = "\nOrders: ";
            double total = 0;
            int count = 0;
            int pcount = 0;

            foreach (var p in ExternalDB.FindList<Data.PizzaBox.DataModel.Purchase>().Where(x => x.LocationId == ID))
            {
                Order o = new Order(p);
                s += $"\n  {o.AsString(true, false, false)}";
                total += o.Price;
                count += 1;
                foreach (var pizza in o.pizzas)
                {
                    pcount += pizza.Amount;
                }
            }

            s += $"\nSale Summary: {total}$ across {count} orders with {pcount} pizzas";
            return s;
        }
    }
}
