using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using Logic.PizzaBox;
using Logic.PizzaBox.DataClasses;

namespace Testing.PizzaBox
{
    [TestClass]
    public class UnitTest
    {
        SystemLogic sysLogic = new SystemLogic();

        [TestMethod]
        public void TestUser ()
        {
            //expected
            bool login1 = false;
            bool create = true;
            bool login2 = true;
            //actual
            bool tlogin1 = sysLogic.Login("TestUser1", "TestPW");
            bool tcreate = sysLogic.NewUser("TestUser1", "TestPW");
            bool tlogin2 = sysLogic.Login("TestUser1", "TestPW");
            //compare
            Assert.IsTrue(login1 == tlogin1);
            Assert.IsTrue(create == tcreate);
            Assert.IsTrue(login2 == tlogin2);
        }

        public void TestRestaurantView ()
        {
            //expected
            string view = "4: Pop J: 321 someother street  Arlington TX, (US) 12348" +
                    "\n    Open: 6:00 AM - 22:00 PM" +
                    "\nInventory: " +
                    "\n  Crusts: " +
                    "\n    Original: 100" +
                    "\n    Thin: 100" +
                    "\n    Pan: 100" +
                    "\n    Gluten Free: 100" +
                    "\n  Toppings: " +
                    "\n    Anchovies: 100" +
                    "\n    Bacon: 100" +
                    "\n    Banana Peppers: 100" +
                    "\n    Beef: 100" +
                    "\n    Black Olives: 100" +
                    "\n    Canadian Bacon: 100" +
                    "\n    Chicken: 100" +
                    "\n    Green Olive: 100" +
                    "\n    Green Pepper: 100" +
                    "\n    Italian Sausage: 100" +
                    "\n    Jalapeno: 100" +
                    "\n    Mushroom: 100" +
                    "\n    Onion: 100" +
                    "\n    Pineapple: 100" +
                    "\n    Pepperoni: 100" +
                    "\n    Sausage: 100" +
                    "\n    Roma Tomatoes: 100" +
                    "\nOrders: " +
                    "\nSale Summary: 0$ across 0 orders with 0 pizzas\n";
            //actual
            string tview = sysLogic.GetRestaurant(4);
            //compare
            Assert.AreEqual<string>(view, tview);
        }

        public void TestOrder ()
        {
            //expected
            string order1 = "Pizzas have been ordered!";
            string order2 = "Unable to create order. You already ordered from a different location today.";
            string order3 = "Unable to create order. You already ordered recently.";
            //actual
            string torder1 = sysLogic.MakeOrder("TestUser1", 2, new List<Pizza>() { new Pizza(1, 1, null, new List<int>()) { Amount = 2 } });
            string torder2 = sysLogic.MakeOrder("TestUser1", 1, new List<Pizza>() { new Pizza(1, 1, null, new List<int>()) { Amount = 2 } });
            string torder3 = sysLogic.MakeOrder("TestUser1", 2, new List<Pizza>() { new Pizza(1, 1, null, new List<int>()) { Amount = 1000 } });
            //compare
            Assert.AreEqual<string>(order1, torder1);
            Assert.AreEqual<string>(order2, torder2);
            Assert.AreEqual<string>(order3, torder3);
        }

        public void TestOrderView()
        {
            //expected
            string oview = "(3) 20$ - 5/21/2019 8:37:02 PM: TestUser1 - 2: Pizza The Hut: 123 someother street  Arlington TX, (US) 12346" +
                    "\n    Open: 0:00 AM - 0:00 AM: " +
                    "\n  2x 10$: Small - Pan crust, toppings: none\n";
            //actual
            string toview = sysLogic.GetOrder(3);
            //compare
            Assert.AreEqual<string>(oview, toview);
        }
    }
}
