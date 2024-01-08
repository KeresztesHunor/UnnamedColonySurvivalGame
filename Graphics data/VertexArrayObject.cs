using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace UnnamedColonySurvivalGame
{
    class VertexArrayObject : GraphicsData
    {
        public VertexArrayObject() : base(GL.GenVertexArray())
        {
            
        }

        public void Link<T>(int index, int size, VertexBufferObject<T> vbo) where T : struct, IEquatable<T>, IFormattable
        {
            Bind();
            vbo.Bind();
            GL.VertexAttribPointer(index, size, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(index);
            Unbind();
        }

        public override void Bind()
        {
            GL.BindVertexArray(ID);
        }

        public override void Delete()
        {
            GL.DeleteVertexArray(ID);
        }

        protected override void Unbind()
        {
            GL.BindVertexArray(0);
        }
    }
}
