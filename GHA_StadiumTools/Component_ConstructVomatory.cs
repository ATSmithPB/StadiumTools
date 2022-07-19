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
    public class ST_ConstructVomatory : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_ConstructVomatory()
            : base(nameof(ST_ConstructVomatory), "cV", "Construct a Vomatory from parameters", "StadiumTools", "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            StadiumTools.Vomatory defaultVomatory = new StadiumTools.Vomatory();
            StadiumTools.Vomatory.InitDefault(defaultVomatory);
            pManager.AddIntegerParameter("Start Row", "sR", "Start Row to replace with vomatory", GH_ParamAccess.item, defaultVomatory.Start);
            pManager.AddIntegerParameter("Height (Rows)", "H", "Height of vomatory in number of rows(+ risers)", GH_ParamAccess.item, defaultVomatory.Height);
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Vomatory", "V", "A Vomatory object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            StadiumTools.Vomatory newVomatory = new StadiumTools.Vomatory();
            ST_ConstructVomatory.ConstructVomatoryFromDA(DA, newVomatory);
            DA.SetData(0, (object)newVomatory);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_ConstructVomatory;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("1f72f1d3-d687-4b4c-ab5f-fc7cdbc21500");

        //Methods
        public static void ConstructVomatoryFromDA(IGH_DataAccess DA, StadiumTools.Vomatory vomatory)
        {
            //Item Containers (Destinations)
            int intItem = 0;

            //Set Start
            if (!DA.GetData<int>(0, ref intItem))
                return;
            vomatory.Start = intItem;

            //Set Width
            if (!DA.GetData<int>(1, ref intItem))
                return;
            vomatory.Height = intItem;

            
        }


    }
}
