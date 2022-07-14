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
        /// The Point of Focus for spectators in this section.
        /// </summary>
        public Pt2d POF { get; set; }
        /// <summary>
        /// The 3d plane of the section.
        /// </summary>
        public Pln3d Plane3d { get; set; }

        //Constructors
        /// <summary>
        /// Construct an empty section object 
        /// </summary>
        public Section()
        {

        }
        
        /// <summary>
        /// Construct a Section from a list of tiers 
        /// </summary>
        /// <param name="tiers"></param>
        public Section(List<Tier> tierList)
        {
            Tier[] tiers = tierList.ToArray();
            this.Tiers = tiers;
            Initialize();
        }

        /// <summary>
        /// Construct a Section from an array of tiers
        /// </summary>
        /// <param name="tiers"></param>
        public Section(Tier[] tiers)
        {
            this.Tiers = tiers;
            Initialize();
        }

        //Methods
   
        /// <summary>
        /// Initialize the section
        /// </summary>
        private void Initialize()
        {
            this.POF = new Pt2d(0.0, 0.0);

            //Force first tier to use Point of Focus as reference point
            this.Tiers[0].BuildFromPreviousTier = false;
            
            //Apply the Section POF to all contained tiers 
            for (int i = 0; i < this.Tiers.Length; i++)
            {
                this.Tiers[i].SectionIndex = i; 
                this.Tiers[i].POF = this.POF;
            }

            //Calculate surface and spectator points for all tiers in the section
            CalcPts(this);
        }

        /// <summary>
        /// Calculates 2d points that define a seating tier surface and it's spectators
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
        private static void CalcTierPoints(Section section, Tier currentTier)
        {
            if (currentTier.BuildFromPreviousTier)
            { 
                //set current tier's RefPt to be last point of previous tier
                Tier lastTier = section.Tiers[currentTier.SectionIndex - 1];
                int lastPtCount = lastTier.Points2d.Length;
                Pt2d lastPt = lastTier.Points2d[lastPtCount - 1];
                currentTier.RefPt = lastPt;
            }
            CalcRowPoints(currentTier);
        }

        /// <summary>
        /// Calculates points for a tier iterativly, satisfying its minimum C-value (unless max rake angle is reached) 
        /// </summary>
        /// <param name="tier"></param>
        private static void CalcRowPoints(Tier tier)
        {
            //Points increment
            int p = 0;

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
                AddRowSpectator(tier, currentPt, row);

                if (tier.SuperHas)
                {
                    if (row + 1 == tier.SuperRow)
                    {
                        if (tier.SuperCurbWidth > 0.0)
                        {
                            if (tier.SuperCurbHeight > 0.0)
                            {
                                currentPt.Y += tier.SuperCurbHeight;
                                tier.Points2d[p] = currentPt;
                                p++;
                            }
                            currentPt.X += tier.SuperCurbWidth;
                            tier.Points2d[p] = currentPt;
                            p++;
                        }
                    }
                    if (row == tier.SuperRow && row == tier.RowCount - 1)
                    {
                        break;
                    }
                }

                //Calc rear riser top point (PtC) for current row and add to list
                double n = RiserHeightFromCVal(tier, currentPt, row, false);
                currentPt.Y += n;
                tier.Points2d[p] = currentPt;
                p++;

                if (tier.SuperHas)
                {
                    if (row == tier.SuperRow)
                    {
                        currentPt.X += tier.SuperGuardrailWidth;
                        tier.Points2d[p] = currentPt;
                        p++;

                        currentPt.Y -= 0.25;
                        tier.Points2d[p] = currentPt;
                        p++;
                    }
                }

                prevPt = currentPt;
            }

            //Add final tier point (PtD) to tier
            prevPt.X += (tier.RowWidths[tier.RowCount - 1]);
            tier.Points2d[p] = prevPt;
            AddRowSpectator(tier, prevPt, tier.RowCount - 1);
        }

        /// <summary>
        /// Adds a spectator object to current row. PtB argument should be rear lower riser point.
        /// </summary>
        /// <param name="tier"></param>
        /// <param name="pt"></param>
        private static void AddRowSpectator(Tier tier, Pt2d ptB, int row)
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
            Pt2d forwardSpec;

            if (row == 0)
            {
                forwardSpec = tier.Points2d[0];
            }
            else
            {
                forwardSpec = tier.Spectators[row - 1].Loc2d;
            }

            Spectator spectator = new Spectator(tier.SectionIndex, row, specPt, specPtSt, tier.POF, sLine, sLineSt, forwardSpec);
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
            double n = 0.0;
            double currentRowEyeX = tier.EyeX;
            double currentRowEyeY = tier.EyeY;
            double nextRowEyeX = currentRowEyeX;
            double nextRowEyeY = currentRowEyeY;
            double nextRowWidth = tier.RowWidths[currentRow + 1];

            if (tier.SuperHas)
            {
                if (currentRow + 1 == tier.SuperRow)
                {
                    n -= tier.SuperCurbHeight;
                    ptB.X -= tier.SuperCurbWidth;
                    ptB.Y -= tier.SuperCurbHeight;
                    currentRowEyeX = tier.SEyeX;
                    currentRowEyeY = tier.SEyeY;
                    nextRowEyeX = tier.SuperEyeX - tier.SuperCurbWidth;
                    nextRowEyeY = tier.SuperEyeY;
                }
                else if (currentRow == tier.SuperRow)
                {
                    nextRowWidth += tier.SuperGuardrailWidth;
                    currentRowEyeX = tier.SuperEyeX;
                    currentRowEyeY = tier.SuperEyeY;
                    n += 0.25;
                }
            }

            double t = (nextRowWidth + currentRowEyeX) - nextRowEyeX;
            double c = tier.MinimumC;
            double h = ptB.Y + currentRowEyeY;
            double d = (ptB.X - currentRowEyeX) + t;

            //Function of N, Triangle Proportionality Theroum 
            double r = ((c + h) / (d - t)) * (d);
            n += (r - nextRowEyeY - ptB.Y);

            if (currentRow + 1 != tier.SuperRow && currentRow != tier.SuperRow)
            {
                double nMax = (Tan(tier.MaxRakeAngle) * t);
                n = n > nMax ? nMax : n;
            }

            double nR = n;
            return nR;
        }

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

        ///
        public static double[][] GetCValues(Section section, bool standing)
        {
            double[][] cValues = new double[section.Tiers.Length][];

            if (standing)
            {
                for (int i = 0; i < section.Tiers.Length; i++)
                {
                    double[] thisTierCValues = new double[section.Tiers[i].RowCount];
                    for (int j = 0; j < section.Tiers[i].RowCount; j++)
                    {
                        double specC = section.Tiers[i].Spectators[j].Cvalue;
                        thisTierCValues[j] = specC;
                    }
                    cValues[i] = thisTierCValues;
                }
            }
            else
            {
                for (int i = 0; i < section.Tiers.Length; i++)
                {
                    double[] thisTierCValues = new double[section.Tiers[i].RowCount];
                    for (int j = 0; j < section.Tiers[i].RowCount; j++)
                    {
                        double specC = section.Tiers[i].Spectators[j].Cvalue;
                        thisTierCValues[j] = specC;
                    }
                    cValues[i] = thisTierCValues;
                }
            }

            return cValues;
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



    }
}
