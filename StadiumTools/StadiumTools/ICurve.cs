using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public interface ICurve
    {
        Pt3d Start { get; set; }
        Pt3d End { get; set; }
        Pt3d Midpoint();
        Pt3d PointAt(double parameter);
        double Length();
        bool Offset(double distance, out ICurve offsetCurve);
    }
}
