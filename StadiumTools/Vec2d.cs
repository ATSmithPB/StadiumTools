using System.Collections.Generic;
using static System.Math;

namespace StadiumTools
{
    /// <summary>
    /// Represents a vector in 2D space (h,v)
    /// </summary>
    public struct Vec2d
    {
        //Properties
        /// <summary>
        /// Horizontal component of vector.
        /// </summary>
        public double H { get; set; }
        /// <summary>
        /// Vertical component of vector.
        /// </summary>
        public double V { get; set; }
        /// <summary>
        /// Length of vector
        /// </summary>
        public double L { get; set; }

        //Constructors
        /// <summary>
        /// Construct a 2d vector from its components
        /// </summary>
        /// <param name="h"></param>
        /// <param name="v"></param>
        /// <param name="l"></param>
        public Vec2d(double h, double v, double l)
        {
            this.H = h;
            this.V = v;
            this.L = l;
        }

        /// <summary>
        /// Construct a 2d vector from two Pt2d point objects
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Vec2d(Pt2d start, Pt2d end)
        {
            this.H = end.H - start.H;
            this.V = end.V - start.V;
            this.L = Sqrt((end.H - start.H) * (end.H - start.H) + (end.V - start.V) * (end.V - start.V));
        }

        //Methods
    }
}
