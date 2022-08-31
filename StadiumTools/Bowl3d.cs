using System;
using System.Collections.Generic;

namespace StadiumTools
{
    public class Bowl3d : ICloneable
    {
        //Properties
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public Section Section { get; set; }
        public BowlPlan BowlPlan { get; set; }


        //Constructors
        public Bowl3d()
        {

        }

        public Bowl3d(Section section, BowlPlan bowlPlan)
        {
            Name = "Default Name";
            Section = section;
            BowlPlan = bowlPlan;
            IsValid = true;
        }

        public Bowl3d(Section section, BowlPlan bowlPlan, string name)
        {
            Name = name;
            Section = section;
            BowlPlan = bowlPlan;
            IsValid = true;
        }

        //Methods

        /// <summary>
        /// create a deep copy clone of a bowl3d
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            //Deep copy
            Bowl3d clone = (Bowl3d)this.MemberwiseClone();
            {
                clone.BowlPlan = (BowlPlan)BowlPlan.Clone();
                clone.Section = (Section)Section.Clone();
            }
            return clone;
        }
    }
}
