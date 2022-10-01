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
    public class ST_DivideArc : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_DivideArc()
            : base(nameof(ST_DivideArc), "dA", "Divide an Arc into a Polyline", "StadiumTools", "Debug")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            var originPoint = new Rhino.Geometry.Point3d(5, 5, 0);
            var xPoint = new Rhino.Geometry.Point3d(0, 5, 0);
            var planePoint = new Rhino.Geometry.Point3d(0, 0, 0);
            var defaultPlane = new Rhino.Geometry.Plane(originPoint, xPoint, planePoint);
            pManager.AddPlaneParameter("Plane", "Pa", "Origin plane of reference arc", GH_ParamAccess.item, Rhino.Geometry.Plane.WorldXY);
            pManager.AddIntervalParameter("Domain", "Da", "Domain of arc angle in radians", GH_ParamAccess.item, new Rhino.Geometry.Interval(0, Math.PI / 2));
            pManager.AddNumberParameter("Radius", "Ra", "The radius of the arc", GH_ParamAccess.item, 5);
            pManager.AddIntegerParameter("Count", "C", "The number of segments", GH_ParamAccess.item, 5);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Plane = 0;
        private static int IN_Domain = 1;
        private static int IN_Radius = 2;
        private static int IN_Count = 3;
        private static int OUT_Curves = 0;
        private static int OUT_Planes = 1;


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Polyline", "P", "The resulting Polyline approximation of the given arc", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Planes", "Pl", "Perpendicular Planes to the polyline kinks", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_DivideArc.DivideArcFromDA(DA);
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
        public override Guid ComponentGuid => new Guid("95e0ca09-a9c9-44aa-8c23-919809af1870");

        //Methods
        private static void DivideArcFromDA(IGH_DataAccess DA)
        {
            //Item Container (Destination)
            var planeItem = Rhino.Geometry.Plane.Unset;
            var intervalItem = Rhino.Geometry.Interval.Unset;
            double doubleItem = 0.0;
            int intItem = 0;

            //Arc0 from paramaters
            if (!DA.GetData<Rhino.Geometry.Plane>(IN_Plane, ref planeItem)) { return; }
            StadiumTools.Pln3d pln3dA = StadiumTools.IO.Pln3dFromPlane(planeItem);
            if (!DA.GetData<Rhino.Geometry.Interval>(IN_Domain, ref intervalItem)) { return; }
            StadiumTools.Domain domainA = StadiumTools.IO.DomainFromInterval(intervalItem);
            if (!DA.GetData<double>(IN_Radius, ref doubleItem)) { return; }
            var arc0 = new StadiumTools.Arc(pln3dA, doubleItem, domainA);



            //divide arc
            if (!DA.GetData<int>(IN_Count, ref intItem)) { return; }

            StadiumTools.Pline arcPline = StadiumTools.Pline.FromArc(arc0, intItem);
            Rhino.Geometry.PolylineCurve plineCurve = StadiumTools.IO.PolylineCurveFromPline(arcPline);
            StadiumTools.Pln3d[] pln3ds = Pln3d.AvgPlanes(arcPline, arc0.Plane);
            Rhino.Geometry.Plane[] planes = StadiumTools.IO.PlanesFromPln3ds(pln3ds);

            DA.SetData(OUT_Curves, plineCurve);
            DA.SetDataList(OUT_Planes, planes);
        }
    }
}
