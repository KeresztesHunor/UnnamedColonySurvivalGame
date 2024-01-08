using System;
using System.Collections.Generic;

namespace UnnamedColonySurvivalGame
{
    abstract class Mesh : Component
    {
        protected Mesh(NotNullable<GameObject> hostGameObject) : base(hostGameObject)
        {
            Game.MeshRenderEvent += Render;
        }

        protected abstract void Render(ShaderProgram shaderProgram);
    }
}
