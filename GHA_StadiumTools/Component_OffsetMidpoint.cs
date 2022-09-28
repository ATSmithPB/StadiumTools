using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;
using Rhino.Geometry;
using StadiumTools;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSuperRiser using the GH_Component as base. 
    /// </summary>
    public class ST_OffsetMidpoint : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_OffsetMidpoint()
            : base(nameof(ST_OffsetMidpoint), "omP", "Offset the midpoint between two points by a given distance", "StadiumTools", "Debug")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Start", "S", "Start Point of Arc", GH_ParamAccess.item, Point3d.Origin);
            pManager.AddPointParameter("End", "E", "End Point of Arc", GH_ParamAccess.item, new Rhino.Geometry.Point3d(5, 0, 0));
            pManager.AddNumberParameter("Radius", "R", "Radius of Arc", GH_ParamAccess.item, 5);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Start = 0;
        private static int IN_End = 1;
        private static int IN_Offset = 2;
        private static int OUT_Offset_Midpoint = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Offset Midpoint", "omP", "The offset midpoint", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_OffsetMidpoint.OffsetMidpointFromDA(DA);
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
        public override Guid ComponentGuid => new Guid("8dc00237-bfa6-492f-a172-b75f7771b27a");

        //Methods
        private static void OffsetMidpointFromDA(IGH_DataAccess DA)
        {
            //Item Container (Destination)
            var pointItem = Rhino.Geometry.Point3d.Origin;
            double doubleItem = 0.0;

            //Get Set Values
            if (!DA.GetData<Rhino.Geometry.Point3d>(IN_Start, ref pointItem)) { return; }
            var start = StadiumTools.IO.Pt3dFromPoint3d(pointItem);
            if (!DA.GetData<Rhino.Geometry.Point3d>(IN_End, ref pointItem)) { return; }
            var end = StadiumTools.IO.Pt3dFromPoint3d(pointItem);
            if (!DA.GetData<double>(IN_Offset, ref doubleItem)) { return; }

            var omp = Pt3d.OffsetMidpoint(start, end, doubleItem);
            Rhino.Geometry.Point3d offsetMidpoint = StadiumTools.IO.Point3dFromPt3d(omp);

            DA.SetData(OUT_Offset_Midpoint, offsetMidpoint);
        }
    }
}
