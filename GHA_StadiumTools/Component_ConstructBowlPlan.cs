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
            double unit = StadiumTools.UnitHandler.FromString("Rhino", Rhino.RhinoDoc.ActiveDoc.GetUnitSystemName(true, false, true, true));
            string types = StadiumTools.BowlPlan.TypeEnumNumberedAsString();
            pManager.AddGenericParameter("PlaySurface", "PS", $"The Pitch/Field type to construct:{System.Environment.NewLine}{types} ", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Bowl Style", "bS", "The style of bowl construction", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Bowl Offset(s)", "bO", "The distance from a pitch boundary edge to its respective bowl boundary edge", GH_ParamAccess.list, 1 * unit);
            pManager.AddNumberParameter("Bowl Radaii", "bR", "If Bowl Style == Radial, The radius of the bowl boundary edges", GH_ParamAccess.list, 1 * unit);
            pManager.AddNumberParameter("Bowl Corner Radaii", "bcR", "The radaii to fillet bowl corners. 0 for sharp corners. Negative values to create open corners", GH_ParamAccess.list, 1 * unit);
            pManager.AddIntegerParameter("Corner Bay Count(s)", "cbC", "The number of inisital structral bays for each corner", GH_ParamAccess.list, 3);
            pManager.AddBooleanParameter("Center Gridline(s)", "cG", "True if a gridline should be centered on bowl boundaries", GH_ParamAccess.list, true);
            pManager.AddNumberParameter("Structural Bay Width(s)", "sbW", "The standard width of structural bays", GH_ParamAccess.list, 30 * unit);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_PlaySurface = 0;
        private static int IN_Bowl_Style = 1;
        private static int IN_Bowl_Offset = 2;
        private static int IN_Bowl_Radaii = 3;
        private static int IN_Corner_Radaii = 4;
        private static int IN_Corner_Bay_Count = 5;
        private static int IN_Center_Gridline = 6;
        private static int IN_Bay_Widths = 7;
        private static int OUT_Plan = 0;

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
            StadiumTools.BowlPlan newPlan = ST_ConstructBowlPlan.ConstructPlanFromDA(DA, this);

            //GH_Goo<T> wrapper
            StadiumTools.BowlPlanGoo newPlanGoo = new StadiumTools.BowlPlanGoo(newPlan);

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
        private static StadiumTools.BowlPlan ConstructPlanFromDA(IGH_DataAccess DA, GH_Component thisComponent)
        {
            //Item Containers  
            StadiumTools.PlaySurfaceGoo playSurfaceGooItem = new StadiumTools.PlaySurfaceGoo();
            StadiumTools.BowlPlan newPlan = new StadiumTools.BowlPlan();
            int intItem = 0;
            List<double> bowlOffsets = new List<double>();
            List<double> bowlRadaii = new List<double>();
            List<double> cornerRadaii = new List<double>();
            List<int> cornerBayCount = new List<int>();
            List<bool> centerGridlines = new List<bool>();
            List<double> bayWidths = new List<double>();
            
            

            //Get PlaySurface
            DA.GetData<StadiumTools.PlaySurfaceGoo>(IN_PlaySurface, ref playSurfaceGooItem);
            newPlan.PlaySurfaceParameters = playSurfaceGooItem.Value;
            int bowlEdgeCount;
            newPlan.BowlEdgeCount = bowlEdgeCount = playSurfaceGooItem.Value.Boundary.Length;

            //Get-Set Bowl Style
            DA.GetData<int>(IN_Bowl_Style, ref intItem);
            newPlan.BowlStyle = (StadiumTools.BowlPlan.Style)intItem;

            //Get-Set Bowl Offset(s)
            DA.GetDataList<double>(IN_Bowl_Offset, bowlOffsets);
            newPlan.BowlOffsets = MatchLength(bowlOffsets, bowlEdgeCount, "Bowl Offset(s)", thisComponent);

            //Get-Set Bowl Radaii
            DA.GetDataList<double>(IN_Bowl_Radaii, bowlRadaii);
            newPlan.BowlRadaii = MatchLength(bowlRadaii, bowlEdgeCount, "Bowl Radaii", thisComponent);

            //Get-Set CornerRadaii
            DA.GetDataList<double>(IN_Corner_Radaii, cornerRadaii);
            newPlan.CornerRadaii = MatchLength(cornerRadaii, bowlEdgeCount, "Corner Radaii", thisComponent);

            //Get-Set Corner Bay Count(s)
            DA.GetDataList<int>(IN_Corner_Bay_Count, cornerBayCount);
            newPlan.CornerBayCount = MatchLength(cornerBayCount, bowlEdgeCount, "Bay Count(s)", thisComponent);

            //Get-Set Center Gridline(s)
            DA.GetDataList<bool>(IN_Center_Gridline, centerGridlines);
            newPlan.CenterGridline = MatchLength(centerGridlines, bowlEdgeCount, "Center Gridline(s)", thisComponent);

            //Get-Set Bay Width(s)
            DA.GetDataList<double>(IN_Bay_Widths, bayWidths);
            newPlan.BayWidths = MatchLength(bayWidths, bowlEdgeCount, "Bay Width(s)", thisComponent);

            return newPlan;
        }

        private static T[] MatchLength<T>(List<T> list, int len, string type, GH_Component thisComponent) where T : struct
        {
            if (list.Count != 1 && list.Count != len)
            {
                thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"{type} count [{list.Count}] must be 1 value OR be a list of values equal to Playsurface Boundary count [{len}]");
            }

            T[] result = new T[len];
            if (list.Count == len)
            {
                for (int i = 0; i < len; i++)
                {
                    result[i] = list[i];
                }
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    result[i] = list[0];
                }
            }
            return result;
        }

    }
}
