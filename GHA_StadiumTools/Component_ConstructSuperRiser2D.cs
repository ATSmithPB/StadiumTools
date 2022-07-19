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
            StadiumTools.SuperRiser defaultSuperRiser = new StadiumTools.SuperRiser();
            StadiumTools.SuperRiser.InitDefault(defaultSuperRiser);
            pManager.AddIntegerParameter("Super Row", "sR", "Row to replace with super riser", GH_ParamAccess.item, defaultSuperRiser.Row);
            pManager.AddNumberParameter("Curb Width", "scX", "Optional width of curb before super riser", GH_ParamAccess.item, defaultSuperRiser.CurbWidth);
            pManager.AddNumberParameter("Curb Height", "ScY", "Optional height of curb before super riser", GH_ParamAccess.item, defaultSuperRiser.CurbHeight);
            pManager.AddNumberParameter("Eye Horizontal", "SeX", "Eye Vertical offset for SuperRiser", GH_ParamAccess.item, defaultSuperRiser.EyeX);
            pManager.AddNumberParameter("Eye Vertical", "SeY", "Eye Horizontal offset for SuperRiser", GH_ParamAccess.item, defaultSuperRiser.EyeY);
            pManager.AddNumberParameter("Guardrail Width", "gW", "Width of guardrail behind SuperRiser", GH_ParamAccess.item, defaultSuperRiser.GuardrailWidth);
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
            if (DA.GetData<int>(0, ref destination))
            {
                if (destination < 0)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Row must be non-negative and not equal 0");
                }
            }

            StadiumTools.SuperRiser newSuperRiser = new StadiumTools.SuperRiser();
            ST_ConstructSuperRiser2D.ConstructSuperRiserFromDA(DA, newSuperRiser);
            DA.SetData(0, (object)newSuperRiser);
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
        public static void ConstructSuperRiserFromDA(IGH_DataAccess DA, StadiumTools.SuperRiser superRiser)
        {
            int intItem = 0;
            double doubleItem = 0.0;
            double[] doubleArrayItem = new double[0];

            //Set SuperRow
            if (!DA.GetData<int>(0, ref intItem))
                return;
            superRiser.Row = intItem;
            
            //Set SuperCurbWidth
            if (!DA.GetData<double>(1, ref doubleItem))
                return;
            superRiser.CurbWidth = doubleItem;

            //Set SuperCurbHeight
            if (!DA.GetData<double>(2, ref doubleItem))
                return;
            superRiser.CurbHeight = doubleItem;

            //Set StartEyeX
            if (!DA.GetData<double>(3, ref doubleItem))
                return;
            superRiser.EyeX = doubleItem;

            //Set SuperEyeY
            if (!DA.GetData<double>(4, ref doubleItem))
                return;
            superRiser.EyeY = doubleItem;

            //Set SuperGuardRailWidth
            if (!DA.GetData<double>(5, ref doubleItem))
                return;
            superRiser.GuardrailWidth = doubleItem;
        }


    }
}