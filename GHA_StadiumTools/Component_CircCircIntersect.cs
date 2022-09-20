using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;
using StadiumTools;
using System.Collections.Generic;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSuperRiser using the GH_Component as base. 
    /// </summary>
    public class ST_CircCircIntersect : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_CircCircIntersect()
            : base(nameof(ST_CircCircIntersect), "ccX", "Solve the intersection points of two circles", "StadiumTools", "Debug")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            var defaultCenterB = new Rhino.Geometry.Plane(new Rhino.Geometry.Point3d(10, 3, 0), Rhino.Geometry.Vector3d.ZAxis);
            pManager.AddPlaneParameter("Center A", "O", "Origin plane of reference arc", GH_ParamAccess.item, Rhino.Geometry.Plane.WorldXY);
            pManager.AddPlaneParameter("Center B", "d", "Domain of arc angle in radians", GH_ParamAccess.item, defaultCenterB);
            pManager.AddNumberParameter("Radius A", "R", "The radius of the arc", GH_ParamAccess.item, 6);
            pManager.AddNumberParameter("Radius B", "L", "The segment divLength of the Pline", GH_ParamAccess.item, 7);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Center_A = 0;
        private static int IN_Center_B = 1;
        private static int IN_Radius_A = 2;
        private static int IN_Radius_B = 3;
        private static int OUT_Intersection_Points = 0;
        private static int OUT_Circles = 1;
        private static int OUT_Flag = 2;
        private static int OUT_Distance = 3;
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Intersection Points", "xP", "The intersection point(s) of the two circles", GH_ParamAccess.list);
            pManager.AddCurveParameter("Circles", "C", "The two circles", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Flag", "F", "The event flag of the intersection", GH_ParamAccess.item);
            pManager.AddTextParameter("Debug", "D", "Debug Message", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_CircCircIntersect.CircCircIntersectFromDA(DA);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_debug;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("a26da4e6-5418-431e-93bd-568f9b6aae09");

        //Methods
        private static void CircCircIntersectFromDA(IGH_DataAccess DA)
        {
            double tolerance = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            List<Rhino.Geometry.ArcCurve> circles = new List<Rhino.Geometry.ArcCurve>();
            //Item Container (Destination)
            var pointItem = new Rhino.Geometry.Point3d(0,0,0);
            var planeItem = Rhino.Geometry.Plane.Unset;
            double doubleItem = 0.0;

            //Get & Set Polyline
            if (!DA.GetData<Rhino.Geometry.Plane>(IN_Center_A, ref planeItem)) { return; }
            if (!DA.GetData<double>(IN_Radius_A, ref doubleItem)) { return; }
            StadiumTools.Pln3d pln3dA = StadiumTools.IO.Pln3dFromPlane(planeItem);
            StadiumTools.Circle cirA = new Circle(pln3dA, doubleItem);
            var circleA = new Rhino.Geometry.Circle(planeItem, doubleItem);
            circles.Add(new Rhino.Geometry.ArcCurve(circleA));

            if (!DA.GetData<Rhino.Geometry.Plane>(IN_Center_B, ref planeItem)) { return; }
            if (!DA.GetData<double>(IN_Radius_B, ref doubleItem)) { return; }
            StadiumTools.Pln3d pln3dB = StadiumTools.IO.Pln3dFromPlane(planeItem);
            StadiumTools.Circle cirB = new Circle(pln3dB, doubleItem);
            var circleB = new Rhino.Geometry.Circle(planeItem, doubleItem);
            circles.Add(new Rhino.Geometry.ArcCurve(circleB));

            int flag = StadiumTools.Circle.Intersect(cirA, cirB, tolerance, out Pt3d[] iPts);
            List<Rhino.Geometry.Point3d> iPtsList = new List<Rhino.Geometry.Point3d>();
            for (int i = 0; i < iPts.Length; i++)
            {
                iPtsList.Add(StadiumTools.IO.Point3dFromPt3d(iPts[i]));
            }
            
            StadiumTools.Vec3d distVec = new Vec3d(cirA.Center.OriginPt - cirB.Center.OriginPt);

            string msg = $"cirA Center: ({cirA.Center.OriginPt.X},{cirA.Center.OriginPt.Y},{cirA.Center.OriginPt.Z}),cirB Center: ({cirB.Center.OriginPt.X},{cirB.Center.OriginPt.Y},{cirB.Center.OriginPt.Z})";

            DA.SetDataList(OUT_Intersection_Points, iPtsList);
            DA.SetDataList(OUT_Circles, circles);
            DA.SetData(OUT_Flag, flag);
            DA.SetData(OUT_Distance, msg);
        }
    }
}
