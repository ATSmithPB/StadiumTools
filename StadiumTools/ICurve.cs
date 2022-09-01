using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public interface ICurve
    {
        Pt3d Midpoint();

        Pt3d PointOn(double parameter);

        double Length();
    }
}
