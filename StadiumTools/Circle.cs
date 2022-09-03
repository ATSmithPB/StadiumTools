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
        //public static Pt3d[] IntersectTwo(Circle a, Circle b)
        //{
        //    if (!Pln3d.AreCoPlanar(a, b))
        //    {
        //        throw new Exception("The two circles do not lie on the same plane");
        //    }

        //    Pt3d[] result = new Pt3d[2];
        //    double d = Pt3d.Distance(a.Center.OriginPt, b.Center.OriginPt);

        //    if (a.Radius + b.Radius < d || d < Math.Abs(a.Radius - b.Radius))
        //    {
        //        throw new Exception("The two circles do not intersect");
        //    }
        //    if (a.Radius + b.Radius == d)
        //    {
        //        a.Center.OriginPt + 
        //    }

        //    return result;
        //}
    }
}
