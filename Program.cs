using System;
using System.Collections.Generic;

namespace UnnamedColonySurvivalGame
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = Game.CreateInstance(1280, 720))
            {
                game.Run();
            }
        }
    }
}
