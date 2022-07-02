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
        public Pt2d(double h, double v)
        {
            this.H = h;
            this.V = v;
        }

        //Methods
    }
}
