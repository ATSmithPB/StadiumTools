using Rhino.Geometry;
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
        public ICurve[] TouchLine { get; set; }
        /// <summary>
        /// a collection of curves that represent the play surface markings
        /// </summary>
        public ICurve[] Markings { get; set; }
        /// <summary>
        /// a polyline approximation of the Touchline (for calculating intersections with pitch)
        /// </summary>
        public Pline TouchLinePL { get; set; }
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

            GetBoundaryMarkings
                (type, 
                plane, 
                unit, 
                lod, 
                out ICurve[] touchline,
                out Pline touchlinePL,
                out ICurve[] markings,
                out double sizeX, 
                out double sizeY);

            TouchLine = touchline;
            TouchLinePL = touchlinePL;
            Markings = markings;
            SizeX = sizeX;
            SizeY = sizeY;
        }

        //Methods
        private static void GetBoundaryMarkings
            (Type type, 
            Pln2d plane, 
            double unit, 
            LOD lod, 
            out ICurve[] touchline,
            out Pline touchlinePL,
            out ICurve[] markings, 
            out double sizeX, 
            out double sizeY)
        {
            switch (type)
            {
                case Type.Soccer:
                    touchline = TouchlineSoccer(plane, unit, out touchlinePL);
                    markings = MarkingsSoccer(plane, unit, lod);
                    sizeX = touchline[0].Length();
                    sizeY = touchline[1].Length();
                    break;
                case Type.Football:
                    touchline = TouchlineFootball(plane, unit, out touchlinePL);
                    markings = MarkingsFootball(plane, unit, lod);
                    sizeX = touchline[0].Length();
                    sizeY = touchline[1].Length();
                    break;
                case Type.Baseball:
                    touchline = TouchlineBaseball(plane, unit, out touchlinePL);
                    markings = MarkingsBaseball(plane, unit, lod);
                    sizeX = 123.61094 * unit;
                    sizeY = 68.968368* unit;
                    break;
                case Type.Cricket:
                    touchline = TouchlineCricket(plane, unit, out touchlinePL);
                    markings = MarkingsCricket(plane, unit, lod);
                    sizeX = 150 * unit;
                    sizeY = 137 * unit;
                    break;
                case Type.Basketball:
                    touchline = TouchlineBasketball(plane, unit, out touchlinePL);
                    markings = MarkingsBasketball(plane, unit, lod);
                    sizeX = touchline[0].Length();
                    sizeY = touchline[1].Length();
                    break;
                case Type.Tennis:
                    touchline = TouchlineTennis(plane, unit, out touchlinePL);
                    markings = MarkingsTennis(plane, unit, lod);
                    sizeX = touchline[0].Length();
                    sizeY = touchline[1].Length();
                    break;
                case Type.IceHockey:
                    touchline = TouchlineIceHockey(plane, unit, out touchlinePL);
                    markings = MarkingsIceHockey(plane, unit, lod);
                    sizeX = 60.959998 * unit;
                    sizeY = 25.907999 * unit;
                    break;
                case Type.FieldHockey:
                    touchline = TouchlineFieldHockey(plane, unit, out touchlinePL);
                    markings = MarkingsFieldHockey(plane, unit, lod);
                    sizeX = touchline[0].Length();
                    sizeY = touchline[1].Length();
                    break;
                case Type.Rugby:
                    touchline = TouchlineRugby(plane, unit, out touchlinePL);
                    markings = MarkingsRugby(plane, unit, lod);
                    sizeX = touchline[0].Length();
                    sizeY = touchline[1].Length();
                    break;
                case Type.Futsal:
                    touchline = TouchlineFutsal(plane, unit, out touchlinePL);
                    markings = MarkingsFutsal(plane, unit, lod);
                    sizeX = touchline[0].Length();
                    sizeY = touchline[1].Length();
                    break;
                case Type.OlympicTrack:
                    touchline = TouchlineOlympicTrack(plane, unit, out touchlinePL);
                    markings = MarkingsOlympicTrack(plane, unit, lod);
                    sizeX = 84.41 * unit;
                    sizeY = 92.5 * unit;
                    break;
                default:
                    touchline = TouchlineSoccer(plane, unit, out touchlinePL);
                    markings = MarkingsSoccer(plane, unit, lod);
                    sizeX = 0.0;
                    sizeY = 0.0;
                    break;
            }
        }

        //Boundaries
        private static ICurve[] TouchlineSoccer(Pln2d plane, double unit, out Pline touchlinePL)
        {
            return RectangularPS(plane, unit, 105.0, 68.0, out touchlinePL);
        }

        private static ICurve[] TouchlineFootball(Pln2d plane, double unit, out Pline touchlinePL)
        {
            return RectangularPS(plane, unit, 109.727996, 48.767998, out touchlinePL);
        }

        

        private static ICurve[] TouchlineBaseball(Pln2d plane, double unit, out Pline touchlinePL)
        {
            Pt2d[] touchlinePts = new Pt2d[8];
            List<Pt3d> ptsPL = new List<Pt3d>();

            touchlinePts[0] = plane.OriginPt + (plane.Xaxis * -11.106291 * unit) + (plane.Yaxis * -14.529324 * unit);
            ptsPL.Add(new Pt3d(touchlinePts[0], 0.0));
            touchlinePts[1] = plane.OriginPt + (plane.Xaxis * 70.261524 * unit) + (plane.Yaxis * -76.727308 * unit);
            ptsPL.Add(new Pt3d(touchlinePts[1], 0.0));
            touchlinePts[2] = plane.OriginPt + (plane.Xaxis * 112.504649 * unit) + (plane.Yaxis * -34.484184 * unit);
            touchlinePts[3] = plane.OriginPt + (plane.Xaxis * 112.504649 * unit) + (plane.Yaxis * 34.484184 * unit);
            touchlinePts[4] = plane.OriginPt + (plane.Xaxis * 70.261524 * unit) + (plane.Yaxis * 76.727308 * unit);
            touchlinePts[5] = plane.OriginPt + (plane.Xaxis * -11.106291 * unit) + (plane.Yaxis * 14.529324 * unit);
            touchlinePts[6] = plane.OriginPt + (plane.Xaxis * 78.020465 * unit);
            touchlinePts[7] = plane.OriginPt;

            Arc outfieldArc = new Arc(touchlinePts[6], touchlinePts[2], touchlinePts[3]);
            ptsPL.Add(outfieldArc.Start);
            Pt3d[] outPts = Arc.Divide(outfieldArc, 10, out double[] t0);
            ptsPL.AddRange(outPts);
            ptsPL.Add(outfieldArc.End);
            ptsPL.Add(new Pt3d(touchlinePts[4], 0.0));
            Arc infieldArc = new Arc(touchlinePts[7], touchlinePts[5], touchlinePts[0]);
            ptsPL.Add(infieldArc.Start);
            Pt3d[] inPts = Arc.Divide(infieldArc, 10, out double[] t1);
            ptsPL.AddRange(inPts);
            ptsPL.Add(infieldArc.End);
            touchlinePL = new Pline(ptsPL);

            ICurve[] touchlineCrvs = new ICurve[6];
            touchlineCrvs[0] = new Line(touchlinePts[0], touchlinePts[1]);
            touchlineCrvs[1] = new Line(touchlinePts[1], touchlinePts[2]);
            touchlineCrvs[2] = outfieldArc; 
            touchlineCrvs[3] = new Line(touchlinePts[3], touchlinePts[4]);
            touchlineCrvs[4] = new Line(touchlinePts[4], touchlinePts[5]);
            touchlineCrvs[5] = infieldArc;

            return touchlineCrvs;
        }

        private static ICurve[] TouchlineCricket(Pln2d plane, double unit, out Pline touchlinePL)
        {
            ICurve[] touchline = new ICurve[1];
            Ellipse ellipse = new Ellipse(plane, 150 * unit, 137 * unit);
            touchline[0] = ellipse;
            touchlinePL = Pline.FromEllipse(ellipse, 30);
            return touchline;
        }

        private static ICurve[] TouchlineBasketball(Pln2d plane, double unit, out Pline touchlinePL)
        {
            return RectangularPS(plane, unit, 28.651199, 15.239999, out touchlinePL);
        }

        private static ICurve[] TouchlineTennis(Pln2d plane, double unit, out Pline touchlinePL)
        {
            return RectangularPS(plane, unit, 23.774399, 10.972799, out touchlinePL);
        }
        private static ICurve[] TouchlineIceHockey(Pln2d plane, double unit, out Pline touchlinePL)
        {
            double cornerRadius = 8.5344 * unit;
            List<Pt3d> ptsPL = new List<Pt3d>();

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
                ptsPL.Add(arcs[i].Start);
                ptsPL.AddRange(Arc.Divide(arcs[i], 10, out double[] t0));
                ptsPL.Add(arcs[i].End);
            }
            ptsPL.Add(arcs[0].Start);

            touchlinePL = new Pline(ptsPL);
            ICurve[] touchline = new ICurve[8];
            touchline[0] = arcs[0];
            touchline[1] = lines[0];
            touchline[2] = arcs[1];
            touchline[3] = lines[1];
            touchline[4] = arcs[2];
            touchline[5] = lines[2];
            touchline[6] = arcs[3];
            touchline[7] = lines[3];

            return touchline;
        }

        private static ICurve[] TouchlineFieldHockey(Pln2d plane, double unit, out Pline touchlinePL)
        {
            return RectangularPS(plane, unit, 91.4, 55.0, out touchlinePL);
        }

        private static ICurve[] TouchlineRugby(Pln2d plane, double unit, out Pline touchlinePL)
        {
            return RectangularPS(plane, unit, 144, 70, out touchlinePL);
        }

        private static ICurve[] TouchlineFutsal(Pln2d plane, double unit, out Pline touchlinePL)
        {
            return RectangularPS(plane, unit, 42, 25, out touchlinePL);
        }

        private static ICurve[] TouchlineOlympicTrack(Pln2d plane, double unit, out Pline touchlinePL)
        {
            List<Pt3d> ptsPL = new List<Pt3d>();
            ICurve[] touchline = new ICurve[5];
            double length = 84.41 * unit;
            double width = 92.5 * unit;
            Pt2d[] touchlinePts = Pt2d.RectangleCentered(plane, length, width);
            touchlinePts[0] = touchlinePts[0] + (plane.Xaxis * -25.61 * unit);
            Pt2d startPt = touchlinePts[0] + (plane.Yaxis * 7.737788949 * unit);
            Pt2d arc0Cen = plane.OriginPt + (plane.Xaxis * (length / 2));
            Pt2d arc1Cen = plane.OriginPt + (plane.Xaxis * (-length / 2));

            Line line0 = new Line(startPt, touchlinePts[0]);
            ptsPL.Add(line0.Start);
            Line line1 = new Line(touchlinePts[0], touchlinePts[1]);
            ptsPL.Add(line1.Start);
            Line line2 = new Line(touchlinePts[2], touchlinePts[3]);

            Arc arc0 = new Arc(arc0Cen, touchlinePts[1], touchlinePts[2]);
            ptsPL.Add(arc0.Start);
            ptsPL.AddRange(Arc.Divide(arc0, 20, out double[] t0));
            ptsPL.Add(arc0.End);
            Arc arc1 = new Arc(arc1Cen, touchlinePts[3], startPt);
            ptsPL.Add(arc1.Start);
            ptsPL.AddRange(Arc.Divide(arc1, 20, out double[] t1));
            ptsPL.Add(ptsPL[0]);
            touchlinePL = new Pline(ptsPL);

            touchline[0] = line0;
            touchline[1] = line1;
            touchline[2] = arc0;
            touchline[3] = line2;
            touchline[4] = arc1;

            return touchline;
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
        /// returns a generic rectangular playsurface given X and Y dimensions (suitable for all rectangular play surfaces)
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="unit"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <param name="tPL"></param>
        /// <returns>ICurve[]</returns>
        private static ICurve[] RectangularPS(Pln2d plane, double unit, double sizeX, double sizeY, out Pline tPL)
        {
            ICurve[] touchline = new ICurve[4];
            Line[] lines = Line.RectangleCentered(plane, sizeX * unit, sizeY * unit);
            Pt3d[] pts = new Pt3d[] { lines[0].Start, lines[1].Start, lines[2].Start, lines[3].Start, lines[0].Start };
            tPL = new Pline(pts);
            Array.Copy(lines, touchline, 4);
            return touchline;
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
