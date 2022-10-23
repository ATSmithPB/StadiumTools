using System;
using System.Collections.Generic;
using static Rhino.Render.TextureGraphInfo;
using static System.Math;

namespace StadiumTools
{
    public struct Vec3d
    {
        //Properties
        /// <summary>
        /// X component of vec.
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y component of vec.
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Z component of a vec 
        /// </summary>
        public double Z { get; set; }
        /// <summary>
        /// Magnitude of the 3d vec
        /// </summary>
        public double M { get; set; }

        //Constructors
        /// <summary>
        /// Constructs a Vec3d object from 3 its three vC (x,y,z)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vec3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.M = Magnitude(x, y, z);
        }

        /// <summary>
        /// Constructs a Vec3d object from a Vec2d object and a z component
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z"></param>
        public Vec3d(Vec2d v, double z)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = z;
            this.M = Magnitude(v.X, v.Y, z);
        }

        /// <summary>
        /// Constructs a Vec3d with the same XYZ vC as a Pt3d
        /// </summary>
        /// <param name="pt3d"></param>
        public Vec3d(Pt3d pt3d)
        {
            this.X = pt3d.X;
            this.Y = pt3d.Y;
            this.Z = pt3d.Z;
            this.M = Magnitude(pt3d.X, pt3d.Y, pt3d.Z);
        }

        public Vec3d(Pt3d start, Pt3d end)
        {
            this.X = end.X - start.X;
            this.Y = end.Y - start.Y;
            this.Z = end.Z - start.Z;
            this.M = Magnitude(this.X, this.Y, this.Z);
        }

        //Delegates
        /// <summary>
        /// Gets Vector with Default XAxis vC (1.0, 0.0, 0.0)
        /// </summary>
        public static Vec3d XAxis => new Vec3d(1.0, 0.0, 0.0);
        /// <summary>
        /// Gets Vector with Default XAxis vC (0.0, 1.0, 0.0)
        /// </summary>
        public static Vec3d YAxis => new Vec3d(0.0, 1.0, 0.0);
        
        /// <summary>
        /// Gets Vector with Default XAxis vC (0.0, 0.0, 1.0)
        /// </summary>
        public static Vec3d ZAxis => new Vec3d(0.0, 0.0, 1.0);

        //Operator Overloads
        /// <summary>
        /// * returns dot product
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double operator * (Vec3d a, Vec3d b)
        {
            return DotProduct(a, b);
        }

        public static Vec3d operator * (double f, Vec3d v)
        {
            v.Scale(f);
            return v;
        }

        public static Vec3d operator + (Vec3d a, Vec3d b)
        {
            return Add(a, b);
        }
        
        public static Vec3d operator - (Vec3d a, Vec3d b)
        {
            return Subtract(a, b);
        }

        //Methods
        /// <summary>
        /// Returns the magnitude (absolute length) of of a vec given its three vC.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns>double</returns>
        public static double Magnitude(double a, double b, double c)
        {
            double x = Abs(Sqrt((a * a) + (b * b) + (c * c)));
            return x;
        }

        /// <summary>
        /// Unitizes an existing 3d vec by reference
        /// </summary>
        /// <param name="v"></param>
        public static void Normalize(ref Vec3d v)
        {
            v.X = v.X / v.M;
            v.Y = v.Y / v.M;
            v.Z = v.Z / v.M;
            v.M = 1;
        }

        /// <summary>
        /// returns true if a vec is successfully Normalized
        /// </summary>
        /// <returns></returns>
        public bool Unitize()
        {
            this.Normalize();
            return true; 
        }

        /// <summary>
        /// Returns a new normalized version of a 3d vec 
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Vec2d</returns>
        public static Vec3d Normalize(Vec3d v)
        {
            Vec3d vN = new Vec3d();
            vN.X = v.X / v.M;
            vN.Y = v.Y / v.M;
            vN.Z = v.Z / v.M;
            vN.M = 1.0;
            return vN;
        }

        public Vec3d Flip()
        {
            return new Vec3d(-this.X, -this.Y, -this.Z);
        }

        /// <summary>
        /// Scale all vC of a vec such that its magnitude is equal to 1
        /// </summary>
        public void Normalize()
        {
            this.X = this.X / this.M;
            this.Y = this.Y / this.M;
            this.Z = this.Z / this.M;
            this.M = 1.0;
        }

        /// <summary>
        /// Uniformly scale all three components of a vec
        /// </summary>
        /// <param name="f"></param>
        public void Scale(double f)
        {
            this.X *= f;
            this.Y *= f;
            this.Z *= f;
            this.M *= Math.Abs(f);
        }

        /// <summary>
        /// Returns the numeric dot product of two vectors
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double DotProduct(Vec3d u, Vec3d v)
        {
            return (u.X * v.X) + (u.Y * v.Y) + (u.Z * v.Z);
        }

        /// <summary>
        /// returns the sum of two vectors 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vec3d Add(Vec3d a, Vec3d b)
        {
            Vec3d result = new Vec3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
            result.M = a.M + b.M;
            return result;
        }

        /// <summary>
        /// returns the result of subtracting one vec from another
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Vec3d</returns>
        public static Vec3d Subtract(Vec3d a, Vec3d b)
        {
            Vec3d result = new Vec3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
            result.M = a.M - b.M;
            return result;
        }

        /// <summary>
        /// returns a new Vec2d with matching X and Y vC as a Vec3d 
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Vec2d</returns>
        public static Vec2d ToVec2d(Vec3d v)
        {
            return new Vec2d(v);
        }

        /// <summary>
        /// returns the angle between two 3d vectors in radians
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Vec3d</returns>
        public static double Angle(Vec3d a, Vec3d b)
        {
            Vec3d aN = Normalize(a);
            Vec3d bN = Normalize(b);

            double d = (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
            if (d > 1.0)
                d = 1.0;
            if (d < -1.0)
                d = -1.0;
            return Acos(d);
        }

        /// <summary>
        /// returns the reflex angle of two 3d vectors in radians
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>double</returns>
        public static double Reflex(Vec3d a, Vec3d b)
        {
            return (2 * PI) - Angle(a, b);
        }

        /// <summary>
        /// returns the perpendicular vec to a given vec
        /// </summary>
        /// <param name="v"></param>
        /// <returns>Vec3d</returns>
        public static Vec3d PerpTo(Vec3d v)
        {
            double[] this_v = new double[3];
            int i, j, k; 
            double a, b;
            k = 2;
            if (Math.Abs(v.Y) > Math.Abs(v.X))
            {
                if (Math.Abs(v.Z) > Math.Abs(v.Y)) 
                {
                    i = 2;
                    j = 1;
                    k = 0;
                    a = v.Z;
                    b = -v.Y;
                }
                else if (Math.Abs(v.Z) >= Math.Abs(v.X))
                {
                    i = 1;
                    j = 2;
                    k = 0;
                    a = v.Y;
                    b = -v.Z;
                }
                else 
                {
                    i = 1;
                    j = 0;
                    k = 2;
                    a = v.Y;
                    b = -v.X;
                }
            }
            else if (Math.Abs(v.Z) > Math.Abs(v.X)) 
            {
                i = 2;
                j = 0;
                k = 1;
                a = v.Z;
                b = -v.X;
            }
            else if (Math.Abs(v.Z) > Math.Abs(v.Y)) 
            {
                i = 0;
                j = 2;
                k = 1;
                a = v.X;
                b = -v.Z;
            }
            else 
            {
                i = 0;
                j = 1;
                k = 2;
                a = v.X;
                b = -v.Y;
            }

            this_v[i] = b;
            this_v[j] = a;
            this_v[k] = 0.0;
          
            return new Vec3d(this_v[0], this_v[1], this_v[2]);
        }

        /// <summary>
        /// returns the Vec3d cross product vec of two 2d vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Vec3d</returns>
        public static Vec3d CrossProduct(Vec2d a, Vec2d b)
        {
            return new Vec3d(0.0, 0.0, (a.X * b.Y) - (b.X * a.Y));
        } 
         
        /// <summary>
        /// returns the Vec3d cross product vec of two 3d vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Vec3d</returns>
        public static Vec3d CrossProduct(Vec3d a, Vec3d b)
        {
            return new Vec3d((a.Y * b.Z) - (b.Y * a.Z), 
                             (a.Z * b.X) - (b.Z * a.X), 
                             (a.X * b.Y) - (b.X * a.Y));
        }

        public static Vec3d CrossProduct(Vec3d a, Vec3d b, out double cross)
        {
            double x = (a.Y * b.Z) - (b.Y * a.Z);
            double y = (a.Z * b.X) - (b.Z * a.X);
            double z = (a.X * b.Y) - (b.X * a.Y);
            cross = x - y + z;
            return new Vec3d(x, y, z);
        }

        /// <summary>
        /// Returns true if two Vec3d vectors are parallel within a given absolute tolerance. Polar opposite vectors are considered parallel
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsParallel(Vec3d u, Vec3d v, double tolerance)
        {
            double cX = u.Y * v.Z + u.Z * v.Y;
            double cY = u.Z * v.X + u.X * v.Z;
            double cZ = u.X * v.Y + u.Y * v.X;
            double cN = cX * cX + cY * cY + cZ * cZ;
            return cN < tolerance; 
        }

        public static bool IsParallel(Vec3d u, Vec3d v, double tolerance, out bool inverted)
        {
            inverted = false;
            double cX = u.Y * v.Z + u.Z * v.Y;
            double cY = u.Z * v.X + u.X * v.Z;
            double cZ = u.X * v.Y + u.Y * v.X;
            double cN = cX * cX + cY * cY + cZ * cZ;
            if (u.Z * v.Z < 0)
            {
                inverted = true;
            }
            return cN < tolerance;
        }

        public static Vec3d Scale(Vec3d v, double factor)
        {
            return new Vec3d(v.X * factor, v.Y * factor, v.Z * factor);
        }


        /// <summary>
        /// find a the unit normal to a triangle defined by 3 points
        /// </summary>
        /// <param name="P0"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns>Vec3d</returns>
        public static Vec3d PerpTo(Pt3d P0, Pt3d P1, Pt3d P2)
        { 
            Vec3d result = new Vec3d();
            Vec3d V0 = new Vec3d(P2 - P1);
            Vec3d V1 = new Vec3d(P0 - P2);
            Vec3d V2 = new Vec3d(P1 - P0);

            Vec3d N0 = CrossProduct(V1, V2);
            if (!N0.Unitize())
            {
                throw new ArgumentException("Error: Invalid Inputs, check for coincedence with P0, P1, P2");
            }
            Vec3d N1 = CrossProduct(V2, V0);
            if (!N1.Unitize())
            {
                throw new ArgumentException("Error: Invalid Inputs, check for coincedence with P0, P1, P2");
            }
            Vec3d N2 = Vec3d.CrossProduct(V0, V1);
            if (!N2.Unitize())
            {
                throw new ArgumentException("Error: Invalid Inputs, check for coincedence with P0, P1, P2");
            }

            double s0 = 1.0/V0.M;
            double s1 = 1.0/V1.M;
            double s2 = 1.0/V2.M;

            // choose normal with smallest total error
            double e0 = (s0 * Abs(DotProduct(N0,V0))) + (s1 * Abs(DotProduct(N0,V1))) + (s2 * Abs(DotProduct(N0,V2)));
            double e1 = (s0 * Abs(DotProduct(N1,V0))) + (s1 * Abs(DotProduct(N1,V1))) + (s2 * Abs(DotProduct(N1,V2)));
            double e2 = (s0 * Abs(DotProduct(N2,V0))) + (s1 * Abs(DotProduct(N2,V1))) + (s2 * Abs(DotProduct(N2,V2)));
            if ( e0 <= e1 ) 
            {
                if ( e0 <= e2 ) 
                {
                    result = N0;
                }
                else 
                {
                    result = N2;
                }
            }
            else if (e1 <= e2) 
            {
                result = N1;
            }
            else 
            {
                result = N2;
            }
          return Vec3d.Normalize(result);
        }

        public static Vec3d Tween2(Vec3d a, Vec3d b)
        {
            return new Vec3d((a.X + b.X)/2, (a.Y + b.Y) / 2, (a.Z + b.Z) / 2);
        }

        public static Vec3d Tween2(Vec3d a, Vec3d b, double parameter)
        {
            double x = (a.X * (1 - parameter)) + (b.X * parameter);
            double y = (a.Y * (1 - parameter)) + (b.Y * parameter);
            double z = (a.Z * (1 - parameter)) + (b.Z * parameter);
            return new Vec3d(x, y, z);
        }

        public static Vec3d[] AvgPerps(Pline pline, Pln3d plane)
        {
            if (pline.Points.Length < 2)
            {
                throw new ArgumentException("Error: pts.Length must be >2 to calculate avg perp");
            }
            Vec3d prev = new Vec3d();
            Vec3d curr = new Vec3d(pline.Points[0], pline.Points[1]);
            int perpCount = pline.Points.Length;
            if (pline.Closed)
            {
                perpCount -= 1;
                prev = new Vec3d(pline.Points[pline.Points.Length - 2], pline.Points[pline.Points.Length - 1]);
            }
            else
            {
                prev = curr;
            }
            
            Vec3d[] perps = new Vec3d[perpCount];
            for (int i = 0; i < pline.Points.Length - 2; i++)
            {
                Vec3d zAxis0 = Tween2(prev, curr).Flip();
                perps[i] = Normalize(CrossProduct(zAxis0, plane.Zaxis));
                prev = curr;
                curr = new Vec3d(pline.Points[i + 1], pline.Points[i + 2]);
            }
            Vec3d zAxis1 = Tween2(prev, curr).Flip();
            perps[pline.Points.Length - 2] = Normalize(CrossProduct(zAxis1, plane.Zaxis));
            if (!pline.Closed)
            {
                Vec3d zAxis2 = Tween2(curr, curr).Flip();
                perps[pline.Points.Length - 1] = Normalize(CrossProduct(zAxis2, plane.Zaxis));
            }
            return perps;
        }

        public static bool Intersect(Pt3d vecOrigin, Vec3d vec, Pt3d start, Pt3d end, out double distance)
        {
            Vec3d v1 = new Vec3d(vecOrigin - start);
            Vec3d v2 = new Vec3d(end - start);
            Vec3d v3 = new Vec3d(-vec.Y, vec.X, vec.Z);

            double dot = v2 * v3;
            if (Math.Abs(dot) < 0.000001)
            {
                distance = 0.0;
                return false;
            }
            CrossProduct(v2, v1, out double t0);
            double t1 =  t0 / dot;
            double t2 = (v1 * v3) / dot;
            if (t1 >= 0.0 && (t2 >= 0.0 && t2 <= 1.0))
            {
                distance = t1;
                return true;
            }
            else
            {
                distance = 0.0;
                return false;
            }
        }

    }
}
