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
            : base(nameof(ST_ConstructSpectator), "cSp", "Construct a Spectator from parameters", "StadiumTools", "2D Section")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //Get Unit coeffecient for default values
            double unit = StadiumTools.UnitHandler.FromString("Rhino", Rhino.RhinoDoc.ActiveDoc.GetUnitSystemName(true, false, true, true));
            var defaultSpectator = new StadiumTools.Spectator();
            StadiumTools.Spectator.InitDefault(defaultSpectator, unit);
            pManager.AddIntegerParameter("Target C-Value", "C", "Target spectator C-value in millimeters", GH_ParamAccess.item, defaultSpectator.TargetCValue);
            pManager.AddNumberParameter("Eye Horizontal", "eX", "Horizontal distance of spectator eyes from rear riser", GH_ParamAccess.item, defaultSpectator.EyeX);
            pManager.AddNumberParameter("Eye Verical", "eY", "Vertical distance of spectator eyes from floor", GH_ParamAccess.item, defaultSpectator.EyeY);
            pManager.AddNumberParameter("Standing Eye Horizontal", "SteX", "Optional Eye Horizontal for standing spectators", GH_ParamAccess.item, defaultSpectator.SEyeX);
            pManager.AddNumberParameter("Standing Eye Vertical", "SteY", "Optional Eye Vertical for standing spectators", GH_ParamAccess.item, defaultSpectator.SEyeY);
            pManager.AddIntegerParameter("Unit Override [in development]", "dU", "0 = Document Unit. 1 = mm. 2 = cm. 3 = m. 4 = in. 5 = ft. 6 = yrd", GH_ParamAccess.item, 0);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_C_Value = 0;
        private static int IN_Eye_Horizontal = 1;
        private static int IN_Eye_Vertical = 2;
        private static int IN_Standing_Eye_Horiz = 3;
        private static int IN_Standing_Eye_Vert = 4;
        private static int IN_Units_Override = 5;
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
            //Check if unit system is supported by StadiumTools
            string unitSystemName = Rhino.RhinoDoc.ActiveDoc.GetUnitSystemName(true, false, true, true);
            
            //Handle Errors
            ST_ConstructSpectator.HandleErrors(DA, this, unitSystemName);

            //Instance a new Spectator
            var newSpectator = new StadiumTools.Spectator();
            newSpectator.Unit = 1.0;

            //Set parameters from Data Access
            ST_ConstructSpectator.ConstructSpectatorFromDA(DA, newSpectator, unitSystemName);

            //GH_Goo<T> wrapper
            var newSpectatorGoo = new StadiumTools.SpectatorGoo(newSpectator);

            //Output Goo
            DA.SetData(OUT_Spectator, newSpectatorGoo);

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
        private static void HandleErrors(IGH_DataAccess DA, GH_Component thisComponent, string unitSystemName)
        {
            

            int intItem = 0;
            double doubleItem = 0.0;

            //Row number must be => 1
            if (DA.GetData<int>(IN_C_Value, ref intItem))
            {
                if (intItem < 60)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "A C-Value below 60mm is not ideal");
                    
                    if (intItem <= 0)
                    {
                        thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "C-Value must be an integer, non-negative, and greater than zero");
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
            if (DA.GetData<int>(IN_Units_Override, ref intItem))
            {
                if (intItem > 6 || intItem < 0)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Units Override: [{intItem}] must be an integer between 0 and 6");
                }
            }
            if (DA.GetData<int>(IN_Units_Override, ref intItem))
            {
                bool validUnitSystem = StadiumTools.UnitHandler.isValid(unitSystemName);

                if (intItem == 0 && !validUnitSystem)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Current Rhino Document Units [{unitSystemName}] not supported. Please change, or use an override.");
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DA"></param>
        /// <param name="newSpectator"></param>
        /// <param name="unitSystemName"></param>
        private static void ConstructSpectatorFromDA(IGH_DataAccess DA, StadiumTools.Spectator newSpectator, string unitSystemName)
        {
            int intItem = 0;
            double doubleItem = 0.0;
            double[] doubleArrayItem = new double[0];

            SetUnits(DA, ref newSpectator, unitSystemName);

            if (!DA.GetData<int>(IN_C_Value, ref intItem)) { return; }
            newSpectator.TargetCValue = intItem;

            if (!DA.GetData<double>(IN_Eye_Horizontal, ref doubleItem)) { return; }
            newSpectator.EyeX = doubleItem;

            if (!DA.GetData<double>(IN_Eye_Vertical, ref doubleItem)) { return; }
            newSpectator.EyeY = doubleItem;

            if (!DA.GetData<double>(IN_Standing_Eye_Horiz, ref doubleItem)) { return; }
            newSpectator.SEyeX = doubleItem;

            if (!DA.GetData<double>(IN_Standing_Eye_Vert, ref doubleItem)) { return; }
            newSpectator.SEyeY = doubleItem;
        }

        /// <summary>
        /// sets a spectator's unit coeffecient (m/unit) based on a set of integer flags from 0-6
        /// </summary>
        /// <param name="DA"></param>
        /// <param name="spectator"></param>
        /// <param name="unitSystemName"></param>
        private static void SetUnits(IGH_DataAccess DA, ref StadiumTools.Spectator spectator, string unitSystemName)
        {
            double metersPerUnit = StadiumTools.UnitHandler.FromString("Rhino", unitSystemName);
            
            int intItem = 0;

            if (!DA.GetData<int>(IN_Units_Override, ref intItem)) { return; }
            switch (intItem)
            {
                case 0:
                    spectator.Unit = metersPerUnit;
                    break;
                case 1:
                    spectator.Unit = StadiumTools.UnitHandler.mm;
                    break;
                case 2:
                    spectator.Unit = StadiumTools.UnitHandler.cm;
                    break;
                case 3:
                    spectator.Unit = StadiumTools.UnitHandler.m;
                    break;
                case 4:
                    spectator.Unit = StadiumTools.UnitHandler.inch;
                    break;
                case 5:
                    spectator.Unit = StadiumTools.UnitHandler.feet;
                    break;
                case 6:
                    spectator.Unit = StadiumTools.UnitHandler.yard;
                    break;
            }
        }
    }
}