using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using LibNoise;
using LibNoise.Primitive;

namespace UnnamedColonySurvivalGame
{
    class Chunk : Mesh
    {
        VertexArrayObject vao;
        VertexBufferObjectVec3 xyzVBO;
        VertexBufferObjectVec2 uvVBO;
        IndexBufferObject ibo;
        Texture texture;
        Block[] blocks;
        List<uint> indices;
        static readonly SimplexPerlin perlinNoise = new SimplexPerlin(0, NoiseQuality.Fast);

        public Chunk(NotNullable<GameObject> hostGameObject) : base(hostGameObject)
        {
            texture = new Texture("TextureStone256.png");
            blocks = new Block[ushort.MaxValue + 1];
            IterateOverChunkBlocks((byte x, byte y, byte z) => {
                blocks[ConvertCoordsToIndex(x, y, z)] = y < perlinNoise.GetValue((HostGameObject.Position.X + x) * 0.0625f, (HostGameObject.Position.Z + z) * 0.0625f) * 4 + 64 ? Stone.Instance : Air.Instance;
            });
            GenerateChunkMesh();
        }

        public Block this[byte x, byte y, byte z]
        {
            get => blocks[ConvertCoordsToIndex(x, y, z)];
            set
            {
                blocks[ConvertCoordsToIndex(x, y, z)] = value;
                GenerateChunkMesh();
            }
        }

        protected override void Render(ShaderProgram shaderProgram)
        {
            vao.Bind();
            ibo.Bind();
            texture.Bind();
            shaderProgram.Bind();
            GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
        }

        void GenerateChunkMesh()
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            indices = new List<uint>();
            uint indexOffset = 0;
            for (short shortY = 0; shortY < 256; shortY++)
            {
                byte y = (byte)shortY;
                int yPlus1 = y + 1;
                int yMinus1 = y - 1;
                for (byte z = 0; z < 16; z++)
                {
                    int zPlus1 = z + 1;
                    int zMinus1 = z - 1;
                    for (byte x = 0; x < 16; x++)
                    {
                        int xPlus1 = x + 1;
                        int xMinus1 = x - 1;
                        if (this[x, y, z] is SolidBlock)
                        {
                            if (zMinus1 < 0 || IsTransparentBlock(x, y, (byte)zMinus1))
                            {
                                AddVertices(
                                    new Vector3(x, y, z),
                                    new Vector3(xPlus1, y, z),
                                    new Vector3(x, yPlus1, z),
                                    new Vector3(xPlus1, yPlus1, z)
                                );
                            }
                            if (zPlus1 > 15 || IsTransparentBlock(x, y, (byte)zPlus1))
                            {
                                AddVertices(
                                    new Vector3(xPlus1, y, zPlus1),
                                    new Vector3(x, y, zPlus1),
                                    new Vector3(xPlus1, yPlus1, zPlus1),
                                    new Vector3(x, yPlus1, zPlus1)
                                );
                            }
                            if (xMinus1 < 0 || IsTransparentBlock((byte)xMinus1, y, z))
                            {
                                AddVertices(
                                    new Vector3(x, y, zPlus1),
                                    new Vector3(x, y, z),
                                    new Vector3(x, yPlus1, zPlus1),
                                    new Vector3(x, yPlus1, z)
                                );
                            }
                            if (xPlus1 > 15 || IsTransparentBlock((byte)xPlus1, y, z))
                            {
                                AddVertices(
                                    new Vector3(xPlus1, y, z),
                                    new Vector3(xPlus1, y, zPlus1),
                                    new Vector3(xPlus1, yPlus1, z),
                                    new Vector3(xPlus1, yPlus1, zPlus1)
                                );
                            }
                            if (yMinus1 < 0 || IsTransparentBlock(x, (byte)yMinus1, z))
                            {
                                AddVertices(
                                    new Vector3(x, y, zPlus1),
                                    new Vector3(xPlus1, y, zPlus1),
                                    new Vector3(x, y, z),
                                    new Vector3(xPlus1, y, z)
                                );
                            }
                            if (yPlus1 > 255 || IsTransparentBlock(x, (byte)yPlus1, z))
                            {
                                AddVertices(
                                    new Vector3(x, yPlus1, z),
                                    new Vector3(xPlus1, yPlus1, z),
                                    new Vector3(x, yPlus1, zPlus1),
                                    new Vector3(xPlus1, yPlus1, zPlus1)
                                );
                            }
                        }
                    }
                }
            }
            vao = new VertexArrayObject();
            xyzVBO = new VertexBufferObjectVec3(vertices.ToArray());
            uvVBO = new VertexBufferObjectVec2(uvs.ToArray());
            ibo = new IndexBufferObject(indices.ToArray());
            vao.Bind();
            xyzVBO.Bind();
            vao.Link(0, 3, xyzVBO);
            uvVBO.Bind();
            vao.Link(1, 2, uvVBO);

            void AddVertices(Vector3 vertexBottomLeft, Vector3 vertexBottomRight, Vector3 vertexTopLeft, Vector3 vertexTopRight)
            {
                vertices.Add(HostGameObject.Position + vertexBottomLeft);
                vertices.Add(HostGameObject.Position + vertexBottomRight);
                vertices.Add(HostGameObject.Position + vertexTopLeft);
                vertices.Add(HostGameObject.Position + vertexTopRight);
                uint bottomRight = indexOffset + 1;
                uint topLeft = indexOffset + 2;
                indices.Add(indexOffset); // bottom left
                indices.Add(bottomRight);
                indices.Add(topLeft);
                indices.Add(bottomRight);
                indices.Add(indexOffset + 3); // top right
                indices.Add(topLeft);
                indexOffset += 4;
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(1, 0));
                uvs.Add(new Vector2(0, 1));
                uvs.Add(new Vector2(1, 1));
            }
        }

        void IterateOverChunkBlocks(Action<byte, byte, byte> callback)
        {
            for (short shortY = 0; shortY < 256; shortY++)
            {
                byte y = (byte)shortY;
                for (byte z = 0; z < 16; z++)
                {
                    for (byte x = 0; x < 16; x++)
                    {
                        callback(x, y, z);
                    }
                }
            }
        }

        bool IsTransparentBlock(byte x, byte y, byte z) => this[x, y, z] is TransparentBlock;

        static ushort ConvertCoordsToIndex(byte x, byte y, byte z)
        {
            if (x > 15)
            {
                throw new IndexOutOfRangeException(ErrorMessage('x'));
            }
            else if (z > 15)
            {
                throw new IndexOutOfRangeException(ErrorMessage('z'));
            }
            return (ushort)((y << 8) | (z << 4) | x);

            string ErrorMessage(char axisName) => $"The {axisName} coordinate must be less than 16";
        }
    }
}
