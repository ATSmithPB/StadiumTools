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
        public double Unit { get; set; }
        /// <summary>
        /// True if the Plan is Valid
        /// </summary>
        public bool IsValid { get; set; } = true;
        /// <summary>
        /// The front edge(s) of the seating bowl
        /// </summary>
        public Boundary Boundary { get; set; }
        /// <summary>
        /// number of Sections. inherited from this.Boundary.Planes.Count
        /// </summary>
        public int SectionCount { get; set; }
        /// <summary>
        /// The method of vom placement within the section
        /// </summary>
        public int VomStyle { get; set; }
        /// <summary>
        /// boolean value for each section plane. True if section has a vomatory
        /// </summary>
        public bool[] VomHas { get; set; }
                
        //Constructors
        public BowlPlan()
        {
        }

        public BowlPlan(Boundary boundary, bool hasVom)
        {
            Unit = boundary.Unit;
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
                clone.Boundary = (Boundary)this.Boundary.Clone();
            }
            return clone;
        }

    }
}
