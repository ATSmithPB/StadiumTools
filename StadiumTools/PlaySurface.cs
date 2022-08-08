using System;
using System.Collections.Generic;

namespace StadiumTools
{
    public struct PlaySurface
    {
        //Enums
        public enum Type
        {
            Soccer = 0,
            Football = 1,
            Baseball = 2,
            Cricket = 3,
            Basketball = 4,
            Tennis = 5,
            IceHockey = 6,
            FieldHockey = 7,
            Rugby = 8,
            Futsal = 9,
            OlympicTrack = 10,
            Custom = 11,
        }


        public enum LOD
        {
            Simple = 0,
            Medium = 1,
            Complex = 2,
        }

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
        public Type SportType { get; set; }
        /// <summary>
        /// Level of detail 
        /// </summary>
        public LOD Lod { get; set; }
        /// <summary>
        /// a collection of curves that describe the outermost poermeter of the play area bounds
        /// </summary>
        public ICurve[] Boundary { get; set; }
        /// <summary>
        /// a collection of curves that represent the play surface markings
        /// </summary>
        public ICurve[] Markings { get; set; }
        /// <summary>
        /// the angle between the PlaySurface axis and the World Y
        /// </summary>
        public double Direction { get; set; }
        /// <summary>
        /// the angle between the PlaySurface axis and North
        /// </summary>
        public double Orientation { get; set; }

        //Constructors
        public PlaySurface(Pln3d plane, double unit, Type type, LOD lod)
        {
            this.Unit = unit;
            this.IsValid = true;
            this.Plane = plane;
            this.SportType = type;
            this.Lod = lod;
            Vec3d direction = (plane.Xaxis - new Vec3d(1, 0, 0));
            this.Direction = direction.X;
            this.Orientation = Math.Abs(direction.X);

            GetBoundaryMarkings(type, plane, unit, lod, out ICurve[] boundary, out ICurve[] markings);

            this.Boundary = boundary;
            this.Markings = markings;
        }

        //Methods
        private static void GetBoundaryMarkings(Type type, Pln3d plane, double unit, LOD lod, out ICurve[] boundary, out ICurve[] markings)
        {
            switch (type)
            {
                case Type.Soccer:
                    boundary = BoundarySoccer(plane, unit);
                    markings = MarkingsSoccer(plane, unit, lod);
                    break;
                case Type.Football:
                    boundary = BoundaryFootball(plane, unit);
                    markings = MarkingsFootball(plane, unit, lod);
                    break;
                case Type.Baseball:
                    boundary = BoundaryBaseball(plane, unit);
                    markings = MarkingsBaseball(plane, unit, lod);
                    break;
                case Type.Cricket:
                    boundary = BoundaryCricket(plane, unit);
                    markings = MarkingsCricket(plane, unit, lod);
                    break;
                case Type.Basketball:
                    boundary = BoundaryBasketball(plane, unit);
                    markings = MarkingsBasketball(plane, unit, lod);
                    break;
                case Type.Tennis:
                    boundary = BoundaryTennis(plane, unit);
                    markings = MarkingsTennis(plane, unit, lod);
                    break;
                case Type.IceHockey:
                    boundary = BoundaryIceHockey(plane, unit);
                    markings = MarkingsIceHockey(plane, unit, lod);
                    break;
                case Type.FieldHockey:
                    boundary = BoundaryFieldHockey(plane, unit);
                    markings = MarkingsFieldHockey(plane, unit, lod);
                    break;
                case Type.Rugby:
                    boundary = BoundaryRugby(plane, unit);
                    markings = MarkingsRugby(plane, unit, lod);
                    break;
                case Type.Futsal:
                    boundary = BoundaryFutsal(plane, unit);
                    markings = MarkingsFutsal(plane, unit, lod);
                    break;
                case Type.OlympicTrack:
                    boundary = BoundaryOlympicTrack(plane, unit);
                    markings = MarkingsOlympicTrack(plane, unit, lod);
                    break;
                default:
                    boundary = BoundaryCustom(plane, unit);
                    markings = MarkingsCustom(plane, unit, lod);
                    break;
            }
        }

        //Boundaries
        private static ICurve[] BoundarySoccer(Pln3d plane, double unit)
        {
            ICurve[] boundary = new ICurve[4];
            Line.RectangleCentered(plane, 105.0 * unit, 68.0 * unit);
            return boundary;
        }

        private static ICurve[] BoundaryFootball(Pln3d plane, double unit)
        {
            ICurve[] boundary = new ICurve[4];
            Line.RectangleCentered(plane, 109.727996 * unit, 48.767998 * unit);
            return boundary;
        }

        private static ICurve[] BoundaryBaseball(Pln3d plane, double unit)
        {
            Pt2d[] boundaryPts = new Pt2d[6];
            boundaryPts[0] = new Pt2d(-11.106291, -14.529324);
            boundaryPts[1] = new Pt2d(70.261524, -76.727308);
            boundaryPts[2] = new Pt2d(112.504649, -34.484184);
            boundaryPts[3] = new Pt2d(112.504649, 34.484184);
            boundaryPts[4] = new Pt2d(70.261524, 76.727308);
            boundaryPts[5] = new Pt2d(-11.106291, 14.529324);

            Pt2d infieldArcCenter = new Pt2d(78.020465, 0);
            Arc infieldArc = new Arc(new Pln3d(infieldArcCenter. ), 18.288, new Domain(0.797147, 2.356194);
            Arc outfieldArc = new Arc(Pln3d.XYPlane, 48.768, new Domain(0.0, 1.0);


            ICurve[] boundary = new ICurve[6];
            boundary[0] = new Line(-11.106, 14.529);

            return boundary;
        }

        private static ICurve[] BoundaryCricket(Pln3d plane, double unit)
        {
            ICurve[] soccerBoundary = new ICurve[10];
            return soccerBoundary;
        }

        private static ICurve[] BoundaryBasketball(Pln3d plane, double unit)
        {
            ICurve[] soccerBoundary = new ICurve[4];
            Line.RectangleCentered(plane, 28.651199 * unit, 15.239999 * unit);
            return soccerBoundary;
        }

        private static ICurve[] BoundaryTennis(Pln3d plane, double unit)
        {
            ICurve[] soccerBoundary = new ICurve[10];
            Line.RectangleCentered(plane, 23.774399 * unit, 10.972799 * unit);
            return soccerBoundary;
        }
        private static ICurve[] BoundaryIceHockey(Pln3d plane, double unit)
        {
            ICurve[] soccerBoundary = new ICurve[10];
            Line.RectangleCentered(plane, 60.959998 * unit, 25.907999 * unit);
            //needs to be filleted
            return soccerBoundary;
        }

        private static ICurve[] BoundaryFieldHockey(Pln3d plane, double unit)
        {
            ICurve[] soccerBoundary = new ICurve[10];
            Line.RectangleCentered(plane, 91.4 * unit, 55.0 * unit);
            return soccerBoundary;
        }

        private static ICurve[] BoundaryRugby(Pln3d plane, double unit)
        {
            ICurve[] soccerBoundary = new ICurve[10];
            Line.RectangleCentered(plane, 144 * unit, 70 * unit);
            return soccerBoundary;
        }

        private static ICurve[] BoundaryFutsal(Pln3d plane, double unit)
        {
            ICurve[] soccerBoundary = new ICurve[10];
            return soccerBoundary;
        }

        private static ICurve[] BoundaryOlympicTrack(Pln3d plane, double unit)
        {
            ICurve[] soccerBoundary = new ICurve[10];
            return soccerBoundary;
        }

        private static ICurve[] BoundaryCustom(Pln3d plane, double unit)
        {
            ICurve[] soccerBoundary = new ICurve[10];
            return soccerBoundary;
        }

        //Markings
        private static ICurve[] MarkingsSoccer(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

        private static ICurve[] MarkingsFootball(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

        private static ICurve[] MarkingsBaseball(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

        private static ICurve[] MarkingsCricket(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

        private static ICurve[] MarkingsBasketball(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

        private static ICurve[] MarkingsTennis(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

        private static ICurve[] MarkingsIceHockey(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

        private static ICurve[] MarkingsFieldHockey(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

        private static ICurve[] MarkingsRugby(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

        private static ICurve[] MarkingsFutsal(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

        private static ICurve[] MarkingsOlympicTrack(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

        private static ICurve[] MarkingsCustom(Pln3d plane, double unit, LOD lod)
        {
            ICurve[] soccerMarkings = new ICurve[10];
            return soccerMarkings;
        }

    }
}
