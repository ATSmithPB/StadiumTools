﻿using System.Collections.Generic;

namespace StadiumTools
{
    /// <summary>
    /// Represents the data of a single seating tier.
    /// </summary>
    public class Tier
    {
        //Properties
        /// <summary>
        /// A spectator object that defines the pectator parameters for generating the tier (Target C-Val, EyeX, EyeY, etc..)
        /// </summary>
        public Spectator SpectatorParameters { get; set; }
        /// <summary>
        /// True if tier reference point is the last point of the previous tier.
        /// </summary>
        public bool BuildFromPreviousTier { get; set; }
        /// <summary>
        /// Reference Point of tier relative to StartX and StartY
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
        /// Point of focus for all spectators in this tier 
        /// </summary>
        public Pt2d POF { get; set; }
        /// <summary>
        /// Horizontal offset of tier start from reference point
        /// </summary>
        public double StartX { get; set; }
        /// <summary>
        /// Vertical offset of tier start from reference point
        /// </summary>
        public double StartY { get; set; }
        /// <summary>
        /// Number of rows in this tier (super riser == row)
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// Default row width to apply to all rows
        /// </summary>
        public double DefaultRowWidth { get; set; }
        /// <summary>
        /// Width(s) of row (distance from riser to riser)
        /// </summary>
        public double[] RowWidths { get; set; }
        /// <summary>
        /// Height(s) of row risers
        /// </summary>
        public double[] RiserHeights { get; set; }
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
        /// Increment size to round riser heights to
        /// </summary>
        public double RoundTo { get; set; }
        /// <summary>
        /// The maximum allowable rake angle in radians. 
        /// </summary>
        public double MaxRakeAngle { get; set; }
        /// <summary>
        /// A SuperRiser object that defines the optional super riser parameters for the tier (Super EyeX, Super EyeY, Guardrail Width etc...)
        /// </summary>
        public SuperRiser SuperRiser { get; set; }
        /// <summary>
        /// True if tier has a super riser
        /// </summary>
        public bool SuperHas { get; set; }
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
            
        }

        //Methods
        /// <summary>
        /// Initializes a tier 2d instance with default values
        /// </summary>
        public void InitializeDefault()
        {
            //Instance a default spectator
            Spectator defaultSpectatorParameters = new Spectator();
            Spectator.InitializeDefault(defaultSpectatorParameters);
            this.SpectatorParameters = defaultSpectatorParameters;

            //Instance a default SuperRiser
            SuperRiser defaultSuperRiserParameters = new SuperRiser();
            defaultSuperRiserParameters.InitializeDefault();

            this.BuildFromPreviousTier = true;
            this.StartX = 5.0 * this.SpectatorParameters.Unit;
            this.StartY = 1.0 * this.SpectatorParameters.Unit;
            this.RowCount = 25;
            this.DefaultRowWidth = 0.8 * this.SpectatorParameters.Unit;

            // Initialize all row widths to default value
            double[] rowWidths = new double[this.RowCount];
            for (int i = 0; i < rowWidths.Length; i++)
            {
                rowWidths[i] = this.DefaultRowWidth;
            }

            if (this.SuperRiser.Row == 0)
            {
                this.SuperHas = true;
            }

            if (this.SuperHas)
            {
                rowWidths[this.SuperRiser.Row] = (this.SuperRiser.Width * rowWidths[this.SuperRiser.Row]);
            }

            this.RowWidths = rowWidths;
            this.RiserHeights = new double[this.RowCount - 1];
            this.VomHas = false;
            this.VomStart = 5;
            this.VomHeight = 5;
            this.SuperHas = true;
            this.RoundTo = 0.001 * this.SpectatorParameters.Unit;
            this.Points2dCount = GetTierPtCount(this);
            this.Points2d = new Pt2d[this.Points2dCount];
            this.MaxRakeAngle = .593412; //radians
            this.Spectators = new Spectator[this.RowCount];
        }

        /// <summary>
        /// Calculates the geometric point count for a Tier
        /// </summary>
        /// <param name="tier"></param>
        private static int GetTierPtCount(Tier tier)
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

    }

}
