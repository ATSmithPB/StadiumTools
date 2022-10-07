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
    public class ST_Plane2Pt : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_Plane2Pt()
            : base(nameof(ST_Plane2Pt), "P2", "Construct a Plane from two points", "StadiumTools", "Debug")
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
            pManager.AddPointParameter("Origin", "Opt", "Origin point of plane", GH_ParamAccess.item, Rhino.Geometry.Point3d.Origin);
            pManager.AddPointParameter("OnZ", "Zpt", "Point on plane Z-axis", GH_ParamAccess.item, new Rhino.Geometry.Point3d(0, 10, 0));
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Origin = 0;
        private static int IN_OnZ = 1;
        private static int OUT_Plane = 0;


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "P", "The resulting Plane", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_Plane2Pt.Plane2PtFromDA(DA);
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
        public override Guid ComponentGuid => new Guid("9c8b29d6-fa29-461b-a10e-babb05703709");

        //Methods
        private static void Plane2PtFromDA(IGH_DataAccess DA)
        {
            //Item Container (Destination)
            var pointItem = Rhino.Geometry.Point3d.Unset;

            //Arc0 from paramaters
            if (!DA.GetData<Rhino.Geometry.Point3d>(IN_Origin, ref pointItem)) { return; }
            StadiumTools.Pt3d origin = StadiumTools.IO.Pt3dFromPoint3d(pointItem);
            if (!DA.GetData<Rhino.Geometry.Point3d>(IN_OnZ, ref pointItem)) { return; }
            StadiumTools.Pt3d onZ = StadiumTools.IO.Pt3dFromPoint3d(pointItem);
            
            var pln3d = new StadiumTools.Pln3d(origin, onZ);
            Rhino.Geometry.Plane plane = StadiumTools.IO.PlaneFromPln3d(pln3d);

            DA.SetData(OUT_Plane, plane);
        }
    }
}
