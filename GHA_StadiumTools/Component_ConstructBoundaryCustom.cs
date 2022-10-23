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
    public class ST_ConstructBoundaryCustom : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D section. 
        /// </summary>
        public ST_ConstructBoundaryCustom()
            : base(nameof(ST_ConstructBoundaryCustom), "cbC", "Construct a Custom Boundary (Front Bowl Edge) from Polylines", "StadiumTools", "2D Plan")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            double unit = StadiumTools.UnitHandler.FromString("Rhino", Rhino.RhinoDoc.ActiveDoc.GetUnitSystemName(true, false, true, true));
            double bw = 7.5 * unit;
            double r = 350 * unit;
            pManager.AddGenericParameter("PlaySurface", "PS", $"The Playsurface to inherit boundary dimensions from", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset X", "oX", "The X-offset of the bowl edges from the playSurface", GH_ParamAccess.item, 5 * unit);
            pManager.AddNumberParameter("Offset Y", "oY", "The Y-offset of the bowl edges from the playSurface", GH_ParamAccess.item, 5 * unit);
            pManager.AddNumberParameter("Radaii", "R", "The radius of the bowl boundary edges", GH_ParamAccess.list, new List<double>() { r, r, r, r });
            pManager.AddNumberParameter("Corner Radaii", "cR", "The radaii to fillet all bowl corners. 0 for sharp corners", GH_ParamAccess.list, 12 * unit);
            pManager.AddNumberParameter("Bay Widths", "bw", "The width of bays (per side)", GH_ParamAccess.list, new List<double>() { bw, bw, bw, bw });
            pManager.AddIntegerParameter("Corner Bay Count", "cbC", "Number of bays within the rounded corners", GH_ParamAccess.item, 5);
            pManager.AddBooleanParameter("P.O.C", "POC", "True if a point should be centered (per side)", GH_ParamAccess.list, new List<bool>() { true, false, true, false });
        }

        //Set parameter indixes to names (for readability)
        private static int IN_PlaySurface = 0;
        private static int IN_Offset_X = 1;
        private static int IN_Offset_Y = 2;
        private static int IN_Radaii = 3;
        private static int IN_Corner_Radaii = 4;
        private static int IN_Bay_Widths = 5;
        private static int IN_Corner_Bay_Count = 6;
        private static int IN_POC = 7;
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
            StadiumTools.Boundary newBoundary = ST_ConstructBoundaryCustom.ConstructBoundaryCustomFromDA(DA, this);
            List<Rhino.Geometry.PolylineCurve> curves = StadiumTools.IO.PolylineCurveListFromPlines(newBoundary.Edges);

            Rhino.Geometry.Plane closestPlane = StadiumTools.IO.PlaneFromPln3d(newBoundary.ClosestPlane);

            DA.SetDataList(OUT_Curves, curves);
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
        protected override System.Drawing.Bitmap Icon => Resources.ST_ConstructBoundaryCustom;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("4f45ae77-8764-45a6-9255-d38d44db28a1");

        //Methods  
        private static StadiumTools.Boundary ConstructBoundaryCustomFromDA(IGH_DataAccess DA, GH_Component thisComponent)
        {
            double tolerance = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            int sideCount = 4;

            //Item Containers  
            var playSurfaceGooItem = new StadiumTools.PlaySurfaceGoo();
            double offsetX = 0.0;
            double offsetY = 0.0;
            var sideRadaiiList = new List<double>();
            var cornerRadaiiList = new List<double>();
            var bayWidthsList = new List<double>();
            int cornerBayCount = 0;
            var pocList = new List<bool>();

            //Get PlaySurface
            DA.GetData<StadiumTools.PlaySurfaceGoo>(IN_PlaySurface, ref playSurfaceGooItem);
            var ps = playSurfaceGooItem.Value;

            DA.GetData<double>(IN_Offset_X, ref offsetX);
            DA.GetData<double>(IN_Offset_Y, ref offsetY);
            DA.GetDataList<double>(IN_Radaii, sideRadaiiList);
            double[] sideRadaii = StadiumTools.Data.MatchLength<double>(sideRadaiiList, sideCount, "Side Radaii");
            DA.GetDataList<double>(IN_Corner_Radaii, cornerRadaiiList);
            double[] cornerRadaii = StadiumTools.Data.MatchLength<double>(cornerRadaiiList, sideCount, "Corner Radaii");
            DA.GetDataList<double>(IN_Bay_Widths, bayWidthsList);
            double[] bayWidths = StadiumTools.Data.MatchLength<double>(bayWidthsList, sideCount, "Bay Width(s)");
            DA.GetData<int>(IN_Corner_Bay_Count, ref cornerBayCount);
            DA.GetDataList<bool>(IN_POC, pocList);
            bool[] poc = StadiumTools.Data.MatchLength<bool>(pocList, sideCount, "P.O.C");

            if (cornerRadaii.Sum() == 0.0)
            {
                return new StadiumTools.Boundary //RadialNonUniform
                    (ps,
                    offsetX,
                    offsetY,
                    sideRadaii,
                    bayWidths,
                    poc);
            }
            else
            {
                return new StadiumTools.Boundary //RadialNonUniformFilleted
                    (ps,
                    offsetX,
                    offsetY,
                    sideRadaii,
                    cornerRadaii,
                    bayWidths,
                    cornerBayCount,
                    poc,
                    tolerance);
            }
        }


    }
}
