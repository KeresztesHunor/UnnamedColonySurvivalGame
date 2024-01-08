using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace UnnamedColonySurvivalGame
{
    class VertexBufferObjectVec2 : VertexBufferObject<Vector2>
    {
        public VertexBufferObjectVec2(Vector2[] data) : base(data, Vector2.SizeInBytes)
        {

        }
    }
}
