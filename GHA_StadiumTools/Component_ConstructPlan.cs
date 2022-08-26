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
    public class ST_ConstructPlan : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D section. 
        /// </summary>
        public ST_ConstructPlan()
            : base(nameof(ST_ConstructPlan), "cP", "Construct a 2D Plan from a PlaySurface and parameters", "StadiumTools", "2D Plan")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            double unit = StadiumTools.UnitHandler.FromString("Rhino", Rhino.RhinoDoc.ActiveDoc.GetUnitSystemName(true, false, true, true));

            pManager.AddGenericParameter("PlaySurface", "PS", "PlaySurface to construct plan around", GH_ParamAccess.item);
            pManager.AddNumberParameter("Structural Bay Width", "SBw", "The standard width of structural bays", GH_ParamAccess.item, 30 * unit);
            pManager.AddNumberParameter("Sightline Offset", "Ow", "An offset of the PlaySurface boundary where sightlines focus", GH_ParamAccess.item, 1 * unit);
            pManager.AddIntegerParameter("Bowl Style", "BS", "The style of bowl construction", GH_ParamAccess.item, 0);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_PlaySurface = 0;
        private static int IN_Structural_Bay_Width = 1;
        private static int IN_Sightline_Offset = 2;
        private static int IN_Plan_Style = 3;
        private static int OUT_Plan = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Plan", "P", "A Plan object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Construct a new section from Data Access
            StadiumTools.Plan newPlan = ST_ConstructPlan.ConstructPlanFromDA(DA, this);

            //GH_Goo<T> wrapper
            StadiumTools.PlanGoo newPlanGoo = new StadiumTools.PlanGoo(newPlan);

            //Output Goo
            DA.SetData(OUT_Plan, newPlanGoo);
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
        private static StadiumTools.Plan ConstructPlanFromDA(IGH_DataAccess DA, GH_Component thisComponent)
        {
            StadiumTools.Plan newPlan = new StadiumTools.Plan();

            //Item Containers  
            StadiumTools.PlaySurfaceGoo playSurfaceGooItem = new StadiumTools.PlaySurfaceGoo();
            int intItem = 0;
            double doubleItem = 0.0;

            //Get PlaySurfaceGoo
            DA.GetData<StadiumTools.PlaySurfaceGoo>(IN_PlaySurface, ref playSurfaceGooItem);

            //Retrieve Tier Array from TiersGoo List
            newPlan.PlaySurfaceParameters = playSurfaceGooItem.Value;
            newPlan.SightlineOffsets = new double[newPlan.PlaySurfaceParameters.Boundary.Length];
        
            //Get-Set Structural Bay Width
            DA.GetData<double>(IN_Structural_Bay_Width, ref doubleItem);
            newPlan.DefaultBayWidth = doubleItem;

            //Get-Set Sightline Offset
            DA.GetData<double>(IN_Sightline_Offset, ref doubleItem);
            newPlan.DefaultSightlineOffset = doubleItem;
            for (int i = 0; i < newPlan.SightlineOffsets.Length; i++)
            {
                newPlan.SightlineOffsets[i] = doubleItem;
            }

            //Get-Set Structural Bay Width
            DA.GetData<int>(IN_Plan_Style, ref intItem);
            newPlan.PlanStyle = (StadiumTools.Plan.Style)intItem;

            return newPlan;
        }

    }
}
