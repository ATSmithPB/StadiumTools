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
        /// Length of vector
        /// </summary>
        public double L { get; set; }

        //Constructor

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
            this.L = v.L;
        }

        /// <summary>
        /// Construct a 2d vector from its components
        /// </summary>
        /// <param name="h"></param>
        /// <param name="v"></param>
        /// <param name="l"></param>
        public Vec2d(double x, double y, double l)
        {
            this.X = x;
            this.Y = y;
            this.L = l;
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
            this.L = Sqrt((end.X - start.X) * (end.X - start.X) + (end.Y - start.Y) * (end.Y - start.Y));
        }

        /// <summary>
        /// Construct a 2d vector from a Spectator object
        /// </summary>
        /// <param name="spec"></param>
        public Vec2d(Spectator spec)
        {
            this.X = spec.POF.Y - spec.Loc2d.Y;
            this.Y = spec.POF.Y - spec.Loc2d.Y;
            this.L = Sqrt((spec.POF.X - spec.Loc2d.X) * (spec.POF.X - spec.Loc2d.X) + (spec.POF.Y - spec.Loc2d.Y) * (spec.POF.Y - spec.Loc2d.Y));
        }

        //Methods

     

    }
}
