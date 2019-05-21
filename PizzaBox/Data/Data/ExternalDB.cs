using System;
using System.Collections.Generic;
using System.Text;
using Data.PizzaBox.DataModel;
using System.Linq;

namespace Data.PizzaBox
{
    public class ExternalDB
    {
        private static ExternalDB instance;
        private PizzaDBContext context;

        private ExternalDB()
        {
            context = new PizzaDBContext();
        }
        public static PizzaDBContext Context()
        {
            if (instance == null) instance = new ExternalDB();
            return instance.context;
        }

        public static void AddUser(Account user)
        {
            Context().Account.Add(user);
            Context().SaveChanges();
        }
        public static void AddOrder(Purchase order, List<Pizza> pizzas, List<List<PizzaTopping1>> toppings)
        {
            Context().Purchase.Add(order);
            for (int i = 0; i < pizzas.Count; i++)
            {
                pizzas[i].OrderId = order.Id;
                Context().Pizza.Add(pizzas[i]);
                Context().PizzaCrustInv.Where(x => x.LocationId == order.LocationId && x.CrustId == pizzas[i].CrustId).FirstOrDefault().Amount -= pizzas[i].Amount;

                foreach (var t in toppings[i])
                {
                    t.PizzaId = pizzas[i].Id;
                    Context().PizzaTopping1.Add(t);
                    Context().PizzaToppingInv.Where(x => x.LocationId == order.LocationId && x.ToppingId == t.ToppingId).FirstOrDefault().Amount -= pizzas[i].Amount;
                }
            }
            Context().SaveChanges();
        }

        public static Account GetUser(string username) => Context().Account.Where(x => x.Username == username).FirstOrDefault();
        public static List<Purchase> GetOrders(int userID) => Context().Purchase.Where(x => x.UserId == userID).OrderByDescending(y => y.Date).ToList();
        public static List<PizzaTopping> GetPizzaToppings(int pizzaID) => Context().PizzaTopping1.Where(x => x.PizzaId == pizzaID).Join(Context().PizzaTopping, y => y.ToppingId, z => z.Id, (y, z) => new { Y = y, Z = z }).Select(a => a.Z).ToList();

        public static Store GetStore(int restaurantID) => Find<Store>(Context().Location.Where(x => x.Id == restaurantID).FirstOrDefault().StoreId);

        public static T Find<T>(int? ID) where T : class => Context().Find<T>(ID);
        public static List<T> FindList<T>() where T : class => Context().Set<T>().ToList();

        public static IQueryable<T> FindPizzaPartsList<T>(int storeID) where T : PizzaPart => Context().Set<T>().Where(x => x.StoreId == storeID);
        public static List<PizzaTopping> GetPresetToppings(int presetID) => Context().PizzaPresetTopping.Where(x => x.PresetId == presetID).Join(Context().PizzaTopping, y => y.ToppingId, z => z.Id, (y, z) => new { Y = y, Z = z }).Select(a => a.Z).ToList();
        
    }
}
