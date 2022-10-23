using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;
using Rhino.DocObjects;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSection2D using the GH_Component as base. 
    /// </summary>
    public class ST_ConstructSection2D : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D section. 
        /// </summary>
        public ST_ConstructSection2D()
            : base(nameof(ST_ConstructSection2D), "cS", "Construct a 2D section from multiple tiers", "StadiumTools", "2D Section")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tier2D(s)", "T", "2D Seating Tiers that comprise a section", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "Pl", "3D plane to preview Section. Plane Origin will be used as the Point-Of-Focus for calculating C-Values", GH_ParamAccess.item, Rhino.Geometry.Plane.WorldXY);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Tiers = 0;
        private static int IN_SectionPlane = 1;
        private static int OUT_Section = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Section", "S", "A Section object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Construct a new section from Data Access
            StadiumTools.Section newSection = ST_ConstructSection2D.ConstructSectionFromDA(DA, this);
            //GH_Goo<T> wrapper
            var newSectionGoo = new StadiumTools.SectionGoo(newSection);
            //Output Goo
            DA.SetData(OUT_Section, newSectionGoo);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_ConstructSection;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("fe579af2-ac4c-4829-b108-b41fc2a4c322");

        //Methods  
        private static StadiumTools.Section ConstructSectionFromDA(IGH_DataAccess DA, GH_Component thisComponent)
        {
            //Item Containers  
            var planeItem = new Rhino.Geometry.Plane();
            var tierGooList = new List<StadiumTools.TierGoo>();

            //Get TiersGoo
            DA.GetDataList<StadiumTools.TierGoo>(IN_Tiers, tierGooList);

            //Retrieve Tier Array from TiersGoo List
            StadiumTools.Tier[] tierArray = StadiumTools.TierGoo.CloneListToArray(tierGooList);
            if (tierArray.Length <= 0)
            {
                thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "tierArray is less than or equal to 0");
            }

            //Get Plane3d
            DA.GetData<Rhino.Geometry.Plane>(IN_SectionPlane, ref planeItem);
            StadiumTools.Pln3d pln = StadiumTools.IO.Pln3dFromPlane(planeItem);
            
            //Construct a new section
            var newSection = new StadiumTools.Section(tierArray, pln);
            return newSection;
        }
    }
}
