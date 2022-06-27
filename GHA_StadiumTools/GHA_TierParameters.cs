using System;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called Tier Parameters using the GH_Component as base. 
    /// </summary>
    public class TierParameters : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new tier. 
        /// </summary>
        public TierParameters()
            : base(nameof(TierParameters), "Tp", "Input Parameters to generate a new seating tier", "StadiumTools" "BowlTools")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            TierParameters.TierParams tierParams = new TierParameters.TierParams();
            tierParams.setDefaultParams();
            pManager.AddNumberParameter("Start H", "sH", "Horizontal start distance from P.O.F", GH_ParamAccess.item, tierParams.startX);
            pManager.AddNumberParameter("Start V", "sV", "Vertical start distance from P.O.F", GH_ParamAccess.item, tierParams.startZ);
            pManager.AddNumberParameter("Row Width", "rW", "Row width", GH_ParamAccess.item, tierParams.rowWidth);
            pManager.AddNumberParameter("C-Value", "C", "Max allowable spectator C-value", GH_ParamAccess.item, tierParams.cValue);
            pManager.AddIntegerParameter("Row Count", "rC", "Number of rows", GH_ParamAccess.item, tierParams.rows);
            pManager.AddBooleanParameter("Has Vom", "V?", "True if tier has a vomitory", GH_ParamAccess.item, tierParams.vomitory);
            pManager.AddIntegerParameter("Vom Start", "vS", "Start row for vomitory", GH_ParamAccess.item, tierParams.vomitoryStart);
            pManager.AddNumberParameter("Vomitory width", "VW", "Width of vomitory", GH_ParamAccess.item, tierParams.aisleWidth);
            pManager.AddNumberParameter("Seat Width", "sW", "Seat width", GH_ParamAccess.item, tierParams.seatWidth);
            pManager.AddNumberParameter("Seated Eye Verical", "eV", "Vertical distance of spectator eyes from floor", GH_ParamAccess.item, tierParams.eyeLevel);
            pManager.AddNumberParameter("Seated Eye Horizontal", "eH", "Horizontal distance of spectator eyes from riser", GH_ParamAccess.item, tierParams.eyeHoriz);
            pManager.AddNumberParameter("Fascia Height", "fH", "Height of fascia below first row", GH_ParamAccess.item, tierParams.boardHeight);
            pManager.AddBooleanParameter("Has Super", "S?", "True if tier has a super riser", GH_ParamAccess.item, tierParams.superR);
            pManager.AddIntegerParameter("Super Start", "sS", "Start row for super riser", GH_ParamAccess.item, tierParams.superStart);
            pManager.AddNumberParameter("Super Chamfer", "sC", "Optional chamfer on super riser", GH_ParamAccess.item, tierParams.superNib);
            pManager.AddNumberParameter("Super Eye Vertical", "seH", "Eye level horizontal on super riser", GH_ParamAccess.item, tierParams.superEyeVert);
            pManager.AddNumberParameter("Super Eye Horizontal", "seV", "Eye level vertical on super riser", GH_ParamAccess.item, tierParams.superEyeHoriz);
            pManager.AddNumberParameter("Standing Eye Level", "SeV", "Standing eye level vertical", GH_ParamAccess.item, tierParams.standingVert);
            pManager.AddNumberParameter("Standing Eye Horizontal", "SeH", "Standing eye level horizontal", GH_ParamAccess.item, tierParams.standingHoriz);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tier Parameters", "TP", "Parameters per tier", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            TierParameters.TierParams thisTierParams = new TierParameters.TierParams();
            thisTierParams.setParamsFromDefault();
            thisTierParams.setParamsFromDA(DA);
            DA.SetData(0, (object)thisTierParams);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("072677e1-616c-4028-ab7e-b9717e7699b1");
    }
}