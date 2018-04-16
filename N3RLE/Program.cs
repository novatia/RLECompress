using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace N3RLE
{
    class Program
    {
        private static string str_welcome = "\r\nRLE/LZW TEXT FILE encoding/decoding tool, N3 S.n.c. - Andrea Novati 2014(c)",
        str_notice = "Please, use the next syntax: <type> <action> <file name>\n\n<type>\t\t[lzw,rle]\n<action>\te=Encode, d=Decode\n<file name>\tsource text file absolute path\n\n(e.g. \"lzw e c:\\test.txt \").";

        private static String EncodeStringRLE(ref string str)
        {
            using (RLE inst_rle = new RLE())
            {
                string str_encoded = inst_rle.Encode(ref str);
                Console.WriteLine("\r\nBase string ({0} chars): {1}\r\nAfter RLE-encoding ({2} chars): {3}\r\nCompression percentage: %{4}",
                                    str.Length, str, str_encoded.Length, str_encoded,
                                    inst_rle.GetPercentage((double)str.Length, (double)str_encoded.Length).ToString());

                return str_encoded;
            }
        }

        private static String EncodeStringLZW(ref string str)
        {
            using (LZW inst_rle = new LZW())
            {
                string str_encoded = inst_rle.Encode(ref str);
                Console.WriteLine("\r\nBase string ({0} chars): {1}\r\nAfter RLE-encoding ({2} chars): {3}\r\nCompression percentage: %{4}",
                                    str.Length, str, str_encoded.Length, str_encoded,
                                    inst_rle.GetPercentage((double)str.Length, (double)str_encoded.Length).ToString());

                return str_encoded;
            }
        }

        private static string DecodeStringRLE(ref string str)
        {
            using (RLE inst_rle = new RLE())
            {
                string str_decoded = inst_rle.Decode(ref str);
                Console.WriteLine("\r\nBase string ({0} chars): {1}\r\nAfter RLE-decoding ({2} chars): {3}",
                        str.Length, str, str_decoded.Length, str_decoded);

                return str_decoded;
            }
        }

        private static string DecodeStringLZW(ref string str)
        {
            using (LZW inst_rle = new LZW())
            {
                string str_decoded = inst_rle.Decode(ref str);
                Console.WriteLine("\r\nBase string ({0} chars): {1}\r\nAfter RLE-decoding ({2} chars): {3}",
                        str.Length, str, str_decoded.Length, str_decoded);

                return str_decoded;
            }
        }

        static String GetFileContent(String path)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(path))
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            string allines = sb.ToString();


            return allines;
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(str_welcome);

                if (args.Length >= 2)
                {
                    if (args[1] == "e")
                    {
                        String encoded;
                        String toEncode="";

                        try
                        {
                            toEncode = GetFileContent(args[2]);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("File not found.");
                            Environment.Exit(1);
                        }

                        if (args[0] == "rle")
                        {
                            encoded = EncodeStringRLE(ref toEncode);
                            System.IO.File.WriteAllText(@".\compress.rle", encoded);
                        }
                        else if (args[0] == "lzw")
                        {
                            encoded = EncodeStringLZW(ref toEncode);
                            System.IO.File.WriteAllText(@".\compress.lzw", encoded);
                        }
                        else
                        {
                            Console.WriteLine("Compression not supported.");
                            return;
                        }
                    }
                    else if (args[1] == "d")
                    {
                        String toDecode="";

                        try
                        {
                            toDecode = GetFileContent(args[2]);
                        }
                        catch (Exception e) {
                            Console.WriteLine("File not found.");
                            Environment.Exit(1);
                        }

                        String decoded ;
                        if (args[0] == "rle")
                        {
                            decoded = DecodeStringRLE(ref toDecode);
                        }
                        else if (args[0] == "lzw")
                        {
                            decoded = DecodeStringLZW(ref toDecode);
                        }
                        else
                        {
                            Console.WriteLine("Compression not supported.");
                            return;
                        }

                        System.IO.File.WriteAllText(@".\decompress.txt", decoded);
                    }
                    else
                    {
                        throw (new Exception(str_notice));
                    }
                }
                else
                {
                    Console.WriteLine(str_notice);
                    Environment.Exit(1);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("\r\n{0}", exc);
                Environment.Exit(1);
            }

        }
    }
}
