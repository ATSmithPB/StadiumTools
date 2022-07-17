using System.Collections.Generic;

namespace StadiumTools
{
    public struct SuperRiser
    {
        //Properties
        /// <summary>
        /// A spectator object that defines the pectator parameters for generating the tier (Target C-Val, EyeX, EyeY, etc..)
        /// </summary>
        public Spectator SpectatorParameters { get; set; }
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

        public void InitializeDefault()
        {
            Spectator defaultSpectatorParameters = new Spectator();
            Spectator.InitializeDefault(defaultSpectatorParameters);
            this.SpectatorParameters = defaultSpectatorParameters;
            this.Row = 10;
            this.Width = 3;
            this.GuardrailWidth = 0.1 * this.SpectatorParameters.Unit;
            this.CurbHeight = 0.1 * this.SpectatorParameters.Unit;
            this.CurbWidth = 0.1 * this.SpectatorParameters.Unit;
            this.EyeX = 1.6 * this.SpectatorParameters.Unit;
            this.EyeY = 1.2 * this.SpectatorParameters.Unit;
            this.SEyeX = 1.8 * this.SpectatorParameters.Unit;
            this.SEyeY = 1.4;
        }
    }
}
