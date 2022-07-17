using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructTier2D using the GH_Component as base. 
    /// </summary>
    public class ST_ConstructTier2D : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D tier. 
        /// </summary>
        public ST_ConstructTier2D()
            : base(nameof(ST_ConstructTier2D), "cT", "Construct a 2D seating tier from parameters", "StadiumTools", "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            StadiumTools.Tier defaultTier = new StadiumTools.Tier();
            defaultTier.InitializeDefault();
            pManager.AddGenericParameter("Spectator", "S", "A Spectator to Inherit Spectator Parameters From", GH_ParamAccess.item);
            pManager.AddNumberParameter("Start X", "sX", "Horizontal start distance of Tier from P.O.F", GH_ParamAccess.item, defaultTier.StartX);
            pManager.AddNumberParameter("Start Y", "sY", "Vertical start distance of Tier from P.O.F", GH_ParamAccess.item, defaultTier.StartY);
            pManager.AddIntegerParameter("Row Count", "rC", "Number of rows", GH_ParamAccess.item, defaultTier.RowCount);
            pManager.AddNumberParameter("Row Width", "rW", "Row width", GH_ParamAccess.item, defaultTier.DefaultRowWidth);
            pManager.AddNumberParameter("Rounding", "r", "Increment to round riser heights up to", GH_ParamAccess.item, defaultTier.RoundTo);
            pManager.AddNumberParameter("Maximum Rake Angle", "mrA", "Maximum rake angle in Radians (Tan(riser/row))", GH_ParamAccess.item, defaultTier.MaxRakeAngle);      
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
            StadiumTools.Tier newTier = new StadiumTools.Tier();
            ST_ConstructTier2D.ConstructTierFromDA(DA, newTier);
            DA.SetData(0, (object)newTier);
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
        public override Guid ComponentGuid => new Guid("072677e1-616c-4028-ab7e-b9717e7699b1");

        //Methods
        public static void ConstructTierFromDA(IGH_DataAccess DA, StadiumTools.Tier tier)
        {
            StadiumTools.Spectator spectatorItem = new StadiumTools.Spectator();
            int intItem = 0;
            double doubleItem = 0.0;
            bool boolItem = false;
            double[] doubleArrayItem = new double[0];

            //Set Spectator Properties
            if (!DA.GetData<StadiumTools.Spectator>(0, ref spectatorItem))
                return;
            tier.SpectatorParameters = spectatorItem;

            //Set StartX
            if (!DA.GetData<double>(1, ref doubleItem))
                return;
            tier.StartX = doubleItem;

            //Set StartY
            if (!DA.GetData<double>(2, ref doubleItem))
                return;
            tier.StartY = doubleItem;

            //Set RowCount
            if (!DA.GetData<int>(3, ref intItem))
                return;
            tier.RowCount = intItem;

            //Set RowWidth
            if (!DA.GetData<double>(4, ref doubleItem))
                return;
            tier.DefaultRowWidth = doubleItem;

            //Set RowWidth
            if (!DA.GetData<double>(5, ref doubleItem))
                return;
            tier.RoundTo = doubleItem;

            //Set RowWidth
            if (!DA.GetData<double>(6, ref doubleItem))
                return;
            tier.MaxRakeAngle = doubleItem;
        }


    }
}