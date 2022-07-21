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
    public class ST_DeconstructSpectator : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D section. 
        /// </summary>
        public ST_DeconstructSpectator()
            : base(nameof(ST_DeconstructSpectator), "dSp", "Deconstruct a Spectator into it's respective data and geometry", "StadiumTools", "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Spectator", "Sp", "A Spectator object to deconstruct", GH_ParamAccess.item);
            
        }

        private static int IN_Spectator = 0;
        private static int OUT_Eye_Point = 0;
        private static int OUT_SightLine = 1;
        private static int OUT_Standing_Eye_Point = 2;
        private static int OUT_Standing_SightLine = 3;
        private static int OUT_C_Value = 4;
        private static int OUT_Target_C_Value = 5;
        private static int OUT_Section_Index = 6;
        private static int OUT_Tier_Index = 7;
        private static int OUT_Row_Index = 8;
        private static int OUT_Blocker = 9;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //pManager.AddPointParameter("Reference Point", "rP", "The origin of the spectator (point from which EyeX, EyeY params are applied) ", GH_ParamAccess.item);
            pManager.AddPointParameter("Eye Point", "EP", "The location of spectator's eyes when seated", GH_ParamAccess.item);
            pManager.AddLineParameter("SightLine", "SL", "The Sightline of the spectator when seated", GH_ParamAccess.item);
            pManager.AddPointParameter("Standing Eye Point", "SEP", "The location of spectator's eyes when standing", GH_ParamAccess.item);
            pManager.AddLineParameter("Standing SightLine", "SSL", "The Sightline of the spectator when standing", GH_ParamAccess.item);
            pManager.AddNumberParameter("C-Value", "C", "The actual C-Value of the Spectator within their tier", GH_ParamAccess.item);
            pManager.AddNumberParameter("Target C-Value", "TC", "The goal C-Value of the Spectator within their tier based on set parameters", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Section Index", "Si", "The numeric index of the section this spectator belongs to", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Tier Index", "Ti", "The numeric index of the tier this spectator belongs to", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Row Index", "Ri", "The numeric index of the row this spectator belongs to", GH_ParamAccess.item);
            pManager.AddPointParameter("Blocker", "B", "The Spectator eyes seated in front if applicable (the sightline blocker) ", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ST_DeconstructSpectator.DeconstructSpectatorFromDA(DA);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_DeConstructSpectator_;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("b2d52636-fcd3-4759-9cb8-3b5ca49bc1ee");

        //Methods
        public static void DeconstructSpectatorFromDA(IGH_DataAccess DA)
        {
            //Item Containers
            StadiumTools.Spectator specItem = new StadiumTools.Spectator();

            //Get Input Spectator
            if (!DA.GetData<StadiumTools.Spectator>(IN_Spectator, ref specItem))
                return;

            //Section plane of spectator
            StadiumTools.Pln3d specPln3d = new StadiumTools.Pln3d();
            Rhino.Geometry.Plane specPlane = new Rhino.Geometry.Plane();
            specPln3d = specItem.Plane;
            specPlane = StadiumTools.IO.PlaneFromPln3d(specPln3d);

            //Set Seated Eye Point
            Rhino.Geometry.Point3d eyePt = StadiumTools.IO.Point3dFromPt3d(specItem.Loc2d.ToPt3d(specPln3d));
            DA.SetData(OUT_Eye_Point, eyePt);

            //Set Seated SightLine
            Rhino.Geometry.Line sightLine = new Rhino.Geometry.Line(eyePt, specPlane.Origin);
            DA.SetData(1, sightLine);

            //Set Standing Eye Point
            Rhino.Geometry.Point3d eyePtStanding = StadiumTools.IO.Point3dFromPt3d(specItem.Loc2dStanding.ToPt3d(specPln3d));
            DA.SetData(2, eyePtStanding);

            //Set Standing SightLine
            Rhino.Geometry.Line sightLineStanding = new Rhino.Geometry.Line(eyePtStanding, specPlane.Origin);
            DA.SetData(3, sightLineStanding);

            //Set C-Value
            DA.SetData(4, specItem.Cvalue);

            //Set Target C-Value
            DA.SetData(5, specItem.TargetCValue);

            //Set Section Index
            DA.SetData(6, specItem.SectionIndex);

            //SetTier Index
            DA.SetData(7, specItem.TierIndex);

            //Set Row Index
            DA.SetData(8, specItem.RowIndex);

            //Set Blocker
            DA.SetData(9, StadiumTools.IO.Point3dFromPt3d(specItem.ForwardSpectatorLoc2d.ToPt3d(specPln3d)));

        }


    }
}
