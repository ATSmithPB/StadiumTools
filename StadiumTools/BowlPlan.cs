using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public class BowlPlan : ICloneable
    {
        //Properties
        /// <summary>
        /// True if the Plan is Valid
        /// </summary>
        public bool IsValid { get; set; } = true;
        /// <summary>
        /// The index of this plan if placed in a Multi-Plan 
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// The playsurface to construct the Plan arround and inherit parameters from
        /// </summary>
        public PlaySurface PlaySurfaceParameters { get; set; }
        /// <summary>
        /// The unit coeffecient for the plan
        /// </summary>
        public double Unit { get; set; }
        /// <summary>
        /// The style of the plan layount geometry
        /// </summary>
        public Style BowlStyle { get; set; }
        /// <summary>
        /// The number of curves that define the bowl edge
        /// </summary>
        public int BowlEdgeCount { get; set; }
        /// <summary>
        /// an ordered collection of offset values for each edge of the bowl
        /// </summary>
        public double[] BowlOffsets { get; set; }
        /// <summary>
        /// an ordered collection of sideRadaii for each edge of the bowl. 0 == Line 
        /// </summary>
        public double[] BowlRadaii { get; set; }
        /// <summary>
        /// an ordered collection of sideRadaii for each filleted corner of the bowl
        /// </summary>
        public double[] CornerRadaii { get; set; }
        /// <summary>
        /// an ordered collection of boolean values. True if a gridline should be centered on a given bowl edge
        /// </summary>
        public bool[] CenterGridline { get; set; }
        /// <summary>
        /// Array of Structural Bay Widths
        /// </summary>
        public double[] BayWidths { get; set; }
        /// <summary>
        /// Number of structural bays to construct within a corner of a pitch for this tier
        /// </summary>
        public int[] CornerBayCount { get; set; }
        /// <summary>
        /// Number of section planes
        /// </summary>
        public int[] SectionCount { get; set; }
        /// <summary>
        /// Curves that represent the front edge of the bowl
        /// </summary>
        public ICurve[] BowlEdges { get; set; }
        /// <summary>
        /// Section Planes of the plan
        /// </summary>
        public Pln3d[][] SectionPlanes { get; set; }

        //Enums
        public enum Style
        {
            Radial = 0,
            Orthagonal = 1,
            NoCorners = 2,
            Circular = 3,
            Elliptical = 4
        }

        //Constructors
        public BowlPlan()
        {
        }

        /// <summary>
        /// Construct a BowlPlan based around a PlaySurface and global parameters
        /// </summary>
        /// <param name="playSurface"></param>
        /// <param name="planStyle"></param>
        /// <param name="defaultBayWidth"></param>
        /// <param name="defaultSightlineOffset"></param>
        /// <param name="cornerBayCount"></param>
        public BowlPlan(PlaySurface playSurface,
                        Style bowlStyle,
                        double bowlOffset,
                        double bowlRadaii,
                        double cornerRadaii,
                        bool centerGridline,
                        double bayWidths,
                        int cornerBayCount
                       )
        {
            this.PlaySurfaceParameters = playSurface;
            this.Unit = playSurface.Unit;
            this.BowlStyle = bowlStyle;
            int boundaryCount = playSurface.Boundary.Length;
            this.BowlOffsets = ArrayValue(bowlOffset, boundaryCount);
            this.BowlRadaii = ArrayValue(bowlRadaii, boundaryCount);
            this.CornerRadaii = ArrayValue(cornerRadaii, boundaryCount);
            this.CenterGridline = ArrayValue(centerGridline, boundaryCount);
            this.BayWidths = ArrayValue(bayWidths, boundaryCount);
            this.CornerBayCount = ArrayValue(cornerBayCount, boundaryCount);
            this.BowlEdgeCount = CalcBowlEdgeCount(playSurface, bowlStyle, this.CornerRadaii);
            this.IsValid = true;
        }

        /// <summary>
        /// Construct a BowlPlan based around a PlaySurface and arrays of unique parameters. Array length must equal PlaySurface.Boundary.Length 
        /// </summary>
        /// <param name="playSurface"></param>
        /// <param name="bowlStyle"></param>
        /// <param name="bowlOffset"></param>
        /// <param name="bowlRadaii"></param>
        /// <param name="cornerRadaii"></param>
        /// <param name="centerGridline"></param>
        /// <param name="bayWidths"></param>
        /// <param name="cornerBayCount"></param>
        public BowlPlan(PlaySurface playSurface,
                        Style bowlStyle,
                        double[] bowlOffset,
                        double[] bowlRadaii,
                        double[] cornerRadaii,
                        bool[] centerGridline,
                        double[] bayWidths,
                        int[] cornerBayCount
                       )
        {
            this.PlaySurfaceParameters = playSurface;
            this.Unit = playSurface.Unit;
            this.BowlStyle = bowlStyle;
            int boundaryCount = playSurface.Boundary.Length;
            this.BowlEdgeCount = boundaryCount;
            this.BowlOffsets = ValidateLength(bowlOffset, "BowlOffsets", boundaryCount);
            this.BowlRadaii = ValidateLength(bowlRadaii, "BowlRadaii", boundaryCount);
            this.CornerRadaii = ValidateLength(cornerRadaii, "CornerRadaii", boundaryCount);
            this.CenterGridline = ValidateLength(centerGridline, "CenterGridline", boundaryCount);
            this.BayWidths = ValidateLength(bayWidths, "BayWidths", boundaryCount);
            this.CornerBayCount = ValidateLength(cornerBayCount, "CornerBayCount", boundaryCount);
            this.BowlEdgeCount = CalcBowlEdgeCount(playSurface, bowlStyle, this.CornerRadaii);
            this.IsValid = true;
        }

        /// <summary>
        /// ceep copies an array of plan objects
        /// </summary>
        /// <param name="plans"></param>
        /// <returns></returns>
        public static BowlPlan[] CloneArray(BowlPlan[] plans)
        {
            //Deep copy
            BowlPlan[] plansCloned = new BowlPlan[plans.Length];
            for (int i = 0; i < plans.Length;)
            {
                plansCloned[i] = (BowlPlan)plans[i].Clone();
            }
            return plansCloned;
        }

        /// <summary>
        /// returns a deep copy of a Plan object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            //Deep Copy
            BowlPlan clone = (BowlPlan)this.MemberwiseClone();
            {
                clone.PlaySurfaceParameters = (PlaySurface)this.PlaySurfaceParameters.Clone();
                clone.BayWidths = (double[])this.BayWidths.Clone();
                clone.BowlOffsets = (double[])this.BowlOffsets.Clone();
                clone.BowlRadaii = (double[])this.BowlRadaii.Clone();
                clone.CornerRadaii = (double[])this.CornerRadaii.Clone();
                clone.CenterGridline = (bool[])this.CenterGridline.Clone();
                clone.BayWidths = (double[])this.BayWidths.Clone();
                clone.CornerBayCount = (int[])this.CornerBayCount.Clone();
                clone.SectionCount = (int[])this.SectionCount.Clone();
                clone.SectionPlanes = (Pln3d[][])this.SectionPlanes.Clone();
            }
            return clone;
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
        /// 
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
                throw new ArgumentException($"{valueName} length [{value.Length}] must be equal to PlaySurface.Boundary.Length {length}");
            }
            else
            {
                return value;
            }
        }

        public static int CalcBowlEdgeCount(PlaySurface playSurface, Style bowlStyle, double[] cornerRadaii)
        {
            int bowlEdgeCount = 0;
            int boundaryCount = playSurface.Boundary.Length;
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
        public static Pline[] RadialRectangleSegmented(Pln3d plane, double sizeX, double sizeY, double[] radaii, double[] divLen, bool[] pointAtCenter)
        {
            if (radaii.Length != 4 || divLen.Length != 4 || pointAtCenter.Length != 4)
            {
                throw new ArgumentException($"sideRadaii[{radaii.Length}], divLength[{divLen.Length}], and pointAtCenter[{pointAtCenter.Length}] must have a length of 4");
            }
            Pt3d[] pts = Pt3d.RectangleCentered(plane, sizeX, sizeY);
            Arc arc0 = new Arc(pts[0], pts[1], radaii[0]);
            Arc arc1 = new Arc(pts[1], pts[2], radaii[1]);
            Arc arc2 = new Arc(pts[2], pts[3], radaii[2]);
            Arc arc3 = new Arc(pts[3], pts[0], radaii[3]);
            Pline pline0 = new Pline(Arc.DivideLinearCentered(arc0, divLen[0], pointAtCenter[0]));
            Pline pline1 = new Pline(Arc.DivideLinearCentered(arc1, divLen[1], pointAtCenter[1]));
            Pline pline2 = new Pline(Arc.DivideLinearCentered(arc2, divLen[2], pointAtCenter[2]));
            Pline pline3 = new Pline(Arc.DivideLinearCentered(arc3, divLen[3], pointAtCenter[3]));
            return new Pline[4] { pline0, pline1, pline2, pline3 };
        }

        public static Pline[] RadialRectangleFilletedSegmented
           (Pln3d plane,
            double sizeX,
            double sizeY,
            double[] sideRadaii,
            double[] cornerRadaii,
            double[] divLength,
            double cornerDivLen,
            bool[] pointAtCenter,
            bool cornerPointAtCenter,
            double tolerance)
        {
            if (sideRadaii.Length != 4 || divLength.Length != 4 || pointAtCenter.Length != 4 || cornerRadaii.Length != 4)
            {
                throw new ArgumentException($"sideRadaii[{sideRadaii.Length}], divLength[{divLength.Length}], and pointAtCenter[{pointAtCenter.Length}] must have a length of 4");
            }
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
            Pline pline0 = new Pline(Arc.DivideLinearCentered(arcs[0], divLength[0], pointAtCenter[0]));
            Pline pline1 = new Pline(Arc.DivideLinearCentered(arcs[4], cornerDivLen, cornerPointAtCenter));
            Pline pline2 = new Pline(Arc.DivideLinearCentered(arcs[1], divLength[1], pointAtCenter[1]));
            Pline pline3 = new Pline(Arc.DivideLinearCentered(arcs[5], cornerDivLen, cornerPointAtCenter));
            Pline pline4 = new Pline(Arc.DivideLinearCentered(arcs[2], divLength[2], pointAtCenter[2]));
            Pline pline5 = new Pline(Arc.DivideLinearCentered(arcs[6], cornerDivLen, cornerPointAtCenter));
            Pline pline6 = new Pline(Arc.DivideLinearCentered(arcs[3], divLength[3], pointAtCenter[3]));
            Pline pline7 = new Pline(Arc.DivideLinearCentered(arcs[7], cornerDivLen, cornerPointAtCenter));

            return new Pline[8] { pline0, pline1, pline2, pline3, pline4, pline5, pline6, pline7 };
        }
    }
}
