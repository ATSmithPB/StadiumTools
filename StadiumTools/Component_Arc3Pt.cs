using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSuperRiser using the GH_Component as base. 
    /// </summary>
    public class ST_ConstructArc3Pt : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_ConstructArc3Pt()
            : base(nameof(ST_ConstructArc3Pt), "A3p", "Construct an Arc from an origin point and two other points", "StadiumTools", "Debug")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Origin", "O", "Origin point of angle", GH_ParamAccess.item, new Rhino.Geometry.Point3d(0,0,0));
            pManager.AddPointParameter("Start", "pA", "Point of angle start", GH_ParamAccess.item, new Rhino.Geometry.Point3d(1, 0, 0));
            pManager.AddPointParameter("End", "pB", "Point of angle end", GH_ParamAccess.item, new Rhino.Geometry.Point3d(0, 1, 0));
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Origin = 0;
        private static int IN_Start = 1;
        private static int IN_End = 2;
        private static int OUT_Arc = 0;
        private static int OUT_DomainST = 1;
        private static int OUT_DomainRC = 2;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Arc", "A", "An Arc", GH_ParamAccess.item);
            pManager.AddNumberParameter("Angle ST", "rcA", "Angle of ST_Arc ", GH_ParamAccess.item);
            pManager.AddNumberParameter("Angle RC", "stA", "Angle of RC_Arc", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_ConstructArc3Pt.ConstructArcFromDA(DA);
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
        public override Guid ComponentGuid => new Guid("f6f52aae-ed08-4239-8033-8f88b9d28f18");

        //Methods
        private static void ConstructArcFromDA(IGH_DataAccess DA)
        {
            double unit = StadiumTools.UnitHandler.FromString("Rhino", Rhino.RhinoDoc.ActiveDoc.GetUnitSystemName(true, false, true, true));

            //Item Container (Destination)
            var pointItem = Rhino.Geometry.Point3d.Origin;

            //Get & Set Polyline
            if (!DA.GetData<Rhino.Geometry.Point3d>(IN_Origin, ref pointItem)) { return; }
            var center = new StadiumTools.Pt2d(StadiumTools.IO.Pt2dFromPoint3d(pointItem));
            if (!DA.GetData<Rhino.Geometry.Point3d>(IN_Start, ref pointItem)) { return; }
            var start = new StadiumTools.Pt2d(StadiumTools.IO.Pt2dFromPoint3d(pointItem));
            if (!DA.GetData<Rhino.Geometry.Point3d>(IN_End, ref pointItem)) { return; }
            var end = new StadiumTools.Pt2d(StadiumTools.IO.Pt2dFromPoint3d(pointItem));

            var arc = new StadiumTools.Arc(center, start, end);
            StadiumTools.ICurve Iarc = arc;
            Rhino.Geometry.Arc rhinoArc = StadiumTools.IO.RCArcFromArc(arc);
            var rhinoArcCrv = new Rhino.Geometry.ArcCurve(rhinoArc);
            Rhino.Geometry.Curve rhinoCurve = rhinoArcCrv;

            double domainSt = arc.Domain.T1 - arc.Domain.T0;
            double domainRc = rhinoArcCrv.AngleRadians;

            DA.SetData(OUT_Arc, rhinoArcCrv);
            DA.SetData(OUT_DomainST, domainSt);
            DA.SetData(OUT_DomainRC, domainRc);
        }
    }
}
