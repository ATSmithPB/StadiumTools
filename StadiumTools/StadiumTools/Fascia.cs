using System;
using System.Collections.Generic;

namespace StadiumTools
{
    /// <summary>
    /// Defines an optional fascia extension from a tier
    /// </summary>
    public struct Fascia
    {
        //Properties
        /// <summary>
        /// The unit space of this Fascia
        /// </summary>
        public double Unit { get; set; }
        /// <summary>
        /// The reference point is the 0,0 of the fascia, and the point of attachment to the tier
        /// </summary>
        public Pt2d RefPt { get; set; }
        /// <summary>
        /// A collection of points that define the outline of the fascia tier where the RefPt is at index [0]
        /// </summary>
        public Pt2d[] Points2d { get; set; }
        /// <summary>
        /// An optional index of a point (in Points2d) to use for calculating the C-Value of the first spectator of a tier.
        /// </summary>
        public int Blocker { get; set; }

        //Constructors
        public Fascia(Pt2d[] points)
        {
            this.Unit = UnitHandler.m;
            this.RefPt = points[0];
            this.Points2d = points;
            this.Blocker = 0;
        }

        public Fascia(Pt2d[] points, double unit)
        {
            this.Unit = unit;
            this.RefPt = points[0];
            this.Points2d = Pt2d.Scale(points, unit);
            this.Blocker = 0;
        }

        public Fascia(Pt2d[] points, int blocker)
        {
            this.Unit = UnitHandler.m;
            this.RefPt = points[0];
            this.Points2d = points;

            if (blocker < 0)
            {
                this.Blocker = 0;
            }
            else if (blocker > points.Length)
            {
                this.Blocker = points.Length;
            }
            else
            {
                this.Blocker = blocker;
            }
        }

        public Fascia(Pt2d[] points, int blocker, double unit)
        {
            this.Unit = unit;
            this.RefPt = points[0];
            this.Points2d = points;

            if (blocker < 0)
            {
                this.Blocker = 0;
            }
            else if (blocker > points.Length)
            {
                this.Blocker = points.Length;
            }
            else
            {
                this.Blocker = blocker;
            }
        }

        //Methods
        /// <summary>
        /// Initializes a new Fascia with a default profile scaled to the unit system parameter 
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static Fascia InitDefault(double unit)
        {
            Pt2d p0 = new Pt2d(0.0 ,0.0) * unit;
            Pt2d p1 = new Pt2d(0.0, 0.52) * unit;
            Pt2d p2 = new Pt2d(-0.15, 0.56) * unit;
            Pt2d p3 = new Pt2d(-0.15, 0.0) * unit;
            Pt2d p4 = new Pt2d(0.0, -0.2) * unit;
            Pt2d p5 = new Pt2d(1.0, -0.2) * unit;

            Pt2d[] defaultPts = new Pt2d[] { p0, p1, p2, p3, p4, p5 };
            return new Fascia(defaultPts, 2, unit);
        }

        
    }
}
