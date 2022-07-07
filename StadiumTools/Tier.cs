using System.Collections.Generic;

namespace StadiumTools
{
    /// <summary>
    /// Represents the data of a single seating tier.
    /// </summary>
    public class Tier
    {
        //Enums
        /// <summary>
        /// Available methods to generate a reference point for a tier
        /// </summary>
        public enum ReferencePtType
        {
            ByPOF,
            ByEndOfPrevTier
        }

        //Properties
        /// <summary>
        /// The method for determining the reference point of the tier
        /// </summary>
        public ReferencePtType RefPtType { get; set; }
        /// <summary>
        /// Reference Point of tier relative to StartH and StartV
        /// </summary>
        public Pt2d RefPt { get; set; }
        /// <summary>
        /// Index of the tier within a section
        /// </summary>
        public int SectionIndex { get; set; }
        /// <summary>
        /// Index of the tier within the plan
        /// </summary>
        public int PlanIndex { get; set; }
        /// <summary>
        /// Coeffecient for model unit space of the tier (mm, m, in, ft) 
        /// </summary>
        public double Unit;
        /// <summary>
        /// Point of focus for all spectators in this tier 
        /// </summary>
        public Pt2d POF { get; set; }
        /// <summary>
        /// Horizontal offset of tier start from reference point
        /// </summary>
        public double StartH { get; set; }
        /// <summary>
        /// Vertical offset of tier start from reference point
        /// </summary>
        public double StartV { get; set; }
        /// <summary>
        /// Minimum allowable c-value for spectators in this tier.
        /// </summary>
        public double Cvalue { get; set; }
        /// <summary>
        /// Horizontal offset of seated spectator eyes from row start 
        /// </summary>
        public double EyeH { get; set; }
        /// <summary>
        /// Vertical offset of seated spectator eyes from row floor
        /// </summary>
        public double EyeV { get; set; }
        /// <summary>
        /// Horizontal offset of standing spectator eyes from row start
        /// </summary>
        public double SEyeH { get; set; }
        /// <summary>
        /// Vertical offset of seated spectator eyes from row floor
        /// </summary>
        public double SEyeV { get; set; }
        /// <summary>
        /// Number of rows in this tier (super riser == row)
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// Width(s) of row (distance from riser to riser)
        /// </summary>
        public List<double> RowWidth { get; set; }
        /// <summary>
        /// True if tier contains a vomitory
        /// </summary>
        public bool VomHas { get; set; }
        /// <summary>
        /// Row number of vomitory start
        /// </summary>
        public int VomStart { get; set; }
        /// <summary>
        /// Height of vomitory (in rows)
        /// </summary>
        public int VomHeight { get; set; }
        /// <summary>
        /// Vertical height of fascia below the first row.
        /// </summary>
        public double FasciaH { get; set; }
        /// <summary>
        /// True if tier has a super riser
        /// </summary>
        public bool HasSuper { get; set; }
        /// <summary>
        /// Start row for inserting super riser
        /// </summary>
        public int SuperRow { get; set; }
        /// <summary>
        /// Optional curb distance before super riser 
        /// </summary>
        public double SuperCurb { get; set; }
        /// <summary>
        /// Horizontal offset of spectator eyes from nose of super riser 
        /// </summary>
        public double SuperEyeH { get; set; }
        /// <summary>
        /// Vertical offset of spectator eyes from floor of super riser
        /// </summary>
        public double SuperEyeV { get; set; }
        /// <summary>
        /// Optional value to round riser increments
        /// </summary>
        public double RoundTo { get; set; }
        /// <summary>
        /// The maximum allowable riser height. 
        /// </summary>
        public double MaxRakeAngle { get; set; }
        /// <summary>
        /// An ordered list of Spectators in the tier
        /// </summary>
        public Spectator[] Spectators { get; set; }
        /// <summary>
        /// Number of Pt2d objects this tier describes
        /// </summary>
        public int Points2dCount { get; set; }
        /// <summary>
        /// Pt2d objects that represents the top outline geometry of the tier
        /// </summary>
        public Pt2d[] Points2d { get; set; }
       
        //Constructors
        /// <summary>
        /// Initializes a new default Tier
        /// </summary>
        public Tier()
        {
            Initialize();
        }

        //Methods
        /// <summary>
        /// Initializes a tier 2d instance with default values
        /// </summary>
        public void Initialize()
        {
            this.Unit = UnitHandler.m;
            this.RefPtType = ReferencePtType.ByPOF;
            this.StartH = 5.0 * Unit;
            this.StartV = 1.0 * Unit;
            this.Cvalue = 0.10 * Unit;
            this.EyeH = 0.4 * Unit;
            this.EyeV = 0.9 * Unit;
            this.SEyeH = 0.6 * Unit;
            this.SEyeV = 1.7 * Unit;
            this.RowCount = 20;

            // Initialize all row widths to default value
            List<double> rowWidths = new List<double>();
            double defaultRW = 0.8;
            for (int i = 0; i < this.RowCount; i++)
            {
                rowWidths.Add(defaultRW);
            }

            this.RowWidth = rowWidths;
            this.VomHas = true;
            this.VomStart = 5;
            this.VomHeight = 5;
            this.FasciaH = 1.0 * Unit;
            this.HasSuper = true;
            this.SuperRow = 15;
            this.SuperCurb = 0.0 * Unit;
            this.SuperEyeH = 0.8 * Unit;
            this.SuperEyeV = 2.5 * Unit;
            this.RoundTo = 0.001 * Unit;
            this.Points2dCount = GetTierPtCount(this);
            this.Points2d = new Pt2d[this.Points2dCount];
            this.MaxRakeAngle = 34.0;
            this.Spectators = new Spectator[this.RowCount];
        }

        /// <summary>
        /// Calculates the geometric point count for a Tier
        /// </summary>
        /// <param name="tier"></param>
        private static int GetTierPtCount(Tier tier)
        {
            int tierPtCount = (tier.RowCount * 2);
            if (tier.FasciaH != 0.0)
            {
                tierPtCount += 1;
            }
            if (tier.SuperCurb != 0.0)
            {
                tierPtCount += 1;
            }

            return tierPtCount;   
        }

    }

}
