using System;
using Data.PizzaBox;
using Logic.PizzaBox;
using Client.PizzaBox;
using Testing.PizzaBox;

namespace PizzaBox
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
            //Test();
        }

        static void Run ()
        {
            SystemLogic sys = new SystemLogic();
            CUI ui = new CUI(sys);
            ui.Run();
        }

        static void Test ()
        {
            UnitTest tester = new UnitTest();

            //tester.TestUser();
            Console.WriteLine("User Test Successful");

            //tester.TestRestaurantView();
            Console.WriteLine("Restaurant View Test Successful");

            //tester.TestOrder();
            Console.WriteLine("Order Test Successful");

            //tester.TestOrderView();
            Console.WriteLine("Order View Test Successful");
        }
    }
}
