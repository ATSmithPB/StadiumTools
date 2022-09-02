using System.Collections.Generic;
using System;
using static System.Math;

namespace StadiumTools
{
    public struct Arc : ICloneable, ICurve
    { 
        //Properties
        /// <summary>
        /// True if the Arc is valid
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// Plane of Circle where the plane origin is he circle centerpoint 
        /// </summary>
        public Pln3d Plane { get; set; }
        /// <summary>
        /// Radius of the circle
        /// </summary>
        public double Radius { get; set; }
        /// <summary>
        /// Domain of Arc. Start Angle and End Angle in Radians
        /// </summary>
        public Domain Domain { get; set; }
        /// <summary>
        /// Arc Start Point
        /// </summary>
        public Pt3d Start { get; set; }
        /// <summary>
        /// Arc End Point
        /// </summary>
        public Pt3d End { get; set; }

        //Constructors
        public Arc(Pln2d plane, double radius, double angleRadians)
        {
            Plane = new Pln3d(plane);
            Radius = radius;
            Domain = new Domain(0.0, angleRadians);
            Start = new Pt3d(new Pt2d(radius, 0.0), this.Plane);
            End = Pt3d.Rotate(this.Plane, this.Start, angleRadians);
            IsValid = ValidateDomain(angleRadians);
        }

        public Arc(Pln3d plane, double radius, double angleRadians)
        {
            Plane = plane;
            Radius = radius;
            Domain = new Domain(0.0, angleRadians);
            Start = new Pt3d(new Pt2d(radius, 0.0), this.Plane);
            End = Pt3d.Rotate(this.Plane, this.Start, angleRadians);
            IsValid = ValidateDomain(angleRadians);
        }

        public Arc(Pln3d plane, double radius, Domain domain)
        {
            Plane = plane;
            Radius = radius;
            Domain = domain;
            Pt2d refPt = new Pt2d(radius, 0);
            Start = new Pt3d(Pt2d.Rotate(refPt, domain.T0), plane); //simplify me
            End = new Pt3d(Pt2d.Rotate(refPt, domain.T1), plane); //simplify me
            IsValid = ValidateDomain(this.Domain.Length);
        }

        public Arc(Pln3d plane, double radius, double domainStart, double domainEnd)
        {
            Plane = plane;
            Radius = radius;
            Domain = new Domain(domainStart, domainEnd);
            Pt2d refPt = new Pt2d(radius, 0);//simplify me
            Start = new Pt3d(Pt2d.Rotate(refPt, domainStart), plane); //simplify me
            End = new Pt3d(Pt2d.Rotate(refPt, domainEnd), plane); //simplify me
            IsValid = ValidateDomain(this.Domain.Length);
        }

        public Arc(Pln2d plane, double radius, double domainStart, double domainEnd)
        {
            Plane = plane.ToPln3d(Pln2d.XYPlane);
            Radius = radius;
            Domain = new Domain(domainStart, domainEnd);
            Pt2d refPt = new Pt2d(radius, 0);//simplify me
            Start = new Pt3d(Pt2d.Rotate(refPt, domainStart), new Pln3d(plane)); //simplify me
            End = new Pt3d(Pt2d.Rotate(refPt, domainEnd), new Pln3d(plane)); //simplify me
            IsValid = ValidateDomain(this.Domain.Length);
        }

        public Arc(Pt2d center, Pt2d start, Pt2d end)
        {
            Plane = new Pln3d(new Pln2d(center, start));
            Radius = Pt2d.Distance(center, start);
            double angleRadians = Pt2d.Angle(center, start, end);
            Domain = new Domain(0.0, angleRadians);
            Start = new Pt3d(start, this.Plane);
            End = new Pt3d(end, this.Plane);
            IsValid = ValidateDomain(this.Domain.Length);
        }

        //Methods
        public object Clone()
        {
            //Shallow copy
            return (Arc)this.MemberwiseClone();
        }

        /// <summary>
        /// Returns true if the input domain is between positive and negative 2 * Pi
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static bool ValidateDomain(double domain)
        {
            bool result = false;
            if (domain <= 2 * Math.PI && domain >= -2 * Math.PI)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// returns the Pt3d midpoint of an arc
        /// </summary>
        /// <returns>Pt3d</returns>
        public Pt3d Midpoint()
        {
            return Pt3d.Rotate(this.Plane, this.Start, this.Domain.T1 / 2);
        }

        /// <summary>
        /// returns a Pt3d along an arc at a specified parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Pt3d</returns>
        public Pt3d PointOn(double parameter)
        {
            double tauP = parameter * (Math.PI * 2);
            return Pt3d.Rotate(this.Plane, this.Start, tauP);
        }

        /// <summary>
        /// calculates the length of an arc
        /// </summary>
        /// <returns>double</returns>
        public double Length()
        {
            return Math.Abs(Domain.Length * this.Radius);
        }
    }

}

