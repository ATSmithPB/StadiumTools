using System.Collections.Generic;
using StadiumTools;
using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace StadiumTools
{

    /// <summary>
    /// Tier Goo wrapper class, makes sure Tier can be used in Grasshopper.  
    /// </summary>
    public class BowlPlanGoo : GH_Goo<BowlPlan>
    {
        //Constructors
        public BowlPlanGoo()
        {
            this.Value = new BowlPlan();
        }
        public BowlPlanGoo(BowlPlan plan)
        {
            this.Value = plan;
        }

        public override IGH_Goo Duplicate()
        {
            return Duplicate();
        }
        public BowlPlanGoo DuplicatePlanGoo()
        {
            return new BowlPlanGoo(Value == null ? new BowlPlan() : (StadiumTools.BowlPlan)Value.Clone());
        }

        public override bool IsValid
        {
            get
            {
                if (Value == null) { return false; }
                return Value.IsValid;
            }
        }

        public override string ToString()
        {
            if (Value == null)
                return "Null BowlPlan";
            else
                return $"BowlPlan: U:{Value.Unit} PS:{Value.Boundary.PlaySurfaceParameters.SportType}, BS:{Value.Boundary.BoundaryStyle}, nS:{Value.SectionCount}";
        }
        public override string TypeName
        {
            get { return ("BowlPlan"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a single StadiumTools BowlPlan"); }
        }

    }

}
