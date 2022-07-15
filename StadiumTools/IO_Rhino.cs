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
               Pt3d pOrigin = Pt3dFromPoint3d(plane.Origin);
               Vec3d xAxis = Vec3dFromVector3d(plane.XAxis);
               Vec3d yAxis = Vec3dFromVector3d(plane.YAxis);
               Vec3d zAxis = Vec3dFromVector3d(plane.ZAxis);

                Pln3d pln = new Pln3d(pOrigin, xAxis, yAxis, zAxis);
                return pln;
            }

            public static Pt3d Pt3dFromPoint3d(Point3d point3d)
            {
                Pt3d pt = new Pt3d(point3d.X, point3d.Y, point3d.Z);
                return pt;
            }

            public static Vec3d Vec3dFromVector3d(Vector3d vector3d)
            {
                Vec3d vec = new Vec3d(vector3d.X, vector3d.Y, vector3d.Z);
                return vec;
            }
        }
    }
}
