using Rhino.Commands;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace StadiumTools
{
    public class Bowl3d : ICloneable
    {
        //Properties
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public Section Section { get; set; }
        public BowlPlan BowlPlan { get; set; }

        //Constructors
        public Bowl3d()
        {

        }

        public Bowl3d(Section section, BowlPlan bowlPlan)
        {
            Name = "Default Name";
            Section = section;
            BowlPlan = bowlPlan;
            IsValid = true;
        }

        public Bowl3d(Section section, BowlPlan bowlPlan, string name)
        {
            Name = name;
            Section = section;
            BowlPlan = bowlPlan;
            IsValid = true;
        }

        //Methods   
        public Mesh[,] ToMesh()
        {
            Mesh[,] result = new Mesh[BowlPlan.SectionCount, Section.Tiers.Length];
            for(int i = 0; i < BowlPlan.SectionCount - 1; i++)
            {
                for (int j = 0; j < Section.Tiers.Length; j++)
                {
                    result[i, j] = CalcTierMesh(this, i, j);
                }
            }
            return result;
        }

        public Mesh CalcTierMesh(Bowl3d bowl3d, int sectionIndex, int tierIndex)
        {
            Tier thisTier = bowl3d.Section.Tiers[tierIndex];
            Pln3d thisPlane = bowl3d.BowlPlan.Boundary.Planes[sectionIndex];
            Pln3d nextPlane = bowl3d.BowlPlan.Boundary.Planes[sectionIndex + 1];
            Pt3d[] pts0 = Pt3d.FromPt2d(thisTier.Points2d, thisPlane);
            Pt3d[] pts3 = Pt3d.FromPt2d(thisTier.Points2d, nextPlane);
            double tierWidth = thisPlane.OriginPt.DistanceTo(nextPlane.OriginPt);
            double aisleWidth = thisTier.AisleWidth;

            if (aisleWidth < tierWidth)
            {
                double[] aisleParameters = GetParameters(tierWidth, aisleWidth);
                Pln3d[] aislePlanes = Pln3d.Tween2(thisPlane, nextPlane, aisleParameters);
                Pt3d[] pts1 = Pt3d.FromPt2d(thisTier.Points2d, aislePlanes[0]);
                Pt3d[] pts2 = Pt3d.FromPt2d(thisTier.Points2d, aislePlanes[1]);
                Mesh result = Mesh.Construct4x(pts0, pts1, pts2, pts3);
                return result;
            }
            else
            {
                Mesh result = Mesh.Construct2x(pts0, pts3);
                return result;
            }
        }

        /// <summary>
        /// create a deep copy clone of a bowl3d
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            //Deep copy
            Bowl3d clone = (Bowl3d)this.MemberwiseClone();
            {
                clone.BowlPlan = (BowlPlan)BowlPlan.Clone();
                clone.Section = (Section)Section.Clone();
            }
            return clone;
        }

        public static double[] GetParameters(double tierWidth, double aisleWidth)
        {
            double[] result = new double[2];
            double halfAisle = (aisleWidth / tierWidth) / 2;
            result[0] = 0.5 - halfAisle;
            result[1] = 0.5 + halfAisle;
            return result;
        }
    }
}
