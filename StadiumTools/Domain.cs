using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    //Properties
    public struct Domain
    {
        /// <summary>
        /// The lower bounds (start) of the domain
        /// </summary>
        public double T0 { get; set; }
        /// <summary>
        /// the upper bounds (end) of the domain
        /// </summary>
        public double T1 { get; set; }
        /// <summary>
        /// The difference in the upper and lower bounds of the array. A negative number indicates a decreasing domain.
        /// </summary>
        public double Length { get; set;}

        //Constructors
        public Domain(double start, double end)
        {
            T0 = start;
            T1 = end;
            Length = end - start;
        }

    }
}
