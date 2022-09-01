using System.Collections.Generic;
using System;
using Rhino.Geometry;

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

        /// <summary>
        /// Construct a Pt3d from a Pt2d and a Plane
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="pln"></param>
        public Pt3d(Pt2d pt, Pln3d pln)
        {
            this = PointOnPlane(pln, pt);
        }

        //Delegates
        /// <summary>
        /// Creates Point with Default Origin components (0.0, 0.0, 0.0)
        /// </summary>
        public static Pt3d Origin => new Pt3d(0.0, 0.0, 0.0);
        
        //Operator Overrides
        /// <summary>
        /// Subtracts one point from another
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Pt3d operator - (Pt3d a, Pt3d b)
        {
            return Subtract(a, b);
        }

        /// <summary>
        /// Returns the sum of a Pt3d and Vec3d
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Pt3d operator + (Pt3d a, Vec3d b)
        {
            return Add(a, b);
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

        /// <summary>
        /// Returns a Pt3d in global coordinates that represents the location of three components in a local coordinate system 
        /// </summary>
        /// <param name="localC"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>Pt3d</returns>
        public static Pt3d PointOnPlane(Pln3d localC, double x, double y, double z)
        {
            Vec3d pos = (x * localC.Xaxis) + (y * localC.Yaxis) + (z * localC.Zaxis);
            Pt3d p = localC.OriginPt + pos;
            return p;
        }

        /// <summary>
        /// Returns a Pt3d in global coordinates that represents the location of three components in a local coordinate system
        /// </summary>
        /// <param name="localC"></param>
        /// <param name="pt"></param>
        /// <returns>Pt3d</returns>
        public static Pt3d PointOnPlane(Pln3d localC, Pt3d pt)
        {
            Vec3d pos = (pt.X * localC.Xaxis) + (pt.Y * localC.Yaxis) + (pt.Y * localC.Zaxis);
            Pt3d result = localC.OriginPt + pos;
            return result;
        }

        /// <summary>
        /// Returns a Pt3d in global coordinates that represents the location of three components in a local coordinate system
        /// </summary>
        /// <param name="localC"></param>
        /// <param name="pt"></param>
        /// <returns>Pt3d</returns>
        public static Pt3d PointOnPlane(Pln3d localC, Pt2d pt2d)
        {
            Pt3d pt = new Pt3d(pt2d, 0.0);
            Vec3d pos = (pt.X * localC.Xaxis) + (pt.Y * localC.Yaxis) + (pt.Z * localC.Zaxis);
            Pt3d result = localC.OriginPt + pos;
            return result;
        }

        //Returns the point pt in local coordinates of the coordinate system parameter
        public static Pt3d LocalCoordinates(Pt3d pt, Pln3d coordSystem)
        {
            Vec3d posVec = (pt - coordSystem.OriginPt).ToVec3d();
            double projX = posVec * coordSystem.Xaxis;
            double projY = posVec * coordSystem.Yaxis;
            double projZ = posVec * coordSystem.Zaxis;

            return new Pt3d(projX, projY, projZ);
        }

        //Returns the point pt in local coordinates of the coordinate system parameter
        public static Pt3d LocalCoordinates(Pt2d pt2d, Pln3d coordSystem)
        {
            Pt3d pt = new Pt3d(pt2d, 0.0);
            Vec3d posVec = (pt - coordSystem.OriginPt).ToVec3d();
            double projX = posVec * coordSystem.Xaxis;
            double projY = posVec * coordSystem.Yaxis;
            double projZ = posVec * coordSystem.Zaxis;

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

        /// <summary>
        /// returns the sum of a 3d vector and a 3d point
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Pt3d Add(Pt3d p, Vec3d v)
        {
            return new Pt3d(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        }

        public static Pt3d Subtract(Pt3d a, Pt3d b)
        {
            return new Pt3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Returns a collection of 3d points that represent the corners of a rectangle aligned to a 3d plane 
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <returns>Pt3d[]</returns>
        public static Pt3d[] RectangleCentered(Pln3d plane, double sizeX, double sizeY)
        {
            Pt3d[] result = new Pt3d[4];
            Pt2d[] pts2d = Pt2d.RectangleCentered(plane.OriginPt.ToPt2d(), sizeX, sizeY);
            for (int i = 0; i < 4; i++)
            {
                result[i] = pts2d[i].ToPt3d(plane);
            }
            return result;
        }

        /// <summary>
        /// Returns a collection of 3d points that represent the corners of a rectangle aligned to a 3d plane 
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <param name="radius"></param>
        /// <returns>Pt3d[]</returns>
        public static Pt3d[] RectangleCenteredChamfered(Pln3d plane, double sizeX, double sizeY, double radius)
        {
            Pt3d[] result = new Pt3d[8];
            Pt2d[] pts2d = Pt2d.RectangleCenteredChamfered(Pt2d.Origin, sizeX, sizeY, radius);
            for (int i = 0; i < 8; i++)
            {
                result[i] = pts2d[i].ToPt3d(plane);
            }
            return result;
        }

        /// <summary>
        /// calculates point between two points at a parameter
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Pt3d Tween2(Pt3d a, Pt3d b, double t)
        {
            Vec3d AB = new Vec3d(b - a);
            Vec3d AC = (t * AB.M) * AB;
            Pt3d c = a + AC;

            return c;
        }

        /// <summary>
        /// calculates the midpoint between two Pt3d objects
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Pt3d Midpoint(Pt3d a, Pt3d b)
        {
            return new Pt3d((a.X + b.X) / 2, (a.Y + b.Y) / 2, (a.Z + b.Z) / 2);
        }

        /// <summary>
        /// calculates the distance between two Pt3d points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(Pt3d a, Pt3d b)
        {
            Vec3d v = new Vec3d(a, b);
            return v.M;
        }

        public static Pt3d Rotate(Pln3d pln, Pt3d pt, double angleRadians)
        {
            Pt2d localPt2d = new Pt2d(LocalCoordinates(pt, pln));
            return new Pt3d(Pt2d.Rotate(localPt2d, angleRadians), pln);
        }
    }

    
}

