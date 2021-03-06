﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data.PizzaBox;
using Data.PizzaBox.DataModel;
using Logic.PizzaBox.DataClasses;

namespace Logic.PizzaBox
{
    public class SystemLogic
    {
        #region Login/Signup
        public bool Login(string username, string password)
        {
            Account user = ExternalDB.GetUser(username);
            bool correct = (user != null && user.Password == password);
            return correct;
        }
        public bool NewUser(string username, string password)
        {
            if (ExternalDB.GetUser(username) == null)
            {
                Account newUser = new Account() { Username = username, Password = password };
                ExternalDB.AddUser(newUser);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ChangeUsername(string oldusername, string username, string password)
        {
            Account user = ExternalDB.GetUser(oldusername);
            Account possible = ExternalDB.GetUser(username);
            if (possible != null && possible != user) return false;

            user.Username = username;
            user.Password = password;
            return true;
        }
        #endregion

        #region Viewing
        public string GetUserOrders(string username)
        {
            Account user = ExternalDB.GetUser(username);
            var orders = ExternalDB.GetOrders(user.Id);

            string s = "Orders:\n";
            foreach (var o in orders)
            {
                s += $"{new Order(o).AsString(false, true, false)}\n";
            }
            return s;
        }

        public string GetOrder(int orderID)
        {
            var o = ExternalDB.Find<Purchase>(orderID);
            if (o == null) return "Order does not exit!\n";
            else return new Order(o).AsString(true, true, true);
        }

        public string GetRestaurants()
        {
            string s = "";
            foreach (var r in ExternalDB.FindList<Location>())
            {
                s += $"{new Restaurant(r).AsString(true, false)}\n";
            }
            return s;
        }
        public string GetRestaurant(int restaurantID)
        {
            Location restaurant = ExternalDB.Find<Location>(restaurantID);
            if (restaurant == null) return null;
            string s = $"{new Restaurant(restaurant).AsString(true, true)}\n";
            return s;
        }
        #endregion

        #region Order
        private string CheckInventory (Order order)
        {
            Dictionary<int, int> crusts = new Dictionary<int, int>();
            Dictionary<int, int> toppings = new Dictionary<int, int>();

            foreach (var p in order.pizzas)
            {
                if (!crusts.Keys.Contains(p.Crust.ID)) crusts[p.Crust.ID] = 0;
                crusts[p.Crust.ID] += p.Amount;

                foreach (var t in p.Toppings)
                {
                    if (!toppings.Keys.Contains(t.ID)) toppings[t.ID] = 0;
                    toppings[t.ID] += p.Amount;
                }
            }

            foreach (var c in crusts)
            {
                if (c.Value > ExternalDB.FindList<PizzaCrustInv>().Where(x => x.CrustId == c.Key && x.LocationId == order.Restaurant.ID).FirstOrDefault().Amount)
                {
                    return $"Unable to create order. Location does not have enough {ExternalDB.Find<PizzaCrust>(c.Key).Name} crust";
                }
            }
            foreach (var t in toppings)
            {
                if (t.Value > ExternalDB.FindList<PizzaToppingInv>().Where(x => x.ToppingId == t.Key && x.LocationId == order.Restaurant.ID).FirstOrDefault().Amount)
                {
                    return $"Unable to create order. Location does not have enough {ExternalDB.Find<PizzaTopping>(t.Key).Name}";
                }
            }

            return null;
        }

        public string MakeOrder(string username, int restaurantID, List<Logic.PizzaBox.DataClasses.Pizza> pizzas)
        {
            Order order = new Order(username, restaurantID, pizzas);
            Restaurant restaurant = new Restaurant(restaurantID);

            Purchase recentOrder = ExternalDB.GetOrders(order.User.ID).OrderByDescending(x => x.Date).FirstOrDefault();
            DateTime recentDate = DateTime.MinValue;
            if (recentOrder != null) recentDate = recentOrder.Date;
            int minHours = restaurant.Store.MinHours;
            var offset = order.Date.Subtract(recentDate);

            if (recentDate.Date.Equals(order.Date.Date) && recentOrder.LocationId != order.Restaurant.ID)
            {
                return "Unable to create order. You already ordered from a different location today.";
            }
            else if (offset.Hours < minHours)
            {
                return "Unable to create order. You already ordered recently.";
            }
            else if (order.Date.Hour < restaurant.Hours.Open || order.Date.Hour >= restaurant.Hours.Close)
            {
                return "Unable to create order. You can only order while the store is open.";
            }
            else if (restaurant.Store.MaxPizza != null && order.PizzaCount > restaurant.Store.MaxPizza)
            {
                return "Unable to create order. You cannot order that many pizzas.";
            }
            else if (restaurant.Store.MaxPrice != null && order.Price > restaurant.Store.MaxPrice)
            {
                return "Unable to create order. Order exceeds maximum allowed price.";
            }
            else if (CheckInventory(order) != null)
            {
                return CheckInventory(order);
            }
            else
            {
                order.Make();
                return "Pizzas have been ordered!";
            }
        }
        #endregion

    }
}
