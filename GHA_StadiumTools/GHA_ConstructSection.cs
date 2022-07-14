using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called Tier Parameters using the GH_Component as base. 
    /// </summary>
    public class ConstructSection : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new tier. 
        /// </summary>
        public ConstructSection()
            : base(nameof(ConstructSection), "cS", "Construct a 2D section from multiple tiers", "StadiumTools", "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            StadiumTools.Tier defaultTier = new StadiumTools.Tier();
            pManager.AddGenericParameter("Tiers", "T", "Seating Tiers that comprise this section", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Section Plane", "sP", "3D plane of section. Origin should be Point-Of-Focus", GH_ParamAccess.item, Rhino.Geometry.Plane.WorldYZ);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Section", "S", "A Section object", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            StadiumTools.Section newSection = new StadiumTools.Section();
            ConstructSection.ConstructSectionFromDA(DA, newSection);
            DA.SetData(0, (object)newSection);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("fe579af2-ac4c-4829-b108-b41fc2a4c322");

        //Methods
        public static void ConstructSectionFromDA(IGH_DataAccess DA, StadiumTools.Section section)
        {
            Rhino.Geometry.Plane planeItem = new Rhino.Geometry.Plane();
            StadiumTools.Tier tierItem = new StadiumTools.Tier();

            if (!DA.GetData<Rhino.Geometry.Plane>(0, ref planeItem))
                return;
            section.Plane3d = StadiumTools.IO.Rhino.Pln3dFromPlane(planeItem);
            if (!DA.GetData<double>(1, ref doubleItem))
                return;
            section.StartX = doubleItem;
            

        }


    }
}
