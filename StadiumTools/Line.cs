using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    //Properties
    public struct Line : ICurve
    {
        /// <summary>
        /// Start point of line
        /// </summary>
        public Pt3d Start { get; set; }
        /// <summary>
        /// End point in line
        /// </summary>
        public Pt3d End { get; set; }

        //Constructors
        public Line(Pt2d start, Pt2d end)
        {
            this.Start = new Pt3d(start, 0.0);
            this.End = new Pt3d(end, 0.0);
        }

        public Line(Pt3d start, Pt3d end)
        {
            this.Start = start;
            this.End = end;
        }

        //Methods
        public static Line[] RectangleCentered(Pln2d plane, double sizeX, double sizeY)
        {
            Line[] result = new Line[4];
            Pt2d[] pts = Pt2d.RectangleCentered(plane, sizeX, sizeY);
            result[0] = new Line(pts[0], pts[1]);
            result[1] = new Line(pts[1], pts[2]);
            result[2] = new Line(pts[2], pts[3]);
            result[3] = new Line(pts[3], pts[0]);
            return result;
        }

        public static Line[] RectangleCentered(Pln3d plane, double sizeX, double sizeY)
        {
            Line[] result = new Line[4];
            Pt3d[] pts = Pt3d.RectangleCentered(plane, sizeX, sizeY);
            result[0] = new Line(pts[0], pts[1]);
            result[1] = new Line(pts[1], pts[2]);
            result[2] = new Line(pts[2], pts[3]);
            result[3] = new Line(pts[3], pts[0]);
            return result;
        }

        /// <summary>
        /// returns the Pt3d midpoint of a line
        /// </summary>
        /// <returns>Pt3d</returns>
        public Pt3d Midpoint()
        {
            return Pt3d.Midpoint(this.Start, this.End);
        }

        /// <summary>
        /// returns a Pt3d along a line at a specified parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Pt3d</returns>
        public Pt3d PointAt(double parameter)
        {
            return Pt3d.Tween2(this.Start, this.End, parameter);
        }

        /// <summary>
        /// calculates the length of a line
        /// </summary>
        /// <returns>double</returns>
        public double Length()
        {
            return Pt3d.Distance(this.Start, this.End);
        }

        /// <summary>
        /// returns true if Offset is success, Outs the offset of a Line
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="offsetCurve"></param>
        /// <returns>bool</returns>
        public bool Offset(double distance, out ICurve offsetCurve)
        {
            bool result = this.Offset(distance, out Line offsetLine);
            offsetCurve = offsetLine;
            return result;
        }

        /// <summary>
        /// returns true if Offset is success, Outs the offset of a Line
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="offsetLine"></param>
        /// <returns>bool</returns>
        public bool Offset(double distance, out Line offsetLine)
        {
            Vec3d axisNormalized = Vec3d.Normalize(new Vec3d(this.Start, this.End));
            Vec3d perp = Vec3d.CrossProduct(axisNormalized, Vec3d.ZAxis);
            Vec3d perpScaled = Vec3d.Scale(perp, distance);
            offsetLine = new Line(this.Start + perpScaled, this.End + perpScaled);
            return true;
        }

    }
}
