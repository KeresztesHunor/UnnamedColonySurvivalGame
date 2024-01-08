using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace UnnamedColonySurvivalGame
{
    abstract class VertexBufferObject<T> : GraphicsData where T : struct, IEquatable<T>, IFormattable
    {
        BufferTarget bufferTarget { get; }

        protected VertexBufferObject(T[] data, int sizeInBytes, BufferTarget bufferTarget = BufferTarget.ArrayBuffer) : base(GL.GenBuffer())
        {
            this.bufferTarget = bufferTarget;
            GL.BindBuffer(bufferTarget, ID);
            GL.BufferData(bufferTarget, data.Length * sizeInBytes, data, BufferUsageHint.StaticDraw);
            Unbind();
        }

        public override void Bind()
        {
            GL.BindBuffer(bufferTarget, ID);
        }

        public override void Delete()
        {
            GL.DeleteBuffer(ID);
        }

        protected override void Unbind()
        {
            GL.BindBuffer(bufferTarget, 0);
        }
    }
}
