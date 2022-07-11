using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace GHA_StadiumTools
{
    public class GHA_StadiumToolsInfo : GH_AssemblyInfo
    {
        public override string Name => "StadiumTools";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("8ac43183-56cd-4ad0-a2a9-4f82dc35ad14");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}