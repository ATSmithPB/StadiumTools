using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public class Plan : ICloneable
    {
        //Properties
        /// <summary>
        /// True if the Plan is Valid
        /// </summary>
        public bool IsValid { get; set; } = true;
        /// <summary>
        /// The playsurface to construct the Plan arround and inherit parameters from
        /// </summary>
        public PlaySurface PlaySurfaceParameters { get; set; }
        /// <summary>
        /// The style of the plan layount geometry
        /// </summary>
        public Style PlanStyle { get; set; }
        /// <summary>
        /// The unit coeffecient for the plan
        /// </summary>
        public double Unit { get; set; }
        
        /// <summary>
        /// The default width of the structral bays
        /// </summary>
        public double DefaultBayWidth { get; set; }
        /// <summary>
        /// Array of Structural Bay Widths
        /// </summary>
        public double[] BayWidths { get; set; }
        /// <summary>
        /// Total number of structural bays on this tier
        /// </summary>
        public int BayCount { get; set; }
        /// <summary>
        /// The default offset distance between the Playsurface Boundary and Sightline Boundary (used to calculate section Points of Focus
        /// </summary>
        public double DefaultSightlineOffset { get; set; }
        /// <summary>
        /// Array of SightLineOffset values for each edge of the playsurface boundary
        /// </summary>
        public double[] SightlineOffsets { get; set; }
        /// <summary>
        /// Number of structural bays to construct within a corner of a pitch for this tier
        /// </summary>
        public int CornerBayCount { get; set; }
        /// <summary>
        /// The index of this plan if placed in a Multi-Plan 
        /// </summary>
        public int Index { get; set; }

        //Enums
        public enum Style
        {
            Radial = 0,
            Orthagonal = 1,
            NoCorners = 2,
            Circular = 3,
            Sectional = 4,
        }

        //Constructors
        public Plan()
        {
        }

        /// <summary>
        /// Construct a Plan based around a PlaySurface
        /// </summary>
        /// <param name="playSurface"></param>
        /// <param name="planStyle"></param>
        /// <param name="defaultBayWidth"></param>
        /// <param name="defaultSightlineOffset"></param>
        /// <param name="cornerBayCount"></param>
        public Plan(PlaySurface playSurface, Style planStyle, double defaultBayWidth, double defaultSightlineOffset, int cornerBayCount)
        {
            PlaySurfaceParameters = playSurface;
            PlanStyle = planStyle;
            Unit = playSurface.Unit;
            DefaultBayWidth = defaultBayWidth;
            BayCount = 10;

            for (int i = 0; i < this.BayCount; i++)
            {
                BayWidths[i] = DefaultBayWidth;
            }

            DefaultSightlineOffset = defaultSightlineOffset;

            for (int i = 0; i < playSurface.Boundary.Length; i++)
            {
                SightlineOffsets[i] = DefaultSightlineOffset;
            }

            CornerBayCount = cornerBayCount;
            IsValid = true;
        }

        /// <summary>
        /// ceep copies an array of plan objects
        /// </summary>
        /// <param name="plans"></param>
        /// <returns></returns>
        public static Plan[] CloneArray(Plan[] plans)
        {
            //Deep copy
            Plan[] plansCloned = new Plan[plans.Length];
            for (int i = 0; i < plans.Length;)
            {
                plansCloned[i] = (Plan)plans[i].Clone();
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
            Plan clone = (Plan)this.MemberwiseClone();
            {
                clone.PlaySurfaceParameters = (PlaySurface)this.PlaySurfaceParameters.Clone();
                clone.BayWidths = (double[])this.BayWidths.Clone();
                clone.SightlineOffsets = (double[])this.SightlineOffsets.Clone();
            }
            return clone;
        }






    }
}
