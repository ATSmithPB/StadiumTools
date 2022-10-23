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
    public class Bowl3dGoo : GH_Goo<Bowl3d>
    {
        //Constructors
        public Bowl3dGoo()
        {
            this.Value = new Bowl3d();
        }
        public Bowl3dGoo(Bowl3d bowl3d)
        {
            this.Value = bowl3d;
        }

        public override IGH_Goo Duplicate()
        {
            return Duplicate();
        }
        public Bowl3dGoo DuplicateBowl3dGoo()
        {
            return new Bowl3dGoo(Value == null ? new Bowl3d() : (StadiumTools.Bowl3d)Value.Clone());
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
                return "Null Bowl3d";
            else
                return $"Bowl3d: PS:{Value.BowlPlan.Boundary.PlaySurfaceParameters.SportType}, BS:{Value.BowlPlan.Boundary.BoundaryStyle}, nT:{Value.Sections[0].Tiers.Length}, nS:{Value.BowlPlan.SectionCount}";
        }
        public override string TypeName 
        {
            get { return ("Bowl3d"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a single StadiumTools Bowl3d"); }
        }

        

    }

}
