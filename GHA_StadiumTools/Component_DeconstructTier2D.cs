﻿using System;
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
            : base(nameof(ST_DeconstructTier), "dT", "Deconstruct a Tier into it's respective data and geometry", "StadiumTools", "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tier", "T", "A Tier object to deconstruct", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Spectators", "Sp", "The spectators objects seated in this tier", GH_ParamAccess.list);
            pManager.AddPointParameter("Points", "pts", "The points representing the top surface of the tier", GH_ParamAccess.list);
            pManager.AddCurveParameter("Profile", "P", "a Polyline representing the top surface of the tier", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "The reference plane of this tier (0,0 is the Point of Focus)", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Section Index", "Si", "The index of this tier within its host section", GH_ParamAccess.item);
            pManager.AddTextParameter("Debug", "d", "Text to help debug", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
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
            StadiumTools.Tier tierItem = new StadiumTools.Tier();
            

            //Get Input Tier Object
            if (!DA.GetData<StadiumTools.Tier>(0, ref tierItem))
                return;

            List<string> stringList = new List<string>();
            for (int i = 0; i < tierItem.Points2dCount; i++)
            {
                string strx = tierItem.Points2d[i].X.ToString();
                string stry = tierItem.Points2d[i].Y.ToString();
                string str = $"[{strx} , {stry}]";
                stringList.Add(str);
            }

            //Deconstruct Section object and ouput data
            //Set Spectators
            DA.SetDataList(0, tierItem.Spectators);

            //Set Points
            Rhino.Geometry.Point3d[] points = StadiumTools.IO.PointsFromTier(tierItem);
            DA.SetDataList(1, points);

            //Set Profile Polyline
            Rhino.Geometry.Polyline pline = StadiumTools.IO.PolylineFromTier(tierItem);
            DA.SetData(2, pline);

            //Set Plane (POF)
            Rhino.Geometry.Plane tierPlane = StadiumTools.IO.PlaneFromPln3d(tierItem.Plane);
            DA.SetData(3, tierPlane);

            //Set Section Index
            DA.SetData(4, tierItem.SectionIndex);

            DA.SetDataList(5, stringList);
        }


    }
}