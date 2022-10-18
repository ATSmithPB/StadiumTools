using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;
using StadiumTools;
using System.Collections.Generic;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSuperRiser using the GH_Component as base. 
    /// </summary>
    public class ST_STMesh : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_STMesh()
            : base(nameof(ST_STMesh), "stM", "Construct an RC Mesh from an ST Mesh", "StadiumTools", "Debug")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {            
            pManager.AddIntegerParameter("Flag", "F", "Integer flag to test different mesh bug cases", GH_ParamAccess.item, 0);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Flag = 0;
        private static int OUT_Mesh = 0;


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "The resulting Mesh", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_STMesh.STMeshFromDA(DA);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_debug;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("adcb3471-7c0f-4f75-832d-fc7fd3c5d9bc");

        //Methods
        private static void STMeshFromDA(IGH_DataAccess DA)
        {
            //Item Container (Destination)
            var intItem = 0;

            //Arc0 from paramaters
            if (!DA.GetData<int>(IN_Flag, ref intItem)) { return; }

            List<Rhino.Geometry.Mesh> meshes = new List<Rhino.Geometry.Mesh>();

            if (intItem == 0)
            {
                meshes.Add(StadiumTools.IO.RCMeshFromSTMesh(Mesh.PrimitiveCube()));
                meshes.Add(StadiumTools.IO.RCMeshFromSTMesh(Mesh.PrimitiveCube()));
                meshes.Add(StadiumTools.IO.RCMeshFromSTMesh(Mesh.PrimitiveCube()));
            }
            else if (intItem == 1)
            {
                meshes.Add(StadiumTools.IO.RCMeshFromSTMesh(Mesh.PrimitiveCube()));
            }
            else
            {
                throw new ArgumentException($"Flag[{intItem}] not supported.");
            }

            DA.SetDataList(OUT_Mesh, meshes);
        }
    }
}
