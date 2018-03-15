using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitRecognition
{
    public static class Extensions
    {
        public static int Argmax(this double[] input)
        {
            double max = input.Max();
            for (int i = 0; i < input.Length; i++) {
                if (input[i] == max) {
                    return i;
                } 
            }
            return -1;
        }

        public static bool IsNumber(this string s)
        {
            if (s.Length == 0)
                return false;
            for (int i = 0; i < s.Length; i++)
                if (s[i] < '0' || s[i] > '9')
                    return false;
            return true;
        }
    }
}
