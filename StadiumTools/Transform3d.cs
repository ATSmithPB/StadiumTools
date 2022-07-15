using System.Collections.Generic;
using System.Numerics;
using static System.Math;

namespace StadiumTools
{
    internal class Transform3d
    {
        //Properties
        /// <summary>
        /// a 2d array that represents a 4x4 transformation matrix
        /// </summary>
        public double[][] M { get; set; } = new double[3][];
        
        //Constructors
        /// <summary>
        /// Initializes an empty 4x4 transformation matrix object
        /// </summary>
        public Transform3d()
        {
        }
        /// <summary>
        /// constructs a Transform3d object from the components of a plane (Origin,XAxis,YAxis,ZAxis)
        /// </summary>
        /// <param name="P"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public Transform3d(Pt3d P, Vec3d X, Vec3d Y, Vec3d Z)
        {

            this.M[0][0] = X.X;
            this.M[1][0] = X.Y;
            this.M[2][0] = X.Z;
            this.M[3][0] = 0;

            this.M[0][1] = Y.X;
            this.M[1][1] = Y.Y;
            this.M[2][1] = Y.Z;
            this.M[3][1] = 0;

            this.M[0][2] = Z.X;
            this.M[1][2] = Z.Y;
            this.M[2][2] = Z.Z;
            this.M[3][2] = 0;

            this.M[0][3] = P.X;
            this.M[1][3] = P.Y;
            this.M[2][3] = P.Z;
            this.M[3][3] = 1;
        }

        public Transform3d(double dx, double dy, double dz)
        {
            Transform3d transform = new Transform3d();
            transform.SetIdentity();
            transform.M[0][3] = dx;
            transform.M[1][3] = dy;
            transform.M[2][3] = dz;
        }

        //Operator Overrides
        public static Transform3d operator * (Transform3d a, Transform3d b)
        {
            Transform3d result = new Transform3d();
            result.M[0][0] = a.M[0][0] * b.M[0][0];
            result.M[1][0] = a.M[1][0] * b.M[1][0];
            result.M[2][0] = a.M[2][0] * b.M[2][0];
            result.M[3][0] = a.M[3][0] * b.M[3][0];
            result.M[0][1] = a.M[0][1] * b.M[0][1];
            result.M[1][1] = a.M[1][1] * b.M[1][1];
            result.M[2][1] = a.M[2][1] * b.M[2][1];
            result.M[3][1] = a.M[3][1] * b.M[3][1];
            result.M[0][2] = a.M[0][2] * b.M[0][2];
            result.M[1][2] = a.M[1][2] * b.M[1][2];
            result.M[2][2] = a.M[2][2] * b.M[2][2];
            result.M[3][2] = a.M[3][2] * b.M[3][2];
            result.M[0][3] = a.M[0][3] * b.M[0][3];
            result.M[1][3] = a.M[1][3] * b.M[1][3];
            result.M[2][3] = a.M[2][3] * b.M[2][3];
            result.M[3][3] = a.M[3][3] * b.M[3][3];
            return result;
        }

        //Methods
        /// <summary>
        /// perform a change basis transformation with an initial and final plane
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>bool</returns>
        public bool ChangeBasis(Pln3d a, Pln3d b)
        {
            return ChangeBasis(a.OriginPt, a.Xaxis, a.Yaxis, a.Zaxis, b.OriginPt, b.Xaxis, b.Yaxis, b.Zaxis);
        }
        
        /// <summary>
        /// perform a change basis transformation with the components of two planes
        /// </summary>
        /// <param name="P0"></param>
        /// <param name="X0"></param>
        /// <param name="Y0"></param>
        /// <param name="Z0"></param>
        /// <param name="P1"></param>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="Z1"></param>
        /// <returns>bool</returns>
        public bool ChangeBasis(Pt3d P0, Vec3d X0, Vec3d Y0, Vec3d Z0, 
                                Pt3d P1, Vec3d X1, Vec3d Y1, Vec3d Z1)
        {
            bool success = false;

            //Initial Frame
            Transform3d F0 = new Transform3d(P0, X0, Y0, Z0);

            //T1 translates by -P1
            Transform3d T1 = new Transform3d(Pt3d.Origin.X - P1.X, 
                                             Pt3d.Origin.Y - P1.Y, 
                                             Pt3d.Origin.Z - P1.Z);

            //Empty transform to become Change Basis
            Transform3d CB = new Transform3d();
            success = CB.ChangeBasis(Vec3d.XAxis, Vec3d.YAxis, Vec3d.ZAxis, X1, Y1, Z1);

            Transform3d result = CB * T1 * F0;

            return success;
        }

        public bool ChangeBasis(Vec3d X0, Vec3d Y0, Vec3d Z0,
                                Vec3d X1, Vec3d Y1, Vec3d Z1)
        {
            Transform3d TF = new Transform3d();
            TF.Zero();
            double a = X1 * Y1;
            double b = X1 * Z1;
            double c = Y1 * Z1;

            double[][] R =
            {
                new double[] { X1*X1, a, b,X1*X0, X1*Z0 },
                new double[] { a, Y1*Y1, c, Y1*X0, Y1*Y0, Y1*Z0 },
                new double[] { b, c, Z1*Z1, Z1*X0, Z1*Y0, Z1*Z0 }
            };

            int i0 = (R[0][0] >= R[1][1]) ? 0 : 1;
            
            if (R[2][2] > R[i0][i0])
            {
                i0 = 2;
            }

            int i1 = (i0 + 1) % 3;
            int i2 = (i1 + 1) % 3;

            if (R[i0][i0] == 0.0)
            {
                return false;
            }

            double d = 1.0 / R[i0][i0];
            R[i0][0] *= d;
            R[i0][1] *= d;
            R[i0][2] *= d;
            R[i0][3] *= d;
            R[i0][4] *= d;
            R[i0][5] *= d;
            R[i0][i0] = 1.0;
            
            if (R[i1][i0] != 0.0)
            {
                d = -R[i1][i0];
                R[i1][0] += d * R[i0][0];
                R[i1][1] += d * R[i0][1];
                R[i1][2] += d * R[i0][2];
                R[i1][3] += d * R[i0][3];
                R[i1][4] += d * R[i0][4];
                R[i1][5] += d * R[i0][5];
                R[i1][i0] = 0.0;
            }
            
            if (R[i2][i0] != 0.0)
            {
                d = -R[i2][i0];
                R[i2][0] += d * R[i0][0];
                R[i2][1] += d * R[i0][1];
                R[i2][2] += d * R[i0][2];
                R[i2][3] += d * R[i0][3];
                R[i2][4] += d * R[i0][4];
                R[i2][5] += d * R[i0][5];
                R[i2][i0] = 0.0;
            }

            if (Abs((R[i1][i1])) < Abs((R[i2][i2])))
            {
                int i = i1; i1 = i2; i2 = i;
            }

            if (R[i1][i1] == 0.0)
            {
                return false;
            }

            d = 1.0 / R[i1][i1];
            R[i1][0] *= d;
            R[i1][1] *= d;
            R[i1][2] *= d;
            R[i1][3] *= d;
            R[i1][4] *= d;
            R[i1][5] *= d;
            R[i1][i1] = 1.0;
            
            if (R[i0][i1] != 0.0)
            {
                d = -R[i0][i1];
                R[i0][0] += d * R[i1][0];
                R[i0][1] += d * R[i1][1];
                R[i0][2] += d * R[i1][2];
                R[i0][3] += d * R[i1][3];
                R[i0][4] += d * R[i1][4];
                R[i0][5] += d * R[i1][5];
                R[i0][i1] = 0.0;
            }
            
            if (R[i2][i1] != 0.0)
            {
                d = -R[i2][i1];
                R[i2][0] += d * R[i1][0];
                R[i2][1] += d * R[i1][1];
                R[i2][2] += d * R[i1][2];
                R[i2][3] += d * R[i1][3];
                R[i2][4] += d * R[i1][4];
                R[i2][5] += d * R[i1][5];
                R[i2][i1] = 0.0;
            }

            if (R[i2][i2] == 0.0)
            {
                return false;
            }

            d = 1.0 / R[i2][i2];
            R[i2][0] *= d;
            R[i2][1] *= d;
            R[i2][2] *= d;
            R[i2][3] *= d;
            R[i2][4] *= d;
            R[i2][5] *= d;
            R[i2][i2] = 1.0;
            
            if (R[i0][i2] != 0.0)
            {
                d = -R[i0][i2];
                R[i0][0] += d * R[i2][0];
                R[i0][1] += d * R[i2][1];
                R[i0][2] += d * R[i2][2];
                R[i0][3] += d * R[i2][3];
                R[i0][4] += d * R[i2][4];
                R[i0][5] += d * R[i2][5];
                R[i0][i2] = 0.0;
            }
            
            if (R[i1][i2] != 0.0)
            {
                d = -R[i1][i2];
                R[i1][0] += d * R[i2][0];
                R[i1][1] += d * R[i2][1];
                R[i1][2] += d * R[i2][2];
                R[i1][3] += d * R[i2][3];
                R[i1][4] += d * R[i2][4];
                R[i1][5] += d * R[i2][5];
                R[i1][i2] = 0.0;
            }

            TF.M[0][0] = R[0][3];
            TF.M[0][1] = R[0][4];
            TF.M[0][2] = R[0][5];
            TF.M[1][0] = R[1][3];
            TF.M[1][1] = R[1][4];
            TF.M[1][2] = R[1][5];
            TF.M[2][0] = R[2][3];
            TF.M[2][1] = R[2][4];
            TF.M[2][2] = R[2][5];

            return true;

        }
        
        /// <summary>
        /// set default identity to a transform matrix (no transformation)
        /// </summary>
        /// <param name="transform"></param>
        public static void SetIdentity(Transform3d transform)
        {
            transform.SetIdentity();
        }

        /// <summary>
        /// set default identity to a transform matrix (no transformation)
        /// </summary>
        /// <param name="transform"></param>
        private void SetIdentity()
        {
            this.M[0][0] = 1.0;
            this.M[1][1] = 1.0;
            this.M[2][2] = 1.0;
            this.M[3][3] = 1.0;
        }

        /// <summary>
        /// set all index values of matrix to 0.0
        /// </summary>
        /// <param name="transform"></param>
        public static void Zero(Transform3d transform)
        {
            transform.Zero();
        }

        /// <summary>
        /// set all index values of matrix to 0.0
        /// </summary>
        private void Zero()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    this.M[i][j] = 0.0;
                }
            }
        }

    }

    


}
