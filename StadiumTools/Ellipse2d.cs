using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace StadiumTools
{
    public struct Ellipse2d : ICurve
    {
        //Properties
        public Pln2d Center { get; set; }
        public double RadiusX { get; set; }
        public double RadiusY { get; set; }

        //Constructors
        /// <summary>
        /// Constructs an ellipse on 2d plane from two radaii
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radiusX"></param>
        /// <param name="radiusY"></param>
        public Ellipse2d(Pln2d center, double radiusX, double radiusY)
        {
            Center = center;
            RadiusX = radiusX;
            RadiusY = radiusY;
        }

        //Methods
        /// <summary>
        /// returns the Pt3d midpoint of a line
        /// </summary>
        /// <returns>Pt3d</returns>
        Pt3d ICurve.Midpoint()
        {
            //return Pt3d.Midpoint(this.Start, this.End);
            return Pt3d.Origin;
        }

        /// <summary>
        /// returns a Pt3d along a line at a specified parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Pt3d</returns>
        Pt3d ICurve.PointOn(double parameter)
        {
            //return Pt3d.Tween2(this.Start, this.End, parameter);
            return Pt3d.Origin;
        }

        /// <summary>
        /// calculates the length of a line
        /// </summary>
        /// <returns>double</returns>
        double ICurve.Length()
        {
            double a = this.RadiusX;
            double b = this.RadiusY;
            return Math.PI * (3 * (a + b) - Math.Sqrt((3 * a + b) * (a + 3 * b)));
        }


    }
}
