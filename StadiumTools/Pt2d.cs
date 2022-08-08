using System.Collections.Generic;
using System;

namespace StadiumTools
{
    /// <summary>
    /// Represents pts point in 2D space (x,y)
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
        /// Construct pts Pt2d from an existing Pt2d object
        /// </summary>
        /// <param name="pt"></param>
        public Pt2d(Pt2d pt)
        {
            this.X = pt.X;
            this.Y = pt.Y;
        }

        /// <summary>
        /// Construct pts Pt2d from its component coordinates
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

        //Operator Override
        public static Pt2d operator * (Pt2d a, double b)
        {
            return new Pt2d(a.X * b, a.Y * b);
        }

        public static Pt2d operator + (Pt2d a, Pt2d b)
        {
            return new Pt2d(a.X + b.X, a.Y + b.Y);
        }

        //Methods
        public Pt3d ToPt3d(Pln3d pln)
        {
            Pt3d pt3d = Pt3d.PointOnPlane(pln, this);
            return pt3d;
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

        public static Pt2d[] Scale(Pt2d[] pts, double factor)
        {
            Pt2d[] ptsScaled = new Pt2d[pts.Length];
            for (int i = 0; i < pts.Length; i++)
            {
                ptsScaled[i] = pts[i] * factor;
            }
            return ptsScaled;
        }

        /// <summary>
        /// Returns a collection of 2D points that represent the corners of a rectangle aligned to a default XY planr
        /// </summary>
        /// <param name="center"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <returns></returns>
        public static Pt2d[] RectangleCentered(Pt2d center, double sizeX, double sizeY)
        {
            Pt2d[] result = new Pt2d[4];
            double halfX = sizeX / 2;
            double halfY = sizeY / 2;
            result[0].X = -halfX;
            result[0].Y = -halfY;
            result[1].X = halfX;
            result[1].Y = -halfY;
            result[2].X = halfX;
            result[2].Y = halfY;
            result[3].X = -halfX;
            result[3].Y = halfY;
            return result;
        }

    }
}
