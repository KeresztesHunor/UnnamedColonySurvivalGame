using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace UnnamedColonySurvivalGame
{
    class Texture : GraphicsData
    {
        public Texture(string filePath) : base(GL.GenTexture())
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, ID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult texture = ImageResult.FromStream(File.OpenRead("../../../Textures/" + filePath), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, texture.Data);
            Unbind();
        }

        public override void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, ID);
        }

        public override void Delete()
        {
            GL.DeleteTexture(ID);
        }

        protected override void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
