using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N3RLE
{
    class LZW : IDisposable
    {
        public void Dispose()
        {
           
        }

        internal string Encode(ref string uncompressed)
        {
            // build the dictionary
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(((char)i).ToString(), i);

            string w = string.Empty;
            List<int> compressed = new List<int>();

            foreach (char c in uncompressed)
            {
                string wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    // write w to output
                    compressed.Add(dictionary[w]);
                    // wc is a new sequence; add it to the dictionary
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }

            // write remaining output if necessary
            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            String compres="";

            foreach (int current in compressed) {
                compres += current.ToString()+" ";
            }

            return compres;
        }

        internal string Decode(ref string compres)
        {
            string[] comp = compres.Split(' ');

            List<int> compressed = new List<int>();

            for (int i = 0; i < comp.Length; i++) {
                if (comp[i]!="\r\n")
                    compressed.Add(Convert.ToInt32(comp[i]));
            }

            // build the dictionary
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            string w = dictionary[compressed[0]];
            compressed.RemoveAt(0);
            StringBuilder decompressed = new StringBuilder(w);

            foreach (int k in compressed)
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);

                // new sequence; add it to the dictionary
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            return decompressed.ToString();
        }

        internal double GetPercentage(double x, double y)
        {
            return (100 * (x - y)) / x;
        }

    }
}
