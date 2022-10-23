using Rhino.Collections;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public struct Pline : ICurve
    {
        //Properties
        public Pt3d[] Points { get; set; }
        public Pt3d Start { get; set; }
        public Pt3d End { get; set; }
        public bool Closed { get; set; }
        
        //Constructors
        public Pline(Pt3d[] pts)
        {
            Points = pts;
            Start = pts[0];
            End = pts[pts.Length - 1];
            Closed = Pt3d.IsCoincident(Start, End, UnitHandler.abstol);
        }

        public Pline(List<Pt3d> pts)
        {
            Points = pts.ToArray();
            Start = pts[0];
            End = pts[pts.Count - 1];
            Closed = Pt3d.IsCoincident(Start, End, UnitHandler.abstol);
        }

        //Methods
        /// <summary>
        /// returns a polyline approximation of an arc based on a given segment length, and pointAtMiddle modifier
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="divLength"></param>
        /// <param name="pointAtMiddle"></param>
        /// <returns>Pline</returns>
        public static Pline FromArc(Arc arc, double divLength, bool pointAtMiddle)
        {
            List<Pt3d> pts = new List<Pt3d>();
            pts.Add(arc.Start);
            pts.AddRange(Arc.DivideLinearCentered(arc, divLength, pointAtMiddle));
            pts.Add(arc.End);
            Pline result = new Pline(pts);
            return result;
        }

        /// <summary>
        /// returns a polyline approximation of an arc based on a given segment count
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="segmentCount"></param>
        /// <returns>Pline</returns>
        public static Pline FromArc(Arc arc, int segmentCount)
        {
            List<Pt3d> pts = new List<Pt3d>();
            pts.Add(arc.Start);
            pts.AddRange(Arc.Divide(arc, segmentCount, out double[] tParams));
            pts.Add(arc.End);
            Pline result = new Pline(pts);
            return result;
        }

        /// <summary>
        /// returns a polyline along a line. Method: 0 = remainder@end. 1 = gap@center. 2 = point@center
        /// </summary>
        /// <param name="line"></param>
        /// <param name="divLength"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Pline FromLine(Line line, double divLength, int method)
        {
            List<Pt3d> pts = new List<Pt3d>();
            pts.Add(line.Start);
            pts.AddRange(Line.DivideLength(line, divLength, method));
            pts.Add(line.End);
            Pline result = new Pline(pts);
            return result;
        }

        public static Pline FromEllipse(Ellipse ellipse, int segmentCount)
        {
            List<Pt3d> pts = new List<Pt3d>();
            pts.Add(ellipse.Start);
            pts.AddRange(Ellipse.Divide(ellipse, segmentCount));
            pts.Add(ellipse.End);
            Pline result = new Pline(pts);
            return result;
        }

        /// <summary>
        /// returns a collection representing the length of each polyline segment
        /// </summary>
        /// <returns>double[]</returns>
        public double[] SegmentLengths()
        {
            double[] segLengths = new double[this.Points.Length - 1];
            for (int i = 0; i < this.Points.Length - 2; i++)
            {
                segLengths[i] = this.Points[i].DistanceTo(this.Points[i + 1]);
            }
            return segLengths;
        }

        /// <summary>
        /// returns the midpoint of a polyline
        /// </summary>
        /// <returns>Pt3d</returns>
        public Pt3d Midpoint()
        {
            return PointAt(0.5);
        }

        /// <summary>
        /// returns a 3d point at  specified parameter on a polyline
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Pt3d</returns>
        public Pt3d PointAt(double parameter)
        {
            double[] segLengths = this.SegmentLengths();
            double length = segLengths.Sum();
            double paramLength = length * parameter;
            int i = 0;
            double curLen = 0.0;
            while (curLen < paramLength)
            {
                curLen += segLengths[i];
                i++;
            }
            double remainder = paramLength - curLen;
            double tParam = remainder / segLengths[i];
            return Pt3d.Tween2(this.Points[i], this.Points[i + 1], tParam);
        }

        /// <summary>
        /// returns the total length of the polyline
        /// </summary>
        /// <returns>double</returns>
        public double Length()
        {
            double[] segLengths = this.SegmentLengths();
            return segLengths.Sum();
        }

        public bool Offset(double distance, out ICurve offsetCurve)
        {
            //need to add this method ! ! !
            throw new Exception("This method doesnt exist yet!");
            offsetCurve = null;
            return false;
        }

        public Pln3d[] PlanesPerp()
        {
            return Pln3d.PerpPlanes(this.Points);
        }

        /// <summary>
        /// tries to join a series of polylines into one if uniform direction and end-to-end connection 
        /// </summary>
        /// <param name="plines"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool Join(Pline[] plines, out Pline result)
        {
            bool isSuccess = true;
            double tol = UnitHandler.abstol;
            List<Pt3d> pts = new List<Pt3d>();
            for (int i = 0; i < plines.Length - 1; i++)
            {
                if (Pt3d.IsCoincident(plines[i].End, plines[i + 1].Start, tol))
                {
                    pts.AddRange(plines[i].Points);
                    pts.RemoveAt(pts.Count - 1);
                }
                else
                {
                    isSuccess = false;
                    throw new ArgumentException($"Error: Join failed. pline[{i}] & pline[{i + 1}] are [{Pt3d.Distance(plines[i].End, plines[i + 1].Start)}] units apart");
                    //break;
                }
            }

            if (isSuccess)
            {
                pts.AddRange(plines[plines.Length - 1].Points);
                result = new Pline(pts);
            }
            else
            {
                result = new Pline();
            }
            return isSuccess;
        }

        public Pline SubDivide()
        {
            int resultCount = (this.Points.Length * 2) - 1;
            Pt3d[] resultPts = new Pt3d[resultCount];
            int j = 0;
            for (int i = 0; i < this.Points.Length - 1; i++)
            {
                resultPts[j] = this.Points[i];
                j++;
                resultPts[j] = Pt3d.Midpoint(this.Points[i], this.Points[i + 1]);
                j++;
            }
            resultPts[j] = this.Points[Points.Length - 1];

            return new Pline(resultPts);
        }

        /// <summary>
        /// calculates the perpendicular plane (Avg method) at a given index of a polyline
        /// </summary>
        /// <param name="index"></param>
        /// <param name="cPlane"></param>
        /// <returns>Pln3d</returns>
        /// <exception cref="ArgumentException"></exception>
        public Pln3d AvgPlaneAt(int index, Pln3d cPlane)
        {
            if (this.Points.Length < 2)
            {
                throw new ArgumentException("pts.Length must be >1 to calculate avg plane");
            }
            Vec3d previous = new Vec3d();
            Vec3d next = new Vec3d(this.Points[index], this.Points[index + 1]);
            if (index == 0)
            {
                if (this.Closed)
                {
                    previous = new Vec3d(this.Points[this.Points.Length - 2], this.Points[this.Points.Length - 1]);
                }
                else
                {
                    previous = next;
                }
            }
            Vec3d zAxis = Vec3d.Tween2(previous, next).Flip();
            Pt3d ptOnY = this.Points[index] + cPlane.Zaxis;
            Pln3d result = new Pln3d(this.Points[index], zAxis, ptOnY);
            return result;
        }

        public Vec3d AvgPerpAt(int index, Pln3d cPlane)
        {
            if (this.Points.Length < 2)
            {
                throw new ArgumentException("pts.Length must be >1 to calculate avg plane");
            }
            Vec3d previous = new Vec3d();
            Vec3d next = new Vec3d(this.Points[index], this.Points[index + 1]);
            if (index == 0)
            {
                if (this.Closed)
                {
                    previous = new Vec3d(this.Points[this.Points.Length - 2], this.Points[this.Points.Length - 1]);
                }
                else
                {
                    previous = next;
                }
            }
            Vec3d zAxis = Vec3d.Tween2(previous, next);
            Vec3d result = Vec3d.CrossProduct(zAxis, cPlane.Zaxis);
            return result;
        }

        public bool Intersect(Pt3d vecOrigin, Vec3d vector, out double distance)
        {
            double closest = double.MaxValue;
            for (int i = 0; i < this.Points.Length - 1; i++)
            {
                bool xSuccess = Vec3d.Intersect(vecOrigin, vector, this.Points[i], this.Points[i + 1], out double dist);
                if (xSuccess && dist < closest)
                {
                    closest = dist;
                }
            }
            distance = closest;
            if (closest < double.MaxValue)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        
    }
}
