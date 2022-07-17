using System.Collections.Generic;
using GHA_StadiumTools.Properties;

namespace GHA_StadiumTools
{
    public class StadiumToolsCategoryIcon : Grasshopper.Kernel.GH_AssemblyPriority
    {
        public override Grasshopper.Kernel.GH_LoadingInstruction PriorityLoad()
        {
            Grasshopper.Instances.ComponentServer.AddCategoryIcon("StadiumTools", Resources.ST_Icon);
            Grasshopper.Instances.ComponentServer.AddCategorySymbolName("StadiumTools", 'S');
            return Grasshopper.Kernel.GH_LoadingInstruction.Proceed;
        }
    }
}
