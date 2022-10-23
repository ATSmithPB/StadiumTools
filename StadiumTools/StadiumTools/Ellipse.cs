using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace StadiumTools
{
    public struct Ellipse : ICurve
    {
        //Properties
        /// <summary>
        /// The plane of the ellipse where the origin is the ellipse center
        /// </summary>
        public Pln3d Center { get; set; }
        /// <summary>
        /// the length of the ellipse semi-major axis, aligned to Center.XAxis 
        /// </summary>
        public double RadiusX { get; set; }
        /// <summary>
        /// the length of the ellipse semi-minor axis, aligned to Center.YAxis 
        /// </summary>
        public double RadiusY { get; set; }
        /// <summary>
        /// the start point of the ellipse
        /// </summary>
        public Pt3d Start { get; set; }
        /// <summary>
        /// the end point of the ellipse
        /// </summary>
        public Pt3d End { get; set; }

        //Constructors
        /// <summary>
        /// Constructs an ellipse on 2d plane from two radaii
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radiusX"></param>
        /// <param name="radiusY"></param>
        public Ellipse(Pln2d center, double radiusX, double radiusY)
        {
            Center = new Pln3d(center);
            RadiusX = radiusX;
            RadiusY = radiusY;
            Start = new Pt3d(new Pt2d(radiusX, 0), this.Center);
            End = this.Start;
        }

        public Ellipse(Pln3d center, double radiusX, double radiusY)
        {
            Center = center;
            RadiusX = radiusX;
            RadiusY = radiusY;
            Start = new Pt3d(new Pt2d(radiusX, 0.0), center);
            End = this.Start;
        }

        //Methods
        /// <summary>
        /// returns the Pt3d midpoint of a line
        /// </summary>
        /// <returns>Pt3d</returns>
        public Pt3d Midpoint()
        {
            return new Pt3d(new Pt2d(-this.RadiusX, 0.0), this.Center);
        }

        /// <summary>
        /// returns a Pt3d along a line at a specified parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Pt3d</returns>
        public Pt3d PointAt(double parameter)
        {
            double pTau = parameter * (Math.PI * 2);
            double x = (this.RadiusX * Math.Cos(pTau)) * this.Center.Xaxis.X;
            double y = (this.RadiusY * Math.Sin(pTau)) * this.Center.Yaxis.Y;
            double xCoord = this.Center.OriginPt.X + x;
            double yCoord = this.Center.OriginPt.Y + y;
            double zCoord = this.Center.OriginPt.Z;
            return new Pt3d(xCoord, yCoord, zCoord);
        }

        /// <summary>
        /// calculates the length of a line
        /// </summary>
        /// <returns>double</returns>
        public double Length()
        {
            double a = this.RadiusX;
            double b = this.RadiusY;
            return Math.PI * (3 * (a + b) - Math.Sqrt((3 * a + b) * (a + 3 * b)));
        }

        /// <summary>
        /// returns true if successful. out offset ICurve. A negative value will offset towards ellipse center. 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="offsetCurve"></param>
        /// <returns></returns>
        public bool Offset(double distance, out ICurve offsetCurve)
        {
            bool result = this.Offset(distance, out Ellipse offsetEllipse);
            offsetCurve = offsetEllipse;
            return result;
        }

        /// <summary>
        /// returns true if successful. out offset Ellipse. A negative value will offset towards ellipse center. 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="offsetEllipse"></param>
        /// <returns></returns>
        public bool Offset(double distance, out Ellipse offsetEllipse)
        {
            offsetEllipse = new Ellipse(this.Center, this.RadiusX + distance, this.RadiusY + distance);
            return true;
        }

        public static Pt3d[] Divide(Ellipse ellipse, int segmentCount)
        {
            if (segmentCount < 3)   
            {
                throw new ArgumentException($"Error: segmentCount [{segmentCount}] must be >2. Cannot divide ellipse into less than three segments");
            }

            Pt3d[] result = new Pt3d[segmentCount];
            double segmentParam = 1 / Convert.ToDouble(segmentCount);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ellipse.PointAt(segmentParam + (segmentParam * i));
            }

            return result;
        }
    }
}
