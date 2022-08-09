using System;
using System.Collections.Generic;

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

    }
}
