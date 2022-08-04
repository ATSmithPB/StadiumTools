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
    public class ST_ConstructFascia : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new spectator. 
        /// </summary>
        public ST_ConstructFascia()
            : base(nameof(ST_ConstructFascia), "cV", "Construct a Tier Fascia from parameters", "StadiumTools", "2D Section")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Polyline", "PL","A polyline that represents the 2D fascia you would like to add", GH_ParamAccess.item);
            //pManager.AddPlaneParameter("Plane", "Pl", "The plane of the polyline. Plane origin will be attachment point to tier", GH_ParamAccess.item, defaultVomatory.Height);
            pManager[0].Optional = true;
        }

        //Set parameter indixes to names (for readability)
        private static int IN_Polyline = 0;
        private static int IN_Plane = 1;
        private static int OUT_Fascia = 0;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Fascia", "F", "A Fascia object", GH_ParamAccess.item);
            
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_ConstructFascia.ConstructFasciaFromDA(DA);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_ConstructFascia;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("580a5fe9-5ef7-46e6-acfb-aa15b9f4efbc");

        //Methods
        private static void ConstructFasciaFromDA(IGH_DataAccess DA)
        {
            //Initialize Default Polyline
            double unit = StadiumTools.UnitHandler.FromString("Rhino", Rhino.RhinoDoc.ActiveDoc.GetUnitSystemName(true, false, true, true));
            StadiumTools.Fascia defaultFascia = StadiumTools.Fascia.InitDefault(unit);
            Rhino.Geometry.Point3d[] defaultPts = StadiumTools.IO.Point3dFromPt2d(defaultFascia.Points2d);
            Rhino.Geometry.Polyline defaultPolyline = new Rhino.Geometry.Polyline(defaultPts);

            //Item Containers (Destinations)
            Rhino.Geometry.Polyline polyItem = new Rhino.Geometry.Polyline();
            Rhino.Geometry.PolyCurve polyCrvItem = new Rhino.Geometry.PolyCurve();

            //Set Polyline
            if (DA.GetData<Rhino.Geometry.Polyline>(IN_Polyline, ref polyItem))
            {
                StadiumTools.Pt2d[] pts = new StadiumTools.Pt2d[polyItem.Count];
                pts = StadiumTools.IO.Pt2dFromPolyline(polyItem);
                StadiumTools.Fascia newFascia = new StadiumTools.Fascia(pts, unit);
                DA.SetData(OUT_Fascia, newFascia);
            }
            else
            {
                DA.SetData(OUT_Fascia, defaultFascia);
            }
        }

    }
}
