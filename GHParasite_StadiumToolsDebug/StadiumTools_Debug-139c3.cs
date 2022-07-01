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
    tier1.StartH = 5.0;
    tier1.StartV = 1.0;
    tier1.RowWidth = 0.8;
    tier2.RowWidth = 0.8;
    tier3.RowWidth = 0.8;

    Tier[] tiers = new Tier[3] {tier1, tier2, tier3};

    Section section1 = new Section(tiers);

    bool success = false;

    List<Pt2d> tier1Pts = GetSectionPts(section1, out success);
    Point2d[] t1Pts = Pt2dToPoint2d(tier1Pts);

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
  public List<Pt2d> GetSectionPts(Section s, out bool success)
  {
    List<Pt2d> sectionPts = new List<Pt2d>();
    List<Spectator> spectators = new List<Spectator>();
    //Ensure first tier uses POF as reference point
    if (s.Tiers[0].RefPt != Tier.RefPtType.ByPOF)
    {
      s.Tiers[0].RefPt = Tier.RefPtType.ByPOF;
    }

    //Set current Reference point to equal the POF
    Pt2d refPt = s.POF;

    for (int t = 0; t < s.Tiers.Length; t++)
    {

      //Get optional Fascia Point for current tier
      if (s.Tiers[t].FasciaH != 0.0)
      {
        sectionPts.Add(new Pt2d(refPt.H + s.Tiers[t].StartH, ((refPt.V + s.Tiers[t].StartV) - s.Tiers[t].FasciaH)));
      }

      //Get first point for current tier
      Pt2d prevPt = new Pt2d(refPt.H + s.Tiers[t].StartH, refPt.V + s.Tiers[t].StartV);
      sectionPts.Add(prevPt);

      //Get point for each row in rowcount
      for (int r = 0; r < s.Tiers[t].RowCount; r++)
      {
        //Get rear riser bottom point for current row and add to list
        Pt2d currentPt = new Pt2d();
        currentPt.H = prevPt.H + (s.Tiers[t].RowWidth);
        currentPt.V = prevPt.V;
        sectionPts.Add(currentPt);

        //Generate a spectator for current row and add to list
        Pt2d specPt = new Pt2d(prevPt.H + s.Tiers[t].EyeH, prevPt.V + s.Tiers[t].EyeV);
        Pt2d specPtSt = new Pt2d(prevPt.H + s.Tiers[t].SEyeH, prevPt.V + s.Tiers[t].SEyeV);
        Vec2d sLine = new Vec2d(specPt, s.Tiers[t].POF);
        Vec2d sLineSt = new Vec2d(specPtSt, s.Tiers[t].POF);
        Spectator spectator = new Spectator(t, r, specPt, specPtSt, s.POF, sLine, sLineSt);

        //Get rear riser top point for current row and add to list
        currentPt.V += 0.37;
        sectionPts.Add(currentPt);

        //reset prevPt for next row iteration
        prevPt = currentPt;
      }
    }

    success = true;
    return sectionPts;
    
  }

  /// <summary>
  /// Casts a list of Pt2d objects to an array of RhinoCommon Point2d 
  /// </summary>
  /// <param name="pts"></param>
  /// <returns></returns>
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
  /// Casts a list of Vec2d objects to an array of RhinoCommon Vector2d
  /// </summary>
  /// <param name="vecs"></param>
  /// <returns></returns>
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