using System.Collections.Generic;
using System;
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
    /// Section Goo wrapper class, makes sure Section can be used in Grasshopper.
    /// </summary>
    public class SectionGoo : GH_Goo<Section>
    {
        //Constructors
        public SectionGoo()
        {
            this.Value = new Section();
        }
        public SectionGoo(Section section)
        {
            if (section == null)
            {
                section = new Section();    
            }
            else
            {
                this.Value = section;
            }
        }

        public override IGH_Goo Duplicate()
        {
            return Duplicate();
        }
        public SectionGoo DuplicateSectionGoo()
        {
            return new SectionGoo(Value == null ? new Section() : (StadiumTools.Section)Value.Clone());
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
            {
                return "Null Section";
            }
            else
            {
                return $"Section: U:{Value.Unit} T:{Value.Tiers.Length}";
            }
        }
        public override string TypeName
        {
            get { return ("Section"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a single StadiumTools Section"); }
        }
    }

}
