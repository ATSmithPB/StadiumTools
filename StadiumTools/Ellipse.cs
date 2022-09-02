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
        public Pt3d PointOn(double parameter)
        {
            double pTau = parameter * (Math.PI * 2);
            double x = this.RadiusX * Math.Cos(pTau);
            double y = this.RadiusY * Math.Sin(pTau);
            return new Pt3d(new Pt2d(x, y), this.Center);
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

    }
}
