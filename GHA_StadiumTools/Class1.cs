using System;
using System.Collections.Generic;

namespace StadiumTools
{
    internal class TierParameters
    {
        //pManager.AddNumberParameter("Start H", "sH", "Horizontal start distance from P.O.F", GH_ParamAccess.item, tierParams.startX);
        //    pManager.AddNumberParameter("Start V", "sV", "Vertical start distance from P.O.F", GH_ParamAccess.item, tierParams.startZ);
        //    pManager.AddNumberParameter("Row Width", "rW", "Row width", GH_ParamAccess.item, tierParams.rowWidth);
        //    pManager.AddNumberParameter("C-Value", "C", "Max allowable spectator C-value", GH_ParamAccess.item, tierParams.cValue);
        //    pManager.AddIntegerParameter("Row Count", "rC", "Number of rows", GH_ParamAccess.item, tierParams.rows);
        //    pManager.AddBooleanParameter("Has Vom", "V?", "True if tier has a vomitory", GH_ParamAccess.item, tierParams.vomitory);
        //    pManager.AddIntegerParameter("Vom Start", "vS", "Start row for vomitory", GH_ParamAccess.item, tierParams.vomitoryStart);
        //    pManager.AddNumberParameter("Vomitory width", "VW", "Width of vomitory", GH_ParamAccess.item, tierParams.aisleWidth);
        //    pManager.AddNumberParameter("Seat Width", "sW", "Seat width", GH_ParamAccess.item, tierParams.seatWidth);
        //    pManager.AddNumberParameter("Seated Eye Verical", "eV", "Vertical distance of spectator eyes from floor", GH_ParamAccess.item, tierParams.eyeLevel);
        //    pManager.AddNumberParameter("Seated Eye Horizontal", "eH", "Horizontal distance of spectator eyes from riser", GH_ParamAccess.item, tierParams.eyeHoriz);
        //    pManager.AddNumberParameter("Fascia Height", "fH", "Height of fascia below first row", GH_ParamAccess.item, tierParams.boardHeight);
        //    pManager.AddBooleanParameter("Has Super", "S?", "True if tier has a super riser", GH_ParamAccess.item, tierParams.superR);
        //    pManager.AddIntegerParameter("Super Start", "sS", "Start row for super riser", GH_ParamAccess.item, tierParams.superStart);
        //    pManager.AddNumberParameter("Super Chamfer", "sC", "Optional chamfer on super riser", GH_ParamAccess.item, tierParams.superNib);
        //    pManager.AddNumberParameter("Super Eye Vertical", "seH", "Eye level horizontal on super riser", GH_ParamAccess.item, tierParams.superEyeVert);
        //    pManager.AddNumberParameter("Super Eye Horizontal", "seV", "Eye level vertical on super riser", GH_ParamAccess.item, tierParams.superEyeHoriz);
        //    pManager.AddNumberParameter("Standing Eye Level", "SeV", "Standing eye level vertical", GH_ParamAccess.item, tierParams.standingVert);
        //    pManager.AddNumberParameter("Standing Eye Horizontal", "SeH", "Standing eye level horizontal", GH_ParamAccess.item, tierParams.standingHoriz);
        /// <summary>
        /// Level of Development based on available properties
        /// </summary>
        public int lod { get; }
        /// <summary>
        /// Horizontal offset of Tier Start from Point of Focus (P.O.F)
        /// </summary>
        public double startH { get; set; }
        /// <summary>
        /// Vertical offset of tier start from Point of Focus (P.O.F)
        /// </summary>
        public double startV { get; set; }
        /// <summary>
        /// Maximum allowable c-value for spectators in this tier.
        /// </summary>
        public double cValue { get; set; }
        /// <summary>
        /// Width of row (horizontal distance from riser to riser)
        /// </summary>
        public double rowWidth { get; set; }
        /// <summary>
        /// Number of rows in this tier not including super risers
        /// </summary>
        public double rowCount { get; set; }
        public bool hasVom { get; set; }
        public int vomStart { get; set; }
        public double vomWidth { get; set; }
        public double seatWidth { get; set; }
        public double eyeH { get; set; }
        public double eyeV { get; set; }
        public double sEyeH { get; set; }
        public double sEyeV { get; set; }
        public double fasciaH { get; set; }
        public bool hasSuper { get; set; }
        public int superStart { get; set; }
        public double superChamfer { get; set; }
        public double superEyeH { get; set; }
        public double superEyeV { get; set; }
        



    }
}
