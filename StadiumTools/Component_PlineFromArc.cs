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
    public class ST_PlineFromArc : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_PlineFromArc()
            : base(nameof(ST_PlineFromArc), "aPl", "Construct an Pline from an Arc with specified segment divLength", "StadiumTools", "Debug")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "O", "Origin plane of reference arc", GH_ParamAccess.item, Rhino.Geometry.Plane.WorldXY);
            pManager.AddIntervalParameter("Domain","d" , "Domain of arc angle in radians", GH_ParamAccess.item, new Rhino.Geometry.Interval(0, Math.PI / 2));
            pManager.AddNumberParameter("Radius", "R", "The radius of the arc", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Length", "L", "The segment divLength of the Pline", GH_ParamAccess.item, 0.3);
            pManager.AddBooleanParameter("Point at Center", "pac", "True if a Pline point is required at the arc Midpoint", GH_ParamAccess.item, false);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Plane = 0;
        private static int IN_Domain = 1;
        private static int IN_Radius = 2;
        private static int IN_Length = 3;
        private static int IN_Point_at_Center = 4;
        private static int OUT_PolylineCurve = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Polyline", "Pl", "A Polyline Curve", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_PlineFromArc.ConstructPlineFromDA(DA);
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
        public override Guid ComponentGuid => new Guid("5c37fc2d-dfd2-4f08-845e-3d9b2394dbe2");

        //Methods
        private static void ConstructPlineFromDA(IGH_DataAccess DA)
        {

            //Item Container (Destination)
            var planeItem = Rhino.Geometry.Plane.Unset;
            var intervalItem = Rhino.Geometry.Interval.Unset;
            double radius = 0.0;
            double divLength = 0.0;
            bool pointAtMiddle = false;

            //Get & Set Polyline
            if (!DA.GetData<Rhino.Geometry.Plane>(IN_Plane, ref planeItem)) { return; }
            if (!DA.GetData<Rhino.Geometry.Interval>(IN_Domain, ref intervalItem)) { return; }
            if (!DA.GetData<double>(IN_Radius, ref radius)) { return; }
            if (!DA.GetData<double>(IN_Length, ref divLength)) { return; }
            if (!DA.GetData<bool>(IN_Point_at_Center, ref pointAtMiddle)) { return; }

            var pln3d = StadiumTools.IO.Pln3dFromPlane(planeItem);
            var dom = StadiumTools.IO.DomainFromInterval(intervalItem);
            var arc = new StadiumTools.Arc(pln3d, radius, dom);
            Pline pline = Pline.FromArc(arc, divLength, pointAtMiddle);
            Rhino.Geometry.Polyline polyline = StadiumTools.IO.PolylineFromPline(pline);
            Rhino.Geometry.PolylineCurve polylineCurve = new Rhino.Geometry.PolylineCurve(polyline);
            DA.SetData(OUT_PolylineCurve, polylineCurve);
        }
    }
}
