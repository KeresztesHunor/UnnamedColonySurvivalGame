using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace UnnamedColonySurvivalGame
{
    class VertexBufferObjectVec3 : VertexBufferObject<Vector3>
    {
        public VertexBufferObjectVec3(Vector3[] data) : base(data, Vector3.SizeInBytes)
        {

        }
    }
}
