using System;
using System.Collections.Generic;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructTier2D using the GH_Component as base. 
    /// </summary>
    public class ST_ConstructTier2D : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D thisNewTier. 
        /// </summary>
        public ST_ConstructTier2D()
            : base(nameof(ST_ConstructTier2D), "cT", "Construct a 2D seating thisNewTier from parameters", "StadiumTools", "2D Section")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            double unit = StadiumTools.UnitHandler.FromString("Rhino", Rhino.RhinoDoc.ActiveDoc.GetUnitSystemName(true, false, true, true));
            StadiumTools.Tier defaultTier = new StadiumTools.Tier();
            StadiumTools.Tier.InitDefault(defaultTier, unit);
            pManager.AddGenericParameter("Spectator", "S", "A Spectator to inherit Spectator Parameters From", GH_ParamAccess.item);
            pManager.AddNumberParameter("Seperation", "Ss", "Distance between spectators seated on the same row (side-by-side)", GH_ParamAccess.item, defaultTier.SpecSeperation);
            pManager.AddNumberParameter("Start X", "sX", "Horizontal start distance of Tier Start", GH_ParamAccess.item, defaultTier.StartX);
            pManager.AddNumberParameter("Start Y", "sY", "Vertical start distance of Tier Start", GH_ParamAccess.item, defaultTier.StartY);
            pManager.AddIntegerParameter("Row Count", "rC", "Number of rows", GH_ParamAccess.item, defaultTier.RowCount);
            pManager.AddNumberParameter("Row Width", "rW", "Row width", GH_ParamAccess.item, defaultTier.DefaultRowWidth);
            pManager.AddNumberParameter("Aisle Width", "aW", "Minimum Width of Aisles in this Tier", GH_ParamAccess.item, defaultTier.AisleWidth);
            pManager.AddNumberParameter("Rounding", "r", "Increment to round riser heights up to", GH_ParamAccess.item, defaultTier.RoundTo);
            pManager.AddNumberParameter("Maximum Rake Angle", "mrA", "Maximum rake angle in Radians (Tan(riser/row))", GH_ParamAccess.item, defaultTier.MaxRakeAngle);
            pManager.AddGenericParameter("SuperRiser", "SR", "An optional SuperRiser object to inherit parameters from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Vomatory", "V", "An optional Vomatory object to inherit parameters from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Fascia", "F", "An optional Fascia object to apply to the beginning of the tier", GH_ParamAccess.item);
            
            pManager[9].Optional = true;
            pManager[10].Optional = true;
            pManager[11].Optional = true;
        }

        //Set parameter indixes to names  (for readability)
        private static int IN_Spectator = 0;
        private static int IN_Seperation = 1;
        private static int IN_Start_X = 2;
        private static int IN_Start_Y = 3;
        private static int IN_Row_Count = 4;
        private static int IN_Row_Width = 5;
        private static int IN_Aisle_Width = 6;
        private static int IN_Rounding = 7;
        private static int IN_Maximum_Rake_Angle = 8;
        private static int IN_SuperRiser = 9;
        private static int IN_Vomatory = 10;
        private static int IN_Fascia = 11;
        private static int OUT_Tier = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tier2D", "T", "A Tier object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Handle Errors
            ST_ConstructTier2D.HandleErrors(DA, this);

            //Instance a new tier
            StadiumTools.Tier newTier = new StadiumTools.Tier();

            //Set tier parameters from Data Access
            ST_ConstructTier2D.ConstructTierFromDA(DA, newTier);

            //GH_Goo<T> wrapper
            StadiumTools.TierGoo newTierGoo = new StadiumTools.TierGoo(newTier);

            //Output Goo
            DA.SetData(OUT_Tier, newTierGoo);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_ConstructTier;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("072677e1-616c-4028-ab7e-b9717e7699b1");

        //Methods
        private static void HandleErrors(IGH_DataAccess DA, GH_Component thisComponent)
        {
            int rowCount = 0;
            double doubleItem = 0.0;
            var superItem = new StadiumTools.SuperRiser();

            if (DA.GetData<int>(IN_Row_Count, ref rowCount))
            {
                if (rowCount < 3)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Row Count must be non-negative and greater than 2");
                }
            }
            if (DA.GetData<StadiumTools.SuperRiser>(IN_SuperRiser, ref superItem))
            {
                if (superItem.Row > rowCount - 1)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Tier Row Count [{rowCount}] must exceed SuperRiser Row [{superItem.Row}]");
                }
            }
            if (DA.GetData<double>(IN_Maximum_Rake_Angle, ref doubleItem))
            {
                if (doubleItem > 0.593412)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Warning: Maximum Rake Angle exceeds 34 degrees");
                }
            }
        }
    
        private static void ConstructTierFromDA(IGH_DataAccess DA, StadiumTools.Tier tier)
        {
            //Item Containers (Destinations)
            var spectatorGooItem = new StadiumTools.SpectatorGoo();
            var superItem = new StadiumTools.SuperRiser();
            var vomItem = new StadiumTools.Vomatory();
            var fasciaItem = new StadiumTools.Fascia();
            int intItem = 0;
            double doubleItem = 0.0;

            //Get & Set Tier parameters
            if(!DA.GetData(IN_Spectator, ref spectatorGooItem)) { return; }   
            tier.SpectatorParameters = spectatorGooItem.Value;

            if (!DA.GetData(IN_Seperation, ref doubleItem)) { return; }
            tier.SpecSeperation = doubleItem;

            if (!DA.GetData(IN_Start_X, ref doubleItem)) { return; }
            tier.StartX = doubleItem;

            if (!DA.GetData(IN_Start_Y, ref doubleItem)) { return; }
            tier.StartY = doubleItem;

            if (!DA.GetData(IN_Row_Count, ref intItem)) { return; }
            tier.RowCount = intItem;

            tier.Spectators = new StadiumTools.Spectator[tier.RowCount];

            if (!DA.GetData(IN_Row_Width, ref doubleItem)) { return; }
            tier.DefaultRowWidth = doubleItem;

            if (!DA.GetData(IN_Aisle_Width, ref doubleItem)) { return; }
            tier.AisleWidth = doubleItem;

            if (DA.GetData(IN_Fascia, ref fasciaItem))
            {
                tier.FasciaHas = true;
                tier.Fascia = fasciaItem;
            }

            //Initialize an array with all default row widths 
            double[] rowWidths = new double[tier.RowCount];
            for (int i = 0; i < rowWidths.Length; i++)
            {
                rowWidths[i] = tier.DefaultRowWidth;
            }

            if (!DA.GetData(IN_Rounding, ref doubleItem)) { return; }
            tier.RoundTo = doubleItem;

            if (!DA.GetData(IN_Maximum_Rake_Angle, ref doubleItem)) { return; }
            tier.MaxRakeAngle = doubleItem;

            //Set SuperRiser properties if SuperRiser object is input
            DA.GetData(IN_SuperRiser, ref superItem);
            if (superItem.Row > 0)
            {
                tier.SuperHas = true;
                superItem.EyeX += tier.DefaultRowWidth * (superItem.Width - 1);
                superItem.SEyeX += tier.DefaultRowWidth * (superItem.Width - 1);
                tier.SuperRiser = (StadiumTools.SuperRiser)superItem.Clone();
                rowWidths[tier.SuperRiser.Row] = (tier.SuperRiser.Width * rowWidths[tier.SuperRiser.Row]);
            }

            //Set Vomatory property if input
            DA.GetData(IN_Vomatory, ref vomItem);
            tier.VomatoryParameters = vomItem;

            //Set Fascia property if input

            //Set Remaining properties
            tier.RowWidths = rowWidths; 
            tier.RiserHeights = new double[tier.RowCount - 1];
            tier.Points2dCount = StadiumTools.Tier.GetTierPtCount(tier);
            tier.Points2d = new StadiumTools.Pt2d[tier.Points2dCount];
            tier.AisleStepHeight = 0.230 * tier.SpectatorParameters.Unit;
            tier.AisleStepWidth = 0.260 * tier.SpectatorParameters.Unit;
            tier.AislePoints2d = new StadiumTools.Pt2d[0];
        }
    }
}