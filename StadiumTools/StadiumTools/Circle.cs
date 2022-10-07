using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public Circle(Pt3d a, Pt3d b, double radius)
        {
            if (Pt3d.IsCoincident(a, b, UnitHandler.abstol))
            {
                throw new ArgumentException("Error: Input Points are coincident");
            }

            double distance = a.DistanceTo(b);
            if (distance < radius)
            {
                throw new ArgumentException($"Error: Radius [{radius}] must be => [{distance}] (the distance between input points)");
            }

            double offset = ChordMidCen(radius, distance);
            Pt3d centerPt = Pt3d.OffsetMidpoint(a, b, offset);
            Center = new Pln3d(centerPt, a, b);
            Radius = radius;
            Start = a;
            End = a;
        }

        /*
        public Circle(Pt3d P, Pt3d Q, Pt3d R)
        {
          Pt3d C = new Pt3d();
          Vec3d X = new Vec3d();
          Vec3d Y = new Vec3d();
          Vec3d Z = new Vec3d();
          
          while(true)
          {
            if (!Z.PerpendicularTo(P, Q, R))
            {
              break;
            }
            // get center as the intersection of 3 planes
            Pln3d plane0 = new Pln3d(P, Z);
            Pln3d plane1 = new Pln3d(0.5*(P+Q), P-Q);
            Pln3d plane2 = new Pln3d(0.5*(R+Q), R-Q);

            if (!ON_Intersect(plane0, plane1, plane2, C))
            {
              break;
            }

            X = P - C;
            this.Radius = X.Length();
            if (!(this.Radius > 0.0))
            {
              break;
            }

            if (!X.Unitize())
            {
              break;
            }

            Y = ON_CrossProduct(Z, X);
            if ( !Y.Unitize() )
              break;

            plane.origin = C;
            plane.xaxis = X;
            plane.yaxis = Y;
            plane.zaxis = Z;

            plane.UpdateEquation();

            return true;
          }

          plane = ON_Plane::World_xy;
          radius = 0.0;
          return false;
        }

        */

        //Methods
        /// <summary>
        /// returns the intersection event between two circls, and if successful the intersection points
        /// </summary>
        /// <param name="circleA"></param>
        /// <param name="circleB"></param>
        /// <param name="tolerance"></param>
        /// <param name="iPt0"></param>
        /// <param name="iPt1"></param>
        /// <returns>int</returns>
        /// <exception cref="ArgumentException"></exception>
        public static int Intersect(Circle circleA, Circle circleB, double tolerance, out Pt3d[] intersectionPts)
        {
            int result = -1;
            if (!Pln3d.IsCoPlanar(circleA.Center, circleB.Center, tolerance))
            {
                throw new ArgumentException("Cannot intersect: circles' planes are not coplanar");
            }
            else
            {
                Circle[] circles = new Circle[2] { circleA, circleB };
                if (circleB.Radius >= circleA.Radius)
                {
                    circles[0] = circleB;
                    circles[1] = circleA;
                }
                double rad0 = circles[0].Radius;
                double rad1 = circles[1].Radius;
                Vec3d distVec = new Vec3d(circles[1].Center.OriginPt - circles[0].Center.OriginPt);
                double distance = distVec.M;
                if (!IsIntersecting(distance, rad0, rad1, tolerance, out int flag))
                {
                    intersectionPts = new Pt3d[0];
                    result = flag;
                }
                else
                {
                    distVec.Normalize();
                    Vec3d distVecPerp = Vec3d.CrossProduct(distVec, circles[0].Center.Zaxis);
                    double d1 = ((rad0 * rad0) - (rad1 * rad1) + (distance * distance)) / (2 * distance);
                    double a1 = (rad0 * rad0) - (d1 * d1);
                    if (a1 < 0)
                    {
                        a1 = 0;
                    }
                    a1 = Math.Sqrt(a1);
                    if (a1 < .5 * tolerance)
                    {
                        result = 1; //circles have 1 intersection
                        intersectionPts = new Pt3d[1];
                        intersectionPts[0] = circles[0].Center.OriginPt + d1 * distVec;
                    }
                    else
                    {
                        result = 2; //circles have 2 intersections
                        intersectionPts = new Pt3d[2];
                        intersectionPts[0] = circles[0].Center.OriginPt + d1 * distVec + a1 * distVecPerp;
                        intersectionPts[1] = circles[0].Center.OriginPt + d1 * distVec - a1 * distVecPerp;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// returns true if two circles are intersecting
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerance"></param>
        /// <param name="flag"></param>
        /// <returns>bool</returns>
        public static bool IsIntersecting(Circle a, Circle b, double tolerance, out int flag)
        {
            double distance = Pt3d.Distance(a.Center.OriginPt, b.Center.OriginPt);
            return IsIntersecting(distance, a.Radius, b.Radius, tolerance, out flag);
        }

        /// <summary>
        /// returns true if two circles are intersecting, out distance bwtween circle centers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerance"></param>
        /// <param name="distance"></param>
        /// <param name="flag"></param>
        /// <returns>bool</returns>
        public static bool IsIntersecting(Circle a, Circle b, double tolerance, out double distance, out int flag)
        {
            distance = Pt3d.Distance(a.Center.OriginPt, b.Center.OriginPt);
            return IsIntersecting(distance, a.Radius, b.Radius, tolerance, out flag);
            
        }

        /// <summary>
        /// returns true if two circles are intersecting
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="radius0"></param>
        /// <param name="radius1"></param>
        /// <param name="tolerance"></param>
        /// <param name="flag"></param>
        /// <returns>bool</returns>
        public static bool IsIntersecting(double distance, double radius0, double radius1, double tolerance, out int flag)
        {
            flag = 0;
            bool result = true;
            if (distance < tolerance)
            {
                result = false;
                flag = 3; //circles have coincident center
            }
            else if (distance > radius0 + radius1 + tolerance)
            {
                result = false;
                flag = 4; //circles disjointed
            }
            else if (distance + radius1 + tolerance < radius0)
            {
                result = false;
                flag = 5; //circle1 interior to circle0
            }
            else if (distance + radius0 + tolerance < radius1)
            {
                result = false;
                flag = 6; //circle0 interior to circle1
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
        /// returns the midpoint of a circle. Diametrically opposed pt to the start
        /// </summary>
        /// <returns>Pt3d</returns>
        public Pt3d Midpoint()
        {
            return this.Center.OriginPt + (-this.Radius * this.Center.Xaxis);
        }

        /// <summary>
        /// returns a pt at a specified parameter on a circle.
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
        /// returns parameters of pt on circle that is closest to given pt
        /// </summary>
        /// <param name="point"></param>
        /// <param name="tParam"></param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool ClosestPointTo(Pt3d point, out double tParam)
        {
            if(!this.Center.ClosestPointTo(point, out double u, out double v))
            {
                throw new ArgumentException("Pln3d.ClosestPointTo failed!");
            }
            if (u == 0.0 && v == 0.0)
            {
                tParam = 0.0;
            }
            else
            {
                tParam = Math.Atan2(v, u);
                if (tParam < 0.0)
                {
                    tParam += Math.PI * 2;
                }
            }
            return true;   
        }

        /// <summary>
        /// returns Pt3d on circle that is closest to given Pt3d
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Pt3d</returns>
        public Pt3d ClosestPointTo(Pt3d point)
        {
            this.ClosestPointTo(point, out double tParam);
            return PointAt(tParam);
        }
    }
}
