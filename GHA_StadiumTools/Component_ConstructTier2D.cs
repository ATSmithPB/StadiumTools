using System;
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
            : base(nameof(ST_ConstructTier2D), "cT", "Construct a 2D seating thisNewTier from parameters", "StadiumTools", "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            StadiumTools.Tier defaultTier = new StadiumTools.Tier();
            StadiumTools.Tier.InitDefault(defaultTier);
            pManager.AddGenericParameter("Spectator", "S", "A Spectator to inherit Spectator Parameters From", GH_ParamAccess.item);
            pManager.AddNumberParameter("Start X", "sX", "Horizontal start distance of Tier Start", GH_ParamAccess.item, defaultTier.StartX);
            pManager.AddNumberParameter("Start Y", "sY", "Vertical start distance of Tier Start", GH_ParamAccess.item, defaultTier.StartY);
            pManager.AddBooleanParameter("Start", "St", "True if Tier Start is the end of the previous tier. False to use Section POF", GH_ParamAccess.item, defaultTier.BuildFromPreviousTier);
            pManager.AddIntegerParameter("Row Count", "rC", "Number of rows", GH_ParamAccess.item, defaultTier.RowCount);
            pManager.AddNumberParameter("Row Width", "rW", "Row width", GH_ParamAccess.item, defaultTier.DefaultRowWidth);
            pManager.AddNumberParameter("Rounding", "r", "Increment to round riser heights up to", GH_ParamAccess.item, defaultTier.RoundTo);
            pManager.AddNumberParameter("Maximum Rake Angle", "mrA", "Maximum rake angle in Radians (Tan(riser/row))", GH_ParamAccess.item, defaultTier.MaxRakeAngle);
            pManager.AddGenericParameter("SuperRiser", "SR", "An optional SuperRiser object to inherit parameters from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Vomatory", "V", "An optional Vomatory object to inherit parameters from", GH_ParamAccess.item);
            pManager[8].Optional = true;
            pManager[9].Optional = true;
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Spectator = 0;
        private static int IN_Start_X = 1;
        private static int IN_Start_Y = 2;
        private static int IN_Start = 3;
        private static int IN_Row_Count = 4;
        private static int IN_Row_Width = 5;
        private static int IN_Rounding = 6;
        private static int IN_Maximum_Rake_Angle = 7;
        private static int IN_SuperRiser = 8;
        private static int IN_Vomatory = 9;
        private static int OUT_Tier = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tier", "T", "A Tier object", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Instance a new tier
            StadiumTools.Tier newTier = new StadiumTools.Tier();

            //Set parameters from Data Access
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
        private static void ConstructTierFromDA(IGH_DataAccess DA, StadiumTools.Tier tier)
        {
            //Item Containers (Destinations)
            StadiumTools.SpectatorGoo spectatorGooItem = new StadiumTools.SpectatorGoo();
            StadiumTools.SuperRiser superItem = new StadiumTools.SuperRiser();
            StadiumTools.Vomatory vomItem = new StadiumTools.Vomatory();
            int intItem = 0;
            bool boolItem = false;
            double doubleItem = 0.0;
            double[] doubleArrayItem = new double[0];

            if(!DA.GetData(IN_Spectator, ref spectatorGooItem)) { return; }   
            tier.SpectatorParameters = spectatorGooItem.Value;

            if (!DA.GetData(IN_Start_X, ref doubleItem)) { return; }
            tier.StartX = doubleItem * tier.SpectatorParameters.Unit;

            if (!DA.GetData(IN_Start_Y, ref doubleItem)) { return; }
            tier.StartY = doubleItem * tier.SpectatorParameters.Unit;

            if (!DA.GetData(IN_Start, ref boolItem)) { return; }
            tier.BuildFromPreviousTier = boolItem;

            if (!DA.GetData(IN_Row_Count, ref intItem)) { return; }
            tier.RowCount = intItem;

            tier.Spectators = new StadiumTools.Spectator[tier.RowCount];

            if (!DA.GetData(IN_Row_Width, ref doubleItem)) { return; }
            tier.DefaultRowWidth = doubleItem * tier.SpectatorParameters.Unit;
            
            //Set all row widths to default value
            double[] rowWidths = new double[tier.RowCount];
            for (int i = 0; i < rowWidths.Length; i++)
            {
                rowWidths[i] = tier.DefaultRowWidth;
            }

            if (!DA.GetData(IN_Rounding, ref doubleItem)) { return; }
            tier.RoundTo = doubleItem * tier.SpectatorParameters.Unit;

            if (!DA.GetData(IN_Maximum_Rake_Angle, ref doubleItem)) { return; }
            tier.MaxRakeAngle = doubleItem;

            DA.GetData(IN_SuperRiser, ref superItem);
            tier.SuperRiser = superItem;

            DA.GetData(IN_Vomatory, ref vomItem);
            tier.Vomatory = vomItem;

            if (tier.SuperRiser.Row > 0)
            {
                tier.SuperHas = true;
            }

            if (tier.SuperHas)
            {
                rowWidths[tier.SuperRiser.Row] = (tier.SuperRiser.Width * rowWidths[tier.SuperRiser.Row]);
            }

            //Set Remaining parameters
            tier.Plane = StadiumTools.Pln3d.XYPlane;
            tier.RowWidths = rowWidths; 
            tier.RiserHeights = new double[tier.RowCount - 1];
            tier.Points2dCount = StadiumTools.Tier.GetTierPtCount(tier);
            tier.Points2d = new StadiumTools.Pt2d[tier.Points2dCount];
        }
    }
}