using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSection2D using the GH_Component as base. 
    /// </summary>
    public class ST_DeconstructSection2D : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D section. 
        /// </summary>
        public ST_DeconstructSection2D()
            : base(nameof(ST_DeconstructSection2D), "dS", "Deconstruct a 2D section into it's respective data and geometry", "StadiumTools", "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Section", "S", "A Section object to deconstruct", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tiers", "T", "Section Tiers", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Section Plane", "PlOF", "Plane of Section where 0,0 is Point of Focus", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_DeconstructSection2D.DeconstructSectionFromDA(DA);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_DeConstructSection;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("83d42ddf-835d-4b3d-8aa0-e4bb640807b0");

        //Methods
        public static void DeconstructSectionFromDA(IGH_DataAccess DA)
        {
            //Item Containers (Destinations)
            StadiumTools.Section sectionItem = new StadiumTools.Section();
            
            //Get Input Section Object
            if (!DA.GetData<StadiumTools.Section>(0, ref sectionItem))
                return;

            //Deconstruct Section object and ouput data
            //Set Tiers
            DA.SetDataList(0, sectionItem.Tiers);

            //Set PlanePOF
            Rhino.Geometry.Plane sectionPlane = StadiumTools.IO.PlaneFromPln3d(sectionItem.Plane);
            DA.SetData(1, sectionPlane);
        }


    }
}
