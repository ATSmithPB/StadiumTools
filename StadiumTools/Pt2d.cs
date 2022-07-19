using System.Collections.Generic;

namespace StadiumTools
{
    /// <summary>
    /// Represents a point in 2D space (x,y)
    /// </summary>
    public struct Pt2d
    {
        //Properties
        /// <summary>
        /// Horizontal distance from origin
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Vertical distance from origin
        /// </summary>
        public double Y { get; set; }

        //Constructors
        /// <summary>
        /// Construct a Pt2d from an existing Pt2d object
        /// </summary>
        /// <param name="pt"></param>
        public Pt2d(Pt2d pt)
        {
            this.X = pt.X;
            this.Y = pt.Y;
        }

        /// <summary>
        /// Construct a Pt2d from its component coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Pt2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }


        //Methods
        public Pt3d ToPt3d(Pln3d pln)
        {
            Pt3d pt3d = Pt3d.LocalCoordinates(this, pln);
            return pt3d;
        }
    }
}
