using System.Collections.Generic;
using Rhino.Geometry;

namespace StadiumTools
{   
    /// <summary>
    /// Spectators contain all properties neccessary for v calue analysis.
    /// </summary>
    internal class Spectator
    {
        public Point3d loc { get; set; }
        
        public double absDist { get; set; }

        public double hDist { get; set; }

        public double vDist { get; set; }

        public Vector3d sightLine { get; set; }

        public bool hasSightLine { get; set; }

        public float cVal { get; set; }

    }
}
