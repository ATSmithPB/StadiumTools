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
        /// <summary>
        /// The length of the PlaySurface
        /// </summary>
        public double SizeX { get; set; }
        /// <summary>
        /// The width of the Playsurface
        /// </summary>
        public double SizeY { get; set; }
        

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
            Unit = unit;
            IsValid = true;
            Plane = plane;
            SportType = type;
            Lod = lod;
            North = north;
            Orientation = Vec2d.Angle(north, plane.Xaxis);

            GetBoundaryMarkings(type, plane, unit, lod, out ICurve[] boundary, out ICurve[] markings, out double sizeX, out double sizeY);

            Boundary = boundary;
            Markings = markings;
            SizeX = sizeX;
            SizeY = sizeY;
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
            Unit = unit;
            IsValid = true;
            Plane = plane;
            SportType = Type.Custom;
            SizeX = 0.0;
            SizeY = 0.0;
            Lod = lod;
            North = north;
            Orientation = Vec2d.Angle(north, plane.Xaxis);
            Boundary = boundary;
            Markings = markings;
        }

        //Methods
        private static void GetBoundaryMarkings
            (Type type, 
            Pln2d plane, 
            double unit, 
            LOD lod, 
            out ICurve[] boundary, 
            out ICurve[] markings, 
            out double sizeX, 
            out double sizeY)
        {
            switch (type)
            {
                case Type.Soccer:
                    boundary = BoundarySoccer(plane, unit);
                    markings = MarkingsSoccer(plane, unit, lod);
                    sizeX = boundary[0].Length();
                    sizeY = boundary[1].Length();
                    break;
                case Type.Football:
                    boundary = BoundaryFootball(plane, unit);
                    markings = MarkingsFootball(plane, unit, lod);
                    sizeX = boundary[0].Length();
                    sizeY = boundary[1].Length();
                    break;
                case Type.Baseball:
                    boundary = BoundaryBaseball(plane, unit);
                    markings = MarkingsBaseball(plane, unit, lod);
                    sizeX = 123.61094 * unit;
                    sizeY = 68.968368* unit;
                    break;
                case Type.Cricket:
                    boundary = BoundaryCricket(plane, unit);
                    markings = MarkingsCricket(plane, unit, lod);
                    sizeX = 150 * unit;
                    sizeY = 137 * unit;
                    break;
                case Type.Basketball:
                    boundary = BoundaryBasketball(plane, unit);
                    markings = MarkingsBasketball(plane, unit, lod);
                    sizeX = boundary[0].Length();
                    sizeY = boundary[1].Length();
                    break;
                case Type.Tennis:
                    boundary = BoundaryTennis(plane, unit);
                    markings = MarkingsTennis(plane, unit, lod);
                    sizeX = boundary[0].Length();
                    sizeY = boundary[1].Length();
                    break;
                case Type.IceHockey:
                    boundary = BoundaryIceHockey(plane, unit);
                    markings = MarkingsIceHockey(plane, unit, lod);
                    sizeX = 60.959998 * unit;
                    sizeY = 25.907999 * unit;
                    break;
                case Type.FieldHockey:
                    boundary = BoundaryFieldHockey(plane, unit);
                    markings = MarkingsFieldHockey(plane, unit, lod);
                    sizeX = boundary[0].Length();
                    sizeY = boundary[1].Length();
                    break;
                case Type.Rugby:
                    boundary = BoundaryRugby(plane, unit);
                    markings = MarkingsRugby(plane, unit, lod);
                    sizeX = boundary[0].Length();
                    sizeY = boundary[1].Length();
                    break;
                case Type.Futsal:
                    boundary = BoundaryFutsal(plane, unit);
                    markings = MarkingsFutsal(plane, unit, lod);
                    sizeX = boundary[0].Length();
                    sizeY = boundary[1].Length();
                    break;
                case Type.OlympicTrack:
                    boundary = BoundaryOlympicTrack(plane, unit);
                    markings = MarkingsOlympicTrack(plane, unit, lod);
                    sizeX = 84.41 * unit;
                    sizeY = 92.5 * unit;
                    break;
                default:
                    boundary = BoundarySoccer(plane, unit);
                    markings = MarkingsSoccer(plane, unit, lod);
                    sizeX = 0.0;
                    sizeY = 0.0;
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
            Pt2d[] boundaryPts = new Pt2d[8];
            
            boundaryPts[0] = plane.OriginPt + (plane.Xaxis * -11.106291 * unit) + (plane.Yaxis * -14.529324 * unit);
            boundaryPts[1] = plane.OriginPt + (plane.Xaxis * 70.261524 * unit) + (plane.Yaxis * -76.727308 * unit);
            boundaryPts[2] = plane.OriginPt + (plane.Xaxis * 112.504649 * unit) + (plane.Yaxis * -34.484184 * unit);
            boundaryPts[3] = plane.OriginPt + (plane.Xaxis * 112.504649 * unit) + (plane.Yaxis * 34.484184 * unit);
            boundaryPts[4] = plane.OriginPt + (plane.Xaxis * 70.261524 * unit) + (plane.Yaxis * 76.727308 * unit);
            boundaryPts[5] = plane.OriginPt + (plane.Xaxis * -11.106291 * unit) + (plane.Yaxis * 14.529324 * unit);
            boundaryPts[6] = plane.OriginPt + (plane.Xaxis * 78.020465 * unit);
            boundaryPts[7] = plane.OriginPt;

            Arc outfieldArc = new Arc(boundaryPts[6], boundaryPts[2], boundaryPts[3]);            
            Arc infieldArc = new Arc(boundaryPts[7], boundaryPts[5], boundaryPts[0]);

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
            boundary[0] = new Ellipse(plane, 150 * unit, 137 * unit);
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

            
            Pt2d[] boundaryPts = Pt2d.RectangleCenteredChamfered(plane, 60.959998 * unit, 25.907999 * unit, cornerRadius);
            Pt2d[] arcCenters = Pt2d.RectangleCentered(plane, 43.891198 * unit, 8.839199 * unit);
            Line[] lines = new Line[4];
            Arc[] arcs = new Arc[4];

            for (int i = 0; i < 4; i++)
            {
                int a = (i * 2) + 1;
                int b = (i + 2) + i;
                if (b == 8) { b = 0; }

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
            ICurve[] boundary = new ICurve[5];
            double length = 84.41 * unit;
            double width = 92.5 * unit;
            Pt2d[] boundaryPts = Pt2d.RectangleCentered(plane, length, width);
            boundaryPts[0] = boundaryPts[0] + (plane.Xaxis * -25.61 * unit);
            Pt2d startPt = boundaryPts[0] + (plane.Yaxis * 7.737788949 * unit);
            Pt2d arc0Cen = plane.OriginPt + (plane.Xaxis * (length / 2));
            Pt2d arc1Cen = plane.OriginPt + (plane.Xaxis * (-length / 2));

            Line line0 = new Line(startPt, boundaryPts[0]);
            Line line1 = new Line(boundaryPts[0], boundaryPts[1]);
            Line line2 = new Line(boundaryPts[2], boundaryPts[3]);

            Arc arc0 = new Arc(arc0Cen, boundaryPts[1], boundaryPts[2]);
            Arc arc1 = new Arc(arc1Cen, boundaryPts[3], startPt);

            boundary[0] = line0;
            boundary[1] = line1;
            boundary[2] = arc0;
            boundary[3] = line2;
            boundary[4] = arc1;

            return boundary;
        }

        //Markings
        private static ICurve[] MarkingsSoccer(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            Pt2d[] markingPts = new Pt2d[2];

            markingPts[0] = plane.OriginPt + (plane.Yaxis * -34.0 * unit);
            markingPts[1] = plane.OriginPt + (plane.Yaxis * 34.0 * unit);

            markings[0] = new Line(markingPts[0], markingPts[1]);

            return markings;
        }

        private static ICurve[] MarkingsFootball(Pln2d plane, double unit, LOD lod)
        {
            ICurve[] markings = new ICurve[1];
            Pt2d[] markingPts = new Pt2d[2];

            markingPts[0] = plane.OriginPt + (plane.Yaxis * (-48.767998/2 * unit));
            markingPts[1] = plane.OriginPt + (plane.Yaxis * (48.767998/2 * unit));

            markings[0] = new Line(markingPts[0], markingPts[1]);

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

        /// <summary>
        ///  returns a numbered multiline string representaton of the Type Enum
        /// </summary>
        /// <returns></returns>
        public static string TypeEnumNumberedAsString()
        {
            string[] typeNames = Enum.GetNames(typeof(Type));
            string[] typeNamesNumbered = new string[typeNames.Length];

            for (int i = 0; i < typeNames.Length; i++)
            {
                typeNamesNumbered[i] = $"{i}-{typeNames[i]}";
            }
            string typeNamesUniline = string.Join("@", typeNamesNumbered);
            string typeNamesMultiLine = typeNamesUniline.Replace("@", System.Environment.NewLine);
            return typeNamesMultiLine;
        }

        /// <summary>
        /// shallow copy a PlaySurface
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return (PlaySurface)this.MemberwiseClone();
        }

    }
}
