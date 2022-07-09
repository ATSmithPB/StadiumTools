using System.Collections.Generic;


namespace StadiumTools
{
    /// <summary>
    /// Represents the properties of a single Spectator. 
    /// </summary>
    public class Spectator
    {
        //Properties
        /// <summary>
        /// the index of the spectator's tier within their section
        /// </summary>
        public int TierNum { get; set; }
        /// <summary>
        /// the index of the spectators row within their tier
        /// </summary>
        public int RowNum { get; set; }
        /// <summary>
        /// Pt2d representing the location of the spectator eyes relative to the P.O.F
        /// </summary>
        public Pt2d Loc2d { get; set; }
        /// <summary>
        /// Pt2d representing the location of the STANDING spectator eyes relative to the P.O.F
        /// </summary>
        public Pt2d Loc2dStanding { get; set; }
        /// <summary>
        /// Pt2d representing the location of the P.O.F (Point of focus). Inherited from section.
        /// </summary>
        public Pt2d POF { get; set; }
        /// <summary>
        /// Vec2d representing the 2d X,Y components and Length of a seated spectator's sightline to the P.O.F
        /// </summary>
        public Vec2d SightLine { get; set; }
        /// <summary>
        /// Vec2d representing the 2d X,Y components and Length of a standing spectator's sightline to the P.O.F
        /// </summary>
        public Vec2d SightLineStanding { get; set; }
        /// <summary>
        /// True if the spectator has an unobstructed, seated sightline to the P.O.F
        /// </summary>
        public bool HasSightLine { get; set; } = false;
        /// <summary>
        /// True if the spectator has an unobstructed, STANDING sightline to the P.O.F
        /// </summary>
        public bool HasSightLineStanding { get; set; } = false;
        /// <summary>
        /// The desired C-Value of the spectator. Specified upon creation of Tier object.
        /// </summary>
        public double TargetCValue { get; set; } = 0.0;
        /// <summary>
        /// The actual C-Value of the spectator. Calculated upon creation of Section object.
        /// </summary>
        public double Cvalue { get; set; } = 0.0;

        //Constructors 
        /// <summary>
        /// Initializes a new Spectator2D
        /// </summary>
        public Spectator()
        {
            Initialize();
        }

        /// <summary>
        /// construct a Spectator from a collection of values
        /// </summary>
        /// <param name="tierN"></param>
        /// <param name="rowN"></param>
        /// <param name="pt"></param>
        /// <param name="ptSt"></param>
        /// <param name="pof"></param>
        /// <param name="sLine"></param>
        /// <param name="sLineSt"></param>
        public Spectator(int tierN, int rowN, Pt2d pt, Pt2d ptSt, Pt2d pof, Vec2d sLine, Vec2d sLineSt)
        {
            this.TierNum = tierN;
            this.RowNum = rowN;
            this.Loc2d = pt;
            this.Loc2dStanding = ptSt;
            this.POF = pof;
            this.SightLine = sLine;
            this.SightLineStanding = sLineSt;
        }

        //Methods

        /// <summary>
        /// Initializes a spectator with default values.
        /// </summary>
        private void Initialize()
        {
            this.Loc2d = new Pt2d();
            this.Loc2dStanding = new Pt2d();
            this.POF = new Pt2d();
            this.SightLine = new Vec2d();
        }

        public void;

    }
}
