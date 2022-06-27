using System.Collections.Generic;

namespace StadiumTools
{
    /// <summary>
    /// This object contains the properties of a Spectator. 
    /// </summary>
    internal class Spectator2D
    {
        /// <summary>
        /// a tuple represneting the 2d (h(x), v(y)) coordinates of the spectator relative to the P.O.F (Point of focus)
        /// </summary>
        public (double H, double V) loc2D { get; set; } = (0.0, 0.0);
        /// <summary>
        /// a tuple representing the 2d (h(x), v(y)) components and Length of a spectators sightline to the P.O.F
        /// </summary>
        public (double H, double V, double Length) sightLine { get; set; }
        /// <summary>
        /// True if the spectator has an unobstructed sightline to the P.O.F
        /// </summary>
        public bool hasSightLine { get; set; }
        /// <summary>
        /// The C-Value of the spectator
        /// </summary>
        public double cVal { get; set; }
    }

    /// <summary>
    /// This object stores the properties of a single seating tier.
    /// </summary>
    internal class Tier2D
    {
        /// <summary>
        /// Model unit space of the tier (mm, m, in, ft) 
        /// </summary>
        static double unitC = 1000.00;
        /// <summary>
        /// 2d coordinate representing the point-of-focus of the seating tier
        /// </summary>
        public (double h, double v) pof { get; set; } = (0.0, 0.0);
        /// <summary>
        /// Horizontal offset of Tier Start from Point of Focus (P.O.F)
        /// </summary>
        public double startH { get; set; } = unitC * 5.0;
        /// <summary>
        /// Vertical offset of tier start from Point of Focus (P.O.F)
        /// </summary>
        public double startV { get; set; } = unitC * 2.0;
        /// <summary>
        /// Maximum allowable c-value for spectators in this tier.
        /// </summary>
        public double cValue { get; set; } = unitC * 0.08;
        /// <summary>
        /// Horizontal offset of seated spectator eyes from riser nose 
        /// </summary>
        public double eyeH { get; set; }
        /// <summary>
        /// Vertical offset of seated spectator eyes from floor
        /// </summary>
        public double eyeV { get; set; }
        /// <summary>
        /// Horizontal offset of standing spectator eyes from riser nose 
        /// </summary>
        public double sEyeH { get; set; }
        /// <summary>
        /// Vertical offset of seated spectator eyes from floor
        /// </summary>
        public double sEyeV { get; set; }
        /// <summary>
        /// Number of rows in this tier not including super risers
        /// </summary>
        public int rowCount { get; set; } = 20;
        /// <summary>
        /// True if tier contains a vomitory
        /// </summary>
        public bool vomHas { get; set; } = true;
        /// <summary>
        /// Row number of vomitory start
        /// </summary>
        public int vomStart { get; set; } = 10;
        /// <summary>
        /// Width of vomitory
        /// </summary>
        public double vomWidth { get; set; } = 1.2;
        /// <summary>
        /// Height of vomitory in rows 
        /// </summary>
        public int vomHeight { get; set; } = 5;
        /// <summary>
        /// Vertical height of fascia below the first row.
        /// </summary>
        public double fasciaH { get; set; } = 0.5;
        /// <summary>
        /// True if tier has a super riser
        /// </summary>
        public bool hasSuper { get; set; } = false;
        /// <summary>
        /// Start row for inserting super riser
        /// </summary>
        public int superStart { get; set; } = 10;
        /// <summary>
        /// Optional chamfer distance for nose of super riser
        /// </summary>
        public double superChamfer { get; set; } = 0.05;
        /// <summary>
        /// Horizontal offset of spectator eyes from nose of super riser 
        /// </summary>
        public double superEyeH { get; set; } = 0.8;
        /// <summary>
        /// Vertical offset of spectator eyes from floor of super riser
        /// </summary>
        public double superEyeV { get; set; } = 1.5;
        



    }
}
