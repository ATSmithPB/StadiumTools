using System.Collections.Generic;
using System;
using Rhino.Geometry;
using System.Collections;
using Rhino.Commands;

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
        public static Pt3d operator +(Pt3d a, Pt3d b)
        {
            return Add(a, b);
        }

        public static Pt3d operator - (Pt3d a, Pt3d b)
        {
            return Subtract(a, b);
        }

        public static Pt3d operator - (Pt3d p, Vec3d v)
        {
            return Subtract(p, v);
        }

        public static Pt3d operator *(double f, Pt3d a)
        {
            return Multiply(a, f);
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

        public double DistanceTo(Pt3d to)
        {
            return Distance(this, to);
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

        /// <summary>
        /// Returns the point pt in local coordinates of the coordinate system parameter
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="coordSystem"></param>
        /// <returns></returns>
        public static Pt3d LocalCoordinates(Pt3d pt, Pln3d coordSystem)
        {
            Vec3d posVec = (pt - coordSystem.OriginPt).ToVec3d();
            double projX = posVec * coordSystem.Xaxis;
            double projY = posVec * coordSystem.Yaxis;
            double projZ = posVec * coordSystem.Zaxis;

            return new Pt3d(projX, projY, projZ);
        }

        public static Pt3d[] LocalCoordinates(Pt3d[] pts, Pln3d coordSystem)
        {
            Pt3d[] result = new Pt3d[pts.Length];
            for (int i = 0; i < pts.Length; i++)
            {
                Vec3d posVec = (pts[i] - coordSystem.OriginPt).ToVec3d();
                double projX = posVec * coordSystem.Xaxis;
                double projY = posVec * coordSystem.Yaxis;
                double projZ = posVec * coordSystem.Zaxis;
                result[i] = new Pt3d(projX, projY, projZ);
            }
            return result;
        }

        /// <summary>
        /// Returns the point pt in local coordinates of the coordinate system parameter
        /// </summary>
        /// <param name="pt2d"></param>
        /// <param name="coordSystem"></param>
        /// <returns></returns>
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

        public static Pt3d Add(Pt3d a, Pt3d b)
        {
            return new Pt3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// subracts the components of Pt3d b from the components of Pt3d a and returns a new Pt3d with the resulting components
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Pt3d</returns>
        public static Pt3d Subtract(Pt3d a, Pt3d b)
        {
            return new Pt3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// subracts the components of Vec3d v from the components of Pt3d a and returns a new Pt3d with the resulting components
        /// </summary>
        /// <param name="p"></param>
        /// <param name="v"></param>
        /// <returns>Pt3d</returns>
        public static Pt3d Subtract(Pt3d p, Vec3d v)
        {
            return new Pt3d(p.X - v.X, p.Y - v.Y, p.Z - v.Z);
        }

        public static Pt3d Multiply(Pt3d p, double f)
        {
            return new Pt3d(p.X * f, p.Y * f, p.Z * f);
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
            if(sizeX <= 0 || sizeY <= 0)
            {
                throw new ArgumentException("Error: sizeX and sizeY must be non-negative and > 0");
            }
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
        /// Returns a collection of 3d points that represent the corners of a rectangle aligned to a 3d plane with discrete side lengths
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <returns></returns>
        public static Pt3d[] RectangleCentered(Pln3d plane, double sizeY0, double sizeX1, double sizeY1, double sizeX0)
        {
            Pt3d[] result = new Pt3d[4];
            Pt2d[] pts2d = Pt2d.RectangleCentered(new Pln2d(plane), sizeY0, sizeX1, sizeY1, sizeX0);
            for (int i = 0; i < 4; i++)
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
            Vec3d AC = (t * AB.M) * Vec3d.Normalize(AB);
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

        /// <summary>
        /// Rotates a Pt3d about the Z-axis of a specified Pln3d, by a specified angle in Radians
        /// </summary>
        /// <param name="pln"></param>
        /// <param name="pt"></param>
        /// <param name="angleRadians"></param>
        /// <returns></returns>
        public static Pt3d Rotate(Pln3d pln, Pt3d pt, double angleRadians)
        {
            Pt2d localPt2d = new Pt2d(LocalCoordinates(pt, pln));
            return new Pt3d(Pt2d.Rotate(localPt2d, angleRadians), pln);
        }

        /// <summary>
        /// offset the midpoint between two points perpendicular to the axis of the given points (cross product)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Pt3d OffsetMidpoint(Pt3d start, Pt3d end, double offset)
        {
            Vec3d axisNormalized = Vec3d.Normalize(new Vec3d(start, end));
            Vec3d perp = Vec3d.CrossProduct(axisNormalized, Vec3d.ZAxis);
            Vec3d perpScaled = Vec3d.Scale(perp, offset);
            return Pt3d.Midpoint(start, end) + perpScaled;
        }

        /// <summary>
        /// offset the midpoint between two points perpendicular to the axis of the given points (cross product)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="offset"></param>
        /// <param name="midpoint"></param>
        /// <returns>Pt3d</returns>
        public static Pt3d OffsetMidpoint(Pt3d start, Pt3d end, double offset, out Pt3d midpoint)
        {
            Vec3d axisNormalized = Vec3d.Normalize(new Vec3d(start, end));
            Vec3d perp = Vec3d.CrossProduct(axisNormalized, Vec3d.ZAxis);
            Vec3d perpScaled = Vec3d.Scale(perp, offset);
            midpoint = Pt3d.Midpoint(start, end);
            return midpoint + perpScaled;
        }

        /// <summary>
        /// returns the angle in radians between two points relative to a plane and about its origin
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double Angle(Pln3d plane, Pt3d start, Pt3d end)
        {
            Pt2d start2d = new Pt2d(LocalCoordinates(start, plane));
            Pt2d end2d = new Pt2d(LocalCoordinates(end, plane));
            return Pt2d.Angle(Pt2d.Origin, start2d, end2d);
        }

        public static bool IsCoincident(Pt3d a, Pt3d b, double tolerance)
        {
            bool result = false;
            if (Pt3d.Distance(a, b) < tolerance)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// returns the closer point (p0 or p1) of two points, to the given point 
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns>Pt3d</returns>
        public Pt3d CloserPt(Pt3d p0, Pt3d p1)
        {
            Vec3d v0 = new Vec3d(p1 - this);
            Vec3d v1 = new Vec3d(p1 - this);

            if (v1.M > v0.M)
            {
                return p1;
            }
            else
            {
                return p0;
            }
        }

        /// <summary>
        /// returns the closer point (p0 or p1) of two points, to the given point 
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns>Pt3d</returns>
        public Pt3d CloserPt(Pt3d p0, Pt3d p1, out Vec3d distanceVec)
        {
            Vec3d v0 = new Vec3d(p1 - this);
            Vec3d v1 = new Vec3d(p1 - this);

            if (v1.M < v0.M)
            {
                distanceVec = v1;
                return p1;
            }
            else
            {
                distanceVec = v0;
                return p0;
            }
        }

        public static Pt3d[] FromPt2d(Pt2d[] pts2d, Pln3d pln)
        {
            Pt3d[] result = new Pt3d[pts2d.Length];
            for (int i = 0; i < pts2d.Length; i++)
            {
                result[i] = new Pt3d(pts2d[i], pln);
            }
            return result;
        }
    }
}

