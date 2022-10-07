using System.Collections.Generic;
using StadiumTools;
using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;
using Rhino.Commands;

namespace StadiumTools
{
    public static class IO
    {

        /// <summary>
        /// Returns a Pt2d object from a Rhino Point3d
        /// </summary>
        /// <param name="point3d"></param>
        /// <returns>Pt2d</returns>
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
        /// Construct a Pln2d from the components of a Plane
        /// </summary>
        /// <param name="plane"></param>
        /// <returns>Pln2d</returns>
        public static Pln2d Pln2dFromPlane(Plane plane)
        {
            Pt2d pOrigin = Pt2dFromPoint3d(plane.Origin);
            Vec2d xAxis = Vec2dFromVector3d(plane.XAxis);
            Vec2d yAxis = Vec2dFromVector3d(plane.YAxis);
            Pln2d pln = new Pln2d(pOrigin, xAxis, yAxis);
            pln.IsValid = true;
            return pln;
        }

        /// <summary>
        /// Returns an array of Planes from an array of Pln3d objects
        /// </summary>
        /// <param name="pln3d"></param>
        /// <returns>Plane[]</returns>
        public static Plane[] PlanesFromPln3ds(Pln3d[] pln3d)
        {
            Rhino.Geometry.Plane[] result = new Rhino.Geometry.Plane[pln3d.Length];    
            for (int i = 0; i < pln3d.Length; i++)
            {
                result[i] = PlaneFromPln3d(pln3d[i]);
            }
            return result;
        }

        /// <summary>
        /// Returns a List of Planes from a List of Pln3d objects
        /// </summary>
        /// <param name="pln3d"></param>
        /// <returns>List<Plane></returns>
        public static List<Plane> PlanesFromPln3ds(List<Pln3d> pln3d)
        {
            List<Rhino.Geometry.Plane> result = new List <Rhino.Geometry.Plane>();
            for (int i = 0; i < pln3d.Count; i++)
            {
                result.Add(PlaneFromPln3d(pln3d[i]));
            }
            return result;
        }

        /// <summary>
        /// Returns a Plane from a Pln3d object
        /// </summary>
        /// <param name="pln"></param>
        /// <returns>Plane</returns>
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
        /// <returns>Pt3d</returns>
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

        public static Point3d[] Points3dFromPt3ds(Pt3d[] pt3ds)
        {
            Point3d[] points3d = new Point3d[pt3ds.Length];
            for (int i = 0; i < pt3ds.Length; i++)
            {
                points3d[i] = Point3dFromPt3d(pt3ds[i]);
            }
            return points3d;
        }

        public static Point3d[] Points3dFromPt3ds(List<Pt3d> pt3ds)
        {
            Point3d[] points3d = new Point3d[pt3ds.Count];
            for (int i = 0; i < pt3ds.Count; i++)
            {
                points3d[i] = Point3dFromPt3d(pt3ds[i]);
            }
            return points3d;
        }

        /// <summary>
        /// Returns a Point3d from a given Pt3d object
        /// </summary>
        /// <param name="pt3d"></param>
        /// <returns>Point3d</returns>
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

        public static Polyline PolylineFromPline(Pline pline)
        {
            return new Polyline(Points3dFromPt3ds(pline.Points));
        }

        public static PolylineCurve PolylineCurveFromPline(Pline pline)
        {
            return new PolylineCurve(PolylineFromPline(pline));
        }

        public static List<PolylineCurve> PolylineCurveListFromPlines(Pline[] plines)
        {
            List<PolylineCurve> curveList = new List<PolylineCurve>();
            for (int i = 0; i < plines.Length; i++)
            {
                curveList.Add(PolylineCurveFromPline(plines[i]));
            }
            return curveList;
        }

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

        /// <summary>
        /// returns a collection of points from the data of a tier
        /// </summary>
        /// <param name="tier"></param>
        /// <returns></returns>
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

        public static PolyCurve PolyCurveFromICurveArray(ICurve[] iCrvs)
        {
            PolyCurve result = new PolyCurve();
            for (int i = 0; i < iCrvs.Length; i++)
            {
                result.Append(CurveFromICurve(iCrvs[i]));
            }
            return result;

        }

        public static Curve[] CurveArrayFromICurveArray(ICurve[] iCrvs)
        {
            Curve[] result = new Curve[iCrvs.Length];
            for (int i = 0; i < iCrvs.Length; i++)
            {
                result[i] = CurveFromICurve(iCrvs[i]);
            }
            return result;
        }

        public static List<Curve> CurveListFromICurveArray(ICurve[] iCrvs)
        {
            List<Curve> result = new List<Curve>();
            for (int i = 0; i < iCrvs.Length; i++)
            {
                result.Add(CurveFromICurve(iCrvs[i]));
            }
            return result;
        }

        public static Curve CurveFromICurve(ICurve icrv)
        {
            string typeName = icrv.GetType().Name;
            switch (typeName)
            {
                case "Line":
                    return CurveFromLine((StadiumTools.Line)icrv);
                case "Arc":
                    return CurveFromArc((StadiumTools.Arc)icrv);
                case "Ellipse2d":
                    return CurveFromEllipse((StadiumTools.Ellipse)icrv);
                default:
                    throw new System.Exception($"ICurve type [{icrv.GetType().Name}] not available for conversion to Rhino.Geometry.Curve");
            }
        }

        /// <summary>
        /// construct a NURBS cuvre from an ellipse
        /// </summary>
        /// <param name="stEllipse"></param>
        /// <returns></returns>
        public static Curve CurveFromEllipse(StadiumTools.Ellipse stEllipse)
        {
            Rhino.Geometry.Ellipse rcEllipse = EllipseFromEllipse(stEllipse);
            NurbsCurve nurbsEllipse = rcEllipse.ToNurbsCurve();
            Curve result = nurbsEllipse;
            return result;
        }

        public static Rhino.Geometry.Curve[] CurvesFromArcs(StadiumTools.Arc[] arcs)
        {
            Rhino.Geometry.Curve[] result = new Rhino.Geometry.Curve[arcs.Length];
            for (int i = 0; i < arcs.Length; i++)
            {
                result[i] = CurveFromArc(arcs[i]);
            }
            return result;
        }

        public static Curve CurveFromArc(StadiumTools.Arc stArc)
        { 
            Rhino.Geometry.Arc rcArc = RCArcFromArc(stArc);
            ArcCurve arcCrv = new ArcCurve(rcArc);
            Curve result = arcCrv;
            return result;
        }

        public static Curve CurveFromLine(StadiumTools.Line stLine)
        {
            Rhino.Geometry.Line rcLine = RCLineFromLine(stLine);
            LineCurve lineCrv = new LineCurve(rcLine);
            Curve result = lineCrv;
            return result;
        }

        /// <summary>
        /// construct a Rhino Interval based on a StadiumTools Domain
        /// </summary>
        /// <param name="dom"></param>
        /// <returns></returns>
        public static Interval IntervalFromDomain(Domain domain)
        {
            return new Rhino.Geometry.Interval(domain.T0, domain.T1);
        }

        public static Domain DomainFromInterval(Interval interval)
        {
            return new StadiumTools.Domain(interval.Min, interval.Max);
        }

        public static Rhino.Geometry.Arc RCArcFromArc(StadiumTools.Arc stArc)
        {
            Interval angleIntervalRadians = IntervalFromDomain(stArc.Domain);
            Plane plane = PlaneFromPln3d(stArc.Plane);
            Rhino.Geometry.Circle circle = new Rhino.Geometry.Circle(plane, stArc.Radius);
            return new Rhino.Geometry.Arc(circle, angleIntervalRadians);
        }

        public static Rhino.Geometry.Line[] RCLinesFromLines(StadiumTools.Line[] lines)
        {
            Rhino.Geometry.Line[] result = new Rhino.Geometry.Line[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = RCLineFromLine(lines[i]);
            }
            return result;
        }

        /// <summary>
        /// Constructs a RhinoCommon Line from a StadiumTools Line
        /// </summary>
        /// <param name="stLine"></param>
        /// <returns></returns>
        public static Rhino.Geometry.Line RCLineFromLine(StadiumTools.Line stLine)
        {
            double x0 = stLine.Start.X;
            double y0 = stLine.Start.Y;
            double z0 = stLine.Start.Z;
            double x1 = stLine.End.X;
            double y1 = stLine.End.Y;
            double z1 = stLine.End.Z;
            return new Rhino.Geometry.Line(x0, y0, z0, x1, y1, z1);
        }

        /// <summary>
        /// Constructs a RhinoCommon Ellipse from a StadiumTools Ellipse
        /// </summary>
        /// <param name="stEllipse"></param>
        /// <returns></returns>
        public static Rhino.Geometry.Ellipse EllipseFromEllipse(Ellipse stEllipse)
        {
            Pln3d pln3d = stEllipse.Center;
            Plane plane = PlaneFromPln3d(pln3d);
            return new Rhino.Geometry.Ellipse(plane, stEllipse.RadiusX, stEllipse.RadiusY);
        }

        /// <summary>
        /// Constructs a RhinoCommon Mesh from a StadiumTools Mesh
        /// </summary>
        /// <param name="newSTBowlMesh"></param>
        /// <returns></returns>
        public static Rhino.Geometry.Mesh RCMeshFromSTMesh(StadiumTools.Mesh newSTBowlMesh)
        {
            var result = new Rhino.Geometry.Mesh();
            return result;
        }




    }
}
        
