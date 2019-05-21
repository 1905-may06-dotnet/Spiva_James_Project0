using System;
using Data.PizzaBox;
using Logic.PizzaBox;
using Client.PizzaBox;

namespace PizzaBox
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemLogic sys = new SystemLogic();
            CUI ui = new CUI(sys);
            ui.Run();
        }
    }
}
