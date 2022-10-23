using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;
using System.Linq;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSection2D using the GH_Component as base. 
    /// </summary>
    public class ST_ConstructBoundaryOrtho : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D section. 
        /// </summary>
        public ST_ConstructBoundaryOrtho()
            : base(nameof(ST_ConstructBoundaryOrtho), "cbO", "Construct an Orthagonal Boundary (Front Bowl Edge) from Parameters", "StadiumTools", "2D Plan")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            double unit = StadiumTools.UnitHandler.FromString("Rhino", Rhino.RhinoDoc.ActiveDoc.GetUnitSystemName(true, false, true, true));
            double bw = 7.5 * unit;
            pManager.AddGenericParameter("PlaySurface", "PS", $"The Playsurface to inherit boundary dimensions from", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset X", "oX", "The X-offset of the bowl edges from the playSurface", GH_ParamAccess.item, 5 * unit);
            pManager.AddNumberParameter("Offset Y", "oY", "The Y-offset of the bowl edges from the playSurface", GH_ParamAccess.item, 5 * unit);
            pManager.AddNumberParameter("Corner Radaii", "cR", "The radaii to fillet all bowl corners. 0 for sharp corners", GH_ParamAccess.item, 12 * unit);
            pManager.AddNumberParameter("Bay Widths", "bw", "The width of bays (per side)", GH_ParamAccess.list, new List<double>() { bw, bw, bw, bw });
            pManager.AddIntegerParameter("Corner Bay Count", "cbC", "Number of bays within the rounded corners", GH_ParamAccess.item, 5);
            pManager.AddBooleanParameter("P.O.C", "POC", "True if a point should be centered (per side)", GH_ParamAccess.list, new List<bool>() { true, false, true, false });
        }

        //Set parameter indixes to names (for readability)
        private static int IN_PlaySurface = 0;
        private static int IN_Offset_X = 1;
        private static int IN_Offset_Y = 2;
        private static int IN_Corner_Radaii = 3;
        private static int IN_Bay_Widths = 4;
        private static int IN_Corner_Bay_Count = 5;
        private static int IN_POC = 6;
        private static int OUT_Boundary = 0;
        private static int OUT_Curves = 1;
        private static int OUT_Planes = 2;
        private static int OUT_Closest_Plane = 3;
        private static int OUT_Closest_Distance = 4;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Boundary", "B", "A Boundary object", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curves", "BC", "Boundary Curves", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Planes", "Pl", "Section Planes", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Closest Plane", "CPl", "Closest Section Plane on Boundary to the Touchline", GH_ParamAccess.item);
            pManager.AddNumberParameter("Closest Distance", "Cd", "Distance of Closest Section Plane on Boundary to the Touchline", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Construct a new section from Data Access
            StadiumTools.Boundary newBoundary = ST_ConstructBoundaryOrtho.ConstructBoundaryOrthoFromDA(DA, this);
            List<Rhino.Geometry.PolylineCurve> curves = StadiumTools.IO.PolylineCurveListFromPlines(newBoundary.Edges);
            Rhino.Geometry.Plane[] planes = StadiumTools.IO.PlanesFromPln3ds(newBoundary.Planes);
            Rhino.Geometry.Plane closestPlane = StadiumTools.IO.PlaneFromPln3d(newBoundary.ClosestPlane);

            DA.SetDataList(OUT_Curves, curves);
            DA.SetDataList(OUT_Planes, planes);
            DA.SetData(OUT_Closest_Plane, closestPlane);
            DA.SetData(OUT_Closest_Distance, newBoundary.ClosestPlaneDist);

            //GH_Goo<T> wrapper
            var newBoundaryGoo = new StadiumTools.BoundaryGoo(newBoundary);

            //Output Goo
            DA.SetData(OUT_Boundary, newBoundaryGoo);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_ConstructBoundaryOrthagonal;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("cd0428a1-1a72-4e62-8177-06e5acb42502");

        //Methods  
        private static StadiumTools.Boundary ConstructBoundaryOrthoFromDA(IGH_DataAccess DA, GH_Component thisComponent)
        {
            double tolerance = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            int sideCount = 4;

            //Item Containers  
            var playSurfaceGooItem = new StadiumTools.PlaySurfaceGoo();
            double offsetX = 0.0;
            double offsetY = 0.0;
            var cornerRadaii = 0.0;
            var bayWidthsList = new List<double>();
            int cornerBayCount = 0;
            var pocList = new List<bool>();

            //Get PlaySurface
            DA.GetData<StadiumTools.PlaySurfaceGoo>(IN_PlaySurface, ref playSurfaceGooItem);
            var ps = playSurfaceGooItem.Value;

            DA.GetData<double>(IN_Offset_X, ref offsetX);
            DA.GetData<double>(IN_Offset_Y, ref offsetY);
            DA.GetData<double>(IN_Corner_Radaii, ref cornerRadaii);
            DA.GetDataList<double>(IN_Bay_Widths, bayWidthsList);
            double[] bayWidths = StadiumTools.Data.MatchLength<double>(bayWidthsList, sideCount, "Bay Width(s)");
            DA.GetData<int>(IN_Corner_Bay_Count, ref cornerBayCount);
            DA.GetDataList<bool>(IN_POC, pocList);
            bool[] poc = StadiumTools.Data.MatchLength<bool>(pocList, sideCount, "P.O.C");

            return new StadiumTools.Boundary
                (ps,
                offsetX,
                offsetY,
                cornerRadaii,
                bayWidths,
                cornerBayCount,
                poc);
            
        }


    }
}
