using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called Tier Parameters using the GH_Component as base. 
    /// </summary>
    public class ConstructTier : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new tier. 
        /// </summary>
        public ConstructTier()
            : base(nameof(ConstructTier), "cT", "Construct a seating tier from parameters", "StadiumTools", "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            StadiumTools.Tier defaultTier = new StadiumTools.Tier();
            defaultTier.InitializeDefault();
            pManager.AddPointParameter("Point of Focus", "POF", "Optional Point of Focus for Tier", GH_ParamAccess.item, Rhino.Geometry.Point3d.Origin);
            pManager.AddNumberParameter("Start X", "sX", "Horizontal start distance from P.O.F", GH_ParamAccess.item, defaultTier.StartX);
            pManager.AddNumberParameter("Start Y", "sY", "Vertical start distance from P.O.F", GH_ParamAccess.item, defaultTier.StartY);
            pManager.AddNumberParameter("C-Value", "C", "Max allowable spectator C-value", GH_ParamAccess.item, defaultTier.MinimumC);
            pManager.AddIntegerParameter("Row Count", "rC", "Number of rows", GH_ParamAccess.item, defaultTier.RowCount);
            pManager.AddNumberParameter("Row Width", "rW", "Row width", GH_ParamAccess.item, defaultTier.DefaultRowWidth);
            pManager.AddNumberParameter("Eye Horizontal", "eX", "Horizontal distance of spectator eyes from rear riser", GH_ParamAccess.item, defaultTier.EyeX);
            pManager.AddNumberParameter("Eye Verical", "eY", "Vertical distance of spectator eyes from floor", GH_ParamAccess.item, defaultTier.EyeY);
            pManager.AddNumberParameter("Standing Eye Horizontal", "SteX", "Eye Horizontal for standing spectators", GH_ParamAccess.item, defaultTier.SEyeX);
            pManager.AddNumberParameter("Standing Eye Vertical", "SteY", "Eye Vertical for standing spectators", GH_ParamAccess.item, defaultTier.SEyeY);
            pManager.AddBooleanParameter("Super?", "S?", "True if tier has a super riser", GH_ParamAccess.item, defaultTier.SuperHas);
            pManager.AddIntegerParameter("Super Row", "sR", "Start row for super riser", GH_ParamAccess.item, defaultTier.SuperRow);
            pManager.AddNumberParameter("Super Curb Width", "scX", "Optional width of curb before super riser", GH_ParamAccess.item, defaultTier.SuperCurbWidth);
            pManager.AddNumberParameter("Super Curb Height", "ScY", "Optional height of curb before super riser", GH_ParamAccess.item, defaultTier.SuperCurbHeight);
            pManager.AddNumberParameter("Super Eye Horizontal", "SeX", "Eye Vertical offset for SuperRiser", GH_ParamAccess.item, defaultTier.SuperEyeX);
            pManager.AddNumberParameter("Super Eye Vertical", "SeY", "Eye Horizontal offset for SuperRiser", GH_ParamAccess.item, defaultTier.SuperEyeY);
            pManager.AddNumberParameter("Super Rail Width", "srW", "Width of guardrail behind SuperRiser", GH_ParamAccess.item, defaultTier.SuperGuardrailWidth);
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
            ConstructTier.ConstructTierFromDA(DA, newTier);
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
            Rhino.Geometry.Point3d pointItem = new Rhino.Geometry.Point3d();
            int intItem = 0;
            double doubleItem = 0.0;
            bool boolItem = false;
            double[] doubleArrayItem = new double[0];
            
            if (!DA.GetData<Rhino.Geometry.Point3d>(0, ref pointItem))
                return;
            tier.POF = StadiumTools.IO.Rhino.Pt2dFromPoint3d(pointItem);
            if (!DA.GetData<double>(1, ref doubleItem))
                return;
            tier.StartX = doubleItem;
            if (!DA.GetData<double>(2, ref doubleItem))
                return;
            tier.StartY = doubleItem;
            if (!DA.GetData<double>(3, ref doubleItem))
                return;
            tier.MinimumC = doubleItem;
            if (!DA.GetData<int>(4, ref intItem))
                return;
            tier.RowCount = intItem;
            if (!DA.GetData<double>(5, ref doubleItem))
                return;
            tier.DefaultRowWidth = doubleItem;
            if (!DA.GetData<double>(6, ref doubleItem))
                return;
            tier.EyeX = doubleItem;
            if (!DA.GetData<double>(7, ref doubleItem))
                return;
            tier.EyeY = doubleItem;
            if (!DA.GetData<double>(8, ref doubleItem))
                return;
            tier.SEyeX = doubleItem;
            if (!DA.GetData<double>(9, ref doubleItem))
                return;
            tier.SEyeY = doubleItem;
            if (!DA.GetData<bool>(10, ref boolItem))
                return;
            tier.SuperHas = boolItem;
            if (!DA.GetData<int>(11, ref intItem))
                return;
            tier.SuperRow = intItem;
            if (!DA.GetData<double>(12, ref doubleItem))
                return;
            tier.SuperCurbWidth = doubleItem;
            if (!DA.GetData<double>(13, ref doubleItem))
                return;
            tier.SuperCurbHeight = doubleItem;
            if (!DA.GetData<double>(14, ref doubleItem))
                return;
            tier.SuperEyeX = doubleItem;
            if (!DA.GetData<double>(15, ref doubleItem))
                return;
            tier.SuperEyeY = doubleItem;
            if (!DA.GetData<double>(16, ref doubleItem))
                return;
            tier.SuperGuardrailWidth = doubleItem;

        }


    }
}