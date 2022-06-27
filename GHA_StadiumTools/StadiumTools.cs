using System.Collections.Generic;

namespace StadiumTools
{
    /// <summary>
    /// Stores a collection of unit conversion coeffecients to handle various modeling unit spaces
    /// </summary>
    public struct UnitHandler
    {
        public const double mm = 1000.0;
        public const double cm = 100.0;
        public const double m = 1.0;
        public const double inch = 39.3701;
        public const double feet = 3.28084;
        public const double yard = 1.09361;
    }
    
    /// <summary>
    /// This object contains the properties of a Spectator. 
    /// </summary>
    internal class Spectator2D
    {
        //Properties
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

        //Constructors 
        /// <summary>
        /// Initializes a new Spectator2D
        /// </summary>
        public Spectator2D()
        {
            Initialize();
        }

        //Methods
        public void Initialize()
        {
            this.loc2D = (0.0, 0.0);
            this.sightLine = (0.0, 0.0, 0.0);
            this.hasSightLine = false;
            this.cVal = 0.0;
        }
    }

    /// <summary>
    /// This object represents a 2D single seating tier.
    /// </summary>
    internal class Tier2D
    {
        //Properties
        /// <summary>
        /// Model unit space of the tier (mm, m, in, ft) 
        /// </summary>
        public double unit;
        /// <summary>
        /// 2d coordinate representing the point-of-focus of the seating tier
        /// </summary>
        public (double h, double v) pof { get; set; }
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
        public int rowCount { get; set; }
        /// <summary>
        /// True if tier contains a vomitory
        /// </summary>
        public bool vomHas { get; set; }
        /// <summary>
        /// Row number of vomitory start
        /// </summary>
        public int vomStart { get; set; }
        /// <summary>
        /// Height of vomitory in rows 
        /// </summary>
        public int vomHeight { get; set; }
        /// <summary>
        /// Vertical height of fascia below the first row.
        /// </summary>
        public double fasciaH { get; set; }
        /// <summary>
        /// True if tier has a super riser
        /// </summary>
        public bool hasSuper { get; set; }
        /// <summary>
        /// Start row for inserting super riser
        /// </summary>
        public int superStart { get; set; }
        /// <summary>
        /// Optional chamfer distance for nose of super riser
        /// </summary>
        public double superChamfer { get; set; }
        /// <summary>
        /// Horizontal offset of spectator eyes from nose of super riser 
        /// </summary>
        public double superEyeH { get; set; }
        /// <summary>
        /// Vertical offset of spectator eyes from floor of super riser
        /// </summary>
        public double superEyeV { get; set; }
       
        //Constructors
        public Tier2D()
        {
            Initialize();
        }

        //Methods
        /// <summary>
        /// Initializes a tier 2d instance with default values
        /// </summary>
        public void Initialize()
        {
            this.unit = UnitHandler.m;
            this.pof = (0.0, 0.0);
            this.startH = 2.0 * unit;
            this.startV = 5 * unit;
            this.cValue = 0.10 * unit;
            this.eyeH = 0.8 * unit;
            this.eyeV = 1.2 * unit;
            this.sEyeH = 0.8 * unit;
            this.sEyeV = 2.5 * unit;
            this.rowCount = 20;
            this.vomHas = true;
            this.vomStart = 5;
            this.vomHeight = 5;
            this.fasciaH = 1.0 * unit;
            this.hasSuper = true;
            this.superStart = 15;
            this.superChamfer = 0.01 * unit;
            this.superEyeH = 0.8 * unit;
            this.superEyeV = 2.5 * unit;
        }
    }
}
