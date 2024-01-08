using System;
using System.Collections.Generic;

namespace UnnamedColonySurvivalGame
{
    abstract class Component
    {
        GameObject hostGameObject;
        protected GameObject HostGameObject
        {
            get => hostGameObject;
            set
            {
                if (value && value != hostGameObject)
                {
                    hostGameObject.componentList.Remove(this);
                    hostGameObject = value;
                    hostGameObject.componentList.Add(this);
                }
            }
        }

        protected Component(NotNullable<GameObject> hostGameObject)
        {
            this.hostGameObject = hostGameObject;
        }
    }
}
