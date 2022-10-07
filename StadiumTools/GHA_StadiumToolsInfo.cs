using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using GHA_StadiumTools.Properties;

namespace GHA_StadiumTools
{
    public class GHA_StadiumToolsInfo : GH_AssemblyInfo
    {
        public override string Name => "StadiumTools";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => Resources.ST_Icon;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "A collection of components to aid in the parametric modeling and analysis of common stadium elements";

        public override Guid Id => new Guid("8ac43183-56cd-4ad0-a2a9-4f82dc35ad14");

        //Return a string identifying you or your company.
        public override string AuthorName => "Andrew T Smith";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "github.com/ATSmithPB/";
    }
}