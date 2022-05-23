using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPoleEmploi
{
    internal class Utils
    {
        public static string TrimTrailingNumbers(string input)
        {
            var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ' };
            var result = input.TrimEnd(digits);

            return result;
        }
    }
}
