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
        /// an ordered collection of radaii for each edge of the bowl. 0 == Line 
        /// </summary>
        public double[] BowlRadaii { get; set; }
        /// <summary>
        /// an ordered collection of radaii for each filleted corner of the bowl
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

        /// <summary>
        /// returns the total number of values in an array who's value is NOT equal to 0
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int NonZeroCount(double[] values)
        {
            int nonZeroCount = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != 0)
                {
                    nonZeroCount++;
                }
            }
            return nonZeroCount;
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

    }
}
