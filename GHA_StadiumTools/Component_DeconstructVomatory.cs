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
    public class ST_DeonstructVomatory : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_DeonstructVomatory()
            : base(nameof(ST_DeonstructVomatory), "dV", "Deonstruct a Vomatory into its parameters", "StadiumTools", "2D Section")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Vomatory", "V", "A Vomatory object", GH_ParamAccess.item);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Vomatory = 0;
        private static int OUT_Start_Row = 0;
        private static int OUT_Height = 1;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Start Row", "sR", "Start Row of vomatory", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Height (Rows)", "H", "Height of vomatory in number of rows(+ risers)", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_DeonstructVomatory.DeconstructVomatoryFromDA(DA);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_DeconstructVomatory;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("d97a242d-7ec4-461d-9934-fed350a41dd9");

        //Methods
        public static void DeconstructVomatoryFromDA(IGH_DataAccess DA)
        {
            //Item Containers (Destinations)
            StadiumTools.Vomatory vomItem = new StadiumTools.Vomatory();

            //Get Vomatory
            if (!DA.GetData<StadiumTools.Vomatory>(IN_Vomatory, ref vomItem))
                return;
            
            //Set Start
            DA.SetData(OUT_Start_Row, vomItem.Start);

            //Set Height
            DA.SetData(OUT_Height, vomItem.Height);
        }

    }
}
