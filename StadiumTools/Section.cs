using System.Collections.Generic;
using static System.Math;

namespace StadiumTools
{
    /// <summary>
    /// Represents an ordered collection of Tier objects that exist in the same plane
    /// </summary>
    public class Section
    {
        //Properties
        /// <summary>
        /// An ordered list of tiers to comprise the section.
        /// </summary>
        public Tier[] Tiers { get; set; }
        /// <summary>
        /// A list of 2d points that represent the outline of a seating tier
        /// </summary>
        public Pt2d POF { get; set; }
        /// <summary>
        /// The 3d plane of the section.
        /// </summary>
        public Pln3d Plane3d { get; set; }

        //Constructors
        /// <summary>
        /// Construct a Section from a list of tiers 
        /// </summary>
        /// <param name="tiers"></param>
        public Section(List<Tier> tierList)
        {
            Tier[] tiers = tierList.ToArray();
            this.Tiers = tiers;
            this.POF = new Pt2d(0.0, 0.0);

            //Force first tier to get reference point from Point of Focus
            if (tiers[0].RefPtType != Tier.ReferencePtType.ByPOF)
            {
                tiers[0].RefPtType = Tier.ReferencePtType.ByPOF;
            }

            //Apply the section POF to all contained tiers 
            foreach (Tier t in tiers)
            {
                t.POF = this.POF;
            }

            //Calculate points for all tiers in the section
            CalcPts(this);
        }

        /// <summary>
        /// Construct a Section from an array of tiers
        /// </summary>
        /// <param name="tiers"></param>
        public Section(Tier[] tiers)
        {
            this.Tiers = tiers;
            this.POF = new Pt2d(0.0, 0.0);

            for (int i = 0; i < tiers.Length; i++)
            {
                tiers[i].SectionIndex = i;
            }

            CalcPts(this);
        }

        /// <summary>
        /// Construct an empty Section from riserHeight tiers 
        /// </summary>
        /// <param name="nTiers"></param>
        public Section(int nTiers)
        {
            Tier[] tiers = new Tier[nTiers];
            this.Tiers = tiers;
            this.POF = new Pt2d(0.0, 0.0);
        }

        //Methods

        /// <summary>
        /// Return a jagged array of all tier points in this section
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Pt2d[][] GetSectionPts(Section s)
        {
            Pt2d[][] sectionPts = new Pt2d[s.Tiers.Length][];

            for (int i = 0; i < s.Tiers.Length; i++)
            {
                sectionPts[i] = s.Tiers[i].Points2d;
            }
            return sectionPts;
        }

        /// <summary>
        /// Return a jagged array of all spectator points in this section
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Pt2d[][] GetSpectatorPts(Section section, bool standing)
        {
            Pt2d[][] specPts = new Pt2d[section.Tiers.Length][];

            if (standing)
            {
                for (int i = 0; i < section.Tiers.Length; i++)
                {
                    Pt2d[] thisTierSpecPts = new Pt2d[section.Tiers[i].RowCount];
                    for (int j = 0; j < section.Tiers[i].RowCount; j++)
                    {
                        Pt2d specPt = section.Tiers[i].Spectators[j].Loc2dStanding;
                        thisTierSpecPts[j] = specPt;
                    }
                    specPts[i] = thisTierSpecPts;
                }
            }
            else
            {
                for (int i = 0; i < section.Tiers.Length; i++)
                {
                    Pt2d[] thisTierSpecPts = new Pt2d[section.Tiers[i].RowCount];
                    for (int j = 0; j < section.Tiers[i].RowCount; j++)
                    {
                        Pt2d specPt = section.Tiers[i].Spectators[j].Loc2d;
                        thisTierSpecPts[j] = specPt;
                    }
                    specPts[i] = thisTierSpecPts;
                }
            }

            return specPts;
        }

        /// <summary>
        /// Returns an array of Vec2d objects for each seated spectator sightline
        /// </summary>
        /// <param name="section"></param>
        /// <returns>Vec2d[][]</returns>
        public static Vec2d[][] GetSightlines(Section section, bool standing)
        {
            Vec2d[][] sightLines = new Vec2d[section.Tiers.Length][];
            
            if (standing)
            {
                for (int i = 0; i < section.Tiers.Length; i++)
                {
                    Vec2d[] thisTierSightLines = new Vec2d[section.Tiers[i].RowCount];
                    for (int j = 0; j < section.Tiers[i].RowCount; j++)
                    {
                        Vec2d sightLine = section.Tiers[i].Spectators[j].SightLineStanding;
                        thisTierSightLines[j] = sightLine;
                    }
                    sightLines[i] = thisTierSightLines;
                }
            }
            else
            {
                for (int i = 0; i < section.Tiers.Length; i++)
                {
                    Vec2d[] thisTierSightLines = new Vec2d[section.Tiers[i].RowCount];
                    for (int j = 0; j < section.Tiers[i].RowCount; j++)
                    {
                        Vec2d sightLine = section.Tiers[i].Spectators[j].SightLine;
                        thisTierSightLines[j] = sightLine;
                    }
                    sightLines[i] = thisTierSightLines;
                }
            }
            return sightLines;
        }

        /// <summary>
        /// Returns a list of 2d points that define a seating tier
        /// </summary>
        /// <param name="t"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        private static void CalcPts(Section section)
        {
            for (int t = 0; t < section.Tiers.Length; t++)
            {
                CalcTierPoints(section, section.Tiers[t]);
            }
        }

        /// <summary>
        /// Returns an array of Pt2d objects for a given tier within a section
        /// </summary>
        /// <param name="section"></param>
        /// <param name="tier"></param>
        /// <returns>Pt2d[]</returns>
        private static void CalcTierPoints(Section section, Tier tier)
        {
            if (tier.RefPtType == Tier.ReferencePtType.ByEndOfPrevTier)
            {
                //This tier's RefPt == Last point of previous tier
                tier.RefPt = section.Tiers[tier.SectionIndex - 1].Points2d[section.Tiers[tier.SectionIndex - 1].Points2dCount - 1];
            }
            CalcRowPoints(tier);
        }

        /// <summary>
        /// Calculates B and C points for a tier iterativly.
        /// </summary>
        /// <param name="tier"></param>
        private static void CalcRowPoints(Tier tier)
        {
            //Points increment
            int p = 0;

            //Calc optional Fascia Point
            if (tier.FasciaH != 0.0)
            {
                tier.Points2d[p] = (new Pt2d(tier.RefPt.X + tier.StartX, ((tier.RefPt.Y + tier.StartY) - tier.FasciaH)));
                p++;
            }

            //Calc first tier point (PtA)
            Pt2d prevPt = new Pt2d(tier.RefPt.X + tier.StartX, tier.RefPt.Y + tier.StartY);
            tier.Points2d[p] = prevPt;
            p++;

            //Calc riser points for each row iterativly | Pts(B) & Pts(C)
            for (int row = 0; row < (tier.RowCount - 1); row++)
            {
                //Get rear riser bottom point (PtB) for current row
                Pt2d currentPt = new Pt2d();
                currentPt.X = prevPt.X + (tier.RowWidths[row]);
                currentPt.Y = prevPt.Y;
                tier.Points2d[p] = currentPt;
                p++;

                //Instance a spectator for current row
                CalcRowSpectator(tier, currentPt, row);

                //Calc rear riser top point (PtC) for current row and add to list
                double n = RiserHeightFromCVal(tier, currentPt, row, false);
                currentPt.Y += n;
                tier.Points2d[p] = currentPt;
                p++;

                prevPt = currentPt;
            }

            //Add final tier point (PtD) to tier
            prevPt.X += (tier.RowWidths[tier.RowCount - 1]);
            tier.Points2d[p] = prevPt;
            CalcRowSpectator(tier, prevPt, tier.RowCount - 1);
        }

        /// <summary>
        /// Creates and adds a row's spectator to the tier. PtB argument should be rear lower riser point.
        /// </summary>
        /// <param name="tier"></param>
        /// <param name="pt"></param>
        private static void CalcRowSpectator(Tier tier, Pt2d ptB, int row)
        {
            double eyeX = tier.EyeX;
            double eyeY = tier.EyeY;
            double eyeXStanding = tier.SEyeX;
            double eyeYStanding = tier.SEyeY;

            if (tier.SuperHas && row == tier.SuperRow)
            {
                eyeX = tier.SuperEyeX;
                eyeY = tier.SuperEyeY;
                eyeXStanding = tier.SuperSEyeX;
                eyeYStanding = tier.SuperSEyeY;
            }

            Pt2d specPt = new Pt2d(ptB.X - eyeX, ptB.Y + eyeY);
            Pt2d specPtSt = new Pt2d(ptB.X - eyeXStanding, ptB.Y + eyeYStanding);
            Vec2d sLine = new Vec2d(specPt, tier.POF);
            Vec2d sLineSt = new Vec2d(specPtSt, tier.POF);
            Spectator spectator = new Spectator(tier.SectionIndex, row, specPt, specPtSt, tier.POF, sLine, sLineSt);
            tier.Spectators[row] = spectator;
        }

        /// <summary>
        /// Calculates the riser height based on minimum C-Value requirement using triangle proportionality
        /// </summary>
        /// <param name="tier"></param>
        /// <param name="ptB"></param>
        /// <param name="row"></param>
        /// <param name="standing"></param>
        /// <returns>double</returns>
        private static double RiserHeightFromCVal(Tier tier, Pt2d ptB, int currentRow, bool standing)
        {
            double currentRowEyeX = tier.EyeX;
            double currentRowEyeY = tier.EyeY;
            double nextRowEyeX = currentRowEyeX;
            double nextRowEyeY = currentRowEyeY;
            double maxRakeAngle = tier.MaxRakeAngle;

            if (tier.SuperHas)
            {
                if (currentRow + 1 == tier.SuperRow)
                {
                    currentRowEyeX = tier.SEyeX;
                    currentRowEyeY = tier.SEyeY;
                    nextRowEyeX = tier.SuperEyeX;
                    nextRowEyeY = tier.SuperEyeY;
                    //Unlimited riser height for super riser
                    maxRakeAngle = 1.57;
                }
                else if (currentRow == tier.SuperRow)
                {
                    currentRowEyeX = tier.SuperEyeX;
                    currentRowEyeY = tier.SuperEyeY;
                }
            }

            double t = (tier.RowWidths[currentRow + 1] + currentRowEyeX) - nextRowEyeX;
            double c = tier.MinimumC;
            double h = ptB.Y + currentRowEyeY;
            double d = (ptB.X - currentRowEyeX) + t;

            //Function of N, Triangle Proportionality Theroum 
            double r = ((c + h) / (d - t)) * (d);
            double n = (r - nextRowEyeY - ptB.Y);

            double nMax = (Tan(maxRakeAngle) * t);

            n = n > nMax ? nMax : n;

            double nR = n;
            return nR;
        }




    }
}
