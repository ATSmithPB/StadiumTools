using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public struct Boundary : ICloneable
    {
        //Properties
        public bool IsValid { get; set; }
        /// <summary>
        /// The unit coeffecient for the plan
        /// </summary>
        public double Unit { get; set; }
        /// <summary>
        /// The style of the plan layount geometry
        /// </summary>
        public Style BoundaryStyle { get; set; }
        /// <summary>
        /// an ordered collection of offset values for each edge of the bowl
        /// </summary>
        public Pline[] Edges { get; set; }
        /// <summary>
        /// The polyline perimeter of the TouchLine
        /// </summary>
        public Pline Polyline { get; set; }
        /// <summary>
        /// Section planeOffsets of the plan
        /// </summary>
        public double[] PlaneOffsets { get; set; }
        /// <summary>
        /// Section planes of the plan
        /// </summary>
        public Pln3d[] Planes { get; set; }
        /// <summary>
        /// The playsurface to generate the boundary around
        /// </summary>
        public PlaySurface PlaySurfaceParameters { get; set; }
        /// <summary>
        /// The index of the closest boundary point to the touchline (used for C-Val)
        /// </summary>
        public Pln3d ClosestPlane { get; set; }
        /// <summary>
        /// The distance of the closest boundary point to the touchline
        /// </summary>
        public double ClosestPlaneDist { get; set; }

        //Enums
        public enum Style
        {
            Radial = 0,
            RadialNonUniform = 1,
            Orthagonal = 2,
            NoCorners = 3,
            Circular = 4,
            Elliptical = 5,
            Custom = 6
        }

        //Constructors
        public Boundary //Radial : Non Uniform : Filleted
            (PlaySurface playSurface,
            double offsetX,
            double offsetY,
            double[] sideRadaii,
            double[] cornerRadaii,
            double[] divLength,
            int cornerBayCount,
            bool[] pointAtCenter,
            double tolerance)
        {
            PlaySurfaceParameters = playSurface;
            BoundaryStyle = Style.RadialNonUniform;
            Unit = playSurface.Unit;
            Edges = Boundary.RadialFilletedNonUniform
                (playSurface,
                 offsetX,
                 offsetY,
                 sideRadaii,
                 cornerRadaii,
                 divLength,
                 cornerBayCount,
                 pointAtCenter,
                 tolerance,
                 out Pline polyline,
                 out List<double> planeOffsets,
                 out Pln3d closestPlane,
                 out List<Pln3d> planes,
                 out double closestDistance
                 );
            PlaneOffsets = planeOffsets.ToArray();
            Polyline = polyline;
            IsValid = true;
            ClosestPlane = closestPlane;
            ClosestPlaneDist = closestDistance;
            Planes = planes.ToArray();
        }

        public Boundary ////Radial : Non Uniform
            (PlaySurface playSurface,
            double offsetX,
            double offsetY,
            double[] sideRadaii,
            double[] divLength,
            bool[] pointAtCenter)
        {
            PlaySurfaceParameters = playSurface;
            BoundaryStyle = Style.RadialNonUniform;
            Unit = playSurface.Unit;
            Edges = Boundary.RadialNonUniform
                (playSurface,
                 offsetX,
                 offsetY,
                 sideRadaii,
                 divLength,
                 pointAtCenter,
                 out Pline polyline,
                 out List<double> planeOffsets,
                 out Pln3d closestPlane,
                 out List<Pln3d> planes,
                 out double closestDistance
                 );
            PlaneOffsets = planeOffsets.ToArray();
            Polyline = polyline;
            IsValid = true;
            ClosestPlane = closestPlane;
            ClosestPlaneDist = closestDistance;
            Planes = planes.ToArray();
        }

        public Boundary //Orthagonal : Filleted
            (PlaySurface playSurface,
            double offsetX,
            double offsetY,
            double cornerRadius,
            double[] divLength,
            int cornerBayCount,
            bool[] pointsAtCenter
            )
        {
            PlaySurfaceParameters = playSurface;
            BoundaryStyle = Style.Orthagonal;
            Unit = playSurface.Unit;
            Edges = OrthagonalFilleted
                (playSurface,
                 offsetX,
                 offsetY,
                 cornerRadius,
                 divLength,
                 cornerBayCount,
                 pointsAtCenter,
                 out Pline polyline,
                 out List<double> planeOffsets,
                 out Pln3d closestPlane,
                 out List<Pln3d> planes,
                 out double closestDistance
                 );
            PlaneOffsets = planeOffsets.ToArray();
            Polyline = polyline;
            IsValid = true;
            ClosestPlane = closestPlane;
            ClosestPlaneDist = closestDistance;
            Planes = planes.ToArray();
        }

        public static Boundary[] CloneArray(Boundary[] boundaries)
        {
            //Deep copy
            Boundary[] plansCloned = new Boundary[boundaries.Length];
            for (int i = 0; i < boundaries.Length;)
            {
                plansCloned[i] = (Boundary)boundaries[i].Clone();
            }
            return plansCloned;
        }

        public object Clone()
        {
            return (Boundary)this.MemberwiseClone();
        }

        /// <summary>
        /// returns a multiline string representation of the Style Enum
        /// </summary>
        /// <returns></returns>
        public static string TypeEnumAsString()
        {
            string typeNames = string.Join("@", Enum.GetNames(typeof(Style)));
            string typeNamesMultiLine = typeNames.Replace("@", System.Environment.NewLine);
            return typeNamesMultiLine;
        }

        /// <summary>
        /// returns a numbered multiline string representaton of the Style Enum
        /// </summary>
        /// <returns></returns>
        public static string TypeEnumNumberedAsString()
        {
            string[] typeNames = Enum.GetNames(typeof(Style));
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
        /// Copy any type object to an array of a given length
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[] ArrayValue<T>(T value, int length)
        {
            T[] result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = value;
            }
            return result;
        }

        /// <summary>
        /// returns a new argument exception of an array of values does not equal a specified length
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="valueName"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static T[] ValidateLength<T>(T[] value, string valueName, int length)
        {
            if (value.Length != length)
            {
                throw new ArgumentException($"{valueName} length [{value.Length}] must be equal to PlaySurface.TouchLine.Length {length}");
            }
            else
            {
                return value;
            }
        }

        public static int CalcBowlEdgeCount(PlaySurface playSurface, Style bowlStyle, double[] cornerRadaii)
        {
            int bowlEdgeCount = 0;
            int boundaryCount = playSurface.TouchLine.Length;
            int roundedCornerCount = NonZeroValues(cornerRadaii);

            switch ((int)bowlStyle)
            {
                case ((int)Style.Radial):
                    {
                        bowlEdgeCount += boundaryCount;
                        bowlEdgeCount += roundedCornerCount;
                        break;
                    }
                case ((int)Style.Orthagonal):
                    {
                        bowlEdgeCount = boundaryCount;
                        bowlEdgeCount += roundedCornerCount;
                        break;
                    }
                case ((int)Style.NoCorners):
                    {
                        bowlEdgeCount = boundaryCount;
                        break;
                    }
                case ((int)Style.Circular):
                    {
                        bowlEdgeCount = 1;
                        break;
                    }
                case ((int)Style.Elliptical):
                    {
                        bowlEdgeCount = 1;
                        break;
                    }
            }
            return bowlEdgeCount;
        }

        /// <summary>
        /// returns the count of items in a collection whose value is not == 0
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int NonZeroValues(double[] values)
        {
            int nonZero = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != 0)
                {
                    nonZero++;
                }
            }
            return nonZero;
        }

        /// <summary>
        /// returns collection of polylines that represent the curved/radial boundary of a rectangle 
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <param name="radaii"></param>
        /// <param name="divLen"></param>
        /// <param name="pointAtCenter"></param>
        /// <returns>Pline[]</returns>
        public static Pline[] RadialNonUniform
            (PlaySurface ps,
            double offsetX,
            double offsetY,
            double[] radaii,
            double[] divLen,
            bool[] pointAtCenter,
            out Pline polyline,
            out List<double> planeOffsets,
            out Pln3d closestPlane,
            out List<Pln3d> planes,
            out double closestDistance)
        {
            Pln3d plane = new Pln3d(ps.Plane);
            double sizeX = ps.SizeX + offsetX * 2;
            double sizeY = ps.SizeY + offsetY * 2;
            ValidateLength(radaii, "Side Radaii", 4);
            ValidateLength(divLen, "Div Length", 4);
            ValidateLength(pointAtCenter, "Point At Center", 4);
            ValidateRadiusMin(0, radaii[0], sizeX / 2);
            ValidateRadiusMin(1, radaii[1], sizeY / 2);
            ValidateRadiusMin(2, radaii[2], sizeX / 2);
            ValidateRadiusMin(3, radaii[3], sizeY / 2);

            Pt3d[] pts = Pt3d.RectangleCentered(plane, sizeX, sizeY);
            Arc arc0 = new Arc(pts[0], pts[1], radaii[0]);
            Arc arc1 = new Arc(pts[1], pts[2], radaii[1]);
            Arc arc2 = new Arc(pts[2], pts[3], radaii[2]);
            Arc arc3 = new Arc(pts[3], pts[0], radaii[3]);

            Pline[] plines = new Pline[4];
            plines[0] = new Pline(Arc.DivideLinearCentered(arc0, divLen[0], pointAtCenter[0]));
            plines[1] = new Pline(Arc.DivideLinearCentered(arc1, divLen[1], pointAtCenter[1]));
            plines[2] = new Pline(Arc.DivideLinearCentered(arc2, divLen[2], pointAtCenter[2]));
            plines[3] = new Pline(Arc.DivideLinearCentered(arc3, divLen[3], pointAtCenter[3]));
            
            Pline.Join(plines, out Pline pline);
            polyline = pline;
            CalcAvgPerps(ps, polyline, out planeOffsets, out planes, out closestPlane, out double cd);
            closestDistance = cd;

            return plines;
        }

        public static Pline[] RadialFilletedNonUniform
           (PlaySurface ps,
            double offsetX,
            double offsetY,
            double[] sideRadaii,
            double[] cornerRadaii,
            double[] divLength,
            int cornerBayCount,
            bool[] pointAtCenter,
            double tolerance,
            out Pline polyline,
            out List<double> planeOffsets,
            out Pln3d closestPlane,
            out List<Pln3d> planes,
            out double closestDistance)
        {
            Pln3d plane = new Pln3d(ps.Plane);
            double sizeX = ps.SizeX + offsetX * 2;
            double sizeY = ps.SizeY + offsetY * 2;
            ValidateLength(sideRadaii, "Side Radaii", 4);
            ValidateLength(cornerRadaii, "Corner Radaii", 4);
            ValidateLength(divLength, "Div Length", 4);
            ValidateLength(pointAtCenter, "Point At Center", 4);
            ValidateRadiusMin(0, sideRadaii[0], sizeX / 2);
            ValidateRadiusMin(1, sideRadaii[1], sizeY / 2);
            ValidateRadiusMin(2, sideRadaii[2], sizeX / 2);
            ValidateRadiusMin(3, sideRadaii[3], sizeY / 2);
            ValidateRadiusMax(cornerRadaii[0], sizeX / 2);
            ValidateRadiusMax(cornerRadaii[1], sizeY / 2);
            ValidateRadiusMax(cornerRadaii[2], sizeX / 2);
            ValidateRadiusMax(cornerRadaii[3], sizeY / 2);

            Pt3d[] pts = Pt3d.RectangleCentered(plane, sizeX, sizeY);
            Arc[] arcs = new Arc[8];
            arcs[0] = new Arc(pts[0], pts[1], sideRadaii[0]);
            arcs[1] = new Arc(pts[1], pts[2], sideRadaii[1]);
            arcs[2] = new Arc(pts[2], pts[3], sideRadaii[2]);
            arcs[3] = new Arc(pts[3], pts[0], sideRadaii[3]);
            arcs[4] = Arc.Fillet(arcs[0], arcs[1], cornerRadaii[1], tolerance);
            arcs[5] = Arc.Fillet(arcs[1], arcs[2], cornerRadaii[2], tolerance);
            arcs[6] = Arc.Fillet(arcs[2], arcs[3], cornerRadaii[3], tolerance);
            arcs[7] = Arc.Fillet(arcs[3], arcs[0], cornerRadaii[0], tolerance);

            arcs[0] = new Arc(arcs[7].End, arcs[4].Start, sideRadaii[0]);
            arcs[1] = new Arc(arcs[4].End, arcs[5].Start, sideRadaii[1]);
            arcs[2] = new Arc(arcs[5].End, arcs[6].Start, sideRadaii[2]);
            arcs[3] = new Arc(arcs[6].End, arcs[7].Start, sideRadaii[3]);

            Pline[] plines = new Pline[8];
            plines[0] = new Pline(Arc.DivideLinearCentered(arcs[0], divLength[0], pointAtCenter[0]));
            plines[1] = Pline.FromArc(arcs[4], cornerBayCount);
            plines[2] = new Pline(Arc.DivideLinearCentered(arcs[1], divLength[1], pointAtCenter[1]));
            plines[3] = Pline.FromArc(arcs[5], cornerBayCount);
            plines[4] = new Pline(Arc.DivideLinearCentered(arcs[2], divLength[2], pointAtCenter[2]));
            plines[5] = Pline.FromArc(arcs[6], cornerBayCount);
            plines[6] = new Pline(Arc.DivideLinearCentered(arcs[3], divLength[3], pointAtCenter[3]));
            plines[7] = Pline.FromArc(arcs[7], cornerBayCount);

            Pline.Join(plines, out Pline pline);
            polyline = pline;
            CalcAvgPerps(ps, polyline, out planeOffsets, out planes, out closestPlane, out double cd);
            closestDistance = cd;

            return plines;
        }

        public static Pline[] OrthagonalFilleted
            (PlaySurface ps,
            double offsetX,
            double offsetY,
            double cornerRadius,
            double[] divLength,
            int cornerBayCount,
            bool[] pointAtCenter,
            out Pline polyline,
            out List<double> planeOffsets,
            out Pln3d closestPlane,
            out List<Pln3d> planes,
            out double closestDistance)
        {
            Pln3d plane = new Pln3d(ps.Plane);
            double sizeX = ps.SizeX + offsetX * 2;
            double sizeY = ps.SizeY + offsetY * 2;
            ValidateLength(divLength, "Div Length", 4);
            ValidateLength(pointAtCenter, "Point at Center", 4);
            ValidateRadiusMax(cornerRadius, sizeY / 2);
            ValidateRadiusMax(cornerRadius, sizeX / 2);

            Pt3d[] boundaryPts = Pt3d.RectangleCenteredChamfered(plane, sizeX, sizeY, cornerRadius);
            Pt3d[] arcCenters = Pt3d.RectangleCentered(plane, sizeX - cornerRadius * 2, sizeY - cornerRadius * 2);
            Line[] lines = new Line[4];
            Arc[] arcs = new Arc[4];

            //for (int i = 0; i < 4; i++)
            //{
            //    int a = (i * 2) + 1;
            //    int b = (i + 2) + i;
            //    if (b == 8) { b = 0; }
            //
            //    lines[i] = new Line(boundaryPts[a], boundaryPts[b]);
            //    Pln3d arcPln = new Pln3d(arcCenters[i], boundaryPts[i * 2]);
            //    arcs[i] = new Arc(arcPln, cornerRadius, Math.PI / 2);
            //}


            Pln3d arcPln = new Pln3d(arcCenters[0], boundaryPts[0], boundaryPts[1]);
            arcs[0] = new Arc(arcPln, cornerRadius, Math.PI / 2);
            lines[0] = new Line(boundaryPts[1], boundaryPts[2]);

            arcPln = new Pln3d(arcCenters[1], boundaryPts[2], boundaryPts[3]);
            arcs[1] = new Arc(arcPln, cornerRadius, Math.PI / 2);
            lines[1] = new Line(boundaryPts[3], boundaryPts[4]);

            arcPln = new Pln3d(arcCenters[2], boundaryPts[4], boundaryPts[5]);
            arcs[2] = new Arc(arcPln, cornerRadius, Math.PI / 2);
            lines[2] = new Line(boundaryPts[5], boundaryPts[6]);

            arcPln = new Pln3d(arcCenters[3], boundaryPts[6], boundaryPts[7]);
            arcs[3] = new Arc(arcPln, cornerRadius, Math.PI / 2);
            lines[3] = new Line(boundaryPts[7], boundaryPts[0]);

            int[] method = new int[4];
            method[0] = Convert.ToInt32(pointAtCenter[0]) + 1;
            method[1] = Convert.ToInt32(pointAtCenter[1]) + 1;
            method[2] = Convert.ToInt32(pointAtCenter[2]) + 1;
            method[3] = Convert.ToInt32(pointAtCenter[3]) + 1;

            Pline[] plines = new Pline[8];
            plines[0] = Pline.FromArc(arcs[0], cornerBayCount);
            plines[1] = Pline.FromLine(lines[0], divLength[0], method[0]);
            plines[2] = Pline.FromArc(arcs[1], cornerBayCount);
            plines[3] = Pline.FromLine(lines[1], divLength[1], method[1]);
            plines[4] = Pline.FromArc(arcs[2], cornerBayCount);
            plines[5] = Pline.FromLine(lines[2], divLength[2], method[2]);
            plines[6] = Pline.FromArc(arcs[3], cornerBayCount);
            plines[7] = Pline.FromLine(lines[3], divLength[3], method[3]);

            Pline.Join(plines, out Pline pline);
            polyline = pline;
            CalcAvgPerps(ps, polyline, out planeOffsets, out planes, out closestPlane, out double cd);
            closestDistance = cd;

            return plines;
        }

        public static void ValidateRadiusMin(int index, double value, double minimumAllowed)
        {
            if (value <= minimumAllowed)
            {
                throw new ArgumentException($"Error: Radius too small. Radius {index} [{value}] must exceed Side Length * 0.5 [{minimumAllowed}]");
            }
        }

        public static void ValidateRadiusMax(double value, double maximumAllowed)
        {
            if (value >= maximumAllowed)
            {
                throw new ArgumentException($"Error: Radius too big. Corner Radius [{value}] must not exceed 1/2 X OR Y length [{maximumAllowed}]");
            }
        }

        public static void CalcAvgPerps
            (PlaySurface ps, 
            Pline polyline, 
            out List<double> planeOffsets,
            out List<Pln3d> planes,
            out Pln3d closestPlane, 
            out double closestDistance)
        {
            Pln3d plane = new Pln3d(ps.Plane);
            planeOffsets = new List<double>();
            planes = new List<Pln3d>();
            
            //subdivide boundary pline
            Pline psd = polyline.SubDivide();
            Vec3d[] avgPerps = Vec3d.AvgPerps(psd, plane);
            int closestIndex = 0;
            closestDistance = double.MaxValue;
            
            // scale all perpendicular boundary vectors to intersection with playsurface
            // and find shortest distance 
            for (int i = 0; i < avgPerps.Length; i++)
            {
                bool xSuccess = ps.TouchLinePL.Intersect(psd.Points[i], avgPerps[i], out double factor);
                if(!xSuccess)
                {
                    throw new ArgumentException($"Error: Boundary Plane[{i}] does not intersect the playSurface (and it must)");
                }
                if (factor < closestDistance)
                {
                    closestDistance = factor;
                    closestIndex = i;
                }

                avgPerps[i].Scale(factor);
                planeOffsets.Add(factor);
                Pt3d origin = psd.Points[i] += avgPerps[i];
                Pln3d psdPlane = new Pln3d(origin, avgPerps[i].Flip(), plane.Zaxis);
                planes.Add(psdPlane);
            }
            closestPlane = planes[closestIndex];
        }

    }
}
