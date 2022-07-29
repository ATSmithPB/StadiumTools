using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    internal struct PlaySurface
    {
        public enum Type
        {
            Soccer = 0,
            Football = 1,
            Baseball = 2,
            Cricket = 3,
        }

        //Properties
        public bool IsValid { get; set; }
        /// <summary>
        /// the plane of the Playing Surface. Where plane origin is the center of the playing field.
        /// </summary>
        public Pln3d Plane { get; set; }
        /// <summary>
        /// The playing surface sport type
        /// </summary>
        public Type Sport { get; set; }
        /// <summary>
        /// A collection of poins that describe the lines of the field
        /// </summary>
        public Pt2d[][] Points2d { get; set; }
        /// <summary>
        /// a collection of points that describe the outermost poermeter of the play area
        /// </summary>
        public Pt2d[] Perimeter { get; set; }
        /// <summary>
        /// the angle between the PlaySurface Y and the World Y
        /// </summary>
        public double Direction { get; set; }
        /// <summary>
        /// the angle between the PlaySurface Y and North
        /// </summary>
        public double Orientation { get; set; }

        //Methods

    }
}
