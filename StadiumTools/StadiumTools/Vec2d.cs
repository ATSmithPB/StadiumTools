using Rhino.Geometry;
using System;
using System.Collections.Generic;
using static System.Math;

namespace StadiumTools
{
    /// <summary>
    /// Represents a vector in 2D space (X,Y)
    /// </summary>
    public struct Vec2d
    {
        //Properties
        /// <summary>
        /// Horizontal component of vector.
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Vertical component of vector.
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Magnitude of vector
        /// </summary>
        public double M { get; set; }

        //Constructors
        /// <summary>
        /// Construct a Vec2d from another Vec2d
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="l"></param>
        public Vec2d(Vec2d v)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.M = v.M;
        }

        /// <summary>
        /// Construct a 2d vector from its components
        /// </summary>
        /// <param name="h"></param>
        /// <param name="v"></param>
        /// <param name="l"></param>
        public Vec2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.M = Magnitude(x, y);
        }

        /// <summary>
        /// Construct a 2d vector from two Pt2d point objects
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Vec2d(Pt2d start, Pt2d end)
        {
            this.X = end.X - start.X;
            this.Y = end.Y - start.Y;
            this.M = Magnitude(this.X, this.Y);
        }

        /// <summary>
        /// Construct a 2d vector from the XY components of a Pt2d
        /// </summary>
        /// <param name="pt2d"></param>
        public Vec2d(Pt2d pt2d)
        {
            this.X = pt2d.X;
            this.Y = pt2d.Y;
            this.M = Magnitude(pt2d.X, pt2d.Y);
        }

        /// <summary>
        /// Construct a 2d vector from a Spectator object
        /// </summary>
        /// <param name="spec"></param>
        public Vec2d(Spectator spec)
        {
            this.X = spec.POF.Y - spec.Loc2d.Y;
            this.Y = spec.POF.Y - spec.Loc2d.Y;
            this.M = Magnitude(this.X, this.Y);
        }

        public Vec2d(Vec3d v)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.M = Magnitude(this.X, this.Y);
        }

        //Delegates
        /// <summary>
        /// Gets Vector with Default XAxis components (1.0, 0.0, 0.0)
        /// </summary>
        public static Vec2d XAxis => new Vec2d(1.0, 0.0);
        /// <summary>
        /// Gets Vector with Default XAxis components (0.0, 1.0, 0.0)
        /// </summary>
        public static Vec2d YAxis => new Vec2d(0.0, 1.0);

        //Operator Overloads
        public static Vec2d operator * (Vec2d v, double d)
        {
            return Scale(v, d);
        }

        public static double operator * (Vec2d a, Vec2d b)
        {
            return DotProduct(a, b);
        }

        public static Vec2d operator + (Vec2d a, Vec2d b)
        {
            return Add(a, b);
        }

        //Methods
        /// <summary>
        /// Calculates the length of a Vec2d
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static double Magnitude(double a, double b)
        {
            double m = Abs(Sqrt((a * a) + (b * b)));
            return m;
        }

        public void Normalize()
        {
            Normalize(this);
        }
        /// <summary>
        /// Scales a vector such that its length = 1.0 (by reference)
        /// </summary>
        /// <param name="v"></param>
        public static void Normalize(ref Vec2d v)
        {
            v.X = v.X / v.M;
            v.Y = v.Y / v.M;
            v.M = 1;
        }

        /// <summary>
        /// returns true if a vector is successfully Normalized
        /// </summary>
        /// <returns></returns>
        public bool Unitize()
        {
            this.Normalize();
            return true;
        }

        /// <summary>
        /// Returns a scaled vector of the same direction with its length = 1.0
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Vec2d</returns>
        public static Vec2d Normalize(Vec2d v)
        {
            Vec2d vN = new Vec2d();
            vN.X = v.X / v.M;
            vN.Y = v.Y / v.M;
            vN.M = 1.0;
            return vN;
        }

        /// <summary>
        /// returns a 2D vector that is rotate3d 90 degrees CCW with the same origin
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vec2d CCW90(Vec2d v)
        {
            return new Vec2d(-v.Y, v.X);
        }

        /// <summary>
        /// returns a 2D vector that is rotate3d 90 degrees CW with the same origin
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vec2d CW90(Vec2d v)
        {
            return new Vec2d(v.Y, -v.X);
        }

        /// <summary>
        /// Returns a Vec3d from a Vec2d and a z component
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z"></param>
        /// <returns>Vec3d</returns>
        public static Vec3d ToVec3d(Vec2d v, double z)
        {
            Vec3d result = new Vec3d(v, z);
            return result;
        }

        public static Vec2d Scale(Vec2d v, double d)
        {
            return new Vec2d(v.X * d, v.Y * d); 
        }

        /// <summary>
        /// returns the angle between two vectors 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Angle(Vec2d a, Vec2d b)
        {
            Vec2d aN = Normalize(a);
            Vec2d bN = Normalize(b);

            double d = (aN.X * bN.X) + (aN.Y * bN.Y);
            if (d > 1.0)
                d = 1.0;
            if (d < -1.0)
                d = -1.0;
            return Acos(d);
        }

        public static double Angle(Pt2d origin, Pt2d start, Pt2d end)
        {
            Vec2d a = new Vec2d(origin, start);
            Vec2d b = new Vec2d(origin, end);
            return Angle(a, b);
        }

        public static double Reflex(Vec2d a, Vec2d b)
        {
            return (2* PI) - Angle(a, b);
        }

        public static double Reflex(Pt2d origin, Pt2d start, Pt2d end)
        {
            Vec2d a = new Vec2d(origin, start);
            Vec2d b = new Vec2d(origin, end);
            return (2* PI) - Angle(a, b);
        }

        /// <summary>
        /// returns the distance from the vecOrigin to the intersection point on a line (null if no intersection)
        /// </summary>
        /// <param name="vecOrigin"></param>
        /// <param name="vec"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool Intersect(Pt2d vecOrigin, Vec2d vec, Pt2d start, Pt2d end, out double distance)
        {
            Vec2d v1 = new Vec2d(vecOrigin - start);
            Vec2d v2 = new Vec2d(end - start);
            Vec2d v3 = new Vec2d(-vec.Y, vec.X);

            double dot = v2 * v3;
            if (Math.Abs(dot) < 0.000001)
            {
                distance = 0.0;
                return false;
            }
            double t1 = CrossProduct(v2, v1) / dot;
            double t2 = (v1 * v3) / dot;
            if (t1 >= 0.0 && (t2 >= 0.0 && t2 <= 1.0))
            {
                distance = t1;
                return true;
            }
            else
            {
                distance = 0.0;
                return false;
            }
        }

        public static double DotProduct(Vec2d a, Vec2d b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        } 

        public static double CrossProduct(Vec2d a, Vec2d b)
        {
            return (a.X * b.Y) - (b.X * a.Y);
        }

        public static Vec2d Add(Vec2d a, Vec2d b)
        {
            return new Vec2d(a.X + b.X, a.Y + b.Y);
        }


    }
}
