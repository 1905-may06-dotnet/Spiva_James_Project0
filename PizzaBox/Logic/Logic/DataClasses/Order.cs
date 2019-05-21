using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.PizzaBox;

namespace Logic.PizzaBox.DataClasses
{
    public class Order
    {
        public int? ID { get; set; }
        public (int ID, string Name) User { get; set; }
        public (int ID, string Name) Restaurant { get; set; }
        public DateTime Date { get; set; }

        public List<Pizza> pizzas;

        public double Price { get; set; }
        public int PizzaCount { get; set; }

        public Order(string username, int restaurantID, List<Pizza> pizzas)
        {
            var u = ExternalDB.GetUser(username);
            var r = ExternalDB.Find<Data.PizzaBox.DataModel.Location>(restaurantID);
            var s = ExternalDB.Find<Data.PizzaBox.DataModel.Store>(r.StoreId);

            User = (u.Id, u.Username);
            Restaurant = (r.Id, s.Name);

            this.pizzas = pizzas;
            Date = DateTime.Now;

            foreach (var p in pizzas)
            {
                Price += p.Price * p.Amount;
                PizzaCount += p.Amount;
            }

        }
        public Order(Data.PizzaBox.DataModel.Purchase order)
        {
            var u = ExternalDB.Find<Data.PizzaBox.DataModel.Account>(order.UserId);
            var r = ExternalDB.Find<Data.PizzaBox.DataModel.Location>(order.LocationId);
            var s = ExternalDB.Find<Data.PizzaBox.DataModel.Store>(r.StoreId);

            ID = order.Id;
            User = (u.Id, u.Username);
            Restaurant = (r.Id, s.Name);
            Date = order.Date;

            pizzas = new List<Pizza>();
            foreach (var v in ExternalDB.Context().Pizza.Where(x => x.OrderId == ID))
            {
                Pizza p = new Pizza(v);
                pizzas.Add(p);
                Price += p.Price * p.Amount;
                PizzaCount += p.Amount;
            }
        }

        public Data.PizzaBox.DataModel.Purchase ToPurchaseData()
        {
            Data.PizzaBox.DataModel.Purchase purchase = new Data.PizzaBox.DataModel.Purchase() { UserId = User.ID, LocationId = Restaurant.ID, Date = Date };
            return purchase;
        }

        public string AsString(bool showUser, bool showRestaurant, bool detailed) //TODO : Price
        {
            string s = $"({ID}) {Price}$ - {Date}: ";

            if (showUser) s += User.Name;
            if (showUser && showRestaurant) s += " - ";
            if (showRestaurant) s += new Restaurant(ExternalDB.Find<Data.PizzaBox.DataModel.Location>(Restaurant.ID)).AsString(detailed, false);
            if (showUser || showRestaurant) s += ": \n";

            if (detailed)
            {
                foreach (var p in pizzas)
                {
                    s += $"  {p.AsString()}\n";
                }
            }
            else
            {
                int c = 0;
                foreach (var p in pizzas)
                {
                    c += p.Amount;
                }
                s += $" -> {c} pizzas";
            }
            return s;
        }
    }
}
