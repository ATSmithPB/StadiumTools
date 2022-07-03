using System;
using System.Collections;
using System.Collections.Generic;

using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using StadiumTools;


/// <summary>
/// This class will be instantiated on demand by the Script component.
/// </summary>
public abstract class Script_Instance_139c3 : GH_ScriptInstance
{
  #region Utility functions
  /// <summary>Print a String to the [Out] Parameter of the Script component.</summary>
  /// <param name="text">String to print.</param>
  private void Print(string text) { /* Implementation hidden. */ }
  /// <summary>Print a formatted String to the [Out] Parameter of the Script component.</summary>
  /// <param name="format">String format.</param>
  /// <param name="args">Formatting parameters.</param>
  private void Print(string format, params object[] args) { /* Implementation hidden. */ }
  /// <summary>Print useful information about an object instance to the [Out] Parameter of the Script component. </summary>
  /// <param name="obj">Object instance to parse.</param>
  private void Reflect(object obj) { /* Implementation hidden. */ }
  /// <summary>Print the signatures of all the overloads of a specific method to the [Out] Parameter of the Script component. </summary>
  /// <param name="obj">Object instance to parse.</param>
  private void Reflect(object obj, string method_name) { /* Implementation hidden. */ }
  #endregion

  #region Members
  /// <summary>Gets the current Rhino document.</summary>
  private readonly RhinoDoc RhinoDocument;
  /// <summary>Gets the Grasshopper document that owns this script.</summary>
  private readonly GH_Document GrasshopperDocument;
  /// <summary>Gets the Grasshopper script component that owns this script.</summary>
  private readonly IGH_Component Component;
  /// <summary>
  /// Gets the current iteration count. The first call to RunScript() is associated with Iteration==0.
  /// Any subsequent call within the same solution will increment the Iteration count.
  /// </summary>
  private readonly int Iteration;
  #endregion
  /// <summary>
  /// This procedure contains the user code. Input parameters are provided as regular arguments,
  /// Output parameters as ref arguments. You don't have to assign output parameters,
  /// they will have a default value.
  /// </summary>
  #region Runscript
  private void RunScript(ref object pline, ref object s)
  {
    Tier tier1 = new Tier();
    Tier tier2 = new Tier();
    Tier tier3 = new Tier();
    tier1.SuperCurb = 0.0;
    tier2.SuperCurb = 0.0;
    tier3.SuperCurb = 0.0;
    tier1.FasciaH = 1.0;
    tier2.FasciaH = 1.0;
    tier3.FasciaH = 1.0;
    tier2.RefPtType = Tier.ReferencePtType.ByEndOfPrevTier;
    tier3.RefPtType = Tier.ReferencePtType.ByEndOfPrevTier;
    tier2.StartH = -2.0;
    tier3.StartH = -2.0;
    tier2.StartV = 4.0;
    tier3.StartV = 4.0;

    Tier[] tiers = new Tier[3] {tier1, tier2, tier3};

    Section section1 = new Section(tiers);

    bool success = true;

    Pt2d[][] tier1Pts = GetSectionPts(section1);
    DataTree<Point2d> t1Pts = Pt2dToPoint2d(tier1Pts);

    //Outputs
    pline = t1Pts;
    s = success;
  }
  #endregion
  #region Additional

  /// <summary>
  /// Returns a list of 2d points that define a seating tier
  /// </summary>
  /// <param name="t"></param>
  /// <param name="success"></param>
  /// <returns></returns>
  public Pt2d[][] GetSectionPts(Section section)
  {
    //Initialize jagged array to store section points
    int tierCount = section.Tiers.Length;
    Pt2d[][] sectionPoints = new Pt2d[tierCount][];

    //Set Tier Reference point
    for (int i = 0; i < section.Tiers.Length; i++)
    {
      sectionPoints[i] = GetTierPoints(section, section.Tiers[i]);
    }

    return sectionPoints;
  }

  public Pt2d[] GetTierPoints(Section section, Tier tier)
  {
    //Tier points increment
    int p = 0;
    
    //Set Reference Point
    Pt2d RefPt = tier.POF;
    if (tier.RefPtType == Tier.ReferencePtType.ByEndOfPrevTier)
    {
      RefPt = section.Tiers[tier.SectionIndex - 1].endPt;
    }

    //Calculate point count Tier to initialize sub-array
    int tierPtCount = ((tier.RowCount * 2));
    if (tier.FasciaH != 0.0)
    {
      tierPtCount += 1;
    }
    if (tier.SuperCurb != 0.0)
    {
      tierPtCount += 1;
    }

    Pt2d[] tierPts = new Pt2d[tierPtCount];
    //Add optional Fascia Point to point array
    if (tier.FasciaH != 0.0)
    {
      tierPts[p] = (new Pt2d(RefPt.H + tier.StartH, ((RefPt.V + tier.StartV) - tier.FasciaH)));
      p++;
    }

    //Add first row, first Point to point array
    Pt2d prevPt = new Pt2d(RefPt.H + tier.StartH, RefPt.V + tier.StartV);
    tierPts[p] = prevPt;
    p++;

    //Add riser points for each row in rowcount to point array
    for (int r = 0; r < (tier.RowCount - 1); r++)
    {
      //Get rear riser bottom point for current row and add to list
      Pt2d currentPt = new Pt2d();
      currentPt.H = prevPt.H + (tier.RowWidth[r]);
      currentPt.V = prevPt.V;
      tierPts[p] = currentPt;
      p++;

      //Generate a spectator for current row and add to list
      Pt2d specPt = new Pt2d(prevPt.H + tier.EyeH, prevPt.V + tier.EyeV);
      Pt2d specPtSt = new Pt2d(prevPt.H + tier.SEyeH, prevPt.V + tier.SEyeV);
      Vec2d sLine = new Vec2d(specPt, tier.POF);
      Vec2d sLineSt = new Vec2d(specPtSt, tier.POF);
      Spectator spectator = new Spectator(tier.SectionIndex, r, specPt, specPtSt, section.POF, sLine, sLineSt);

      //Get rear riser top point for current row and add to list
      currentPt.V += 0.37;
      tierPts[p] = currentPt;
      p++;

      //reset prevPt for next row iteration
      prevPt = currentPt;
    }

    //Add final tier point to tier
    prevPt.H += (tier.RowWidth[tier.RowCount - 1]);
    tierPts[p] = prevPt;
    tier.endPt = prevPt;

    return tierPts;
  }
  


  /// <summary>
  /// Casts a list of Pt2d objects into an array of RhinoCommon Point2d
  /// </summary>
  /// <param name="pts"></param>
  /// <returns>Point2d[]</returns>
  public Point2d[] Pt2dToPoint2d(List<Pt2d> pts)
  {
    Point2d[] rcPts = new Point2d[pts.Count];

    for (int i = 0; i < pts.Count; i++)
    {
      rcPts[i] = new Point2d(pts[i].H, pts[i].V);
    }

    return rcPts;
  }

  /// <summary>
  /// Casts a jagged array of Pt2d objects into a data tree of RhinoCommon Point2d
  /// </summary>
  /// <param name="pts"></param>
  /// <returns>DataTree<Point2d></Point2d></returns>
  public DataTree<Point2d> Pt2dToPoint2d(Pt2d[][] pts)
  {
    DataTree<Point2d> rcPts = new DataTree<Point2d>();
    for (int i = 0; i < pts.Length; i++)
    {
      for (int j = 0; j < pts[i].Length; j++)
      {
        GH_Path path = new GH_Path(i);
        Point2d item = new Point2d(pts[i][j].H, pts[i][j].V);
        rcPts.Add(item, path);
      }
    }
    return rcPts;
  }

  /// <summary>
  /// Casts a list of Vec2d objects to an array of RhinoCommon Vector2d
  /// </summary>
  /// <param name="vecs"></param>
  /// <returns>Vector2d[]</returns>
  public Vector2d[] Vec2dToVector2d(List<Vec2d> vecs)
  {
    Vector2d[] rcVecs = new Vector2d[vecs.Count];

    for (int i = 0; i < vecs.Count; i++)
    {
      rcVecs[i] = new Vector2d(vecs[i].H, vecs[i].V);
    }

    return rcVecs;
  }
  #endregion
}