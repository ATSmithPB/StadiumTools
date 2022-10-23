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
            IsValid = false;
            Plane = new Pln3d(plane);
            Radius = radius;
            Domain = new Domain(0.0, angleRadians);
            Start = new Pt3d(new Pt2d(radius, 0.0), this.Plane);
            End = Pt3d.Rotate(this.Plane, this.Start, angleRadians);
            Angle = angleRadians;
            GetValidity(this);
        }

        public Arc(Pln3d plane, double radius, double angleRadians)
        {
            IsValid = false;
            Plane = plane;
            Radius = radius;
            Angle = angleRadians;
            Domain = new Domain(0.0, angleRadians);
            Start = new Pt3d(new Pt2d(radius, 0.0), this.Plane);
            End = Pt3d.Rotate(this.Plane, this.Start, angleRadians);
            GetValidity(this);

        }

        public Arc(Pln3d plane, double radius, Domain domain)
        {
            IsValid = false;
            Plane = plane;
            Radius = radius;
            Domain = domain;
            Pt2d refPt = new Pt2d(radius, 0);
            Start = new Pt3d(Pt2d.Rotate(refPt, domain.T0), plane); //simplify me
            End = new Pt3d(Pt2d.Rotate(refPt, domain.T1), plane); //simplify me
            Angle = this.Domain.Length;
            GetValidity(this);
        }

        public Arc(Pln3d plane, double radius, double domainStart, double domainEnd)
        {
            IsValid = false;
            Plane = plane;
            Radius = radius;
            Domain = new Domain(domainStart, domainEnd);
            Pt2d refPt = new Pt2d(radius, 0);//simplify me
            Start = new Pt3d(Pt2d.Rotate(refPt, domainStart), plane); //simplify me
            End = new Pt3d(Pt2d.Rotate(refPt, domainEnd), plane); //simplify me
            Angle = this.Domain.Length;
            GetValidity(this);
        }

        public Arc(Pln2d plane, double radius, double domainStart, double domainEnd)
        {
            IsValid = false;
            Plane = plane.ToPln3d(Pln2d.XYPlane);
            Radius = radius;
            Domain = new Domain(domainStart, domainEnd);
            Pt2d refPt = new Pt2d(radius, 0);//simplify me
            Start = new Pt3d(Pt2d.Rotate(refPt, domainStart), new Pln3d(plane)); //simplify me
            End = new Pt3d(Pt2d.Rotate(refPt, domainEnd), new Pln3d(plane)); //simplify me
            Angle = this.Domain.Length;
            GetValidity(this);
        }

        /// <summary>
        /// construct an Arc from Pt2d origin, start and end. (END POINT SETS ANGLE/DOMAIN ONLY)
        /// </summary>
        /// <param name="center"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Arc(Pt2d center, Pt2d start, Pt2d end)
        {
            IsValid = false;
            Plane = new Pln3d(new Pln2d(center, start));
            Radius = Pt2d.Distance(center, start);
            Angle = Pt2d.Angle(center, start, end);
            Domain = new Domain(0.0, this.Angle);
            Start = new Pt3d(start, 0.0);
            End = Pt3d.Rotate(this.Plane, this.Start, this.Angle);
            GetValidity(this);
        }

        public Arc(Pt3d center, Pt3d start, Pt3d end)
        {
            IsValid = false;
            Plane = new Pln3d(center, start, end);
            Radius = Pt3d.Distance(center, start);
            Angle = Pt3d.Angle(this.Plane, start, end);
            Domain = new Domain(0.0, this.Angle);
            Start = start;
            End = Pt3d.Rotate(this.Plane, this.Start, this.Angle);
            GetValidity(this);
        }

        /// <summary>
        /// construct an arc from a startPt, endPt and filletRadius. 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="radius"></param>
        public Arc(Pt3d start, Pt3d end, double radius)
        {
            IsValid = false;
            if (Math.Abs(radius) * 2 <= Pt3d.Distance(start, end))
            {
                throw new Exception("Error: Radius too small. Radius*2 must be > the distance between start and end points");
            }
            if (Pt3d.IsCoincident(start, end, UnitHandler.abstol))
            {
                throw new Exception("Error: Start and End are coincident");
            }
            Start = start;
            End = end;
            Radius = radius;
            double offset = Circle.ChordMidCen(radius, start.DistanceTo(end));
            Pt3d centerPt = Pt3d.OffsetMidpoint(end, start, offset, out Pt3d midpoint);
            Vec3d x = new Vec3d(centerPt, start);
            Vec3d y = Vec3d.CrossProduct(Vec3d.ZAxis, x);
            Plane = new Pln3d(centerPt, x, y);
            double angleRadians = Pt3d.Angle(this.Plane, start, end);
            Domain = new Domain(0.0, angleRadians);
            Angle = angleRadians;
            GetValidity(this);
        }

        //Methods
        public object Clone()
        {
            //Shallow copy
            return (Arc)this.MemberwiseClone();
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
                throw new Exception($"Division length [{divLength}] must be greater than zero and smaller than [{Pt3d.Distance(arc.Start, arc.End)}] the distance from Arc.Start point to Arc.End point.");
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

            delta0.Add(arc.Angle);
            delta0.Add(0.0);
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

        /// <summary>
        /// Fillet two connected arcs
        /// </summary>
        /// <param name="arc0"></param>
        /// <param name="arc1"></param>
        /// <param name="filletRadius"></param>
        /// <param name="tolerance"></param>
        /// <returns>ICurve[]</returns>
        /// <exception cref="ArgumentException"></exception>
        public static ICurve[] FilletTrim(Arc arc0, Arc arc1, double filletRadius, double tolerance)
        {
            if (filletRadius >= arc0.Radius || filletRadius >= arc0.Radius)
            {
                throw new ArgumentException("Error: Fillet Radius cannot exceed the radius of either Arc being filleted");
            }
            ICurve[] result = new ICurve[3];
            result[1] = Arc.Fillet(arc0, arc1, filletRadius, tolerance);
            result[0] = new Arc(arc0.Start, result[1].Start, arc0.Radius);
            result[2] = new Arc(result[1].End, arc1.End, arc1.Radius);

            return result;
        }

        /// <summary>
        /// returns resulting Arc from a fillet operation on two other arcs. Does not trim input arcs
        /// </summary>
        /// <param name="arc0"></param>
        /// <param name="arc1"></param>
        /// <param name="radius"></param>
        /// <param name="tolerance"></param>
        /// <returns>Arc</returns>
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
            Circle offsetCircle0 = new Circle(offsetArc0);
            Circle offsetCircle1 = new Circle(offsetArc1);
            int xcnt = Circle.Intersect(offsetCircle0, offsetCircle1, tolerance, out Pt3d[] cxcPts);

            if (xcnt != 2)
            {
                throw new ArgumentException($"Error: Arc0.Circle|Arc1.Circle intersection event[{xcnt}] did not result in 2 points");
            }

            Pt3d circleFilletCenPt = cornerPt.CloserPt(cxcPts[0], cxcPts[1]);
            Pln3d circleFilletCen = new Pln3d(circleFilletCenPt, arc0.Plane.Zaxis);
            Circle circleFillet = new Circle(circleFilletCen, radius);
            Circle circle0 = new Circle(arc0);
            Circle circle1 = new Circle(arc1);

            int result0 = Circle.Intersect(circleFillet, circle0, tolerance, out Pt3d[] iPts0);
            if (result0 != 1)
            {
                throw new Exception($"Error: Fillet Circle and Arc0 intersection event[{result0}] did not result in 1 point");
            }
            int result1 = Circle.Intersect(circleFillet, circle1, tolerance, out Pt3d[] iPts1);
            if (result1 != 1)
            {
                throw new Exception($"Error: Fillet Circle and Arc1 intersection event [{result1}] did not result in 1 point");
            }

            Arc result = new Arc(circleFilletCenPt, iPts0[0], iPts1[0]);
            return result;
        }

        //public static Arc Fillet(Arc arc0, Arc arc1, double filletRadius)
        //{
        //    //Wishlist: Fillet interecting, but not connected arcs
        //}

        public static int Intersect(Arc arc0, Arc arc1, double tolerance, out Pt3d[] intersectionPts)
        {
            int cxcFlag = Circle.Intersect(new Circle(arc0), new Circle(arc1), tolerance, out Pt3d[] cxcPts);
            int axaFlag = 0;

            if (cxcFlag != 1 && cxcFlag != 2)
            {
                throw new ArgumentException($"Error: Invalid Circle|Circle intersection flag [{cxcFlag}]. Must be 1 or 2");
            }
            else
            {
                List<Pt3d> iPts = new List<Pt3d>();
                for (int i = 0; i < cxcFlag; i++)
                {
                    double tParam0 = 0.0;
                    double tParam1 = 0.0;
                    if (arc0.ClosestPointTo(cxcPts[i], ref tParam0))
                    {
                        if (cxcPts[i].DistanceTo(arc0.PointAt(tParam0)) < tolerance)
                        {
                            if (arc1.ClosestPointTo(cxcPts[i], ref tParam1))
                            {
                                if (cxcPts[i].DistanceTo(arc1.PointAt(tParam1)) < tolerance)
                                {
                                    iPts[axaFlag] = cxcPts[i];
                                    axaFlag++;
                                }
                            }
                        }
                    }
                }
                intersectionPts = iPts.ToArray();
                return axaFlag;
            }
        }

        /// <summary>
        /// Get the point on the arc that is closest to a given point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>Pt3d</returns>
        /// <exception cref="ArgumentException"></exception>
        public Pt3d ClosestPointTo(Pt3d pt)
        {
          double tParam = 0.0;
          if (!ClosestPointTo(pt, ref tParam))
          {
                throw new ArgumentException("Arc.ClosestPointTo Failed!");
          }
          return PointAt(tParam);
        }

        /// <summary>
        /// returns parameters of point on arc that is closest to given point
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="tParam"></param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool ClosestPointTo(Pt3d pt, ref double tParam)
        {
            Circle circle = new Circle(this);
            if(!circle.ClosestPointTo(pt, out tParam))
            {
                throw new ArgumentException("Circle.ClosestPointTo Failed!");
            }
            return true;
        }

        /// <summary>
        /// check if the endpoints of two arcs are connected  
        /// </summary>
        /// <param name="arc0"></param>
        /// <param name="arc1"></param>
        /// <param name="tolerance"></param>
        /// <param name="connectionPt"></param>
        /// <returns>int</returns>
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

        private static bool GetValidity(Arc a)
        {
            if (a.Radius <= 0)
            {
                throw new ArgumentException($"Error: Radius must be non-negative and not equal to zero");
            }
            if (a.Domain.Length > (Math.PI * 2))
            {
                throw new ArgumentException($"Error: Domain exceeds Tau");
            }
            if (a.Domain.Length <= 0)
            {
                throw new ArgumentException($"Error: Domain must be non-negative and not equal to zero");
            }
            a.IsValid = true;
            return true;
        }

        /// <summary>
        /// divides an arc by a given number of segments and returns division points.
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="segmentCount"></param>
        /// <param name="tParams"></param>
        /// <returns>Pt3d[]</returns>
        public static Pt3d[] Divide(Arc arc, int segmentCount, out double[] tParams)
        {
            tParams = Divide(arc, segmentCount);
            Pt3d[] result = new Pt3d[tParams.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = arc.PointAt(tParams[i]);
            }
            return result;
        }

        /// <summary>
        /// divides an arc by a given number of segments and returns division parameters.
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="segmentCount"></param>
        /// <returns>double[]</returns>
        public static double[] Divide(Arc arc, int segmentCount)
        {
            if (segmentCount < 2)
            {
                throw new ArgumentException($"Error: segmentCount [{segmentCount}] must be >2. Cannot divide arc into less than two segments");
            }
            
            double[] result = new double[segmentCount - 1];
            double theta = arc.Angle / segmentCount;
            
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (theta + theta * i) / arc.Angle;
            }
            
            return result;
        }

    }

}

