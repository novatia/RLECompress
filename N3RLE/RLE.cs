using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N3RLE
{
    class RLE : IDisposable
    {
        private string str_base, str_rle;

        internal string Encode(ref string input)
        {
            str_rle = null;
            str_base = input;

            for (int i = 0; i < str_base.Length; i++)
            {
                char symbol = str_base[i];
                int count = 1;

                for (int j = i; j < str_base.Length - 1; j++)
                {
                    if (str_base[j + 1] != symbol) break;

                    count++;
                    i++;
                }

                if (count == 1) str_rle += symbol;
                else str_rle += count.ToString() + symbol;
            }

            return str_rle;
        }

        internal string Decode(ref string input)
        {
            str_rle = null;
            str_base = input;
            int count = 0;

            for (int i = 0; i < str_base.Length; i++)
            {
                if (Char.IsNumber(str_base[i]))
                {
                    count++;
                }
                else
                {
                    if (count > 0)
                    {
                        int value_repeat = Convert.ToInt32(str_base.Substring(i - 1, count));

                        for (int j = 0; j < value_repeat; j++)
                        {
                            str_rle += str_base[i];
                        }

                        count = 0;
                    }
                    else if (count == 0)
                    {
                        str_rle += str_base[i];
                    }
                }
            }

            return str_rle;
        }

        internal double GetPercentage(double x, double y)
        {
            return (100 * (x - y)) / x;
        }

        public void Dispose()
        {
            if (str_rle != null || str_base != null)
            {
                str_rle = str_base = null;
            }
        }
    }
}
