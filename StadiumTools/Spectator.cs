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
        /// the index of the spectator's tier within a section
        /// </summary>
        public int tierNum { get; set; }
        /// <summary>
        /// the index of the spectators row within a tier
        /// </summary>
        public int rowNum { get; set; }
        /// <summary>
        /// a point representing the location of the spectator seated in 2d space relative to the P.O.F
        /// </summary>
        public Pt2d Loc2d { get; set; }
        /// <summary>
        /// a point representing the location of spectator standing in 2d space relative to the P.O.F
        /// </summary>
        public Pt2d Loc2dStanding { get; set; }
        /// <summary>
        /// a point represneting the location of the P.O.F (Point of focus)
        /// </summary>
        public Pt2d pof { get; set; }
        /// <summary>
        /// a Vec2d representing the 2d (h(x), v(y)) components and Length of a seated spectator's sightline to the P.O.F
        /// </summary>
        public Vec2d sightLine { get; set; }
        /// <summary>
        /// a Vec2d representing the 2d (h(x), v(y)) components and Length of a standing spectator's sightline to the P.O.F
        /// </summary>
        public Vec2d sightLineStanding { get; set; }
        /// <summary>
        /// True if the spectator has an unobstructed sightline to the P.O.F
        /// </summary>
        public bool hasSightLine { get; set; } = false;
        /// <summary>
        /// The C-Value of the spectator
        /// </summary>
        public double cVal { get; set; } = 0.0;

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
            this.tierNum = tierN;
            this.rowNum = rowN;
            this.Loc2d = pt;
            this.Loc2dStanding = ptSt;
            this.pof = pof;
            this.sightLine = sLine;
            this.sightLineStanding = sLineSt;
        }

        //Methods

        /// <summary>
        /// Initializes a spectator with default values.
        /// </summary>
        public void Initialize()
        {
            this.Loc2d = new Pt2d();
            this.Loc2dStanding = new Pt2d();
            this.pof = new Pt2d();
            this.sightLine = new Vec2d();
        }
    }
}
