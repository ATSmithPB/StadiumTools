using System.Collections.Generic;
using System;


namespace StadiumTools
{
    /// <summary>
    /// Represents the properties of a single Spectator. 
    /// </summary>
    public class Spectator : ICloneable
    {
        //Properties
        public bool IsValid { get; set; } = true;
        /// <summary>
        /// Coeffecient for model unit space (meter / Unit)
        /// </summary>
        public double Unit { get; set; } = UnitHandler.m;
        /// <summary>
        /// Horizontal offset of seated spectator eyes from row start 
        /// </summary>
        public double EyeX { get; set; }
        /// <summary>
        /// Vertical offset of seated spectator eyes from row floor
        /// </summary>
        public double EyeY { get; set; }
        /// <summary>
        /// Horizontal offset of standing spectator eyes from row start
        /// </summary>
        public double SEyeX { get; set; }
        /// <summary>
        /// Vertical offset of seated spectator eyes from row floor
        /// </summary>
        public double SEyeY { get; set; }
        /// <summary>
        /// the index of the spectator's section within a plan
        /// </summary>
        public int SectionIndex { get; set; }
        /// <summary>
        /// the index of the spectator's tier within their section
        /// </summary>
        public int TierIndex { get; set; }
        /// <summary>
        /// the index of the spectators row within their tier
        /// </summary>
        public int RowIndex { get; set; }
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
        public Pt2d POF { get; set; } = Pt2d.Origin;
        /// <summary>
        /// Pt2d representing the eyes of the spectator in front (for determing C-Value)
        /// </summary>
        public Pt2d ForwardSpectatorLoc2d { get; set; }
        /// <summary>
        /// Vec2d representing the 2d X,Y components and Magnitude of a seated spectator's sightline to the P.O.F
        /// </summary>
        public Vec2d SightLine { get; set; }
        /// <summary>
        /// Vec2d representing the 2d X,Y components and Magnitude of a standing spectator's sightline to the P.O.F 
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
        /// The desired C-Value of the spectator in mm. Specified upon creation of Tier object.
        /// </summary>
        public int TargetCValue { get; set; }
        /// <summary>
        /// The actual C-Value of the spectator in mm. Calculated upon creation of Section object.
        /// </summary>
        public double Cvalue { get; set; } = 0.0;
        /// <summary>
        /// The 3d Plane of this spectators tier if hosted
        /// </summary>
        public Pln3d Plane { get; set; }
        /// <summary>
        /// The distance between this spectator and the spectator(s) seated side-by-side, adjacent, on the same row. 
        /// </summary>
        public double Seperation { get; set; }

        //Constructors 
        public Spectator()
        {
        }

        /// <summary>
        /// construct a hostedSpectator from a collection of values
        /// </summary>
        /// <param name="tierN"></param>
        /// <param name="rowN"></param>
        /// <param name="pt"></param>
        /// <param name="ptSt"></param>
        /// <param name="sLine"></param>
        /// <param name="sLineSt"></param>
        /// <param name="forwardSpec"></param>
        public Spectator
            (
            int tierN, 
            int rowN, 
            Pt2d pt, 
            Pt2d ptSt, 
            Vec2d sLine, 
            Vec2d sLineSt, 
            Pt2d forwardSpec, 
            Pln3d plane,
            double seperation
            )

        {
            this.TierIndex = tierN;
            this.RowIndex = rowN;
            this.Loc2d = pt;
            this.Loc2dStanding = ptSt;
            this.SightLine = sLine;
            this.SightLineStanding = sLineSt;
            this.ForwardSpectatorLoc2d = forwardSpec;
            this.Plane = plane;
            this.Seperation = seperation;

            CalcSpectatorCValue(this);
        }

        //Methods
        /// <summary>
        /// Init a spectator with default parameters based on a coeffecient (meter / unit)
        /// </summary>
        public static void InitDefault(Spectator spectator, double unit)
        {
            spectator.Unit = unit;
            spectator.EyeX = 0.15 * unit;
            spectator.EyeY = 1.2 * unit;
            spectator.SEyeX = 0.6 * unit;
            spectator.SEyeY = 1.4 * unit;
            spectator.TargetCValue = 90;
        }

        /// <summary>
        /// Calculates the CValues for a spectator if it has a valid ForwardSpectator property
        /// </summary>
        /// <param name="section"></param>
        private static void CalcSpectatorCValue(Spectator spectator)
        {
            double r = spectator.Loc2d.Y;
            double d = spectator.Loc2d.X;
            double t = d - spectator.ForwardSpectatorLoc2d.X;
            double n = r - spectator.ForwardSpectatorLoc2d.Y;
            double h = spectator.ForwardSpectatorLoc2d.Y;
            double Tan02 = (r / d);
            double c = (Tan02 * (d - t)) - h;
            spectator.Cvalue = c;
        }

        /// <summary>
        /// create a shallow copy clone of a spectator
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            //Shallow Copy
            Spectator clone = (Spectator)this.MemberwiseClone();
            return clone;
        }

        public void MoveX(double offsetX)
        {
            
        }
    }
}
