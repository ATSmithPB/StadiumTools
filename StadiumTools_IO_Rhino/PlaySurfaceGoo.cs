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
    public class PlaySurfaceGoo : GH_Goo<PlaySurface>
    {
        //Constructors
        public PlaySurfaceGoo()
        {
            this.Value = new PlaySurface();
        }
        public PlaySurfaceGoo(PlaySurface playSurface)
        {
            if (playSurface.IsValid == false)
                playSurface = new PlaySurface();
            this.Value = playSurface;
        }

        public override IGH_Goo Duplicate()
        {
            return Duplicate();
        }
        public PlaySurfaceGoo DuplicatePlaySurfaceGoo()
        {
            return new PlaySurfaceGoo(Value.IsValid == false ? new PlaySurface() : (StadiumTools.PlaySurface)Value.Clone());
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
                return "Null Playsurface";
            else
                return $"PlaySurface: U:{Value.Unit} T:{Value.SportType} TL:{Value.TouchLine.Length}, P:[{Value.Plane.OriginX},{Value.Plane.OriginY}]";
        }
        public override string TypeName
        {
            get { return ("PlaySurface"); }
        }
        public override string TypeDescription
        {
            get { return ("Defines a single StadiumTools PlaySurface"); }
        }
    }

}
