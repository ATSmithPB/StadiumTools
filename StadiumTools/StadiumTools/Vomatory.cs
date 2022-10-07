using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public struct Vomatory
    {
        //Properties
        /// <summary>
        /// Row number of vomitory start
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// Height of vomitory (in rows)
        /// </summary>
        public int Height { get; set; }

        //Constructors
        /// <summary>
        /// Construct a Vomatory object from a start row and height parameter
        /// </summary>
        /// <param name="start"></param>
        /// <param name="height"></param>
        public Vomatory(int start, int height)
        {
            this.Start = start;
            this.Height = height;
        }

        //Methods

        public static void InitDefault(Vomatory vomatory)
        {
            vomatory.Start = 3;
            vomatory.Height = 5;
        }
    }
}
