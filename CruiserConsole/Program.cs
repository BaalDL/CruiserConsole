using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CruiserConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            CruiserGame game = new CruiserGame();
            game.initialize();
            game.startGame();

            Console.Write("Press any key to leave.");
            Console.ReadKey();
        }

    }
}
