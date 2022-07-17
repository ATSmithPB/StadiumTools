using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;

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
            : base(nameof(ST_ConstructSpectator), "cS", "Construct a Spectator from parameters", "StadiumTools", "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            StadiumTools.Spectator defaultSpectator = new StadiumTools.Spectator();
            StadiumTools.Spectator.InitializeDefault(defaultSpectator);
            pManager.AddNumberParameter("C-Value", "C", "Target spectator C-value", GH_ParamAccess.item, defaultSpectator.TargetCValue);
            pManager.AddNumberParameter("Eye Horizontal", "eX", "Horizontal distance of spectator eyes from rear riser", GH_ParamAccess.item, defaultSpectator.EyeX);
            pManager.AddNumberParameter("Eye Verical", "eY", "Vertical distance of spectator eyes from floor", GH_ParamAccess.item, defaultSpectator.EyeY);
            pManager.AddNumberParameter("Standing Eye Horizontal", "SteX", "Eye Horizontal for standing spectators", GH_ParamAccess.item, defaultSpectator.SEyeX);
            pManager.AddNumberParameter("Standing Eye Vertical", "SteY", "Eye Vertical for standing spectators", GH_ParamAccess.item, defaultSpectator.SEyeY);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Spectator", "S", "A Spectator object", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            StadiumTools.Spectator newSpectator = new StadiumTools.Spectator();
            ST_ConstructSpectator.ConstructSpectatorFromDA(DA, newSpectator);
            DA.SetData(0, (object)newSpectator);
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
        public override Guid ComponentGuid => new Guid("58377ebd-7006-4eb0-8b11-c419e01b50d3");

        //Methods
        public static void ConstructSpectatorFromDA(IGH_DataAccess DA, StadiumTools.Spectator spectator)
        {
            double doubleItem = 0.0;
            double[] doubleArrayItem = new double[0];

            if (!DA.GetData<double>(0, ref doubleItem))
                return;
            spectator.TargetCValue = doubleItem;
            if (!DA.GetData<double>(1, ref doubleItem))
                return;
            spectator.EyeX = doubleItem;
            if (!DA.GetData<double>(2, ref doubleItem))
                return;
            spectator.EyeY = doubleItem;
            if (!DA.GetData<double>(3, ref doubleItem))
                return;
            spectator.SEyeX = doubleItem;
            if (!DA.GetData<double>(4, ref doubleItem))
                return;
            spectator.SEyeY = doubleItem;
        }


    }
}