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
    public class ST_DeconstructTier : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D section. 
        /// </summary>
        public ST_DeconstructTier()
            : base(nameof(ST_DeconstructTier), "dT", "Deconstruct a Tier into it's respective data and geometry", "StadiumTools", "2D Section")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tier2D", "T", "A Tier object to deconstruct", GH_ParamAccess.item);

        }

        //Set parameter indixes to names (for readability)
        private static int IN_Tier = 0;
        private static int OUT_Spectators = 0;
        private static int OUT_Points = 1;
        private static int OUT_Profile = 2;
        private static int OUT_Plane = 3;
        private static int OUT_Section_Index = 4;
        private static int OUT_AislePoints = 5;
        private static int OUT_AisleProfile = 6;
        private static int OUT_Debug = 7;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Spectators", "Sp", "The spectators objects seated in this tier", GH_ParamAccess.list);
            pManager.AddPointParameter("Points", "pts", "The 3d points representing the top surface of the tier", GH_ParamAccess.list);
            pManager.AddCurveParameter("Profile", "Pr", "a Polyline representing the top surface of the tier", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "Pl", "The reference plane of this tier (0,0 is the Point of Focus)", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Section Index", "Si", "The index of this tier within its host section", GH_ParamAccess.item);
            pManager.AddPointParameter("AislePoints", "Apts", "The 3d points representing the top surface of the tier's aisle/vomatory", GH_ParamAccess.list);
            pManager.AddCurveParameter("AisleProfile", "APr", "a Polyline representing the top surface of the tier's aisle/vomatory", GH_ParamAccess.item);
            pManager.AddTextParameter("Pt2d", "2d", "String representation of Pt2d objects of the tier (for debugging)", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_DeconstructTier.HandleErrors(DA, this);
            ST_DeconstructTier.DeconstructTierFromDA(DA);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_DeConstructTier;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("c5060562-1cc0-4a35-9454-570182885cd4");

        //Methods
        public static void DeconstructTierFromDA(IGH_DataAccess DA)
        {
            //Item Containers
            var tierGooItem = new StadiumTools.TierGoo();

            //Get Input Tier Object
            if (!DA.GetData<StadiumTools.TierGoo>(IN_Tier, ref tierGooItem)) { return; }

            //Uwrap TierGoo
            StadiumTools.Tier tierItem = tierGooItem.Value;

            var stringList = new List<string>();
            for (int i = 0; i < tierItem.Points2dCount; i++)
            {
                string strx = tierItem.Points2d[i].X.ToString();
                string stry = tierItem.Points2d[i].Y.ToString();
                string str = $"[{strx} , {stry}]";
                stringList.Add(str);
            }

            if (tierItem.Spectators.Length > 0)
            {
                //Wrap Speectators in Goo
                var spectatorGooList = new List<StadiumTools.SpectatorGoo>();
                for (int i = 0; i < tierItem.Spectators.Length; i++)
                {
                    spectatorGooList.Add(new StadiumTools.SpectatorGoo(tierItem.Spectators[i]));
                }

                //Deconstruct Section object and ouput data
                //Set Spectators
                DA.SetDataList(OUT_Spectators, spectatorGooList);
            }

            Rhino.Geometry.Point3d[] points = StadiumTools.IO.PointsFromTier(tierItem);
            DA.SetDataList(OUT_Points, points);
            Rhino.Geometry.Polyline pline = StadiumTools.IO.PolylineFromTier(tierItem);
            DA.SetData(OUT_Profile, pline);

            Rhino.Geometry.Point3d[] aislePoints = StadiumTools.IO.AislePointsFromTier(tierItem);
            DA.SetDataList(OUT_AislePoints, aislePoints);
            Rhino.Geometry.Polyline aislePline = StadiumTools.IO.AislePolylineFromTier(tierItem);
            DA.SetData(OUT_AisleProfile, aislePline);

            Rhino.Geometry.Plane tierPlane = StadiumTools.IO.PlaneFromPln3d(tierItem.Plane);
            DA.SetData(OUT_Plane, tierPlane);
            DA.SetData(OUT_Section_Index, tierItem.SectionIndex);
            DA.SetDataList(OUT_Debug, stringList);
        }

        /// <summary>
        /// handles some possible invalid input values
        /// </summary>
        /// <param name="DA"></param>
        /// <param name="thisComponent"></param>
        private static void HandleErrors(IGH_DataAccess DA, GH_Component thisComponent)
        {
            StadiumTools.TierGoo tierGooItem = new StadiumTools.TierGoo();
            if (DA.GetData<StadiumTools.TierGoo>(IN_Tier, ref tierGooItem))
            {
                if (tierGooItem.Value.inSection == false)
                {
                    thisComponent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Tier must be passed through a ConstructSection2D component before being deconstructed");
                }
            }
        }
    }
}
