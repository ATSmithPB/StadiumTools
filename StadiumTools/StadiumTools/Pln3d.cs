using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static System.Math;

namespace StadiumTools
{
    /// <summary>
    /// Represents a Plane in 3D space 
    /// </summary>
    public struct Pln3d
    {
        //Enums
        /// <summary>
        /// Available default plane orientations
        /// </summary>
        public enum ReferencePtType
        {
            XY,
            YZ,
            XZ
        }

        //Prtoperties
        /// <summary>
        /// True if plane components are valid.
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// Pt3d representing the origin point of the Plane
        /// </summary>
        public Pt3d OriginPt { get; set; }
        /// <summary>
        /// x coordinate of plane origin
        /// </summary>
        public double OriginX { get; set; }
        /// <summary>
        /// y coordinate of plane origin
        /// </summary>
        public double OriginY { get; set; }
        /// <summary>
        /// z coordinate of plane origin
        /// </summary>
        public double OriginZ { get; set; }
        /// <summary>
        /// Vec3d representing the x axis
        /// </summary>
        public Vec3d Xaxis { get; set; }
        /// <summary>
        /// Vec3d representing the y axis
        /// </summary>
        public Vec3d Yaxis { get; set; }
        /// <summary>
        /// Vec3d representing the z axis
        /// </summary>
        public Vec3d Zaxis { get; set; }

        //Constructors
        /// <summary>
        /// construct a Pln3d with an origin point and three vectors
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Pln3d(Pt3d origin, Vec3d x, Vec3d y, Vec3d z)
        {
            IsValid = false;
            OriginPt = origin;
            OriginX = origin.X;
            OriginY = origin.Y;
            OriginZ = origin.Z;
            Xaxis = Vec3d.Normalize(x);
            Yaxis = Vec3d.Normalize(y);
            Zaxis = Vec3d.Normalize(z);
            GetValidity(this);
        }

        public Pln3d(Pt3d origin, Vec3d x, Vec3d y)
        {
            IsValid = false;
            OriginPt = origin;
            OriginX = origin.X;
            OriginY = origin.Y;
            OriginZ = origin.Z;
            Xaxis = Vec3d.Normalize(x);
            Yaxis = Vec3d.Normalize(y);
            Zaxis = Vec3d.Normalize(Vec3d.CrossProduct(this.Xaxis, this.Yaxis));
            GetValidity(this);
        }

        public Pln3d(Pt3d origin, Vec3d z, Pt3d ptOnY)
        {
            IsValid = false;
            OriginPt = origin;
            OriginX = origin.X;
            OriginY = origin.Y;
            OriginZ = origin.Z;
            Zaxis = Vec3d.Normalize(z);
            Yaxis = Vec3d.Normalize(new Vec3d(origin, ptOnY));
            Xaxis = Vec3d.Normalize(Vec3d.CrossProduct(Yaxis, Zaxis));
            GetValidity(this);
        }

        public Pln3d(Pln2d plane)
        {
            IsValid = false;
            OriginPt = new Pt3d(plane.OriginPt, 0.0);
            OriginX = this.OriginPt.X;
            OriginY = this.OriginPt.Y;
            OriginZ = this.OriginPt.Z;
            Xaxis = Vec3d.Normalize(new Vec3d(plane.Xaxis, 0.0));
            Yaxis = Vec3d.Normalize(new Vec3d(plane.Yaxis, 0.0));
            Zaxis = Vec3d.ZAxis;
            GetValidity(this);
        }

        public Pln3d(Pt3d origin, Pt3d ptOnZAxis)
        {
            this.IsValid = false;
            this.OriginPt = origin;
            this.OriginX = origin.X;
            this.OriginY = origin.Y;
            this.OriginZ = origin.Z;
            this.Zaxis = Vec3d.Normalize(new Vec3d(origin, ptOnZAxis));
            this.Xaxis = Vec3d.Normalize(Vec3d.PerpTo(this.Zaxis));
            this.Yaxis = Vec3d.Normalize(Vec3d.CrossProduct(this.Zaxis, this.Xaxis));
            GetValidity(this);
        }

        public Pln3d(Pt3d origin, Vec3d zAxis)
        {
            this.IsValid = false;
            this.OriginPt = origin;
            this.OriginX = origin.X;
            this.OriginY = origin.Y;
            this.OriginZ = origin.Z;
            this.Zaxis = Vec3d.Normalize(zAxis);
            this.Xaxis = Vec3d.Normalize(Vec3d.PerpTo(zAxis));
            this.Yaxis = Vec3d.Normalize(Vec3d.CrossProduct(zAxis, this.Xaxis));
            GetValidity(this);
        }

        /// <summary>
        /// Construct a plane aligned to defauly XYZ with a specified origin
        /// </summary>
        /// <param name="origin"></param>
        public Pln3d(Pt3d origin)
        {
            this.IsValid = false;
            this.OriginPt = origin;
            this.OriginX = origin.X;
            this.OriginY = origin.Y;
            this.OriginZ = origin.Z;
            this.Xaxis = Vec3d.XAxis;
            this.Yaxis = Vec3d.YAxis;
            this.Zaxis = Vec3d.ZAxis;
            GetValidity(this);
        }

        /// <summary>
        /// construct a Pln3d from three points 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="ptOnX"></param>
        /// <param name="ptOnPlane"></param>
        public Pln3d(Pt3d origin, Pt3d ptOnX, Pt3d ptOnPlane)
        {
            this.IsValid = false;
            this.OriginPt = origin;
            this.OriginX = origin.X;
            this.OriginY = origin.Y;
            this.OriginZ = origin.Z;
            this.Xaxis = Vec3d.Normalize(new Vec3d(origin, ptOnX));
            this.Zaxis = Vec3d.Normalize(Vec3d.PerpTo(origin, ptOnX, ptOnPlane));
            this.Yaxis = Vec3d.Normalize(Vec3d.CrossProduct(this.Zaxis, this.Xaxis));
            GetValidity(this);
        }

        //Delegates
        /// <summary>
        /// Gets Vector with Default XAxis components (1.0, 0.0, 0.0)
        /// </summary>
        public static Pln3d XYPlane => new Pln3d(Pt3d.Origin, Vec3d.XAxis, Vec3d.YAxis, Vec3d.ZAxis);

        //Methods
        /// <summary>
        /// Check if all plane components construct a valid plane.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>bool</returns>
        private static bool GetValidity(Pln3d p)
        {
            if (p.Xaxis.M != 1.0)
            {
                throw new ArgumentException($"Plane Xaxis length [{p.Xaxis.M}] is not unitized");
            }
            if (p.Yaxis.M != 1.0)
            {
                throw new ArgumentException($"Plane Yaxis length [{p.Yaxis.M}] is not unitized");
            }
            if (p.Zaxis.M != 1.0)
            {
                throw new ArgumentException($"Plane Zaxis length [{p.Zaxis.M}] is not unitized");
            }
            return true;
        }

        public Pt3d ToPt3d(Pln3d pln)
        {
            Pt3d pt3d = pln.OriginPt;
            return pt3d;
        }

        /// <summary>
        /// returns a collection of perpendicular planes to a list of points
        /// </summary>
        /// <param name="pts"></param>
        /// <returns>Pln3d[]</returns>
        public static Pln3d[] PerpPlanes(Pt3d[] pts)
        {
            if (pts.Length < 2)
            {
                throw new ArgumentException("pts.Length must be >1 to calculate perp plane");
            }
            Pln3d[] pln3ds = new Pln3d[pts.Length];
            for (int i = 0; i < pts.Length - 1; i++)
            {
                pln3ds[i] = new Pln3d(pts[i], pts[i + 1]);
            }
            return pln3ds;
        }

        /// <summary>
        /// returns a collection of perpendicular planes to a list of points
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static Pln3d[] PerpPlanes(List<Pt3d> pts)
        {
            Pt3d[] ptsArray = pts.ToArray();
            return PerpPlanes(ptsArray);
        }

        public static Pln3d[] AvgPlanes(Pline pline, Pln3d cPlane)
        {
            return AvgPlanes(pline.Points, pline.Closed, cPlane);
        }

        public static Pln3d[] AvgPlanes(Pt3d[] pts, bool closed, Pln3d cPlane)
        {
            if (pts.Length < 2)
            {
                throw new ArgumentException("pts.Length must be >1 to calculate avg plane");
            }
            
            Pln3d[] planes = new Pln3d[pts.Length - 1];
            Vec3d prev = new Vec3d();
            Vec3d curr = new Vec3d(pts[0], pts[1]);
            if (closed)
            {
                prev = new Vec3d(pts[pts.Length - 2], pts[pts.Length - 1]);
            }
            else
            {
                prev = curr;
            }

            for (int i = 0; i < pts.Length - 2; i++) // i < p.L - 3? 
            {
                Vec3d zAxis = Vec3d.Tween2(prev, curr).Flip();
                Pt3d ptOnY = pts[i] + cPlane.Zaxis;
                planes[i] = new Pln3d(pts[i], zAxis, ptOnY);
                prev = curr;
                curr = new Vec3d(pts[i + 1], pts[i + 2]);
            }

            Vec3d lastZAxis = Vec3d.Tween2(prev, curr).Flip();
            Pt3d lastPtOnY = pts[pts.Length - 2] + cPlane.Zaxis;
            planes[pts.Length - 2] = new Pln3d(pts[pts.Length - 2], lastZAxis, lastPtOnY);

            return planes;
        }

        /// <summary>
        /// returns true if two Pln3d planes are coplanar within a given absolute tolerance
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerance"></param>
        /// <param name="angleTolerance"></param>
        /// <returns></returns>
        public static bool IsCoPlanar(Pln3d a, Pln3d b, double tolerance)
        {
            bool result = false;
            if (Vec3d.IsParallel(a.Zaxis, b.Zaxis, tolerance))
            {
                if(Math.Abs(Pt3d.LocalCoordinates(a.OriginPt, b).Z) < tolerance)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// returns a point on the plane closest to specified point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>Pt3d</returns>
        public Pt3d ClosestPointTo(Pt3d pt)
        {
            ClosestPointTo(pt, out double u, out double v);
            return PointAt(u, v, 0.0);
        }

        /// <summary>
        /// returns a point on the place closest to the specified (u,v) parameters
        /// </summary>
        /// <param name="p"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool ClosestPointTo(Pt3d p, out double u, out double v)
        {
            Vec3d vec = new Vec3d(p - this.OriginPt);
            u = vec * this.Xaxis;
            v = vec * this.Yaxis;
            return true;
        }

        /// <summary>
        /// returns a point (in world coordinates) at the specified u,v parameters of this Plane. 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns>Pt3d</returns>
        public Pt3d PointAt(double u, double v, double w)
        {
            return this.OriginPt + (u * this.Xaxis) + (v * this.Yaxis) + w * this.Zaxis;   
        }

        public static Pln3d[] Tween2(Pln3d a, Pln3d b, double[] parameters)
        {
            Pln3d[] result = new Pln3d[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                Pt3d origin = Pt3d.Tween2(a.OriginPt, b.OriginPt, parameters[i]);
                Vec3d xAvg = Vec3d.Tween2(a.Xaxis, b.Xaxis, parameters[i]);
                Vec3d yAvg = Vec3d.Tween2(a.Yaxis, b.Yaxis, parameters[i]);
                result[i] = new Pln3d(origin, xAvg, yAvg); 
            }
            return result;
        }

        public static Pln3d Tween2(Pln3d a, Pln3d b, double parameter)
        {
            Pt3d origin = Pt3d.Midpoint(a.OriginPt, b.OriginPt);
            Vec3d xAvg = Vec3d.Tween2(a.Xaxis, b.Xaxis, 0.5);
            Vec3d yAvg = Vec3d.Tween2(a.Yaxis, b.Yaxis, 0.5);
            
            return new Pln3d(origin, xAvg, yAvg);
        }

        public Pln3d Clone()
        {
            return (Pln3d)this.MemberwiseClone();
        }
    }
}
