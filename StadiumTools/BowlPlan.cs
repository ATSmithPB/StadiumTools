using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
        /// The playsurface to construct the Plan arround and inherit parameters from
        /// </summary>
        public PlaySurface PlaySurfaceParameters { get; set; }
        /// <summary>
        /// The front edge(s) of the seating bowl
        /// </summary>
        public Boundary Boundary { get; set; }
        public int SectionCount { get; set; }
        public int VomPlacement { get; set; }
        public bool[] VomHas { get; set; }
                
        //Constructors
        public BowlPlan()
        {
        }

        public BowlPlan(PlaySurface playSurface, Boundary boundary, bool[] hasVom)
        {
            if (hasVom.Length != boundary.Planes.Length)
            {
                throw new ArgumentException($"Error: VomHas.Lengh [{hasVom.Length}] must equal section count [{boundary.Planes.Length}]");
            }
            PlaySurfaceParameters = playSurface;
            Boundary = boundary;
            SectionCount = boundary.Planes.Length;
            VomHas = hasVom;
            IsValid = true;
        }

        public BowlPlan(PlaySurface playSurface, Boundary boundary, bool hasVom)
        {
            PlaySurfaceParameters = playSurface;
            Boundary = boundary;
            SectionCount = boundary.Planes.Length;
            
            bool[] hasVomArray = new bool[boundary.Planes.Length];
            for (int i = 0; i < boundary.Planes.Length; i++)
            {
                hasVomArray[i] = hasVom;
            }

            VomHas = hasVomArray;
            IsValid = true;
        }

        //Methods
        /// <summary>
        /// deep copies an array of plan objects
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
                clone.Boundary = (Boundary)this.Boundary.Clone();
            }
            return clone;
        }

    }
}
