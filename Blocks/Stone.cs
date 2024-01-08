using System;
using System.Collections.Generic;

namespace UnnamedColonySurvivalGame
{
    class Stone : SolidBlock
    {
        public static Stone Instance
        {
            get => getInstance();
        }

        static Func<Stone> getInstance = () => {
            Stone instance = new Stone();
            getInstance = () => instance;
            return instance;
        };

        Stone() : base()
        {

        }
    }
}
