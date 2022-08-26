using System.Collections.Generic;
using static System.Math;

namespace StadiumTools
{
    /// <summary>
    /// Represents a Plane in 3D space 
    /// </summary>
    public struct Pln2d
    {
        //Prtoperties
        /// <summary>
        /// True if plane components are valid.
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// Pt3d representing the origin point of the Plane
        /// </summary>
        public Pt2d OriginPt { get; set; }
        /// <summary>
        /// x coordinate of plane origin
        /// </summary>
        public double OriginX { get; set; }
        /// <summary>
        /// y coordinate of plane origin
        /// </summary>
        public double OriginY { get; set; }
        /// <summary>
        /// Vec3d representing the x axis
        /// </summary>
        public Vec2d Xaxis { get; set; }
        /// <summary>
        /// Vec3d representing the y axis
        /// </summary>
        public Vec2d Yaxis { get; set; }

        //Constructors
        /// <summary>
        /// construct a Pln3d with an origin point and three vectors
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Pln2d(Pt2d origin, Vec2d x, Vec2d y)
        {
            this.IsValid = false;
            this.OriginPt = origin;
            this.OriginX = origin.X;
            this.OriginY = origin.Y;
            this.Xaxis = x;
            this.Yaxis = y;
            SetValid(this);
        }
        
        /// <summary>
        /// Constructs a Pln2d from an origin Pt2d and 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Pln2d(Pt2d origin, Pt2d pointOnXAxis)
        {
            this.IsValid = false;
            this.OriginPt = origin;
            this.OriginX = origin.X;
            this.OriginY = origin.Y;

            Vec2d xVec = new Vec2d(origin, pointOnXAxis);
            
            this.Xaxis = Vec2d.Normalize(xVec);
            this.Yaxis = Vec2d.CCW90(this.Xaxis);
            SetValid(this);
        }

        /// <summary>
        /// Construct a Pln2d from the components of a Pln3d
        /// </summary>
        /// <param name="plane"></param>
        public Pln2d(Pln3d plane)
        {
            this.IsValid = false;

            Pt2d origin = plane.OriginPt.ToPt2d();

            this.OriginPt = origin;
            this.OriginX = origin.X;
            this.OriginY = origin.Y;

            Vec2d xVec = new Vec2d(plane.Xaxis);

            this.Xaxis = Vec2d.Normalize(xVec);
            this.Yaxis = Vec2d.CCW90(this.Xaxis);
            SetValid(this);
        }

        //Delegates
            /// <summary>
            /// Gets Vector with Default XAxis components (1.0, 0.0, 0.0)
            /// </summary>
        public static Pln2d XYPlane => new Pln2d(Pt2d.Origin, Vec2d.XAxis, Vec2d.YAxis);

        //Methods
        /// <summary>
        /// Check if all plane components construct a valid plane.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool SetValid(Pln2d p)
        {
            bool isValid = true;

            return isValid;
        }

        public Pt2d ToPt2d(Pln2d pln)
        {
            Pt2d pt2d = pln.OriginPt;
            return pt2d;
        }

        public Pln3d ToPln3d(Pln2d pln)
        {
            Pln3d pln3d = new Pln3d(pln);
            return pln3d;
        }
    }
}
