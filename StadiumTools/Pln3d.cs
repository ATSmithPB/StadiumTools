using System.Collections.Generic;
using static System.Math;

namespace StadiumTools
{
    /// <summary>
    /// Represents a Plane in 3D space 
    /// </summary>
    public struct Pln3d
    {
        //Enums
        /// <summary>
        /// Available default plane orientations
        /// </summary>
        public enum ReferencePtType
        {
            XY,
            YZ,
            XZ
        }

        //Prtoperties
        /// <summary>
        /// True if plane components are valid.
        /// </summary>
        public bool isValid { get; set; }
        /// <summary>
        /// Pt3d representing the origin point of the Plane
        /// </summary>
        public Pt3d OriginPt { get; set; }
        /// <summary>
        /// x coordinate of plane origin
        /// </summary>
        public double OriginX { get; set; }
        /// <summary>
        /// y coordinate of plane origin
        /// </summary>
        public double OriginY { get; set; }
        /// <summary>
        /// z coordinate of plane origin
        /// </summary>
        public double OriginZ { get; set; }
        /// <summary>
        /// Vec3d representing the x axis
        /// </summary>
        public Vec3d Xaxis { get; set; }
        /// <summary>
        /// Vec3d representing the y axis
        /// </summary>
        public Vec3d Yaxis { get; set; }
        /// <summary>
        /// Vec3d representing the z axis
        /// </summary>
        public Vec3d Zaxis { get; set; }

        //Constructors
        /// <summary>
        /// construct a Pln3d with an origin point and three vectors
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Pln3d(Pt3d origin, Vec3d x, Vec3d y, Vec3d z)
        {
            this.isValid = false;
            this.OriginPt = origin;
            this.OriginX = origin.X;
            this.OriginY = origin.Y;
            this.OriginZ = origin.Z;
            this.Xaxis = x;
            this.Yaxis = y;
            this.Zaxis = z;
            
            IsValid(this);
        }

        //Methods
        /// <summary>
        /// Check if all plane components construct a valid plane.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool IsValid(Pln3d p)
        {
            bool isValid = false;

            return isValid;
        }

    }
}
