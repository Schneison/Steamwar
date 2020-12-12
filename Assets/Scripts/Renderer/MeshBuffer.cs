using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Steamwar.Utils;

namespace Steamwar.Renderer
{
    public class MeshBuffer
    {
        public Vector3[] vertices = new Vector3[0];
        public int[] triangles = new int[0];
        public Vector2[] uvs = new Vector2[0];
        public Vector3 currentPosition = Vector3.zero;

        /// <summary>
        /// Adds a series of triangles to the buffer.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="uvs"></param>
        public void AddFace(Vector3[] vertices, int[] triangles, Vector2[] uvs)
        {
            for(int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i] + currentPosition;
            }
            int vertexStart = this.vertices.Length;
            int trianglesStart = this.triangles.Length;
            Array.Resize(ref this.vertices, this.vertices.Length + vertices.Length);
            Array.Copy(vertices, 0, this.vertices, this.vertices.Length - vertices.Length, vertices.Length);
            Array.Resize(ref this.uvs, this.uvs.Length + uvs.Length);
            Array.Copy(uvs, 0, this.uvs, this.uvs.Length - uvs.Length, uvs.Length);
            Array.Resize(ref this.triangles, this.triangles.Length + triangles.Length);
            //Array.Copy(triangles, 0, this.triangles, this.triangles.Length - triangles.Length, triangles.Length);
            for(int i = 0;i < triangles.Length; i++)
            {
                this.triangles[trianglesStart + i] = vertexStart + triangles[i];
            }
        }

        public void AddCube(Vector3 position, Vector2[] uvs, RenderInfo info)
        {
            currentPosition = position;
            if (info.IsFaceVisible(RenderFace.FRONT)) 
            {
                //Front
                AddFace(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0) },
                   new int[] { 0, 1, 2, 2, 1, 3 },
                   new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) });
            }
            if (info.IsFaceVisible(RenderFace.BACK))
            {
                //Back
                AddFace(new Vector3[] { new Vector3(1, 1, 1), new Vector3(0, 1, 1), new Vector3(1, 0, 1), new Vector3(0, 0, 1) },
                new int[] { 0, 1, 2, 2, 1, 3 },
                new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) });
            }
            if (info.IsFaceVisible(RenderFace.TOP))
            {
                //Top
                AddFace(new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 0) },
                new int[] { 0, 1, 2, 0, 2, 3 },
                new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) });
            }
            if (info.IsFaceVisible(RenderFace.BOTTOM))
            {
                //Bottom
                AddFace(new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 0, 1), new Vector3(0, 0, 1), new Vector3(1, 0, 0) },
                new int[] { 0, 1, 2, 0, 3, 1 },
                new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) });
            }
            if (info.IsFaceVisible(RenderFace.LEFT))
            {
                //Left
                AddFace(new Vector3[] { new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 1), new Vector3(1, 1, 1) },
                new int[] { 0, 1, 2, 1, 3, 2 },
                new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) });
            }
            if (info.IsFaceVisible(RenderFace.RIGHT))
            {
                //Right
                AddFace(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 1, 0) },
                new int[] { 0, 1, 2, 0, 2, 3 },
                new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) });
            }
            currentPosition = Vector3.zero;
            /*int[] triangles = new int[] { 0, 1, 2, 2, 1, 3,//Front
                                          7, 5, 6, 6, 5, 4,//Back
                                          1, 5, 7, 1, 7, 3,//Top
                                          0, 6, 4, 0, 2, 6,//Bottom
                                          2, 3, 6, 3, 7, 6,//Left
                                          0, 4, 5, 0, 5, 1//Right

            };
            Vector3[] verticles = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0),
                                                  new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(1, 0, 1), new Vector3(1, 1, 1)};*/
        }

        public void FillMesh(Mesh mesh)
        {
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
        }

        public void Merge(MeshBuffer buffer)
        {
            int triangleLength = this.triangles.Length;
            Array.Resize(ref this.triangles, this.triangles.Length + buffer.triangles.Length);
            for (int i = 0; i < buffer.triangles.Length; i++)
            {
                this.triangles[triangleLength + i] = buffer.triangles[i] + vertices.Length;
            }
            Array.Resize(ref this.vertices, this.vertices.Length + buffer.vertices.Length);
            Array.Copy(vertices, 0, this.vertices, 0, buffer.vertices.Length);
            Array.Resize(ref this.uvs, this.uvs.Length + buffer.uvs.Length);
            Array.Copy(uvs, 0, this.uvs, 0, buffer.uvs.Length);
        }

        public (Vector3[] vertices, int[] triangles, Vector2[] uvs) GetMeshData()
        {
            return (this.vertices, this.triangles, this.uvs);
        }
    }
}
