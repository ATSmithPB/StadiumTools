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

        //Delegates
        public static Pt2d Origin => new Pt2d(0.0, 0.0);

        //Operator Overrides
        public static Pt2d operator * (Pt2d a, double b)
        {
            return Multiply(a, b);
        }

        public static Pt2d operator + (Pt2d p, Vec2d v)
        {
            return Add(p, v);
        }

        public static Pt2d operator +(Pt2d p, Pt2d b)
        {
            return Add(p, b);
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
            double halfX = sizeX / 2;
            double halfY = sizeY / 2;

            Pt2d[] result = new Pt2d[4];
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

        public static Pt2d[] RectangleCentered(Pln2d center, double sizeX, double sizeY)
        {
            double halfX = sizeX / 2;
            double halfY = sizeY / 2;

            Pt2d[] result = new Pt2d[4];

            result[0] = center.OriginPt + (center.Xaxis * -halfX) + (center.Yaxis * -halfY);
            result[1] = center.OriginPt + (center.Xaxis * halfX) + (center.Yaxis * -halfY);
            result[2] = center.OriginPt + (center.Xaxis * halfX) + (center.Yaxis * halfY);
            result[3] = center.OriginPt + (center.Xaxis * -halfX) + (center.Yaxis * halfY);

            return result;
        }

        public static Pt2d[] RectangleCenteredChamfered(Pt2d center, double sizeX, double sizeY, double radius)
        {
            double halfX = sizeX / 2;
            double halfXR = halfX - radius;
            double halfY = sizeY / 2;
            double halfYR = halfY - radius;

            Pt2d[] boundaryPts = new Pt2d[8];
            boundaryPts[0] = new Pt2d(-halfX, -halfYR);
            boundaryPts[1] = new Pt2d(-halfXR, -halfY);
            boundaryPts[2] = new Pt2d(halfXR, -halfY);
            boundaryPts[3] = new Pt2d(halfX, -halfYR);
            boundaryPts[4] = new Pt2d(halfX, halfYR);
            boundaryPts[5] = new Pt2d(halfXR, halfY);
            boundaryPts[6] = new Pt2d(-halfXR, halfY);
            boundaryPts[7] = new Pt2d(-halfX, halfYR);

            return boundaryPts;
        }

        public static Pt2d[] RectangleCenteredChamfered(Pln2d center, double sizeX, double sizeY, double radius)
        {
            double halfX = sizeX / 2;
            double halfXR = halfX - radius;
            double halfY = sizeY / 2;
            double halfYR = halfY - radius;

            Pt2d[] boundaryPts = new Pt2d[8];
            boundaryPts[0] = center.OriginPt + (center.Xaxis * -halfX) + (center.Yaxis * -halfYR);
            boundaryPts[1] = center.OriginPt + (center.Xaxis * -halfXR) + (center.Yaxis * -halfY);
            boundaryPts[2] = center.OriginPt + (center.Xaxis * halfXR) + (center.Yaxis * -halfY);
            boundaryPts[3] = center.OriginPt + (center.Xaxis * halfX) + (center.Yaxis * -halfYR);
            boundaryPts[4] = center.OriginPt + (center.Xaxis * halfX) + (center.Yaxis * halfYR);
            boundaryPts[5] = center.OriginPt + (center.Xaxis * halfXR) + (center.Yaxis * halfY);
            boundaryPts[6] = center.OriginPt + (center.Xaxis * -halfXR) + (center.Yaxis * halfY);
            boundaryPts[7] = center.OriginPt + (center.Xaxis * -halfX) + (center.Yaxis * halfYR);

            return boundaryPts;
        }

        /// <summary>
        /// returns the distance between two points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(Pt2d a, Pt2d b)
        {
            return Math.Sqrt((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));
        }

        /// <summary>
        /// returns the angle between two points relative to a center point
        /// </summary>
        /// <param name="centerPt"></param>
        /// <param name="startPt"></param>
        /// <param name="endPt"></param>
        /// <returns></returns>
        public static double Angle(Pt2d centerPt, Pt2d startPt, Pt2d endPt)
        {
            Vec2d start = new Vec2d(centerPt, startPt);
            Vec2d end = new Vec2d(centerPt, endPt);
            return Vec2d.Angle(start, end);
        }

        /// <summary>
        /// returns a new Pt2d with the sum of two Pt2d coordinates
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Pt2d Add(Pt2d a, Pt2d b)
        {
            return new Pt2d(a.X + b.X, a.Y + b.Y);
        }

        public static Pt2d Add(Pt2d a, Vec2d v)
        {
            return new Pt2d(a.X + v.X, a.Y + v.Y);
        }

        public static Pt2d Multiply(Pt2d a, Pt2d b)
        {
            return new Pt2d(a.X * b.X, a.Y * b.Y);
        }

        public static Pt2d Multiply(Pt2d a, double b)
        {
            return new Pt2d(a.X * b, a.Y * b);
        }
    }
}
