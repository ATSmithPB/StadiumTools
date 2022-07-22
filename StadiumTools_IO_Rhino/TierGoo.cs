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
    public class TierGoo : GH_Goo<Tier>
    {
        //Constructors
        public TierGoo()
        {
            this.Value = new Tier();
        }
        public TierGoo(Tier tier)
        {
            this.Value = tier;
        }

        public override IGH_Goo Duplicate()
        {
            return Duplicate();
        }
        public TierGoo DuplicateTierGoo()
        {
            return new TierGoo(Value == null ? new Tier() : (StadiumTools.Tier)Value.Clone());
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
                return "Null Tier";
            else
                return $"StadiumTools Tier: R:{this.Value.RowCount} X:{this.Value.StartX} Y:{this.Value.StartY} EyeX: {this.Value.SpectatorParameters.EyeX}";
        }
        public override string TypeName
        {
            get { return ("Tier"); }
        }
        public override string TypeDescription 
        {
            get { return ("Defines a single StadiumTools Tier"); }
        }
    }
    
}
