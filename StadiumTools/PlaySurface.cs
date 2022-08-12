using System;
using System.Collections.Generic;

namespace StadiumTools
{
    public struct PlaySurface : ICloneable
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
        public Pln2d Plane { get; set; }
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
        /// a normalized vector pointing north from the pitch center
        /// </summary>
        public Vec2d North { get; set; }
        /// <summary>
        /// the angle between the PlaySurface X-Axis (Long Axis) and North
        /// </summary>
        public double Orientation { get; set; }

        //Constructors
        /// <summary>
        /// Constructs a PlaySurface from a selection of preset sports types, a unit scale, and a level of development
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="unit"></param>
        /// <param name="type"></param>
        /// <param name="lod"></param>
        public PlaySurface(Pln2d plane, Vec2d north, double unit, Type type, LOD lod)
        {
            this.Unit = unit;
            this.IsValid = true;
            this.Plane = plane;
            this.SportType = type;
            this.Lod = lod;
            this.North = north;
            this.Orientation = Vec2d.Angle(north, plane.Xaxis);

            GetBoundaryMarkings(type, plane, unit, lod, out ICurve[] boundary, out ICurve[] markings);

            this.Boundary = boundary;
            this.Markings = markings;
        }

        /// <summary>
        /// Constructs a PlaySurface from custom boundaries
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="unit"></param>
        /// <param name="type"></param>
        /// <param name="lod"></param>
        public PlaySurface(Pln2d plane, Vec2d north, double unit, LOD lod, ICurve[] boundary, ICurve[] markings)
        {
            this.Unit = unit;
            this.IsValid = true;
            this.Plane = plane;
            this.SportType = Type.Custom;
            this.Lod = lod;
            this.North = north;
            this.Orientation = Vec2d.Angle(north, plane.Xaxis);
            this.Boundary = boundary;
            this.Markings = markings;
        }

        //Methods
        private static void GetBoundaryMarkings(Type type, Pln2d plane, double unit, LOD lod, out ICurve[] boundary, out ICurve[] markings)
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
                    boundary = BoundarySoccer(plane, unit);
                    markings = MarkingsSoccer(plane, unit, lod);
                    break;
            }
        }

        //Boundaries
        private static ICurve[] BoundarySoccer(Pln2d plane, double unit)
        {
            ICurve[] boundary = new ICurve[4];
            Line[] lines = Line.RectangleCentered(plane, 105.0 * unit, 68.0 * unit);
            Array.Copy(lines, boundary, 4);
            return boundary;
        }

        private static ICurve[] BoundaryFootball(Pln2d plane, double unit)
        {
            ICurve[] boundary = new ICurve[4];
            Line[] lines = Line.RectangleCentered(plane, 109.727996 * unit, 48.767998 * unit);
            Array.Copy(lines, boundary, 4);
            return boundary;
        }

        private static ICurve[] BoundaryBaseball(Pln2d plane, double unit)
        {
            Pt2d[] boundaryPts = new Pt2d[6];
            boundaryPts[0] = new Pt2d(-11.106291 * unit, -14.529324 * unit);
            boundaryPts[1] = new Pt2d(70.261524 * unit, -76.727308 * unit);
            boundaryPts[2] = new Pt2d(112.504649 * unit, -34.484184 * unit);
            boundaryPts[3] = new Pt2d(112.504649 * unit, 34.484184 * unit);
            boundaryPts[4] = new Pt2d(70.261524 * unit, 76.727308 * unit);
            boundaryPts[5] = new Pt2d(-11.106291 * unit, 14.529324 * unit);

            Pt2d outfieldArcCen = new Pt2d(78.020465 * unit, 0);
            Pln2d outfieldArcPln = new Pln2d(outfieldArcCen, boundaryPts[2]);
            Arc outfieldArc = new Arc(outfieldArcPln, 48.768 * unit, Math.PI / 2);

            Pln2d infieldArcPln = new Pln2d(Pt2d.Origin, boundaryPts[5]);
            Arc infieldArc = new Arc(infieldArcPln, 18.288 * unit, 1.836279);

            ICurve[] boundaryCrvs = new ICurve[6];
            boundaryCrvs[0] = new Line(boundaryPts[0], boundaryPts[1]);
            boundaryCrvs[1] = new Line(boundaryPts[1], boundaryPts[2]);
            boundaryCrvs[2] = outfieldArc; 
            boundaryCrvs[3] = new Line(boundaryPts[3], boundaryPts[4]);
            boundaryCrvs[4] = new Line(boundaryPts[4], boundaryPts[5]);
            boundaryCrvs[5] = infieldArc;


            return boundaryCrvs;
        }

        private static ICurve[] BoundaryCricket(Pln2d plane, double unit)
        {
            ICurve[] boundary = new ICurve[1];
            new Ellipse2d(plane, 150 * unit, 137 * unit);
            return boundary;
        }

        private static ICurve[] BoundaryBasketball(Pln2d plane, double unit)
        {
            ICurve[] boundary = new ICurve[4];
            Line[] lines = Line.RectangleCentered(plane, 28.651199 * unit, 15.239999 * unit);
            Array.Copy(lines, boundary, 4);
            return boundary;
        }

        private static ICurve[] BoundaryTennis(Pln2d plane, double unit)
        {
            ICurve[] boundary = new ICurve[4];
            Line[] lines = Line.RectangleCentered(plane, 23.774399 * unit, 10.972799 * unit);
            Array.Copy(lines, boundary, 4);
            return boundary;
        }
        private static ICurve[] BoundaryIceHockey(Pln2d plane, double unit)
        {
            double cornerRadius = 8.5344 * unit;

            Pt2d[] boundaryPts = Pt2d.RectangleCenteredChamfered(Pt2d.Origin, 60.959998 * unit, 25.907999 * unit, cornerRadius);
            Pt2d[] arcCenters = Pt2d.RectangleCentered(Pt2d.Origin, 43.891198, 8.839199);
            Line[] lines = new Line[4];
            Arc[] arcs = new Arc[4];

            for (int i = 0; i < 4; i++)
            {
                int a = (i * 2) + 1;
                int b = (i + 2) + i;
                if (b == 8) { b = 1; }

                lines[i] = new Line(boundaryPts[a], boundaryPts[b]);
                Pln2d arcPln = new Pln2d(arcCenters[i], boundaryPts[i * 2]);
                arcs[i] = new Arc(arcPln, cornerRadius, Math.PI / 2);
            }

            ICurve[] boundaryCrvs = new ICurve[8];
            boundaryCrvs[0] = arcs[0];
            boundaryCrvs[1] = lines[0];
            boundaryCrvs[2] = arcs[1];
            boundaryCrvs[3] = lines[1];
            boundaryCrvs[4] = arcs[2];
            boundaryCrvs[5] = lines[2];
            boundaryCrvs[6] = arcs[3];
            boundaryCrvs[7] = lines[3];

            return boundaryCrvs;
        }

        private static ICurve[] BoundaryFieldHockey(Pln2d plane, double unit)
        {
            ICurve[] boundary = new ICurve[4];
            Line[] lines = Line.RectangleCentered(plane, 91.4 * unit, 55.0 * unit);
            Array.Copy(lines, boundary, 4);
            return boundary;
        }

        private static ICurve[] BoundaryRugby(Pln2d plane, double unit)
        {
            ICurve[] boundary = new ICurve[4];
            Line[] lines = Line.RectangleCentered(plane, 144 * unit, 70 * unit);
            Array.Copy(lines, boundary, 4);
            return boundary;
        }

        private static ICurve[] BoundaryFutsal(Pln2d plane, double unit)
        {
            ICurve[] boundary = new ICurve[4];
            Line[] lines = Line.RectangleCentered(plane, 42 * unit, 25 * unit);
            Array.Copy(lines, boundary, 4);
            return boundary;
        }

        private static ICurve[] BoundaryOlympicTrack(Pln2d plane, double unit)
        {
            ICurve[] boundary = new ICurve[4];
            double length = 84.41;
            double width = 92.5;
            Pt2d[] boundaryPts = Pt2d.RectangleCentered(Pt2d.Origin, length, width);
            Line line0 = new Line(boundaryPts[0], boundaryPts[1]);
            Line line1 = new Line(boundaryPts[2], boundaryPts[3]);

            Pt2d arc0Cen = new Pt2d(length / 2, 0.0);
            Pt2d arc1Cen = new Pt2d(-length / 2, 0.0);
            Pln2d arc0Pln = new Pln2d(arc0Cen, boundaryPts[1]);
            Pln2d arc1Pln = new Pln2d(arc1Cen, boundaryPts[3]);
            Arc arc0 = new Arc(arc0Pln, width / 2, Math.PI);
            Arc arc1 = new Arc(arc0Pln, width / 2, Math.PI);

            return boundary;
        }


        //Markings
        private static ICurve[] MarkingsSoccer(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            return markings;
        }

        private static ICurve[] MarkingsFootball(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            return markings;
        }

        private static ICurve[] MarkingsBaseball(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            return markings;
        }

        private static ICurve[] MarkingsCricket(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            return markings;
        }

        private static ICurve[] MarkingsBasketball(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            return markings;
        }

        private static ICurve[] MarkingsTennis(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            return markings;
        }

        private static ICurve[] MarkingsIceHockey(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            return markings;
        }

        private static ICurve[] MarkingsFieldHockey(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            return markings;
        }

        private static ICurve[] MarkingsRugby(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            return markings;
        }

        private static ICurve[] MarkingsFutsal(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            return markings;
        }

        private static ICurve[] MarkingsOlympicTrack(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            return markings;
        }

        /// <summary>
        /// returns a multiline string representation of the Types Enum
        /// </summary>
        /// <returns></returns>
        public static string TypeEnumAsString()
        {
            string typeNames = string.Join("@", Enum.GetNames(typeof(Type)));
            string typeNamesMultiLine = typeNames.Replace("@", System.Environment.NewLine);
            return typeNamesMultiLine;
        }

        public object Clone()
        {
            return (SuperRiser)this.MemberwiseClone();
        }

    }
}
