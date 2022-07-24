using System.Collections.Generic;
using System;

namespace StadiumTools
{
    /// <summary>
    /// Represents the data of a single seating tier.
    /// </summary>
    public class Tier : ICloneable
    {
        //Properties
        public bool IsValid { get; set; } = true;
        /// <summary>
        /// The 3d plane of this tier, where (0,0,0) is also the Point of Focus.
        /// </summary>
        public Pln3d Plane { get; set; }
        /// <summary>
        /// A spectator object that contains the spectator parameters for generating the tier.
        /// </summary>
        public Spectator SpectatorParameters { get; set; }
        /// <summary>
        /// A SuperRiser object that defines the optional super riser parameters for the tier (Super EyeX, Super EyeY, Guardrail Width etc...)
        /// </summary>
        public SuperRiser SuperRiser { get; set; }
        /// <summary>
        /// True if tier has a super riser
        /// </summary>
        public bool SuperHas { get; set; }
        /// <summary>
        /// A Vomatory object that defines the optional vomitory parameters for the tier (Vomatory Start, Vomatory Width, etc...)
        /// </summary>
        public Vomatory Vomatory { get; set; }
        /// <summary>
        /// True if tier contains a vomitory
        /// </summary>
        public bool VomHas { get; set; }
        /// <summary>
        /// True if tier reference point is the last point of the previous tier.
        /// </summary>
        public bool BuildFromPreviousTier { get; set; }
        /// <summary>
        /// Reference Point of tier relative to StartX and StartY
        /// </summary>
        public Pt2d StartPt { get; set; }
        /// <summary>
        /// Index of the tier within a section
        /// </summary>
        public int SectionIndex { get; set; }
        /// <summary>
        /// Index of the tier within a plan
        /// </summary>
        public int PlanIndex { get; set; }
        /// <summary>
        /// Horizontal offset of tier start from the start point
        /// </summary>
        public double StartX { get; set; }
        /// <summary>
        /// Vertical offset of tier start from the start point
        /// </summary>
        public double StartY { get; set; }
        /// <summary>
        /// Number of rows in this tier (a super riser is treated as a row)
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// A default row width to apply to all rows
        /// </summary>
        public double DefaultRowWidth { get; set; }
        /// <summary>
        /// Width(s) of all rows (distance from riser to riser)
        /// </summary>
        public double[] RowWidths { get; set; }
        /// <summary>
        /// Height(s) of row risers
        /// </summary>
        public double[] RiserHeights { get; set; }
        /// <summary>
        /// Increment size to round riser heights to
        /// </summary>
        public double RoundTo { get; set; }
        /// <summary>
        /// The maximum allowable rake angle in radians. Tan(Riser / Row) 
        /// </summary>
        public double MaxRakeAngle { get; set; }
        /// <summary>
        /// An ordered list of Spectators in the tier
        /// </summary>
        public Spectator[] Spectators { get; set; }
        /// <summary>
        /// Number of Pt2d objects this tier profile contains
        /// </summary>
        public int Points2dCount { get; set; }
        /// <summary>
        /// Pt2d objects that represents the top outline geometry of the tier profile
        /// </summary>
        public Pt2d[] Points2d { get; set; }

        //Constructors
        /// <summary>
        /// Initializes a new default Tier
        /// </summary>
        public Tier()
        {
        }

        //Methods
        /// <summary>
        /// Initializes a tier 2d instance with default values
        /// </summary>
        public static void InitDefault(Tier tier, double unit)
        {
            //Instance a default spectator
            Spectator defaultSpectatorParameters = new Spectator();
            Spectator.InitDefault(defaultSpectatorParameters, unit);
            tier.SpectatorParameters = defaultSpectatorParameters;

            //Instance a default SuperRiser
            SuperRiser defaultSuperRiserParameters = new SuperRiser();
            SuperRiser.InitDefault(defaultSuperRiserParameters);

            tier.Plane = Pln3d.XYPlane;
            tier.BuildFromPreviousTier = true;
            tier.StartX = 5.0 * tier.SpectatorParameters.Unit;
            tier.StartY = 1.0 * tier.SpectatorParameters.Unit;
            tier.RowCount = 25;
            tier.DefaultRowWidth = 0.8 * tier.SpectatorParameters.Unit;

            // Init all row widths to default value
            double[] rowWidths = new double[tier.RowCount];
            for (int i = 0; i < rowWidths.Length; i++)
            {
                rowWidths[i] = tier.DefaultRowWidth;
            }

            if (tier.SuperRiser.Row > 0)
            {
                tier.SuperHas = true;
            }

            if (tier.SuperHas)
            {
                rowWidths[tier.SuperRiser.Row] = (tier.SuperRiser.Width * rowWidths[tier.SuperRiser.Row]);
            }

            tier.RowWidths = rowWidths;
            tier.RiserHeights = new double[tier.RowCount - 1];
            tier.RoundTo = 0.001 * tier.SpectatorParameters.Unit;
            tier.Points2dCount = GetTierPtCount(tier);
            tier.Points2d = new Pt2d[tier.Points2dCount];
            tier.MaxRakeAngle = .593412; //radians
            tier.Spectators = new Spectator[tier.RowCount];
        }

        /// <summary>
        /// Calculates the geometric point count for a Tier
        /// </summary>
        /// <param name="tier"></param>
        public static int GetTierPtCount(Tier tier)
        {
            int tierPtCount = (tier.RowCount * 2);

            if (tier.SuperHas)
            {
                if (tier.SuperRiser.Row < (tier.RowCount - 1) && tier.SuperRiser.GuardrailWidth > 0.0)
                {
                    tierPtCount += 2;
                }
                if (tier.SuperRiser.CurbWidth > 0.0)
                {
                    tierPtCount += 1;
                    if (tier.SuperRiser.CurbHeight > 0.0)
                    {
                        tierPtCount += 1;
                    }
                }
            }
            return tierPtCount;
        }

        public static Tier[] CloneArray(Tier[] tiers)
        {
            //Deep copy
            Tier[] tiersCloned = new Tier[tiers.Length];
            for (int i = 0; i < tiers.Length;)
            {
                tiersCloned[i] = (Tier)tiers[i].Clone();
            }
            return tiersCloned;
        }

        /// <summary>
        /// create a deep copy clone of a tier
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            //Deep Copy
            Tier clone = (Tier)this.MemberwiseClone();
            {
                clone.SpectatorParameters = (Spectator)this.SpectatorParameters.Clone();
                clone.RowWidths = (double[])this.RowWidths.Clone();
                clone.RiserHeights = (double[])this.RiserHeights.Clone();
                clone.Spectators = (Spectator[])this.Spectators.Clone();
                clone.Points2d = (Pt2d[])this.Points2d.Clone();
            }
            return clone;
        }
    
    }

}
