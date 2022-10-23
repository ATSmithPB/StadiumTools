using Rhino.Commands;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;

namespace StadiumTools
{
    public class Bowl3d : ICloneable
    {
        //Properties
        public bool IsValid { get; set; }
        public Section[] Sections { get; set; }
        public Section ClosestSection { get; set; }
        public BowlPlan BowlPlan { get; set; }

        public double Unit { get; set; }

        //Constructors
        public Bowl3d()
        {
        }

        public Bowl3d(Section sectionParameters, BowlPlan bowlPlan)
        {
            if (sectionParameters.Unit != bowlPlan.Unit)
            {
                throw new ArgumentException($"Error: Section[{sectionParameters.Unit}] and BowlPlan[{bowlPlan.Unit}] must have the same Unit.");
            }
            BowlPlan = bowlPlan;
            // Calculate Worst Case Section (closest section to touchline)  ie. worst c-Values    
            Section closestSection = Section.CalcClosestSection(sectionParameters, bowlPlan);
            ClosestSection = closestSection;
            Spectator parameters = closestSection.Tiers[0].SpectatorParameters;
            // Apply worst case section geometry to all sections and re-calculate the spectators;
            Section[] sections = new Section[bowlPlan.SectionCount];
            for (int i = 0; i < bowlPlan.SectionCount; i++)
            {
                double xOffset = bowlPlan.Boundary.PlaneOffsets[i] - bowlPlan.Boundary.ClosestPlaneDist;
                Section newSection = Section.ReCalculateFixedGeometry(closestSection, bowlPlan.Boundary.Planes[i], xOffset);
                sections[i] = newSection;
            }
            Sections = sections;
            
            IsValid = true;
        }

        //Methods   
        public Mesh[,] ToMesh()
        {
            int iCount = (int)BowlPlan.SectionCount / 2;
            int jCount = Sections[0].Tiers.Length;
            Mesh[,] result = new Mesh[iCount, jCount];
            for(int i = 0; i < iCount; i++)
            {
                for (int j = 0; j < Sections[i].Tiers.Length; j++)
                {
                    result[i, j] = CalcTierMesh(this, i * 2, j);
                }
            }
            return result;
        }

        public Mesh CalcTierMesh(Bowl3d bowl3d, int sectionIndex, int tierIndex)
        {
            Tier thisTier = bowl3d.Sections[sectionIndex].Tiers[tierIndex];
            Pln3d thisPlane = bowl3d.BowlPlan.Boundary.Planes[sectionIndex];
            Pln3d nextPlane = new Pln3d();
            if (sectionIndex == bowl3d.BowlPlan.SectionCount - 2)
            {
                nextPlane = bowl3d.BowlPlan.Boundary.Planes[0];
            }
            else
            {
                nextPlane = bowl3d.BowlPlan.Boundary.Planes[sectionIndex + 2];
            }

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
                clone.Sections = (Section[])Sections.Clone();
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
