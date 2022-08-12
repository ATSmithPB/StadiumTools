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

        //Constructors
        public Arc(Pln2d plane, double radius, double angle)
        {
            Plane = new Pln3d(plane);
            Radius = radius;
            Domain = new Domain(0.0, angle);
            IsValid = ValidateDomain(angle);
        }

        public Arc(Pln3d plane, double radius, double angle)
        {
            Plane = plane;
            Radius = radius;
            Domain = new Domain(0.0, angle);
            IsValid = ValidateDomain(angle);
        }

        public Arc(Pln3d plane, double radius, Domain domain)
        {
            Plane = plane;
            Radius = radius;
            Domain = domain;
            IsValid = ValidateDomain(this.Domain.Length);
        }

        public Arc(Pln3d plane, double radius, double domainStart, double domainEnd)
        {
            Plane = plane;
            Radius = radius;
            Domain = new Domain(domainStart, domainEnd);
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
            if (domain < 2 * Math.PI && domain > -2 * Math.PI)
            {
                result = true;
            }
            return result;
        }
    }
}
