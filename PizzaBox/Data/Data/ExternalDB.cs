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

        public static Account GetUser(string username) => Context().Account.Where(x => x.Username == username).FirstOrDefault();
        public static List<Purchase> GetOrders(int userID) => Context().Purchase.Where(x => x.UserId == userID).OrderByDescending(y => y.Date).ToList();
        public static List<PizzaTopping> GetPizzaToppings(int pizzaID) => Context().PizzaTopping1.Where(x => x.PizzaId == pizzaID).Join(Context().PizzaTopping, y => y.ToppingId, z => z.Id, (y, z) => new { Y = y, Z = z }).Select(a => a.Z).ToList();

        public static void Add<T>(T t) where T : class { Context().Add<T>(t); Context().SaveChanges(); }
        public static T Find<T>(int? ID) where T : class => Context().Find<T>(ID);
        public static List<T> FindList<T>() where T : class => Context().Set<T>().ToList();

        public static IQueryable<T> FindPizzaPartsList<T>(int storeID) where T : PizzaPart => Context().Set<T>().Where(x => x.StoreId == storeID);
        public static List<PizzaTopping> GetPresetToppings(int presetID) => Context().PizzaPresetTopping.Where(x => x.PresetId == presetID).Join(Context().PizzaTopping, y => y.ToppingId, z => z.Id, (y, z) => new { Y = y, Z = z }).Select(a => a.Z).ToList();
        
    }
}
