using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSuperRiser using the GH_Component as base. 
    /// </summary>
    public class ST_ConstructSuperRiser2D : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_ConstructSuperRiser2D()
            : base(nameof(ST_ConstructSuperRiser2D), "cS", "Construct a SuperRiser from parameters", "StadiumTools", "2D Section")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            double unit = StadiumTools.UnitHandler.FromString("Rhino", Rhino.RhinoDoc.ActiveDoc.GetUnitSystemName(true, false, true, true));
            var defaultSpectator = new StadiumTools.Spectator();
            StadiumTools.Spectator.InitDefault(defaultSpectator, unit);
            pManager.AddGenericParameter("Spectator", "Sp", "Spectator object to inerit parameters from", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Row", "R", "Row to replace with super riser", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("Width", "sW", "Width of SuperRiser as a multiple of the default tier row width", GH_ParamAccess.item, 3);
            pManager.AddNumberParameter("Curb Width", "scX", "Optional width of curb before super riser", GH_ParamAccess.item, 0.1 * defaultSpectator.Unit);
            pManager.AddNumberParameter("Curb Height", "ScY", "Optional height of curb before super riser", GH_ParamAccess.item, 0.1 * defaultSpectator.Unit);
            pManager.AddNumberParameter("Guardrail Width", "gW", "Width of guardrail behind SuperRiser", GH_ParamAccess.item, 0.1 * defaultSpectator.Unit);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Spectator = 0;
        private static int IN_Row = 1;
        private static int IN_Width = 2;
        private static int IN_Curb_Width = 3;
        private static int IN_Curb_Height = 4;
        private static int IN_Guardrail_Width = 5;
        private static int OUT_SuperRiser = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SuperRiser", "SR", "A SuperRiser object", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            ST_ConstructSuperRiser2D.HandleErrors(DA, this);
            ST_ConstructSuperRiser2D.ConstructSuperRiserFromDA(DA);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_ConstructSuperRiser;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("89a005fd-3c4c-4ee4-be01-c9fc11ef0a7d");

        //Methods
        private static void HandleErrors(IGH_DataAccess DA, GH_Component thisComponent)
        {
            int intItem = 0;
            double doubleItem = 0.0;

            //Row number must be => 1
            if (DA.GetData<int>(IN_Row, ref intItem))
            {
                if (intItem < 1)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Row must be an integer, non-negative, and not equal to 0");
                }
            }
            if (DA.GetData<int>(IN_Width, ref intItem))
            {
                if (intItem < 1)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Width must be an integer, non-negative, and not equal to 0");
                }
            }
            //Guardrail Width must be > 0
            if (DA.GetData<double>(IN_Guardrail_Width, ref doubleItem))
            {
                if (doubleItem <= 0)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Guardrail Width must be non-negative and not equal to 0");
                }
            }

        }

        private static void ConstructSuperRiserFromDA(IGH_DataAccess DA)
        {
            var superRiser = new StadiumTools.SuperRiser();
            var spectatorGooItem = new StadiumTools.SpectatorGoo();
            int intItem = 0;
            double doubleItem = 0.0;

            //Set Spectator Params
            if (!DA.GetData<StadiumTools.SpectatorGoo>(IN_Spectator, ref spectatorGooItem)) { return; }
            StadiumTools.Spectator spectatorItem = spectatorGooItem.Value;
            double unit;
            superRiser.Unit = unit = spectatorItem.Unit;
            superRiser.EyeX = spectatorItem.EyeX;
            superRiser.EyeY = spectatorItem.EyeY;
            superRiser.SEyeX = spectatorItem.SEyeX;
            superRiser.SEyeY = spectatorItem.SEyeY;

            //Set SuperRow
            if (!DA.GetData<int>(IN_Row, ref intItem))
                return;
            superRiser.Row = intItem;

            //Set Width
            if (!DA.GetData<int>(IN_Width, ref intItem))
                return;
            superRiser.Width = intItem;

            //Set SuperCurbWidth
            if (!DA.GetData<double>(IN_Curb_Width, ref doubleItem))
                return;
            superRiser.CurbWidth = doubleItem;

            //Set SuperCurbHeight
            if (!DA.GetData<double>(IN_Curb_Height, ref doubleItem))
                return;
            superRiser.CurbHeight = doubleItem;

            //Set SuperGuardRailWidth
            if (!DA.GetData<double>(IN_Guardrail_Width, ref doubleItem))
                return;
            superRiser.GuardrailWidth = doubleItem;

            //Output New Super Riser
            DA.SetData(OUT_SuperRiser, superRiser);
        }


    }
}