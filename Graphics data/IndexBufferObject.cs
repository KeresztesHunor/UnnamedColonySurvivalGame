using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace UnnamedColonySurvivalGame
{
    class IndexBufferObject : VertexBufferObject<uint>
    {
        public IndexBufferObject(uint[] data) : base(data, sizeof(uint), BufferTarget.ElementArrayBuffer)
        {

        }
    }
}
