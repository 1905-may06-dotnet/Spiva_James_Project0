using System;
using System.Collections.Generic;
using System.Linq;
using Data.PizzaBox;
using Logic.PizzaBox;
using Logic.PizzaBox.DataClasses;

namespace Client.PizzaBox
{
    public class CUI
    {
        SystemLogic systemLogic;
        string username;
        int restaurantID;

        public CUI(SystemLogic systemLogic)
        {
            this.systemLogic = systemLogic;
        }

        public void Run()
        {
            Console.WriteLine("Welcome.");
            while (true)
            {
                List<string> commands = new List<string>() { "login", "signup" };
                int command = OptionsInput("Commands (-1 to exit): ", commands, true);

                if (command == -1) break;
                else if (command == 0) { Login(); }
                else if (command == 1) { SignUp(); }

            }
            Console.WriteLine("Exiting...");
        }

        #region Login/Signup
        public void Login()
        {
            string username = StringInput("Please Enter Username: ");
            string password = StringInput("Please Enter Password: ");

            bool rv = systemLogic.Login(username, password);

            if (rv)
            {
                Console.WriteLine("Logged in!");
                this.username = username;
                LoggedInLoop();
                this.username = null;
            }
            else
            {
                Console.WriteLine("Incorrect Username/Password");
            }
        }

        public void SignUp()
        {
            string username = StringInput("Please Enter Username: ");
            string password = StringInput("Please Enter Password: ");

            bool rv = systemLogic.NewUser(username, password);

            if (rv)
            {
                Console.WriteLine("Account Created!");
                Console.WriteLine("Please Login");
            }
            else
            {
                Console.WriteLine("Username Already Taken");
            }
        }

        public void LoggedInLoop()
        {
            while (true)
            {
                List<string> commands = new List<string>() { "order", "view" };
                int command = OptionsInput("Options (-1 to logout): ", commands, true);

                if (command == -1) break;
                else if (command == 0) { OrderLoop(); }
                else if (command == 1) { ViewLoop(); }
            }

            Console.WriteLine("Logging out...");
        }
        #endregion

        #region Ordering
        public void OrderLoop()
        {

            if (!ChooseLocation()) return;

            var store = GetStore();
            List<Pizza> pizzas = new List<Pizza>();

            while (true)
            {
                if (store.MaxPizza != null) Console.WriteLine($"Cannot order more than {store.MaxPizza} pizzas");
                if (store.MaxPrice != null) Console.WriteLine($"Order cannot exceed {store.MaxPrice}$");

                List<string> commands = new List<string>() { "new", "change amount", "remove", "view", "done" };
                int command = OptionsInput("Order Menu (-1 to cancel): ", commands, true);

                if (command == -1 && ConfirmCancel()) { return; }
                else if (command == 0) { NewPizza(pizzas); }
                else if (command == 1) { ChangePizzaAmount(pizzas); }
                else if (command == 2) { RemovePizza(pizzas); }
                else if (command == 3) { ViewPizza(pizzas); }
                else if (command == 4 && ConfirmOrder(pizzas)) break;
            }

            MakeOrder(pizzas);
        }
        public bool ChooseLocation()
        {
            while (true)
            {
                ViewRestaurants(false);
                Console.WriteLine("Warning: you cannot order from two locations on the same day!");
                int id = IntInput("Please enter a Restaurant ID to order from (-1 to cancel): ", -1, int.MaxValue);
                if (id == -1) return false;
                else if (systemLogic.GetRestaurant(id) != null)
                {
                    restaurantID = id;
                    return true;
                }
                else
                {
                    Console.WriteLine("Unrecognized Restaurant");
                }
            }
        }
        public bool ConfirmCancel()
        {
            Console.Write("Cancel order? y/n: ");
            char r = Console.ReadLine().ToLower()[0];
            return r == 'y';
        }
        public bool ConfirmOrder(List<Pizza> pizzas)
        {
            ViewPizza(pizzas);
            Console.Write("Is this order correct? y/n: ");
            char r = Console.ReadLine().ToLower()[0];
            return r == 'y';
        }
        public void MakeOrder(List<Pizza> pizzas)
        {
            Console.WriteLine(systemLogic.MakeOrder(username, restaurantID, pizzas));
        }

        public void NewPizza(List<Pizza> pizzas)
        {
            var store = GetStore();
            var cList = GetPartList<Data.PizzaBox.DataModel.PizzaCrust>();
            var sList = GetPartList<Data.PizzaBox.DataModel.PizzaSize>();
            var pList = GetPartList<Data.PizzaBox.DataModel.PizzaPreset>();
            var tList = GetPartList<Data.PizzaBox.DataModel.PizzaTopping>();

            int crust = cList[OptionsInput("Please Enter Crust: ", cList.Select(x => x.name).ToList(), false)].id;
            int size = sList[OptionsInput("Please Enter Size: ", sList.Select(x => x.name).ToList(), false)].id;
            int? preset = null;
            List<int> toppings = new List<int>();

            int useCustom = OptionsInput("Preset or custom toppings?: ", new List<string>() { "presets", "custom" }, false);

            if (useCustom == 0)
            {
                preset = OptionsInput("Presets (-1 to use custom toppings): ", pList.Select(x => x.name).ToList(), true);
                if (preset == -1) { useCustom = 1; preset = null; }
                else { preset = pList[(int)preset].id; toppings = PresetToppings((int)preset); }
            }
            if (useCustom == 1) //intentional to cancel presets
            {
                int? maxToppings = store.MaxToppings;
                Console.WriteLine($"Enter '-1' to stop adding toppings ");
                if (maxToppings != null) Console.Write($"({maxToppings} max)");
                Console.WriteLine();

                for (int i = 0; maxToppings == null || i < maxToppings; i++)
                {
                    int t = tList[OptionsInput("Toppings (-1 to stop): ", tList.Select(x => x.name).ToList(), true)].id;
                    if (t == -1) break;
                    toppings.Add(t);
                }
            }

            Logic.PizzaBox.DataClasses.Pizza pizza = new Logic.PizzaBox.DataClasses.Pizza(size, crust, preset, toppings);

            Console.WriteLine($"Pizza will cost {pizza.Price}$");

            int count = IntInput("How many of these would you like? ", 1, int.MaxValue);
            pizza.Amount = count;
            pizzas.Add(pizza);
        }
        public void ChangePizzaAmount(List<Pizza> pizzas)
        {
            ViewPizza(pizzas);

            int index = IntInput("Please enter the index of the pizza to change (-1 to cancel): ", -1, pizzas.Count - 1);
            if (index == -1) return;
            Pizza p = pizzas[index];

            int count = IntInput("How many of these would you prefer? ", 1, int.MaxValue);
            p.Amount = count;
        }
        public void RemovePizza(List<Pizza> pizzas)
        {
            ViewPizza(pizzas);
            int index = IntInput("Please enter the index of the pizzas to remove (-1 to cancel): ", -1, pizzas.Count - 1);
            if (index == -1) return;
            pizzas.RemoveAt(index);
        }
        public void ViewPizza(List<Pizza> pizzas)
        {
            double totalPrice = 0;
            int totalPizzas = 0;
            foreach (var p in pizzas)
            {
                Console.WriteLine(p.AsString());
                totalPrice += p.Price * p.Amount;
                totalPizzas += p.Amount;
            }
            Console.WriteLine($"Total Price: {totalPrice}$ -> {totalPizzas} pizzas");
        }
        #endregion

        #region Viewing
        public void ViewLoop()
        {
            while (true)
            {
                List<string> commands = new List<string>() { "locations", "orders" };
                int command = OptionsInput("View Menu (-1 to exit): ", commands, true);

                if (command == -1) break;
                else if (command == 0) { ViewRestaurants(true); }
                else if (command == 1) { ViewOrders(); }
            }
        }

        public void ViewRestaurants(bool detailed)
        {
            Console.Write(systemLogic.GetRestaurants());

            if (detailed)
            {
                int id = IntInput("Enter id to view or -1 to cancel: ", -1, int.MaxValue);
                if (id == -1) return;
                else Console.Write(systemLogic.GetRestaurant(id));
            }
        }
        public void ViewOrders()
        {
            Console.Write(systemLogic.GetUserOrders(username));

            int id = IntInput("Enter id to view or -1 to cancel: ", -1, int.MaxValue);

            if (id == -1)
            {
                return;
            }
            else
            {
                Console.Write(systemLogic.GetOrder(id));
            }
        }
        #endregion

        #region BasicInput
        public string StringInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
        public int IntInput(string message, int min, int max)
        {
            while (true)
            {
                Console.Write(message);
                try
                {
                    int i = int.Parse(Console.ReadLine());
                    if (min <= i && i <= max) return i;
                    else if (min <= i) Console.WriteLine("Too high!");
                    else if (i <= max) Console.WriteLine("Too low!");
                }
                catch (Exception e) { }
            }
        }
        public int OptionsInput(string message, List<string> options, bool allowstop)
        {
            int min = 0, max = options.Count - 1;
            if (allowstop) min = -1;

            while (true)
            {
                Console.WriteLine(message);
                for (int i = 0; i < options.Count; i++)
                {
                    Console.Write($"[{i}]:{options[i]}\n");
                }
                try
                {
                    int i = int.Parse(Console.ReadLine());
                    if (min <= i && i <= max) return i;
                    else if (min <= i) Console.WriteLine("Too high!");
                    else if (i <= max) Console.WriteLine("Too low!");

                }
                catch (Exception e) { Console.WriteLine("Invalid Input!"); }
            }
        }
        #endregion

        #region DB Access
        public Data.PizzaBox.DataModel.Store GetStore ()
        {
            return ExternalDB.GetStore(restaurantID);
        }
        public List<(int id, string name)> GetPartList<T> () where T : Data.PizzaBox.DataModel.PizzaPart
        {
            List<(int id, string name)> list = new List<(int id, string name)>();
            foreach (var p in ExternalDB.FindPizzaPartsList<T>(ExternalDB.GetStore(restaurantID).Id))
            {
                list.Add((p.Id, p.Name));
            }
            return list;
        }
        public List<int> PresetToppings (int pID)
        {
            List<int> list = new List<int>();
            list = ExternalDB.GetPresetToppings(pID).Select(x => x.Id).ToList();
            return list;
        }
        #endregion
    }
}
