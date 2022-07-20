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
            : base(nameof(ST_ConstructSuperRiser2D), "cS", "Construct a SuperRiser from parameters", "StadiumTools", "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            StadiumTools.Spectator s = new StadiumTools.Spectator();
            StadiumTools.Spectator.InitDefault(s);
            pManager.AddGenericParameter("Spectator", "Sp", "Spectator object to inerit parameters from", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Super Row", "sR", "Row to replace with super riser", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("Width", "sW", "Width of SuperRiser as a multiple of the default tier row width", GH_ParamAccess.item, 3);
            pManager.AddNumberParameter("Curb Width", "scX", "Optional width of curb before super riser", GH_ParamAccess.item, 0.01 * s.Unit);
            pManager.AddNumberParameter("Curb Height", "ScY", "Optional height of curb before super riser", GH_ParamAccess.item, 0.01 * s.Unit);
            pManager.AddNumberParameter("Guardrail Width", "gW", "Width of guardrail behind SuperRiser", GH_ParamAccess.item, 0.01 * s.Unit);
        }

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
            int destination = 0;
            if (DA.GetData<int>(1, ref destination))
            {
                if (destination < 0)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Row must be non-negative and not equal 0");
                }
            }

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
        public static void ConstructSuperRiserFromDA(IGH_DataAccess DA)
        {
            StadiumTools.SuperRiser superRiser = new StadiumTools.SuperRiser();
            StadiumTools.Spectator specItem = new StadiumTools.Spectator();
            int intItem = 0;
            double doubleItem = 0.0;

            //Set Spectator Params
            if (!DA.GetData<StadiumTools.Spectator>(0, ref specItem))
                return;
            superRiser.SpectatorParameters = specItem;

            //Set SuperRow
            if (!DA.GetData<int>(1, ref intItem))
                return;
            superRiser.Row = intItem;

            //Set SuperRow
            if (!DA.GetData<int>(2, ref intItem))
                return;
            superRiser.Width = intItem;

            //Set SuperCurbWidth
            if (!DA.GetData<double>(3, ref doubleItem))
                return;
            superRiser.CurbWidth = doubleItem;

            //Set SuperCurbHeight
            if (!DA.GetData<double>(4, ref doubleItem))
                return;
            superRiser.CurbHeight = doubleItem;

            //Set SuperGuardRailWidth
            if (!DA.GetData<double>(5, ref doubleItem))
                return;
            superRiser.GuardrailWidth = doubleItem;

            //Output New Super Riser
            DA.SetData(0, (object)superRiser);
        }


    }
}