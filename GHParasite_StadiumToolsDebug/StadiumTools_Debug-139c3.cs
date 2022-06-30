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

    List<Pt2d> tier1Pts = GetTierPts(tier1, out success);
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
  public List<Pt2d> GetTierPts(Tier t, out bool success)
  {
    List<Pt2d> pts = new List<Pt2d>();

    if (t.RefPt == Tier.RefPtType.ByPOF)
    {
      pts.Add(new Pt2d(t.StartH, t.StartV));
    }

    success = true;
    return pts;
    
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