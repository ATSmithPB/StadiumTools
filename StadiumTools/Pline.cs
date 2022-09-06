using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public struct Pline 
    {
        //Properties
        Pt3d[] Points { get; set; }
        Pln3d[] Planes { get; set; }
        Pt3d Start { get; set; }
        Pt3d End { get; set; }
        
        //Constructors
        public Pline(Pt3d[] pts)
        {
            Points = pts;
            Planes = Pln3d.PerpPlanes(pts);
            Start = pts[0];
            End = pts[pts.Length - 1];
        }

        //Methods
        
        //public static Pline PlineFromArcLinear(Arc arc, bool planeOnCenter, double segmentLength, double tolerance)
        //{
        //    List<Pt3d> pts = new List<Pt3d>();
        //    bool end = false;
        //    pts.Add(arc.Start);
        //    Circle circleMajor = new Circle(arc.Plane, arc.Radius);
        //    Circle circleMinor = new Circle(new Pln3d(arc.Start), segmentLength);

        //    while (!end)
        //    {
        //        Pt3d[] iPts = Circle.Intersect(circleMinor, circleMajor, tolerance);
        //        if (iPts[0] )

        //    }


        //    return new Pline(pts);
        //}

    }
}
