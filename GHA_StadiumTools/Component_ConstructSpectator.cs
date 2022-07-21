using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSpectator using the GH_Component as base. 
    /// </summary>
    public class ST_ConstructSpectator : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_ConstructSpectator()
            : base(nameof(ST_ConstructSpectator), "cSp", "Construct a Spectator from parameters", "StadiumTools", "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            StadiumTools.Spectator defaultSpectator = new StadiumTools.Spectator();
            StadiumTools.Spectator.InitDefault(defaultSpectator);
            pManager.AddNumberParameter("C-Value", "C", "Target spectator C-value", GH_ParamAccess.item, defaultSpectator.TargetCValue);
            pManager.AddNumberParameter("Eye Horizontal", "eX", "Horizontal distance of spectator eyes from rear riser", GH_ParamAccess.item, defaultSpectator.EyeX);
            pManager.AddNumberParameter("Eye Verical", "eY", "Vertical distance of spectator eyes from floor", GH_ParamAccess.item, defaultSpectator.EyeY);
            pManager.AddNumberParameter("Standing Eye Horizontal", "SteX", "Eye Horizontal for standing spectators", GH_ParamAccess.item, defaultSpectator.SEyeX);
            pManager.AddNumberParameter("Standing Eye Vertical", "SteY", "Eye Vertical for standing spectators", GH_ParamAccess.item, defaultSpectator.SEyeY);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_C_Value = 0;
        private static int IN_Eye_Horizontal = 1;
        private static int IN_Eye_Vertical = 2;
        private static int IN_Standing_Eye_Horiz = 3;
        private static int IN_Standing_Eye_Vert = 4;
        private static int OUT_Spectator = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Spectator", "Sp", "A Spectator object", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            StadiumTools.Spectator newSpectator = new StadiumTools.Spectator();
            newSpectator.Unit = 1.0;
            ST_ConstructSpectator.ConstructSpectatorFromDA(DA, newSpectator);
            DA.SetData(OUT_Spectator, (object)newSpectator);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_ConstructSpectator;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("58377ebd-7006-4eb0-8b11-c419e01b50d3");

        //Methods
        private static void HandleErrors(IGH_DataAccess DA, GH_Component thisComponent)
        {
            double doubleItem = 0.0;

            //Row number must be => 1
            if (DA.GetData<double>(IN_C_Value, ref doubleItem))
            {
                if (doubleItem < 0.06)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "A C-Value below 60mm is not ideal");
                    
                    if (doubleItem <= 0)
                    {
                        thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "C-Value must be non-negative and greater than zero");
                    }
                }
            }
            //Guardrail Width must be > 0
            if (DA.GetData<double>(IN_Eye_Vertical, ref doubleItem)) 
            {
                if (doubleItem <= 0.5)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Eye Vertical must be non-negative and not equal to 0");
                }
            }

        }
        private static void ConstructSpectatorFromDA(IGH_DataAccess DA, StadiumTools.Spectator spectator)
        {
            double doubleItem = 0.0;
            double[] doubleArrayItem = new double[0];

            if (!DA.GetData<double>(IN_C_Value, ref doubleItem))
                return;
            spectator.TargetCValue = doubleItem;
            if (!DA.GetData<double>(IN_Eye_Horizontal, ref doubleItem))
                return;
            spectator.EyeX = doubleItem;
            if (!DA.GetData<double>(IN_Eye_Vertical, ref doubleItem))
                return;
            spectator.EyeY = doubleItem;
            if (!DA.GetData<double>(IN_Standing_Eye_Horiz, ref doubleItem))
                return;
            spectator.SEyeX = doubleItem;
            if (!DA.GetData<double>(IN_Standing_Eye_Vert, ref doubleItem))
                return;
            spectator.SEyeY = doubleItem;
        }


    }
}