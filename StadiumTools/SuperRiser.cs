using System.Collections.Generic;
using System;

namespace StadiumTools
{
    public struct SuperRiser : ICloneable
    {
        //Properties
        public double Unit { get; set; }
        /// <summary>
        /// Row number for inserting super riser
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// Width of super riser row in rows (x * tier RowWidth)
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Optional curb distance before super riser 
        /// </summary>
        public double CurbWidth { get; set; }
        /// <summary>
        /// Optional curb height before super riser
        /// </summary>
        public double CurbHeight { get; set; }
        /// <summary>
        /// Horizontal offset of spectator eyes from nose of super riser 
        /// </summary>
        public double EyeX { get; set; }
        /// <summary>
        /// Vertical offset of spectator eyes from floor of super riser
        /// </summary>
        public double EyeY { get; set; }
        /// <summary>
        /// Width of guardrail behind super riser
        /// </summary>
        public double GuardrailWidth { get; set; }
        /// <summary>
        /// Horizontal offset of STANDING spectator eyes from nose of super riser 
        /// </summary>
        public double SEyeX { get; set; }
        /// <summary>
        /// Vertical offset of STANDING spectator eyes from floor of super riser
        /// </summary>
        public double SEyeY { get; set; }

        //Constructors 

        public static void InitDefault(SuperRiser superRiser)
        {
            superRiser.Unit = 1.0;
            superRiser.Row = 10;
            superRiser.Width = 3;
            superRiser.GuardrailWidth = 0.1 * superRiser.Unit;
            superRiser.CurbHeight = 0.1 * superRiser.Unit;
            superRiser.CurbWidth = 0.1 * superRiser.Unit;
            superRiser.EyeX = 1.6 * superRiser.Unit;
            superRiser.EyeY = 1.2 * superRiser.Unit;
            superRiser.SEyeX = 1.8 * superRiser.Unit;
            superRiser.SEyeY = 1.4 * superRiser.Unit;
        }

        //Methods

        public object Clone()
        {
            return (SuperRiser)this.MemberwiseClone();
        }
    }
}
