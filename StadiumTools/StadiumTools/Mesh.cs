using System;
using System.Collections.Generic;
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
        public List<MeshFace> Faces;
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
        

        //Methods
    }
}
