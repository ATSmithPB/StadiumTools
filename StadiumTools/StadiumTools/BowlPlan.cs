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
        /// The playsurface to construct the Plan arround and inherit parameters from
        /// </summary>
        public PlaySurface PlaySurfaceParameters { get; set; }
        /// <summary>
        /// The front edge(s) of the seating bowl
        /// </summary>
        public Boundary Boundary { get; set; }
                
        //Constructors
        public BowlPlan()
        {
        }

        public BowlPlan(PlaySurface playSurface, Boundary boundary)
        {
            PlaySurfaceParameters = playSurface;
            Boundary = boundary;
            IsValid = true;
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
                clone.Boundary = (Boundary)this.Boundary.Clone();
            }
            return clone;
        }

    }
}
