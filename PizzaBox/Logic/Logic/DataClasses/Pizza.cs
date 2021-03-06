﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.PizzaBox;

namespace Logic.PizzaBox.DataClasses
{
    public class Pizza
    {
        public int ID { get; set; }
        public (int ID, string Name) Size { get; set; }
        public (int ID, string Name) Crust { get; set; }
        public (int? ID, string Name) Preset { get; set; }

        public List<(int ID, string Name)> Toppings;

        public int Amount { get; set; }
        public double Price { get; set; }
        public int OrderID { get; set; }

        public Pizza(Data.PizzaBox.DataModel.Pizza pizza)
        {
            ID = pizza.Id;
            Price = pizza.Price;
            Amount = pizza.Amount;
            OrderID = pizza.OrderId;

            var s = ExternalDB.Find<Data.PizzaBox.DataModel.PizzaSize>(pizza.SizeId);
            var c = ExternalDB.Find<Data.PizzaBox.DataModel.PizzaCrust>(pizza.CrustId);
            var p = ExternalDB.Find<Data.PizzaBox.DataModel.PizzaPreset>(pizza.PresetId);

            Size = (s.Id, s.Name);
            Crust = (c.Id, c.Name);
            if (p != null) Preset = (p.Id, p.Name);

            var ts = ExternalDB.GetPizzaToppings(pizza.Id);
            Toppings = new List<(int ID, string Name)>();
            foreach (var t in ts)
            {
                Toppings.Add((t.Id, t.Name));
            }
        }
        public Pizza(int sizeID, int crustID, int? presetID, List<int> toppingsIDs)
        {
            Size = (sizeID, ExternalDB.Find<Data.PizzaBox.DataModel.PizzaSize>(sizeID).Name);
            Crust = (crustID, ExternalDB.Find<Data.PizzaBox.DataModel.PizzaCrust>(crustID).Name);
            if (presetID != null) Preset = (presetID, ExternalDB.Find<Data.PizzaBox.DataModel.PizzaPreset>(presetID).Name);

            Toppings = new List<(int ID, string Name)>();
            foreach (int t in toppingsIDs)
            {
                Toppings.Add((t, ExternalDB.Find<Data.PizzaBox.DataModel.PizzaTopping>(t).Name));
            }

            Price = CalcCost();
        }

        public void Make (int orderID)
        {
            Data.PizzaBox.DataModel.Pizza pizza = new Data.PizzaBox.DataModel.Pizza() { OrderId = orderID, SizeId = Size.ID, CrustId = Crust.ID, PresetId = Preset.ID, Amount = Amount, Price = Price };
            ExternalDB.Add(pizza);
            foreach(var t in Toppings)
            {
                Data.PizzaBox.DataModel.PizzaTopping1 topping = new Data.PizzaBox.DataModel.PizzaTopping1() { PizzaId = pizza.Id, ToppingId = t.ID };
            }
        }

        public double CalcCost()
        {
            return 10;
        }

        public string AsString()
        {
            string s = $"{Amount}x {Price}$: {Size.Name} - {Crust.Name} crust, ";

            if (Preset.ID != null) s += $"{Preset.Name} = ";

            s += "toppings:";
            bool none = true;
            foreach (var t in Toppings)
            {
                none = false;
                s += $" {t.Name},";
            }
            if (none) s += " none";

            return s;
        }
    }
}
