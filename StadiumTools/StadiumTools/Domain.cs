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
        /// <summary>
        /// the middle point value of the Domain
        /// </summary>
        public double Mid { get; set; }

        //Constructors
        public Domain(double start, double end)
        {
            T0 = start;
            T1 = end;
            Length = end - start;
            Mid = (start + end / 2);
        }

        //Methods
        public bool Contains(double parameter)
        {
            bool result = false;
            if (parameter >= this.T0 && parameter <= this.T1)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// returns true if successful. Extend T0 and T1 by a value. Adds 2 * Delta to domain length
        /// </summary>
        /// <param name="delta"></param>
        /// <returns>bool</returns>
        public bool Expand(double delta)
        {
            this.T0 -= delta;
            this.T1 += delta;
            this.Length += delta * 2;
            return true;
        }

        public static Domain Singleton(double t)
        {
            return new Domain(t, t);
        }

        /// <summary>
        /// replace a number outside a domain with the closest domain bound to it 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int Clamp(double v) 
        {
            int rval = 0;
            if (v < this.T0) 
            {
                v = this.T0;
                rval = -1;
            }
            else if (v > this.T1) 
            {
                v = this.T1;
                rval = 1;
            }
            return rval;
        }

    }
}
