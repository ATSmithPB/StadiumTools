using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    internal struct PlaySurface
    {
        //Enums
        public enum Type
        {
            Soccer = 0,
            Football = 1,
            Baseball = 2,
            Cricket = 3,
        }

        
        public enum LOD
        {
            Simple = 0,
            Complex = 1,
        }

        //Constructors
        //public PlaySurface(Pln3d plane, Type type, LOD lod)
        //{
        //    this.IsValid = true;
        //    this.Plane = plane;
        //    this.Sport = type;
        //    this.Lod = lod;
        //    Vec3d direction = (plane.Yaxis - new Vec3d(0, 1, 0));
        //    this.Direction = direction.Y;
        //    this.Orientation = Math.Abs(direction.Y);

        //}


        //Properties
        /// <summary>
        /// The Units coeffecient for this PlaySurface
        /// </summary>
        public double Unit { get; set; }
        /// <summary>
        /// true if the PlaySurface is Valid
        /// </summary>
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
        /// Level of detail 
        /// </summary>
        public LOD Lod { get; set; }
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
        //private static void CalcPoints(PlaySurface ps)
        //{
        //    switch (ps.Sport)
        //    {
        //        case Type.Soccer:
        //            ps.Points2d = SoccerPoints();
        //            ps.Perimeter = ps.Points2d[0];
        //            break;
        //        case Type.Football:
        //            ps.Points2d = FootballPoints();
        //            ps.Perimeter = ps.Points2d[0];
        //            break;
        //        case Type.Baseball:
        //            ps.Points2d = SoccerPoints();
        //            ps.Perimeter = ps.Points2d[0];
        //            break;
        //        case Type.Cricket:
        //            spectator.Unit = StadiumTools.UnitHandler.m;
        //            break;

        //    }
        //}

        private static Pt2d[][] SoccerPoints()
        {
            Pt2d[][] soccerPts = new Pt2d[5][];
            return soccerPts;
        }
    }
}
