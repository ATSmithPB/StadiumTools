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
            StadiumTools.Tier newTier = ST_ConstructTier2D.ConstructTierFromDA(DA);
            DA.SetData(0, newTier);
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
        public static StadiumTools.Tier ConstructTierFromDA(IGH_DataAccess DA)
        {
            //Item Containers (Destinations)
            StadiumTools.Tier tier = new StadiumTools.Tier();
            StadiumTools.Spectator spectatorItem = new StadiumTools.Spectator();
            StadiumTools.SuperRiser superItem = new StadiumTools.SuperRiser();
            StadiumTools.Vomatory vomItem = new StadiumTools.Vomatory();
            int intItem = 0;
            bool boolItem = false;
            double doubleItem = 0.0;
            double[] doubleArrayItem = new double[0];

            DA.GetData(0, ref spectatorItem);
            tier.SpectatorParameters = spectatorItem;

            DA.GetData(1, ref doubleItem); 
            tier.StartX = doubleItem * tier.SpectatorParameters.Unit;

            DA.GetData(2, ref doubleItem);
            tier.StartY = doubleItem * tier.SpectatorParameters.Unit;

            DA.GetData(3, ref boolItem);
            tier.BuildFromPreviousTier = boolItem;

            DA.GetData(4, ref intItem);
            tier.RowCount = intItem;

            DA.GetData(5, ref doubleItem);
            tier.DefaultRowWidth = doubleItem * tier.SpectatorParameters.Unit;

            DA.GetData(6, ref doubleItem);
            tier.RoundTo = doubleItem;

            DA.GetData(7, ref doubleItem);
            tier.MaxRakeAngle = doubleItem;
            tier.Spectators = new StadiumTools.Spectator[tier.RowCount];

            DA.GetData(8, ref superItem);
            tier.SuperRiser = superItem;

            DA.GetData(9, ref vomItem);
            tier.Vomatory = vomItem;

            tier.Plane = StadiumTools.Pln3d.XYPlane;

            // Init all row widths to default value
            double[] rowWidths = new double[tier.RowCount];
            for (int i = 0; i < rowWidths.Length; i++)
            {
                rowWidths[i] = tier.DefaultRowWidth;
            }

            if (tier.SuperRiser.Row > 0)
            {
                tier.SuperHas = true;
            }

            if (tier.SuperHas)
            {
                rowWidths[tier.SuperRiser.Row] = (tier.SuperRiser.Width * rowWidths[tier.SuperRiser.Row]);
            }

            tier.RowWidths = rowWidths; 
            tier.RiserHeights = new double[tier.RowCount - 1];
            tier.RoundTo = 0.001 * tier.SpectatorParameters.Unit;
            tier.Points2dCount = StadiumTools.Tier.GetTierPtCount(tier);
            tier.Points2d = new StadiumTools.Pt2d[tier.Points2dCount];

            return tier;
        }


    }
}