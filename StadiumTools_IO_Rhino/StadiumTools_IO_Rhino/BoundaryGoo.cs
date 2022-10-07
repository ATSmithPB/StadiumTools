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
    public class BoundaryGoo : GH_Goo<Boundary>
    {
        //Constructors
        public BoundaryGoo()
        {
            this.Value = new Boundary();
        }
        public BoundaryGoo(Boundary boundary)
        {
            if (boundary.IsValid == false)
                boundary = new Boundary();
            this.Value = boundary;
        }

        public override IGH_Goo Duplicate()
        {
            return Duplicate();
        }
        public BoundaryGoo DuplicateBoundaryGoo()
        {
            return new BoundaryGoo(Value.IsValid == false ? new Boundary() : (StadiumTools.Boundary)Value.Clone());
        }

        public override bool IsValid
        {
            get
            {
                if (Value.IsValid == false) { return false; }
                return Value.IsValid;
            }
        }

        public override string ToString()
        {
            if (Value.IsValid == false)
                return "Null Boundary";
            else
                return $"Boundary: BS:{Value.BoundaryStyle} U:{Value.Unit}";
        }
        public override string TypeName
        {
            get { return ("Boundary"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a single StadiumTools Boundary"); }
        }
    }

}
