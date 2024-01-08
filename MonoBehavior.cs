using System;
using System.Collections.Generic;

namespace UnnamedColonySurvivalGame
{
    abstract class MonoBehavior : Component
    {
        protected MonoBehavior(NotNullable<GameObject> hostGameObject) : base(hostGameObject)
        {
            Game.UpdateEvent += Update;
        }

        public abstract void Update();
    }
}
