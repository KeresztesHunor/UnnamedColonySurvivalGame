using System;
using System.Collections.Generic;

namespace UnnamedColonySurvivalGame
{
    class Air : TransparentBlock
    {
        public static Air Instance
        {
            get => getInstance();
        }

        static Func<Air> getInstance = () => {
            Air instance = new Air();
            getInstance = () => instance;
            return instance;
        };

        Air() : base()
        {

        }
    }
}
