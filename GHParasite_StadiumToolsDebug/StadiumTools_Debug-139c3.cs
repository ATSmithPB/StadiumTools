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
        currentPt.H = prevPt.H + (s.Tiers[t].RowWidth * r + 1);
        currentPt.V = prevPt.V;
        sectionPts.Add(currentPt);

        //Get spectator eye point for current row and add to list
        Pt2d specPt = new Pt2d(prevPt.H + s.Tiers[t].EyeH, prevPt.V + s.Tiers[t].EyeV);
        
        Spectator spectator = new Spectator(t ,r + 1, specPt, s.POF, sLine)

        //Get rear riser bottom point for current row and add to list

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
  #endregion
}