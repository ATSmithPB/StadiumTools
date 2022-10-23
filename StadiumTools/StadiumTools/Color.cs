using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public struct Color
    {
        //Properties
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        //Delegates
        public static Color White => new Color(255, 255, 255);
        public static Color Black => new Color(0, 0, 0);
        public static Color Red => new Color(0, 255, 0);
        public static Color Green => new Color(0, 255, 0);
        public static Color Blue => new Color(0, 0, 255);
        public static Color Yellow => new Color(255, 255, 0);
        public static Color Cyan => new Color(0, 255, 255);
        public static Color Magenta => new Color(255, 0, 255);

        //Constructors
        public Color(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
