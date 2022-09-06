using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public struct Circle
    {
        //Properties
        public Pln3d Center { get; set; }
        public double Radius { get; set; }

        //Constructors
        public Circle(Pln3d center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        //Methods
        /// <summary>
        /// returns the two intersection points of two circles if intersecting
        /// </summary>
        /// <param name="circleA"></param>
        /// <param name="circleB"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Pt3d[] Intersect(Circle circleA, Circle circleB, double tolerance)
        {
            if (!Pln3d.IsCoPlanar(circleA.Center, circleB.Center, tolerance))
            {
                throw new Exception("The two circles are not coplanar");
            }
            
            if (!IsIntersecting(circleA, circleB, out double dist))
            {
                throw new Exception("The two circles do not intersect");
            }
            
            Circle[] circles = new Circle[2] { circleA, circleB };
            if (circleB.Radius >= circleA.Radius)
            {
                circles[0] = circleB;
                circles[1] = circleA;
            }

            double rad0 = circles[0].Radius;
            double rad1 = circles[1].Radius;
            Vec3d vecD = new Vec3d(circles[1].Center.OriginPt - circles[0].Center.OriginPt);
            double disD = vecD.M;
            double x = disD - (rad0 + rad1);

            if (x < tolerance && x > -tolerance)
            {
                Pt3d[] intersectionPts = new Pt3d[1];
                intersectionPts[0] = Pt3d.Midpoint(circles[0].Center.OriginPt, circles[1].Center.OriginPt);
                return intersectionPts;
            }

            else 
            {
                Pt3d[] intersectPts = new Pt3d[2];
                vecD.Normalize();
                Vec3d Dperp = Vec3d.CrossProduct(vecD, circles[0].Center.Zaxis);
                double d1 = (rad0 * rad0 - rad1 * rad1 + disD * disD) / (2 * disD);
                double a1 = rad0 * rad0 - d1 * d1;

                if (a1 < 0)
                {
                    a1 = 0;
                }
                
                a1 = Math.Sqrt(a1);

                if (a1 < .5 * tolerance)
                {
                    intersectPts[0] = circles[0].Center.OriginPt + d1 * vecD;
                }
                else
                {
                    intersectPts[0] = circles[0].Center.OriginPt + d1 * vecD + a1 * Dperp;
                    intersectPts[1] = circles[0].Center.OriginPt + d1 * vecD - a1 * Dperp;
                }
                return intersectPts;
            }
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

        public static double ChordAngle(double radius, double chordLength)
        {
            return 2 * Math.Asin(chordLength/ (2* radius));
        }
    }
}
