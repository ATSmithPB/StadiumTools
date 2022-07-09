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
  private void RunScript(double x, double y, double z, ref object sectionPoints, ref object specPoints, ref object specSightLines, ref object specPointsStanding, ref object specSightLinesStanding, ref object A)
  {
    Tier tier1 = new Tier();
    Tier tier2 = new Tier();
    Tier tier3 = new Tier();
    tier2.RefPtType = Tier.ReferencePtType.ByEndOfPrevTier;
    tier3.RefPtType = Tier.ReferencePtType.ByEndOfPrevTier;
    tier1.StartX = x;
    tier1.StartY = -0.2;
    tier2.StartX = -2.0;
    tier3.StartX = -3.0;
    tier2.StartY = 2.0;
    tier3.StartY = 2.0;

    Tier[] tiers = new Tier[3] { tier1, tier2, tier3 };

    Section section1 = new Section(tiers);
    //Section Points
    Pt2d[][] section1Pt2d = Section.GetSectionPts(section1);
    DataTree<Point2d> section1Pts = Pt2dToPoint2d(section1Pt2d);
    //Section Spectators Seated
    Pt2d[][] section1Specs = Section.GetSpectatorPts(section1, false);
    DataTree<Point2d> specPts = Pt2dToPoint2d(section1Specs);
    //Section Sightlines Seated
    Vec2d[][] section1Vec2d = Section.GetSightlines(section1, false);
    DataTree<Vector2d> sightLines = Vec2dToVector2d(section1Vec2d);
    //Section Spectators Standing
    Pt2d[][] section1SpecsSt = Section.GetSpectatorPts(section1, true);
    DataTree<Point2d> specPtsSt = Pt2dToPoint2d(section1SpecsSt);
    //Section Sightlines Standing
    Vec2d[][] section1Vecs2dSt = Section.GetSightlines(section1, true);
    DataTree<Vector2d> sightLinesSt = Vec2dToVector2d(section1Vecs2dSt);

    //Outputs
    sectionPoints = section1Pts;
    specPoints = specPts;
    specSightLines = sightLines;
    specPointsStanding = specPtsSt;
    specSightLinesStanding = sightLinesSt;
    A = true;
  }
  #endregion
  #region Additional

  /// <summary>
  /// Casts a list of Vec2d objects to an array of RhinoCommon Vector2d
  /// </summary>
  /// <param name="vecs"></param>
  /// <returns>Vector2d[]</returns>
  public Vector2d[] Vec2dToVector2d(Vec2d[] vecs)
  {
    Vector2d[] rcVecs = new Vector2d[vecs.Length];

    for (int i = 0; i < vecs.Length; i++)
    {
      rcVecs[i] = new Vector2d(vecs[i].X, vecs[i].Y);
    }

    return rcVecs;
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
        Point2d item = new Point2d(pts[i][j].X, pts[i][j].Y);
        rcPts.Add(item, path);
      }
    }
    return rcPts;
  }

  /// <summary>
  /// Casts a jagged array of Vec2d objects into a data tree of RhinoCommon Vector2d
  /// </summary>
  /// <param name="pts"></param>
  /// <returns>DataTree<Point2d></Point2d></returns>
  public DataTree<Vector2d> Vec2dToVector2d(Vec2d[][] vecs)
  {
    DataTree<Vector2d> rcVecs = new DataTree<Vector2d>();
    for (int i = 0; i < vecs.Length; i++)
    {
      for (int j = 0; j < vecs[i].Length; j++)
      {
        GH_Path path = new GH_Path(i);
        Vector2d item = new Vector2d(vecs[i][j].X, vecs[i][j].Y);
        rcVecs.Add(item, path);
      }
    }
    return rcVecs;
  }
  #endregion
}