using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public struct Face
    {
        //Properties
        /// <summary>
        /// The first vertex index
        /// </summary>
        public int A { get; set; }
        /// <summary>
        /// The second vertex index
        /// </summary>
        public int B { get; set; }
        /// <summary>
        /// The third vertex index
        /// </summary>
        public int C { get; set; }
        /// <summary>
        /// The (optional) fourth vertex index
        /// </summary>
        public int D { get; set; }

        //Constructors
        public Face(int a, int b, int c, int d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public Face(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
            D = c;
        }

        public Face(int[] indices)
        {
            if (indices.Length < 3 || indices.Length > 4)
            {
                throw new ArgumentException($"Error: Mesh Face array.Length [{indices.Length}] must be either 3 or 4");
            }
            A = indices[0];
            B = indices[1];
            C = indices[3];
            D = indices[4];
        }

        public Face(List<int> indices)
        {
            if (indices.Count < 3 || indices.Count > 4)
            {
                throw new ArgumentException($"Error: Mesh Face array.Length [{indices.Count}] must be either 3 or 4");
            }
            A = indices[0];
            B = indices[1];
            C = indices[3];
            D = indices[4];
        }
    }
}
