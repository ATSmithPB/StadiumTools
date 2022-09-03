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
        
        public static Pline PlineFromArcLinear(Arc arc, bool planeOnCenter, double segmentLength)
        {
            Pt3d[] pts = new Pt3d[3];


            return new Pline(pts);
        }

    }
}
