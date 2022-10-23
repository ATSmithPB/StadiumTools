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
    public class ST_ConstructBowlPlan : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D section. 
        /// </summary>
        public ST_ConstructBowlPlan()
            : base(nameof(ST_ConstructBowlPlan), "cBP", "Construct a 2D BowlPlan from a PlaySurface and parameters", "StadiumTools", "2D Plan")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Boundary", "B", "A Boundary to construct the BowlPlan from", GH_ParamAccess.item);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Boundary = 0;
        private static int OUT_BowlPlan = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BowlPlan", "BP", "A BowlPlan object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Construct a new section from Data Access
            StadiumTools.BowlPlan newPlan = ST_ConstructBowlPlan.ConstructBowlPlanFromDA(DA, this);

            //GH_Goo<T> wrapper
            var newPlanGoo = new StadiumTools.BowlPlanGoo(newPlan);

            //Output Goo
            DA.SetData(OUT_BowlPlan, newPlanGoo);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_ConstructPlan;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("1b366a05-6d82-43a0-afbc-f8ac47227263");

        //Methods  
        private static StadiumTools.BowlPlan ConstructBowlPlanFromDA(IGH_DataAccess DA, GH_Component thisComponent)
        {
            //Item Containers  
            var boundaryGooItem = new StadiumTools.BoundaryGoo();
            
            DA.GetData<StadiumTools.BoundaryGoo>(IN_Boundary, ref boundaryGooItem);
            StadiumTools.Boundary boundary = boundaryGooItem.Value;
            return new StadiumTools.BowlPlan(boundary, true);
        }

        
    }
}
