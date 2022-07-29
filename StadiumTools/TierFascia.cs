using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    /// <summary>
    /// Defines an optional fascia extension from a tier
    /// </summary>
    public struct Fascia
    {
        //Properties
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
            this.RefPt = points[0];
            this.Points2d = points;
            this.Blocker = 0;
        }

        public Fascia(Pt2d[] points, int blocker)
        {
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
    }
}
