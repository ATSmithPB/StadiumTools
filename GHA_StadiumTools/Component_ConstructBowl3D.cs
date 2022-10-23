using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Rhino;
using Grasshopper.Kernel;
using GHA_StadiumTools.Properties;
using StadiumTools;

namespace GHA_StadiumTools
{
    /// <summary>
    /// Create a custom GH component called ST_ConstructSection2D using the GH_Component as base. 
    /// </summary>
    public class ST_ConstructBowl3D : GH_Component
    {
        /// <summary>
        /// A custom component for input parameters to generate a new 2D section. 
        /// </summary>
        public ST_ConstructBowl3D()
            : base(nameof(ST_ConstructBowl3D), "cB", "Construct a 3D seating bowl from a BowlPlan and a Section", "StadiumTools", "3D")
        {
        }

        /// <summary>
        /// Registers all input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BowlPlan", "BP", "a valid 2D StadiumTools BowlPlan object", GH_ParamAccess.item);
            pManager.AddGenericParameter("Section", "S", "a valid 2D StadiumTools Section object", GH_ParamAccess.item);
        }

        //Set parameter indixes to names (for readability)
        private static int IN_BowlPlan = 0;
        private static int IN_Section = 1;
        private static int OUT_Bowl3D = 0;
        private static int OUT_Closest_Section = 1;
        private static int OUT_Sections = 2;
        private static int OUT_Debug = 3;

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bowl3D", "B3D", "A Bowl3D object", GH_ParamAccess.item);
            pManager.AddGenericParameter("Closest Section", "CS", "The closest Section to the Touchline. Used to calculate riser heights for target C-Values", GH_ParamAccess.item);
            pManager.AddGenericParameter("Sections", "S", "All Sections of the Bowl3d", GH_ParamAccess.list);
            pManager.AddTextParameter("Debug", "Db", "Debug text", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Construct a new section from Data Access
            StadiumTools.Bowl3d newBowl3d = ST_ConstructBowl3D.ConstructBowl3dFromDA(DA, this);
            
            //GH_Goo<T> wrapper
            var newBowl3dGoo = new StadiumTools.Bowl3dGoo(newBowl3d);
            var closestSectionGoo = new StadiumTools.SectionGoo(newBowl3d.ClosestSection);
            string debug = ($"b3d.cs.pln = ({newBowl3d.ClosestSection.Plane.OriginPt.X}, {newBowl3d.ClosestSection.Plane.OriginPt.Y}, {newBowl3d.ClosestSection.Plane.OriginPt.Z}), b3d.b.cp = ({newBowl3d.BowlPlan.Boundary.ClosestPlane.OriginPt.X}, {newBowl3d.BowlPlan.Boundary.ClosestPlane.OriginPt.Y}, {newBowl3d.BowlPlan.Boundary.ClosestPlane.OriginPt.Z})");
            var sectionGoos = SectionGoosFromSections(newBowl3d.Sections);
            

            //Output Goo
            DA.SetData(OUT_Bowl3D, newBowl3dGoo);
            DA.SetData(OUT_Closest_Section, closestSectionGoo);
            DA.SetDataList(OUT_Sections, sectionGoos);
            DA.SetData(OUT_Debug, debug);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.ST_ConstructBowl3D;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("248eabbc-6964-4195-91e9-a594a6c5309c");

        //Methods  
        private static StadiumTools.Bowl3d ConstructBowl3dFromDA(IGH_DataAccess DA, GH_Component thisComponent)
        {
            //Item Containers
            var bowlPlanGooItem = new StadiumTools.BowlPlanGoo();
            var sectionGooItem = new StadiumTools.SectionGoo();

            //Get Goos
            DA.GetData<StadiumTools.BowlPlanGoo>(IN_BowlPlan, ref bowlPlanGooItem);
            DA.GetData<StadiumTools.SectionGoo>(IN_Section, ref sectionGooItem);

            var bowlPlanItem = bowlPlanGooItem.Value;
            var sectionItem = sectionGooItem.Value;
            //Construct a new Bowl3d
            var newBowl3d = new StadiumTools.Bowl3d(sectionItem, bowlPlanItem);
            return newBowl3d;
        }

        private static SectionGoo[] SectionGoosFromSections(StadiumTools.Section[] sections)
        {
            SectionGoo[] result = new SectionGoo[sections.Length];
            for (int i = 0; i < sections.Length; i++)
            {
                SectionGoo newSectionGoo = new SectionGoo(sections[i]);
                result[i] = newSectionGoo;
            }
            return result;
        }


    }
}
