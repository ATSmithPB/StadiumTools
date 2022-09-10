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
        /// <summary>
        /// The angle of the arc domain from its center in radians
        /// </summary>
        public double Angle { get; set;}

        //Constructors
        public Arc(Pln2d plane, double radius, double angleRadians)
        {
            Plane = new Pln3d(plane);
            Radius = radius;
            Domain = new Domain(0.0, angleRadians);
            Start = new Pt3d(new Pt2d(radius, 0.0), this.Plane);
            End = Pt3d.Rotate(this.Plane, this.Start, angleRadians);
            IsValid = ValidateDomain(angleRadians);
            Angle = angleRadians;
        }

        public Arc(Pln3d plane, double radius, double angleRadians)
        {
            Plane = plane;
            Radius = radius;
            Domain = new Domain(0.0, angleRadians);
            Start = new Pt3d(new Pt2d(radius, 0.0), this.Plane);
            End = Pt3d.Rotate(this.Plane, this.Start, angleRadians);
            IsValid = ValidateDomain(angleRadians);
            Angle = angleRadians;
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
            Angle = this.Domain.Length;
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
            Angle = this.Domain.Length;
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
            Angle = this.Domain.Length;
        }

        /// <summary>
        /// construct an Arc from Pt2d origin, start and end. (END POINT SETS ANGLE/DOMAIN ONLY)
        /// </summary>
        /// <param name="center"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Arc(Pt2d center, Pt2d start, Pt2d end)
        {
            Plane = new Pln3d(new Pln2d(center, start));
            Radius = Pt2d.Distance(center, start);
            double angleRadians = Pt2d.Angle(center, start, end);
            Domain = new Domain(0.0, angleRadians);
            Start = new Pt3d(start, this.Plane);
            End = Pt3d.Rotate(this.Plane, this.Start, angleRadians);
            IsValid = ValidateDomain(this.Domain.Length);
            Angle = angleRadians;
        }

        /// <summary>
        /// construct an arc from a startPt, endPt and radius. 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="radius"></param>
        public Arc(Pt3d start, Pt3d end, double radius)
        {
            if (radius * 2 <= Pt3d.Distance(start, end))
            {
                throw new Exception("Radius too small. Radius*2 must be > the distance between start and end points");
            }
            Pt3d cen = Pt3d.OffsetMidpoint(start, end, radius, out Pt3d midpoint);
            Vec3d x = new Vec3d(cen, start);
            Vec3d y = Vec3d.CrossProduct(x, Vec3d.ZAxis);
            Plane = new Pln3d(cen, x, y);
            Radius = radius;
            double angleRadians = Pt3d.Angle(this.Plane, start, end);
            Domain = new Domain(0.0, angleRadians);
            Start = start;
            End = end;
            IsValid = ValidateDomain(this.Domain.Length);
            Angle = angleRadians;
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
            double reParameter = parameter * this.Angle;
            return Pt3d.Rotate(this.Plane, this.Start, reParameter);
        }

        /// <summary>
        /// calculates the length of an arc
        /// </summary>
        /// <returns>double</returns>
        public double Length()
        {
            return Math.Abs(Domain.Length * this.Radius);
        }

        /// <summary>
        /// returns division points on an arc based on a specified chord length and if a division point is coincidnet with the arc midpoint
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="divLength"></param>
        /// <param name="pointAtMiddle"></param>
        /// <returns>Pt3d[]</returns>
        public static Pt3d[] DivideLinearCentered(Arc arc, double divLength, bool pointAtMiddle) //clean me
        {
            if (divLength >= Pt3d.Distance(arc.Start, arc.End) || divLength <= 0)
            {
                throw new Exception("Division length must be greater than zero and smaller than the distance from Arc.Start point to Arc.End point.");
            }
            double theta = Circle.ChordAngle(arc.Radius, divLength);
            List<double> delta0 = new List<double>();
            bool outOfBounds = false;
            double first = theta /2;
            double mid = arc.Angle / 2;

            if (pointAtMiddle)
            {
                first = theta;
                delta0.Add(mid);
            }

            int divs = 0;
            while (!outOfBounds)
            {
                double pos = mid + first + (theta * divs);
                if (arc.Domain.Contains(pos))
                {
                    delta0.Add(pos);
                    delta0.Add(mid - first - (theta * divs));
                    divs++;
                }
                else
                {
                    outOfBounds = true;
                }
            }

            delta0.Sort();

            Pt3d[] result = new Pt3d[delta0.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Pt3d.Rotate(arc.Plane, arc.Start, delta0[i]);
            }
            return result;
        }

    }

}

