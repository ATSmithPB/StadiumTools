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
    public class ST_Bowl3dBreps : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D section. 
        /// </summary>
        public ST_Bowl3dBreps()
            : base(nameof(ST_Bowl3dBreps), "B3dB", "Calculate the Brep geometry of a StadiumTools Bowl3d", "StadiumTools", "3D")
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
        private static int OUT_Bowl3d_Breps = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Bowl3d Breps", "B3dB", "The Brep geometry of a StadiumTools Bowl3d object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Construct a new section from Data Access
            Rhino.Geometry.Mesh newBowl3dMesh = ST_Bowl3dBreps.ConstructBowl3dBrepsFromDA(DA, this);

            //Output Mesh
            DA.SetData(OUT_Bowl3d_Breps, newBowl3dMesh);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_Bowl3DBreps;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("282f2fd8-55db-4ea7-8a2f-7b06d0f044d0");

        //Methods  
        private static Rhino.Geometry.Mesh ConstructBowl3dBrepsFromDA(IGH_DataAccess DA, GH_Component thisComponent)
        {
            //Item Containers
            var bowl3dGooItem = new StadiumTools.Bowl3dGoo();

            //Get Goos
            DA.GetData<StadiumTools.Bowl3dGoo>(IN_Bowl3d, ref bowl3dGooItem);

            var bowl3dItem = bowl3dGooItem.Value;

            //Construct a new Bowl3d Mesh
            var newSTBowl3dMesh = bowl3dItem.ToMesh();
            Rhino.Geometry.Mesh newRCBowl3dMesh = StadiumTools.IO.RCMeshFromSTMesh(newSTBowl3dMesh);
            ///Mesh to nurbs

            return newRCBowl3dMesh;
        }




    }
}
