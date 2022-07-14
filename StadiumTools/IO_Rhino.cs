using System.Collections.Generic;
using Rhino.Geometry;

namespace StadiumTools
{
    public class IO
    {
        public class Rhino
        { 
            /// <summary>
            /// Returns a Pt2d object from a Rhino Point3d
            /// </summary>
            /// <param name="point3d"></param>
            /// <returns></returns>
            public static Pt2d Pt2dFromPoint3d(Point3d point3d)
            {
                Pt2d pt = new Pt2d(point3d.X, point3d.Y);
                return pt;
            }

            public static Pln3d Pln3dFromPlane(Plane plane)
            {
                Pln3d pln = new Pln3d(plane.Origin, plane.XAxis, plane.YAxis, plane.ZAxis)
                return pln;
            }
        }
    }
}
