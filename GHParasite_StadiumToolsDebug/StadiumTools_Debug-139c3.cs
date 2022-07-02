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

    //Set current Reference point to equal the POF
    Pt2d curRefPt = section.POF;

    for (int t = 0; t < section.Tiers.Length; t++)
    {
      //Calculate point count for Tier t to initialize sub-array
      int tierPtCount = ((section.Tiers[t].RowCount * 2));
      if (section.Tiers[t].FasciaH != 0.0)
      {
        tierPtCount += 1;
      }
      if (section.Tiers[t].SuperCurb != 0.0)
      {
        tierPtCount += 1;
      }

      sectionPoints[t] = new Pt2d[tierPtCount];
      int p = 0;
      //Add optional Fascia Point for current tier to point array
      if (section.Tiers[t].FasciaH != 0.0)
      {
        sectionPoints[t][p] = (new Pt2d(curRefPt.H + section.Tiers[t].StartH, ((curRefPt.V + section.Tiers[t].StartV) - section.Tiers[t].FasciaH)));
        p++;
      }

      //Add first row, first Point for current tier to point array
      Pt2d prevPt = new Pt2d(curRefPt.H + section.Tiers[t].StartH, curRefPt.V + section.Tiers[t].StartV);
      sectionPoints[t][p] = prevPt;
      p++;

      //Add riser points for each row in rowcount to point array
      for (int r = 0; r < (section.Tiers[t].RowCount - 1); r++)
      {
        //Get rear riser bottom point for current row and add to list
        Pt2d currentPt = new Pt2d();
        currentPt.H = prevPt.H + (section.Tiers[t].RowWidth[r]);
        currentPt.V = prevPt.V;
        sectionPoints[t][p] = currentPt;
        p++;

        //Generate a spectator for current row and add to list
        Pt2d specPt = new Pt2d(prevPt.H + section.Tiers[t].EyeH, prevPt.V + section.Tiers[t].EyeV);
        Pt2d specPtSt = new Pt2d(prevPt.H + section.Tiers[t].SEyeH, prevPt.V + section.Tiers[t].SEyeV);
        Vec2d sLine = new Vec2d(specPt, section.Tiers[t].POF);
        Vec2d sLineSt = new Vec2d(specPtSt, section.Tiers[t].POF);
        Spectator spectator = new Spectator(t, r, specPt, specPtSt, section.POF, sLine, sLineSt);

        //Get rear riser top point for current row and add to list
        currentPt.V += 0.37;
        sectionPoints[t][p] = currentPt;
        p++;

        //reset prevPt for next row iteration
        prevPt = currentPt;
      }

      //Add final tier point to tier
      prevPt.H += (section.Tiers[t].RowWidth[section.Tiers[t].RowCount - 1]);
      sectionPoints[t][p] = prevPt;
      //p++;
    }

    return sectionPoints;

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