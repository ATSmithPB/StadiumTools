using System.Collections.Generic;
using StadiumTools;
using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;


namespace StadiumTools
{
    public static class IO
    {
        //Extensions Methods
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

        /// <summary>
        /// Returns a Pln3d object from a Rhino Plane
        /// </summary>
        /// <param name="plane"></param>
        /// <returns>Pln3d</returns>
        public static Pln3d Pln3dFromPlane(Plane plane)
        {
            Pt3d pOrigin = Pt3dFromPoint3d(plane.Origin);
            Vec3d xAxis = Vec3dFromVector3d(plane.XAxis);
            Vec3d yAxis = Vec3dFromVector3d(plane.YAxis);
            Vec3d zAxis = Vec3dFromVector3d(plane.ZAxis);
            Pln3d pln = new Pln3d(pOrigin, xAxis, yAxis, zAxis);
            pln.isValid = true;
            return pln;
        }
 
        /// <summary>
        /// Returns a Pt3d object from a Rhino Point3d
        /// </summary>
        /// <param name="point3d"></param>
        /// <returns></returns>
        public static Pt3d Pt3dFromPoint3d(Point3d point3d)
        {
            Pt3d pt = new Pt3d(point3d.X, point3d.Y, point3d.Z);
            return pt;
        }
       
        /// <summary>
        /// Returns a Vec3d object from a Rhino Vector3d3d
        /// </summary>
        /// <param name="vector3d"></param>
        /// <returns></returns>
        public static Vec3d Vec3dFromVector3d(Vector3d vector3d)
        {
            Vec3d vec = new Vec3d(vector3d.X, vector3d.Y, vector3d.Z);
            return vec;
        }
        

        /// <summary>
        /// Casts a list of Vec2d objects to an array of RhinoCommon Vector2d
        /// </summary>
        /// <param name="vecs"></param>
        /// <returns>Vector2d[]</returns>
        public static Vector2d[] Vec2dToVector2d(Vec2d[] vecs)
        {
            Vector2d[] rcVecs = new Vector2d[vecs.Length];

            for (int i = 0; i < vecs.Length; i++)
            {
                rcVecs[i] = new Vector2d(vecs[i].X, vecs[i].Y);
            }
            return rcVecs;
        }

        /// <summary>
        /// Casts a jagged array of Pt2d objects into a data tree of RhinoCommon Point2d
        /// </summary>
        /// <param name="pts"></param>
        /// <returns>DataTree<Point2d></Point2d></returns>
        public static DataTree<Point2d> Pt2dToPoint2d(Pt2d[][] pts)
        {
            DataTree<Point2d> rcPts = new DataTree<Point2d>();
            for (int i = 0; i < pts.Length; i++)
            {
                for (int j = 0; j < pts[i].Length; j++)
                {
                    GH_Path path = new GH_Path(i);
                    Point2d item = new Point2d(pts[i][j].X, pts[i][j].Y);
                    rcPts.Add(item, path);
                }
            }
            return rcPts;
        }

        public static DataTree<double> DataTreeFromArray(double[][] cVals)
        {
            DataTree<double> rcDub = new DataTree<double>();
            for (int i = 0; i < cVals.Length; i++)
            {
                for (int j = 0; j < cVals[i].Length; j++)
                {
                    GH_Path path = new GH_Path(i);
                    double item = cVals[i][j];
                    rcDub.Add(item, path);
                }
            }
            return rcDub;
        }

        public static DataTree<int> DataTreeFromArray(int[][] cVals)
        {
            DataTree<int> rcInt = new DataTree<int>();
            for (int i = 0; i < cVals.Length; i++)
            {
                for (int j = 0; j < cVals[i].Length; j++)
                {
                    GH_Path path = new GH_Path(i);
                    int item = cVals[i][j];
                    rcInt.Add(item, path);
                }
            }
            return rcInt;
        }

        /// <summary>
        /// Casts a jagged array of Vec2d objects into a data tree of RhinoCommon Vector2d
        /// </summary>
        /// <param name="pts"></param>
        /// <returns>DataTree<Point2d></Point2d></returns>
        public static DataTree<Vector2d> Vec2dToVector2d(Vec2d[][] vecs)
        {
            DataTree<Vector2d> rcVecs = new DataTree<Vector2d>();
            for (int i = 0; i < vecs.Length; i++)
            {
                for (int j = 0; j < vecs[i].Length; j++)
                {
                    GH_Path path = new GH_Path(i);
                    Vector2d item = new Vector2d(vecs[i][j].X, vecs[i][j].Y);
                    rcVecs.Add(item, path);
                }
            }
            return rcVecs;
        }
    }
}
        
