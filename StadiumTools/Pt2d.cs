using System.Collections.Generic;
using System;

namespace StadiumTools
{
    /// <summary>
    /// Represents a point in 2D space (x,y)
    /// </summary>
    public struct Pt2d : ICloneable
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

        //Delegate
        public static Pt2d Origin => new Pt2d(0.0, 0.0);

        //Methods
        public Pt3d ToPt3d(Pln3d pln)
        {
            Pt3d pt3d = LocalCoordinates(this, pln);
            return pt3d;
        }

        //Returns the point pt in local coordinates of the coordinate system parameter
        public static Pt3d LocalCoordinates(Pt2d pt2d, Pln3d coordSystem)
        {
            Pt3d pt = new Pt3d(pt2d, 0.0);
            Vec3d posVec = (pt - coordSystem.OriginPt).ToVec3d();
            double projX = Vec3d.DotProduct(posVec, coordSystem.Xaxis); //* in Rhinocommon means dot product
            double projY = Vec3d.DotProduct(posVec, coordSystem.Yaxis);
            double projZ = Vec3d.DotProduct(posVec, coordSystem.Zaxis);

            return new Pt3d(projX, projY, projZ);
        }


        public static Pt2d[] CloneArray(Pt2d[] pts)
        {
            //Shallow copy
            Pt2d[] ptsCloned = (Pt2d[]) pts.Clone();
            return ptsCloned;
        }
        
        public object Clone()
        {
            //Shallow copy
            return (Pt2d)this.MemberwiseClone();
        }

    }
}
