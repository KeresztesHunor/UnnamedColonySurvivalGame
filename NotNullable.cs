using System;
using System.Collections.Generic;

namespace UnnamedColonySurvivalGame
{
    struct NotNullable<T> where T : class
    {
        T value { get; }
        public T Value
        {
            get => value ?? throw new NullReferenceException();
        }

        public NotNullable(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.value = value;
        }

        public static implicit operator T(NotNullable<T> wrapper) => wrapper.Value;
    }
}
