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
        public Pt3d[] Points { get; set; }
        public Pln3d[] Planes { get; set; }
        public Pt3d Start { get; set; }
        public Pt3d End { get; set; }
        
        //Constructors
        public Pline(Pt3d[] pts)
        {
            Points = pts;
            Planes = Pln3d.PerpPlanes(pts);
            Start = pts[0];
            End = pts[pts.Length - 1];
        }

        public Pline(List<Pt3d> pts)
        {
            Points = pts.ToArray();
            Planes = Pln3d.PerpPlanes(pts);
            Start = pts[0];
            End = pts[pts.Count - 1];
        }

        //Methods

        public static Pline FromArc(Arc arc, double divLength, bool pointAtMiddle)
        {
            List<Pt3d> pts = new List<Pt3d>();
            pts.Add(arc.Start);
            pts.AddRange(Arc.DivideLinearCentered(arc, divLength, pointAtMiddle));
            pts.Add(arc.End);
            Pline result = new Pline(pts);
            return result;
        }

    }
}
