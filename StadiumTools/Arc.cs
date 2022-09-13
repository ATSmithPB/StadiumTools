using System.Collections.Generic;
using System;
using static System.Math;
using Rhino.Commands;
using System.Runtime.CompilerServices;

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
            Angle = angleRadians;
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
            Angle = Pt2d.Angle(center, start, end);
            Domain = new Domain(0.0, this.Angle);
            Start = new Pt3d(start, this.Plane);
            End = Pt3d.Rotate(this.Plane, this.Start, this.Angle);
            IsValid = ValidateDomain(this.Domain.Length);
        }

        public Arc(Pt3d center, Pt3d start, Pt3d end)
        {
            Plane = new Pln3d(center, start, end);
            Radius = Pt3d.Distance(center, start);
            Angle = Pt3d.Angle(this.Plane, start, end);
            Domain = new Domain(0.0, this.Angle);
            Start = start;
            End = Pt3d.Rotate(this.Plane, this.Start, this.Angle);
            IsValid = ValidateDomain(this.Domain.Length);
        }

        /// <summary>
        /// construct an arc from a startPt, endPt and filletRadius. 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="radius"></param>
        public Arc(Pt3d start, Pt3d end, double radius)
        {
            if (Math.Abs(radius) * 2 <= Pt3d.Distance(start, end))
            {
                throw new Exception("Radius too small. Radius*2 must be > the distance between start and end points");
            }
            Pt3d cen = Pt3d.OffsetMidpoint(start, end, radius, out Pt3d midpoint);
            Vec3d x = new Vec3d(cen, start);
            Vec3d y = Vec3d.CrossProduct(Vec3d.ZAxis, x);
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
        public Pt3d PointAt(double parameter)
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

        /// <summary>
        /// returns true if successful. out offset ICurve. A negative value will offset towards arc center. 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="offsetCurve"></param>
        /// <returns>ICurve</returns>
        public bool Offset(double distance, out ICurve offsetCurve)
        {
            bool result = this.Offset(distance, out Arc offsetArc);
            offsetCurve = offsetArc;
            return result;
        }

        /// <summary>
        /// returns true if successful. out offset Arc. A negative value will offset towards arc center. 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="offsetArc"></param>
        /// <returns>Arc</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool Offset(double distance, out Arc offsetArc)
        {
            if (distance <= -this.Radius)
            {
                throw new ArgumentException($"offset distance [{distance}] must exceed -1 * filletRadius [{-this.Radius}]");
            }
            offsetArc = new Arc(this.Plane, this.Radius + distance, this.Domain);
            return true;
        }

        public static Arc[] FilletTrim(Arc arc0, Arc arc1, double filletRadius, double tolerance)
        {
            if (filletRadius >= arc0.Radius || filletRadius >= arc0.Radius)
            {
                throw new ArgumentException("Error: Fillet Radius cannot exceed the radius of either Arc being filleted");
            }
            Arc fillet = Arc.Fillet(arc0, arc1, filletRadius, tolerance);

            //Add arc0Trimmed and arc1Trimmed

            return new Arc[3] { arc0, fillet, arc1 };
        }

        /// <summary>
        /// returns the Arc that results from a fillet operation on two other arcs. Does not trim input arcs
        /// </summary>
        /// <param name="arc0"></param>
        /// <param name="arc1"></param>
        /// <param name="radius"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public static Arc Fillet(Arc arc0, Arc arc1, double radius, double tolerance)
        {
            int connection = IsConnected(arc0, arc1, tolerance, out Pt3d cornerPt);
            if (connection == 0)
            {
                throw new ArgumentException("the two arcs are not connected");
            }

            arc0.Offset(-radius, out Arc offsetArc0);
            arc1.Offset(-radius, out Arc offsetArc1);
            int xcnt = Intersect(offsetArc0, offsetArc1, tolerance, out Pt3d P0, out Pt3d P1); //one of these points is correct

            Pt3d circleFilletCenPt = cornerPt.CloserPt(P0, P1);

            Pln3d circleFilletCen = new Pln3d(circleFilletCenPt, arc0.Plane.Zaxis);
            Circle circleFillet = new Circle(circleFilletCen, radius);
            Circle circle0 = new Circle(arc0);
            Circle circle1 = new Circle(arc1);

            int result0 = Circle.Intersect(circleFillet, circle0, tolerance, out Pt3d iPt0, out Pt3d iPt1);
            if (result0 != 1)
            {
                throw new Exception($"Error: Fillet Circle and Arc0 intersection event[{result0}] did not result in 1 point");
            }
            int result1 = Circle.Intersect(circleFillet, circle1, tolerance, out Pt3d iPt2, out Pt3d iPt3);
            if (result1 != 1)
            {
                throw new Exception($"Error: Fillet Circle and Arc1 intersection event [{result1}] did not result in 1 point");
            }

            Arc result = new Arc(circleFilletCenPt, iPt0, iPt2); //Or P1

            return result;
        }

        //public static Arc Fillet(Arc arc0, Arc arc1, double filletRadius)
        //{
        //    //Wishlist: Fillet interecting, but not connected arcs
        //}

        public static int Intersect(Arc A0, Arc A1, double tolerance, out Pt3d P0,  out Pt3d P1) //Re-implemented from OpenNurbs
        {
            P0 = P1 = new Pt3d();
            Pt3d[] P = new Pt3d[] { P0, P1 };
            int xcnt = 0;

            Pt3d[] CCX = new Pt3d[2];
            int cxcnt = Circle.Intersect(new Circle(A0), new Circle(A1), tolerance, out CCX[0], out CCX[1]);
            if ( cxcnt < 3)
            {
                for (int i = 0; i < cxcnt; i++)
                {
                    double t = 0.0;
                    if (A0.ClosestPointTo(CCX[i], t))
                    {
                        if (CCX[i].DistanceTo(A0.PointAt(t)) < tolerance)
                        {
                            if (A1.ClosestPointTo(CCX[i], t))
                            {
                                if (CCX[i].DistanceTo(A1.PointAt(t)) < tolerance)
                                {
                                    P[xcnt++] = CCX[i];
                                }
                            }
                        }
                    }
                }
            }
            else if (cxcnt == 3)
            {
                Arc[] Size = new Arc[] { A0, A1 };     //Size[0]<=Size[1]
                if (A0.Domain.Length > A1.Domain.Length)
                {
                    Size[0] = A1;
                    Size[1] = A0;
                }
                // Match ends of smaller to larger arc
                double[] LittleEndMatch = new double[2];  // relative to Big ArcBig,  0-start, 1-end , .5 (interior),  -1 ( exterior)

                Domain BigInterior = Size[1].Domain;    // interior domain of big arc
                if (!BigInterior.Expand(-tolerance / Size[1].Radius))         // circles are not degenerate
                BigInterior = Domain.Singleton(Size[1].Domain.Mid);
    
                for (int ei = 0; ei < 2; ei++)
                {
                    double t = 0.0; 
                    bool eiBool = Convert.ToBoolean(ei);
                    Pt3d LittleEnd = eiBool ? Size[0].End : Size[0].Start;
                    if (Size[1].ClosestPointTo(LittleEnd, t))
                    {
                        switch (BigInterior.Clamp(t))
                        {
                            case(-1):
                                {
                                    if (Size[1].Start.DistanceTo(LittleEnd) < tolerance)
                                    {
                                        LittleEndMatch[ei] = 0;// start
                                    }
                                    else
                                    {
                                        LittleEndMatch[ei] = -1;// exterior
                                    }
                                    break;
                                }
                            case(0):
                                {
                                    LittleEndMatch[ei] = .5;// interior
                                    break;
                                }
                            case(1):
                                {
                                    if (Size[1].End.DistanceTo(LittleEnd) < tolerance)
                                    {
                                        LittleEndMatch[ei] = 1;// end
                                    }
                                    else
                                    {
                                        LittleEndMatch[ei] = -1;// exterior
                                    }
                                    break;
                                }
                        }
                    }
                }
                if (LittleEndMatch[0] == .5 || LittleEndMatch[1] == .5)
                {
                    xcnt = 3;// an interior match means an overlap
                }
                else if (LittleEndMatch[0] == -1 && LittleEndMatch[1] == -1)
                {
                    xcnt = 0;// both points exterior means  intersection is empty
                }
                else if (LittleEndMatch[0] == -1)
                {
                    P[xcnt++] = Size[0].End;// if start is exterior end must be an intersection point
                }
                else if (LittleEndMatch[1] == -1)
                {
                    P[xcnt++] = Size[0].Start;
                }
                else
                {
                    // Both endpoints match endpoints of Big
                    // LittleEndMatch[ei] \in { 0, 1 }
                    bool Orientation_agree = (A0.Plane.Zaxis * A1.Plane.Zaxis > 0);// true if  the orientations agree
                    if (LittleEndMatch[0] != LittleEndMatch[1])
                    {
                        if (Orientation_agree == (LittleEndMatch[0] == 1.0))
                        {
                            P[xcnt++] = Size[0].Start;
                            P[xcnt++] = Size[0].End;
                        }
                        else
                        {
                            xcnt = 3;
                        }
                    }
                    else
                    {
                        // Degenerate cases
                        if (Size[0].Start.DistanceTo(Size[0].End) < tolerance)
                            P[xcnt++] = Size[0].Start;
                        else
                            xcnt = 3;
                    }
                }
            }
  
          return xcnt;
        }

        /// <summary>
        /// Get the point on the arc that is closest to a given point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>Pt3d</returns>
        public Pt3d ClosestPointTo(Pt3d pt)
        {
          double tParam = this.Domain.T0;
          ClosestPointTo(pt, tParam);
          return PointAt(tParam);
        }
         
        /// <summary>
        /// returns parameters of point on arc that is closest to given point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="tParam"></param>
        /// <returns></returns>
        public bool ClosestPointTo(Pt3d pt, double tParam) //reimplemented from OpenNurbs
        {
            double s = 0.0;
            double tau = 2.0 * Math.PI;
            Circle circle = new Circle(this);
            bool rc = circle.ClosestPointTo(pt, s);
            if (rc)
            {
                s -= this.Domain.T0;
                while (s < 0.0)
                {
                    s += tau;
                }
                while (s >= tau)
                {
                    s -= tau;
                }

                double s1 = this.Domain.Length;
                if (s < 0.0)
                {
                    s = 0.0;
                }
                if (s > s1)
                {
                    if (s > 0.5 * s1 + Math.PI)
                    {
                        s = 0.0;
                    }
                    else
                    {
                        s = s1;
                    }
                }
                if (tParam != 0.0)
                {
                    tParam = this.Domain.T0 + s;
                }
            }
            return rc;
        }

        public static int IsConnected(Arc arc0, Arc arc1, double tolerance, out Pt3d connectionPt)
        {
            int result = 0;
            if (Pt3d.IsCoincident(arc0.Start, arc1.Start, tolerance))
            {
                connectionPt = arc0.Start;
                result = 1;
            }
            else if (Pt3d.IsCoincident(arc0.Start, arc1.End, tolerance))
            {
                connectionPt = arc0.Start;
                result = 2;
            }
            else if (Pt3d.IsCoincident(arc0.End, arc1.Start, tolerance))
            {
                connectionPt = arc0.End;
                result = 3;
            }
            else if (Pt3d.IsCoincident(arc0.End, arc1.End, tolerance))
            {
                connectionPt = arc0.End;
                result = 4;
            }
            else
            {
                connectionPt = Pt3d.Origin;
                result = 0;
            }

            return result;
        }

    }

}

