using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace UnnamedColonySurvivalGame
{
    class ShaderProgram : GraphicsData
    {
        public ShaderProgram(string vertexShaderFilePath, string fragmentShaderFilePath) : base(GL.CreateProgram())
        {
            int vertexShader = SetUpShader(ShaderType.VertexShader, vertexShaderFilePath);
            int fragmentShader = SetUpShader(ShaderType.FragmentShader, fragmentShaderFilePath);
            GL.LinkProgram(ID);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            Unbind();
        }

        public override void Bind()
        {
            GL.UseProgram(ID);
        }

        public override void Delete()
        {
            GL.DeleteProgram(ID);
        }

        protected override void Unbind()
        {
            GL.UseProgram(0);
        }

        int SetUpShader(ShaderType shaderType, string shaderFilePath)
        {
            int shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, LoadShaderSource(shaderFilePath));
            GL.CompileShader(shader);
            GL.AttachShader(ID, shader);
            return shader;
        }

        static string LoadShaderSource(string filePath)
        {
            string shaderSource;
            try
            {
                using (StreamReader sr = new StreamReader("../../../Shaders/" + filePath))
                {
                    shaderSource = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load shader: " + ex.Message);
            }
            return shaderSource;
        }
    }
}
