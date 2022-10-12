using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public struct Mesh
    {
        //Properties
        /// <summary>
        /// List of Mesh Vertices
        /// </summary>
        public List<Pt3d> Vertices;

        /// <summary>
        /// List of Mesh Vertices Colors
        /// </summary>
        public List<Color> VertexColors;
        /// <summary>
        /// List of Mesh Faces
        /// </summary>
        public List<Face> Faces;
        /// <summary>
        /// List of Mesh 
        /// </summary>
        public List<Vec3d> FaceNormals;
        /// <summary>
        /// True if the mesh is closed
        /// </summary>
        public bool IsClosed { get; set; }
        /// <summary>
        /// True if the mesh is closed and all normals point out and are non-manifold
        /// </summary>
        public bool IsSolid { get; set; }


        //Constructors

        public Mesh(List<Pt3d> vertices, List<Face> faces, List<Vec3d> faceNormals)
        {
            IsClosed = false;
            IsSolid = false;
            Vertices = vertices;
            Faces = faces;
            List<Color> vertexColors = new List<Color>();
            vertexColors.Add(Color.White);
            Data.MatchLength(vertexColors, vertices.Count, "Color");
            VertexColors = vertexColors;
            FaceNormals = faceNormals;
            IsClosed = IsMeshClosed(this);
            IsSolid = IsMeshSolid(this);
        }

        public Mesh(List<Pt3d> vertices, List<Color> vertexColors, List<Face> faces, List<Vec3d> faceNormals)
        {
            IsClosed = false;
            IsSolid = false;
            Vertices = vertices;
            Faces = faces;
            VertexColors = vertexColors;
            FaceNormals = faceNormals;
            IsClosed = IsMeshClosed(this);
            IsSolid = IsMeshSolid(this);
        }

        //Methods

        public static bool IsMeshClosed(Mesh mesh)
        {
            return true;
        }

        public static bool IsMeshSolid(Mesh mesh)
        {
            return true;
        }

        public static Mesh Construct4x(Pt3d[] pts0, Pt3d[] pts1, Pt3d[] pts2, Pt3d[] pts3)
        {
            if (pts0.Length != pts1.Length || pts1.Length != pts2.Length || pts2.Length != pts3.Length)
            {
                throw new ArgumentException($"Error: Mesh 4x Inputs must be equal length [{pts0.Length},{pts1.Length},{pts2.Length},{pts3.Length}]");
            }
            List<Pt3d> vertices = new List<Pt3d>();
            List<Face> faces = new List<Face>();
            List<Vec3d> faceNormals = new List<Vec3d>();
            int length = pts0.Length;
            vertices.AddRange(pts0);
            vertices.AddRange(pts1);
            vertices.AddRange(pts2);
            vertices.AddRange(pts3);

            for (int i = 0; i < 3; i++)             
            {
                if (i < 2)
                {
                    int k = length * i;
                    for (int j = 0; j < length; j++)
                    {
                        int a = k + j;
                        int b = k + j + 1;
                        int c = k + length + j + 1;
                        int d = k + length + j;
                        faces.Add(new Face(a, b, c, d));
                        faceNormals.Add(Vec3d.PerpTo(vertices[a], vertices[b], vertices[c]));
                    }
                }
                else
                {
                    int k = length * 2;
                    for (int j = 0; j < length - 1; j++)
                    {
                        int a = k + j;
                        int b = k + j + 1;
                        int c = k + length + j + 1;
                        int d = k + length + j;
                        faces.Add(new Face(a, b, c, d));
                        faceNormals.Add(Vec3d.PerpTo(vertices[a], vertices[b], vertices[c]));
                    }
                }
            }
            return new Mesh(vertices, faces, faceNormals);
        }

        public static Mesh Construct2x(Pt3d[] pts0, Pt3d[] pts1)
        {
            if (pts0.Length != pts1.Length)
            {
                throw new ArgumentException($"Error: Mesh 4x Inputs must be equal length [{pts0.Length},{pts1.Length}]");
            }
            List<Pt3d> vertices = new List<Pt3d>();
            List<Face> faces = new List<Face>();
            List<Vec3d> faceNormals = new List<Vec3d>();
            int length = pts0.Length;
            Pt3d[][] pts = new Pt3d[][] { pts0, pts1 };

            for (int i = 0; i < 2; i++)
            {
                int j = length * i;
                vertices.AddRange(pts[i]);
                int a = j + i;
                int b = j + i + 1;
                int c = j + length + i + 1;
                int d = j + length + i;
                faces.Add(new Face(a, b, c, d));
                faceNormals.Add(Vec3d.PerpTo(vertices[a], vertices[b], vertices[c]));
            }


            return new Mesh(vertices, faces, faceNormals);
        }

    }   
}
