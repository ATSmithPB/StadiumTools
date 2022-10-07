using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StadiumTools
{
    public class Data
    {

        //Methods
        public static T[] MatchLength<T>(List<T> list, int len, string type) where T : struct
        {
            if (list.Count != 1 && list.Count != len)
            {
                throw new ArgumentException($"{type} count [{list.Count}] must be 1 value OR be a list of values equal to [{len}]");
            }
            else
            {
                T[] result = new T[len];
                if (list.Count == len)
                {
                    for (int i = 0; i < len; i++)
                    {
                        result[i] = list[i];
                    }
                }
                else
                {
                    for (int i = 0; i < len; i++)
                    {
                        result[i] = list[0];
                    }
                }
                return result;
            }
        }

    }
}
