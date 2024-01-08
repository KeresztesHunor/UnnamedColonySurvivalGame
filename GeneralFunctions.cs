using System;
using System.Collections.Generic;

namespace UnnamedColonySurvivalGame
{
    static class GeneralFunctions
    {
        public static void ClampWithMinValue<T>(ref T value, T minValue) where T : IComparable<T>
        {
            value = ClampWithMinValue(value, minValue);
        }

        public static T ClampWithMinValue<T>(T value, T minValue) where T : IComparable<T>
        {
            return value.CompareTo(minValue) < 0 ? minValue : value;
        }

        public static void ClampWithMaxValue<T>(ref T value, T maxValue) where T : IComparable<T>
        {
            value = ClampWithMaxValue(value, maxValue);
        }

        public static T ClampWithMaxValue<T>(T value, T maxValue) where T : IComparable<T>
        {
            return value.CompareTo(maxValue) > 0 ? maxValue : value;
        }
    }
}
