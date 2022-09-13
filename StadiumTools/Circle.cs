using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public struct Circle : ICurve
    {
        //Properties
        public Pln3d Center { get; set; }
        public double Radius { get; set; }
        public Pt3d Start { get; set; }
        public Pt3d End { get; set; }

        //Constructors
        public Circle(Pln3d center, double radius)
        {
            Center = center;
            Radius = radius;
            Pt3d seamPt = center.OriginPt + (radius * center.Xaxis);
            Start = seamPt;
            End = seamPt;
        }

        public Circle(Arc arc)
        {
            Center = arc.Plane;
            Radius = arc.Radius;
            Start = arc.Start;
            End = arc.Start;
        }

        //Methods
        /// <summary>
        /// returns the intersection event between two circls, and if successful the intersection points
        /// </summary>
        /// <param name="C0"></param>
        /// <param name="C1"></param>
        /// <param name="abstol"></param>
        /// <param name="P0"></param>
        /// <param name="P1"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int Intersect(Circle C0, Circle C1, double abstol, out Pt3d P0, out Pt3d P1)
        {
            P0 = P1 = new Pt3d();
            int xcnt = -1;
            if (Pln3d.IsCoPlanar(C0.Center, C1.Center, abstol))
            {
                Circle[] C = new Circle[2] { C0, C1 };
                if (C1.Radius >= C0.Radius)
                {
                    C[0] = C1;
                    C[1] = C0;
                }
                double R0 = C[0].Radius;       // largest radius
                double R1 = C[1].Radius;
                Vec3d D = new Vec3d(C[1].Center.OriginPt - C[0].Center.OriginPt);
                double d = D.M;
                if (d > abstol)
                {
                    D.Normalize();
                    Vec3d Dperp = Vec3d.CrossProduct(D, C[0].Center.Zaxis);

                    if (d > R0 + R1 + abstol)
                    {
                        xcnt = 0;// disks are disjoint
                    }
                    else if (d + R1 + abstol < R0)
                    {
                        xcnt = 0;// small disk is in interior of large disk
                    }
                    else
                    {
                        double d1 = (R0 * R0 - R1 * R1 + d * d) / (2 * d);
                        double a1 = R0 * R0 - d1 * d1;
                        if (a1 < 0)
                        {
                            a1 = 0;
                        }

                        a1 = Math.Sqrt(a1);
                        if (a1 < .5 * abstol)
                        {
                            xcnt = 1;
                            P0 = C[0].Center.OriginPt + d1 * D;
                        }
                        else
                        {
                            xcnt = 2;
                            P0 = C[0].Center.OriginPt + d1 * D + a1 * Dperp;
                            P1 = C[0].Center.OriginPt + d1 * D - a1 * Dperp;
                        }
                    }
                }
                else if (R0 - R1 < abstol)
                {
                    xcnt = 3;
                }
                else
                {
                    xcnt = 0;
                }
            }
            else
            {
                throw new ArgumentException("Circle's are not parallel");
            }
            return xcnt;
        }




        public static bool IsIntersecting(Circle a, Circle b)
        {
            bool result = false;
            double d = Pt3d.Distance(a.Center.OriginPt, b.Center.OriginPt);
            if (a.Radius + b.Radius >= d || d >= Math.Abs(a.Radius - b.Radius))
            {
                result = true;
            }
            return result;
        }

        public static bool IsIntersecting(Circle a, Circle b, out double distance)
        {
            bool result = false;
            distance = Pt3d.Distance(a.Center.OriginPt, b.Center.OriginPt);
            if (a.Radius + b.Radius >= distance || distance >= Math.Abs(a.Radius - b.Radius))
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// returns the angle of a chord 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="chordLength"></param>
        /// <returns>double</returns>
        public static double ChordAngle(double radius, double chordLength)
        {
            return 2 * Math.Asin(chordLength / (2 * radius));
        }

        /// <summary>
        /// returns the distance between a chord and it's circle's centerpoint
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="chordLength"></param>
        /// <returns>double</returns>
        public static double ChordMidCen(double radius, double chordLength)
        {
            double half0 = ChordAngle(radius, chordLength) / 2;
            double halfChord = chordLength / 2;
            return Math.Sqrt((radius * radius) - (halfChord * halfChord));
        }

        /// <summary>
        /// returns the perpendicular distance between a chord's midpoint and the circle perimeter away from the circles center  
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="chordLength"></param>
        /// <returns>double</returns>
        public static double ChordMidCirc(double radius, double chordLength)
        {
            return radius - ChordMidCen(radius, chordLength);
        }

        /// <summary>
        /// returns the midpoint of a circle. Diametrically opposed point to the start
        /// </summary>
        /// <returns>Pt3d</returns>
        public Pt3d Midpoint()
        {
            return this.Center.OriginPt + (-this.Radius * this.Center.Xaxis);
        }

        /// <summary>
        /// returns a point at a specified parameter on a circle.
        /// </summary>
        /// <param name="angleRadians"></param>
        /// <returns></returns>
        public Pt3d PointAt(double angleRadians)
        {
            return Pt3d.Rotate(this.Center, this.Start, angleRadians);
        }

        /// <summary>
        /// returns the circumference of a circle
        /// </summary>
        /// <returns>double</returns>
        public double Length()
        {
            return 2 * Math.PI * this.Radius;
        }

        /// <summary>
        /// returns true if successful. out offset ICurve. A negative value will offset towards circle center. 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="offsetCirc"></param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool Offset(double distance, out ICurve offsetCurve)
        {
            bool result = this.Offset(distance, out Circle offsetCircle);
            offsetCurve = offsetCircle;
            return result;
        }

        /// <summary>
        /// returns true if successful. out offset Circle. A negative value will offset towards circle center. 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="offsetCircle"></param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool Offset(double distance, out Circle offsetCircle)
        {
            if (distance <= -this.Radius)
            {
                throw new ArgumentException($"offset distance [{distance}] must exceed -1 * radius [{-this.Radius}]");
            }
            offsetCircle = new Circle(this.Center, this.Radius + distance);
            return true;
        }

        /// <summary>
        /// returns parameters of point on circle that is closest to given point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ClosestPointTo(Pt3d point, double t)
        {
            bool rc = true;
            if (t != 0.0)
            {
                double u = 0.0;
                double v = 0.0;
                rc = this.Center.ClosestPointTo(point, u, v);
                if (u == 0.0 && v == 0.0)
                {
                    t = 0.0;
                }
                else
                {
                    t = Math.Atan2(v, u);
                    if (t < 0.0)
                    {
                        t += 2.0 * Math.PI;
                    }
                }
            }
            return rc;
        }

        /// <summary>
        /// returns the closest point on the circle to a given point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Pt3d ClosestPointTo(Pt3d point)
        {
            Pt3d result = new Pt3d();
            Vec3d vec = new Vec3d(this.Center.ClosestPointTo(point) - this.Center.OriginPt);
            if (vec.Unitize())
            {
                vec.Unitize();
                result = this.Center.OriginPt + this.Radius * vec;
            }
            else 
            {
                result = PointAt(0.0);
            }
            return result;
        }

    }
}
