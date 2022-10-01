using System.Collections.Generic;

namespace StadiumTools
{
    /// <summary>
    /// Stores a collection of unit conversion coeffecients to handle various modeling unit spaces
    /// </summary>
    public struct UnitHandler
    {
        //Properties
        public const double mm = 1000.0;
        public const double cm = 100.0;
        public const double m = 1.0;
        public const double inch = 39.3701;
        public const double feet = 3.28084;
        public const double yard = 1.09361;
        public const double minVal = double.MinValue;
        public const double abstol = 0.000001;

        //Methods
        /// <summary>
        /// returns a Meters/Unit coefficient based on string abbreviations of common unit systems (mm, in, ft) 
        /// </summary>
        /// <param name="programName"></param>
        /// <param name="unitSystemName"></param>
        /// <returns>double</returns>
        public static double FromString(string programName, string unitSystemName)
        {
            unitSystemName.ToLower();
            if (programName == "Rhino")
            {
                switch (unitSystemName)
                {
                    case "mm":
                        return UnitHandler.mm;
                    case "cm":
                        return UnitHandler.cm;
                    case "in":
                        return UnitHandler.inch;
                    case "ft":
                        return UnitHandler.feet;
                    case "yd":
                        return UnitHandler.yard;
                    default:
                        return UnitHandler.m;
                }
            }
            else if (programName == "Revit")
                switch (unitSystemName)
                {
                    case "mm":
                        return UnitHandler.mm;
                    case "cm":
                        return UnitHandler.cm;
                    case "in":
                        return UnitHandler.inch;
                    case "ft":
                        return UnitHandler.feet;
                    case "yrd":
                        return UnitHandler.yard;
                    default:
                        return UnitHandler.m;
                }
            else
            {
                return 0.0;
            }

        }

        /// <summary>
        /// tests if a unit system 2 letter abbreviation is supported and returns a boolean 
        /// </summary>
        /// <param name="unitSystemName"></param>
        /// <returns>bool</returns>
        public static bool isValid(string unitSystemName)
        {
            bool result = false;

            switch (unitSystemName)
            {
                case "mm": result = true; break;
                case "cm": result = true; break;
                case "m": result = true; break;
                case "in": result = true; break;
                case "ft": result = true; break;
                case "yd": result = true; break;
                default : result = false; break;
            }
            return result;
        }

    }
}

