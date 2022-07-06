using System.Collections.Generic;

namespace StadiumTools
{
    /// <summary>
    /// Represents a point in 2D space (h,v)
    /// </summary>
    public struct Pt2d
    {
        //Properties
        /// <summary>
        /// Horizontal distance from origin
        /// </summary>
        public double H { get; set; }
        /// <summary>
        /// Vertical distance from origin
        /// </summary>
        public double V { get; set; }

        //Constructors
        /// <summary>
        /// Construct a Pt2d from an existing Pt2d object
        /// </summary>
        /// <param name="pt"></param>
        public Pt2d(Pt2d pt)
        {
            this.H = pt.H;
            this.V = pt.V;
        }

        /// <summary>
        /// Construct a Pt2d from its component coordinates
        /// </summary>
        /// <param name="h"></param>
        /// <param name="v"></param>
        public Pt2d(double h, double v)
        {
            this.H = h;
            this.V = v;
        }


        //Methods
    }
}
