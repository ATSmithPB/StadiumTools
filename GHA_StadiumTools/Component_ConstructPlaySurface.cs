using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;
using System.Collections.Generic;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSuperRiser using the GH_Component as base. 
    /// </summary>
    public class ST_ConstructPlaySurface : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_ConstructPlaySurface()
            : base(nameof(ST_ConstructPlaySurface), "cPS", "Construct a PlaySurface from parameters", "StadiumTools", "2D Plan")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            string types = StadiumTools.PlaySurface.TypeEnumAsString();
            pManager.AddIntegerParameter("Type", "T", $"The Pitch/Field type to construct: {types} ", GH_ParamAccess.item, 0);
            pManager.AddPlaneParameter("Plane", "Pl", "Plane of Playsurface. Plane Origin is center pitch. X-Axis is aligned to the long axis for each pitch", GH_ParamAccess.item, Rhino.Geometry.Plane.WorldXY);
            pManager.AddVectorParameter("North", "N", "A Vector ", GH_ParamAccess.item, Rhino.Geometry.Vector3d.YAxis);
            pManager.AddIntegerParameter("LOD", "lod", "Optional Level-Of-Detail for Markings output", GH_ParamAccess.item, 0);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Type = 0;
        private static int IN_Plane = 1;
        private static int IN_North = 2;
        private static int IN_LOD = 3;
        private static int OUT_PlaySurface = 0;
        private static int OUT_Boundary = 1;
        private static int OUT_Markings = 2;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PlaySurface", "PS", "A PlaySurface object", GH_ParamAccess.item);
            pManager.AddCurveParameter("Boundary", "b", "A closed polyline that represents the outermost PlaySurface boundary", GH_ParamAccess.item);
            pManager.AddCurveParameter("Markings", "m", "A collection of curves that represent common PlaySurface markings", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            StadiumTools.PlaySurface playSurface = new StadiumTools.PlaySurface();
            
            ST_ConstructPlaySurface.HandleErrors(DA, this);
            ST_ConstructPlaySurface.ConstructPlaySurfaceFromDA(DA, ref playSurface);

            StadiumTools.PlaySurfaceGoo newPlaySurfaceGoo = new StadiumTools.PlaySurfaceGoo(playSurface);
            Rhino.Geometry.PolyCurve boundary = StadiumTools.IO.PolyCurveFromICurve(playSurface.Boundary);
            Rhino.Geometry.Curve[] markings = StadiumTools.IO.CurveArrayFromICurveArray(playSurface.Markings);

            DA.SetData(OUT_PlaySurface, newPlaySurfaceGoo);
            DA.SetData(OUT_Boundary, boundary);
            DA.SetData(OUT_Markings, markings);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_ConstructPlaySurface;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("89a005fd-3c4c-4ee4-be01-c9fc11ef0a7d");

        //Methods
        private static void HandleErrors(IGH_DataAccess DA, GH_Component thisComponent)
        {
            int intItem = 0;

            //Row number must be => 1
            if (DA.GetData<int>(IN_Type, ref intItem))
            {
                if (intItem > 11 || intItem < 0)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Type must be an integer, non-negative, and not greater than 11");
                }
            }
        }

        private static void ConstructPlaySurfaceFromDA(IGH_DataAccess DA, ref StadiumTools.PlaySurface newPlaySurface)
        {
            //Containers (Destination)
            int intItem = 0;
            Rhino.Geometry.Plane planeItem = new Rhino.Geometry.Plane();
            Rhino.Geometry.Vector3d vectorItem = new Rhino.Geometry.Vector3d();

            //Set PlaySurface Params
            if (!DA.GetData<int>(IN_Type, ref intItem)) { return; }
            StadiumTools.PlaySurface.Type type = (StadiumTools.PlaySurface.Type)intItem;

            if (!DA.GetData<Rhino.Geometry.Plane>(IN_Plane, ref planeItem)) { return; }
            StadiumTools.Pln3d pln3d = StadiumTools.IO.Pln3dFromPlane(planeItem);
            StadiumTools.Pln2d plane = new StadiumTools.Pln2d(pln3d);

            if (!DA.GetData<Rhino.Geometry.Vector3d>(IN_North, ref vectorItem)) { return; }
            StadiumTools.Vec2d north = StadiumTools.IO.Vec2dFromVector3d(vectorItem);

            double unit = StadiumTools.UnitHandler.FromString("Rhino", Rhino.RhinoDoc.ActiveDoc.GetUnitSystemName(true, false, true, true));

            newPlaySurface = new StadiumTools.PlaySurface(plane, north, unit, type, 0);
        }

        
    }
}