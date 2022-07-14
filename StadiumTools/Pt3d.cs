using System.Collections.Generic;

namespace StadiumTools
{
    /// <summary>
    /// Represents a point in 3D space (x, y, z)
    /// </summary>
    public struct Pt3d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        //Constructors
        /// <summary>
        /// Construct a Pt3d from X,Y & Z coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Pt3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z; 
        }

        /// <summary>
        /// Construct a Pt3d from a Pt2d and a reference plane
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="pln"></param>
        public Pt3d(Pt2d pt, double z)
        {
            this.X = pt.X;
            this.Y = pt.Y;
            this.Z = z;
        }

        //public Pt3d(Pt2d pt, Pln3d pln)
        //{
        //    this.X = pt.X * pln.;
        //    this.Y = ;
        //    this.Z = ;
        //}

        //Delegates
        /// <summary>
        /// Creates Point with Default Origin components (0.0, 0.0, 0.0)
        /// </summary>
        public static Pt3d Origin => new Pt3d(0.0, 0.0, 0.0);
    }

    
}

