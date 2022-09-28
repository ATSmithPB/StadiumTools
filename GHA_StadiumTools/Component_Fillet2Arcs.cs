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
    public class ST_Fillet2Arcs : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_Fillet2Arcs()
            : base(nameof(ST_Fillet2Arcs), "aaF", "Fillet two Arcs", "StadiumTools", "Debug")
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
            pManager.AddPlaneParameter("PlaneA", "Pa", "Origin plane of reference arc", GH_ParamAccess.item, Rhino.Geometry.Plane.WorldXY);
            pManager.AddIntervalParameter("DomainA", "Da", "Domain of arc angle in radians", GH_ParamAccess.item, new Rhino.Geometry.Interval(0, Math.PI / 2));
            pManager.AddNumberParameter("RadiusA", "Ra", "The doubleItem of the arc", GH_ParamAccess.item, 5);
            pManager.AddPlaneParameter("PlaneB", "Pb", "Origin plane of reference arc", GH_ParamAccess.item, defaultPlane);
            pManager.AddIntervalParameter("DomainB", "Db", "Domain of arc angle in radians", GH_ParamAccess.item, new Rhino.Geometry.Interval(0, Math.PI / 2));
            pManager.AddNumberParameter("RadiusB", "Rb", "The doubleItem of the arc", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("Fillet Radius", "fR", "Fillet radius between the two arcs", GH_ParamAccess.item, 1);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_PlaneA = 0;
        private static int IN_DomainA = 1;
        private static int IN_RadiusA = 2;
        private static int IN_PlaneB = 3;
        private static int IN_DomainB = 4;
        private static int IN_RadiusB = 5;
        private static int IN_Fillet_Radius = 6;
        private static int OUT_Curves = 0;


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "The resulting arc-like Curves", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_Fillet2Arcs.Fillet2ArcsFromDA(DA);
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
        public override Guid ComponentGuid => new Guid("23a12325-ee35-4fd0-be85-4a3924405720");

        //Methods
        private static void Fillet2ArcsFromDA(IGH_DataAccess DA)
        {
            double tolerance = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            //Item Container (Destination)
            var planeItem = Rhino.Geometry.Plane.Unset;
            var intervalItem = Rhino.Geometry.Interval.Unset;
            double doubleItem = 0.0;

            //Arc0 from paramaters
            if (!DA.GetData<Rhino.Geometry.Plane>(IN_PlaneA, ref planeItem)) { return; }
            StadiumTools.Pln3d pln3dA = StadiumTools.IO.Pln3dFromPlane(planeItem);
            if (!DA.GetData<Rhino.Geometry.Interval>(IN_DomainA, ref intervalItem)) { return; }
            StadiumTools.Domain domainA = StadiumTools.IO.DomainFromInterval(intervalItem);
            if (!DA.GetData<double>(IN_RadiusA, ref doubleItem)) { return; }
            var arc0 = new StadiumTools.Arc(pln3dA, doubleItem, domainA);

            //Arc1 from paramaters
            if (!DA.GetData<Rhino.Geometry.Plane>(IN_PlaneB, ref planeItem)) { return; }
            StadiumTools.Pln3d pln3dB = StadiumTools.IO.Pln3dFromPlane(planeItem);
            if (!DA.GetData<Rhino.Geometry.Interval>(IN_DomainB, ref intervalItem)) { return; }
            StadiumTools.Domain domainB = StadiumTools.IO.DomainFromInterval(intervalItem);
            if (!DA.GetData<double>(IN_RadiusB, ref doubleItem)) { return; }
            var arc1 = new StadiumTools.Arc(pln3dB, doubleItem, domainB);

            //fillet arcs
            if (!DA.GetData<double>(IN_Fillet_Radius, ref doubleItem)) { return; }
            StadiumTools.ICurve[] filletArcs = StadiumTools.Arc.FilletTrim(arc0, arc1, doubleItem, tolerance);
            List<Rhino.Geometry.Curve> filletCurves = StadiumTools.IO.CurveListFromICurveArray(filletArcs);

            

            DA.SetDataList(OUT_Curves, filletCurves);
        }
    }
}
