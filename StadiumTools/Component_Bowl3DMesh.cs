using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSection2D using the GH_Component as base. 
    /// </summary>
    public class ST_Bowl3dMesh : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D section. 
        /// </summary>
        public ST_Bowl3dMesh()
            : base(nameof(ST_Bowl3dMesh), "B3dM", "Calculate the mesh geometry of a StadiumTools Bowl3d", "StadiumTools", "3D")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bowl3d", "B3Dd", "a valid StadiumTools Bowl3d object", GH_ParamAccess.item);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Bowl3d = 0;
        private static int OUT_Bowl3d_Mesh = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Bowl3d Mesh", "B3dM", "The mesh geometry of a StadiumTools Bowl3d object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Construct a new section from Data Access
            Rhino.Geometry.Mesh newBowl3dMesh = ST_Bowl3dMesh.ConstructBowl3dMeshFromDA(DA, this);

            //Output Mesh
            DA.SetData(OUT_Bowl3d_Mesh, newBowl3dMesh);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_Bowl3DMesh;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("7f3f39dd-5330-44c6-8c43-0c3c2167e950");

        //Methods  
        private static Rhino.Geometry.Mesh ConstructBowl3dMeshFromDA(IGH_DataAccess DA, GH_Component thisComponent)
        {
            //Item Containers
            var bowl3dGooItem = new StadiumTools.Bowl3dGoo();

            //Get Goos
            DA.GetData<StadiumTools.Bowl3dGoo>(IN_Bowl3d, ref bowl3dGooItem);

            var bowl3dItem = bowl3dGooItem.Value;

            //Construct a new Bowl3d Mesh
            var newSTBowl3dMesh = bowl3dItem.ToMesh(); 
            Rhino.Geometry.Mesh newRCBowl3dMesh = StadiumTools.IO.RCMeshFromSTMesh(newSTBowl3dMesh);

            return newRCBowl3dMesh;
        }




    }
}
