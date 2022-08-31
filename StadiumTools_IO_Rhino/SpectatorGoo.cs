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
    public class SpectatorGoo : GH_Goo<Spectator>
    {
        //Constructors
        public SpectatorGoo()
        {
            this.Value = new Spectator();
        }
        public SpectatorGoo(Spectator spectator)
        {
            if (spectator == null)
                spectator = new Spectator();
            this.Value = spectator;
        }

        public override IGH_Goo Duplicate()
        {
            return Duplicate();
        }
        public SpectatorGoo DuplicateSpectatorGoo()
        {
            return new SpectatorGoo(Value == null ? new Spectator() : (StadiumTools.Spectator)Value.Clone());
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
                return "Null Spectator";
            else
                return $"Spectator: U:{Value.Unit} tC:{Value.TargetCValue} C:{System.Math.Round(Value.Cvalue, 2)} STR:[{Value.SectionIndex},{Value.TierIndex},{Value.RowIndex}] ";
        }
        public override string TypeName
        {
            get { return ("Spectator"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a single StadiumTools Spectator"); }
        }
    }

}
