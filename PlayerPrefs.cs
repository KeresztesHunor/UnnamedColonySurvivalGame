using System;
using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace UnnamedColonySurvivalGame
{
    static class PlayerPrefs
    {
        public static float mouseSensitivity = 4f;

        public static readonly IReadOnlyDictionary<Inputs, Enum> keyboardInputs = new Dictionary<Inputs, Enum>()
        {
            { Inputs.Forward,     Keys.W             },
            { Inputs.Backward,    Keys.S             },
            { Inputs.Right,       Keys.D             },
            { Inputs.Left,        Keys.A             },
            { Inputs.Down,        Keys.LeftShift     },
            { Inputs.Up,          Keys.Space         },
            { Inputs.MouseLeft,   MouseButton.Left   },
            { Inputs.MouseRight,  MouseButton.Right  },
            { Inputs.MouseMiddle, MouseButton.Middle }
        };
    }
}
