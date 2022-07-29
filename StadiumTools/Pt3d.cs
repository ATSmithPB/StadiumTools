using System.Collections.Generic;
using System;

namespace StadiumTools
{
    /// <summary>
    /// Represents a point in 3D space (x, y, z)
    /// </summary>
    public struct Pt3d : ICloneable
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
        /// Construct a Pt3d from a Pt2d and a z offset
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="pln"></param>
        public Pt3d(Pt2d pt, double z)
        {
            this.X = pt.X;
            this.Y = pt.Y;
            this.Z = z;
        }

        //Delegates
        /// <summary>
        /// Creates Point with Default Origin components (0.0, 0.0, 0.0)
        /// </summary>
        public static Pt3d Origin => new Pt3d(0.0, 0.0, 0.0);
        
        //Operator Overrides
        public static Pt3d operator - (Pt3d a, Pt3d b)
        {
            double x = a.X - b.X;
            double y = a.Y - b.Y;
            double z = a.Z - b.Z;

            return new Pt3d(x, y, z);
        }

        //Methods
        /// <summary>
        /// Returns a Pt2d based on the X and Y components of a Pt3d object
        /// </summary>
        /// <param name="pt3d"></param>
        /// <returns></returns>
        public Pt2d ToPt2d()
        {
            Pt2d pt2d = new Pt2d(this.X, this.Y);
            return pt2d;
        }

        public Vec3d ToVec3d()
        {
            Vec3d vec3d = new Vec3d(this.X, this.Y, this.Z);
            return vec3d;
        }

        //Returns the point pt in local coordinates of the coordinate system parameter
        public static Pt3d LocalCoordinates(Pt3d pt, Pln3d coordSystem)
        {
            Vec3d posVec = (pt - coordSystem.OriginPt).ToVec3d();
            double projX = Vec3d.DotProduct(posVec, coordSystem.Xaxis); //* in Rhinocommon means dot product
            double projY = Vec3d.DotProduct(posVec, coordSystem.Yaxis);
            double projZ = Vec3d.DotProduct(posVec, coordSystem.Zaxis);

            return new Pt3d(projX, projY, projZ);
        }

        /// <summary>
        /// clones a Pt3d (shallow copy)
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            //Shallow copy
            return (Pt3d)this.MemberwiseClone();
        }

    }

    
}

