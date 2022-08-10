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

        public static Pt2d[] Pt2dFromPolyline(Polyline polyline)
        {
            Pt2d[] pts = new Pt2d[polyline.Count];
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i] = Pt2dFromPoint3d(polyline[i]);
            }
            return pts;
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
            pln.IsValid = true;
            return pln;
        }

        /// <summary>
        /// Returns a Plane from a Pln3d object
        /// </summary>
        /// <param name="pln"></param>
        /// <returns></returns>
        public static Plane PlaneFromPln3d(Pln3d pln)
        {
            Point3d pOrigin = Point3dFromPt3d(pln.OriginPt);
            Vector3d xAxis = Vector3dFromVec3d(pln.Xaxis);
            Vector3d yAxis = Vector3dFromVec3d(pln.Yaxis);
            Plane plane = new Plane(pOrigin, xAxis, yAxis);
            return plane;
        }

        /// <summary>
        /// Returns a Pt3d object that represents the origin of a Rhino Plane
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Pt3d Pt3dFromPlane(Plane plane)
        {
            Pt3d pt = new Pt3d();
            pt = Pt3dFromPoint3d(plane.Origin);
            return pt;
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
        /// Returns a Point3d from a given Pt3d object
        /// </summary>
        /// <param name="pt3d"></param>
        /// <returns></returns>
        public static Point3d Point3dFromPt3d(Pt3d pt3d)
        {
            Point3d point3d = new Point3d(pt3d.X, pt3d.Y, pt3d.Z);
            return point3d;
        }

        /// <summary>
        /// Returns a Point3d from a given Pt2d object
        /// </summary>
        /// <param name="pt3d"></param>
        /// <returns>Point3d</returns>
        public static Point3d Point3dFromPt2d(Pt2d pt2d)
        {
            Point3d point3d = new Point3d(pt2d.X, pt2d.Y, 0.0);
            return point3d;
        }

        /// <summary>
        /// Returns an array of Point3d objects from a list of Pt2d
        /// </summary>
        /// <param name="pts2d"></param>
        /// <returns>Point3d[]</returns>
        public static Point3d[] Point3dFromPt2d(Pt2d[] pts2d)
        {
            Point3d[] points3d = new Point3d[pts2d.Length]; 
            for (int i = 0; i < pts2d.Length; i++)
            {
                points3d[i] = new Point3d(pts2d[i].X, pts2d[i].Y, 0.0);
            }
            return points3d;
        }

        /// <summary>
        /// Returns a Vec3d object from a Rhino Vector3d3d
        /// </summary>
        /// <param name="vector3d"></param>
        /// <returns></returns>
        public static Vec3d Vec3dFromVector3d(Vector3d vector3d)
        {
            return new Vec3d(vector3d.X, vector3d.Y, vector3d.Z);
        }
         
        public static Vec2d Vec2dFromVector3d(Vector3d vector3d)
        {
            return new Vec2d(vector3d.X, vector3d.Y);
        }

        /// <summary>
        /// Returns a Vector3d from a Vec3d object
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector3d Vector3dFromVec3d(Vec3d vec)
        {
            Vector3d vector3d = new Vector3d(vec.X, vec.Y, vec.Z);
            return vector3d;
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
        public static DataTree<Point2d> DataTreeFromJaggedArray(Pt2d[][] pts)
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

        /// <summary>
        /// Returns a Data Tree of doubles based on a jagged array
        /// </summary>
        /// <param name="cVals"></param>
        /// <returns>DataTree<double></double></returns>
        public static DataTree<double> DataTreeFromJaggedArray(double[][] cVals)
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

        /// <summary> 
        /// Returns a Data Tree of doubles based on a list
        /// </summary>
        /// <param name="cVals"></param>
        /// <returns></returns> 
        public static DataTree<int> DataTreeFromJaggedArray(int[][] cVals)
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

        //public static DataTree<int> DataTreeFromArray(Tier[] tiers)
        //{
        //    DataTree<int> rcInt = new DataTree<int>();
        //    for (int i = 0; i < tiers.Length; i++)
        //    {
                
        //        GH_Path path = new GH_Path(i);
        //        int item = tiers[i];
        //        rcInt.Add(item, path);
                
        //    }
        //    return rcInt;
        //}

        public static Polyline PolylineFromTier(Tier tier)
        {
            Polyline polyline = new Polyline();
            Point3d[] rhinoPts = new Point3d[tier.Points2dCount];
            for (int i = 0; i < tier.Points2dCount; i++)
            {
                Pt3d pt3d = tier.Points2d[i].ToPt3d(tier.Plane);
                rhinoPts[i] = Point3dFromPt3d(pt3d);
            }

            polyline.AddRange(rhinoPts);
            return polyline;
        }

        public static Point3d[] PointsFromTier(Tier tier)
        {
            Point3d[] rhinoPts = new Point3d[tier.Points2dCount];

            for (int i = 0; i < tier.Points2dCount; i++)
            {
                Pt3d pt3d = tier.Points2d[i].ToPt3d(tier.Plane);
                rhinoPts[i] = Point3dFromPt3d(pt3d);
            }

            return rhinoPts;
        }

        /// <summary>
        /// Casts a jagged array of Vec2d objects into a data tree of RhinoCommon Vector2d
        /// </summary>
        /// <param name="pts"></param>
        /// <returns>DataTree<Point2d></Point2d></returns>
        public static DataTree<Vector2d> DataTreeFromJaggedArray(Vec2d[][] vecs)
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

        public static PolyCurve PolyCurveFromICurve(ICurve[] iCrvs)
        {
            PolyCurve result = new PolyCurve();
            //FINISH
            return result
        }

        public static Curve[] CurveArrayFromICurveArray(ICurve[] iCrvs)
        {
            Curve[] result = new Curve[iCrvs.Length];
            //FINISH
            return result
        }

        public static Curve CurveFromICurve(ICurve icrv)
        {
            Curve result = new Curve();

            return result;
        }
    }
}
        
