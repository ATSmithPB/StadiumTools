using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public struct MeshFace
    {
        //Properties
        /// <summary>
        /// The array of Mesh Face Indices
        /// </summary>
        public double[] Indices { get; set; }

        /// <summary>
        /// True is the Face is a Quad. False if Tri
        /// </summary>
        public bool IsQuad { get; set; }
    }
}
