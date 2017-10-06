using System;
using System.IO;
using System.Collections.Generic;

namespace ZXt2txt
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("");
                Console.WriteLine("Convert ZX Spectrum CODE block with Tasword or D-TEXT(SpectralWriter) text to UTF-8 plain text file. CIDSOFT (C)2017, version 1.0");
                Console.WriteLine("");
                Console.WriteLine("Usage: ZXt2txt file(s) [coding]");
                Console.WriteLine(" > file(s): file or mask ('mrs1.tw2cz','files/*.dtcz'...)");
                Console.WriteLine(" > coding:  tw2cz, tw2bcs, dtcz");
                Console.WriteLine("");
                Console.WriteLine("If coding isn't set, converter use coding by file extension (same asi coding).");
                return;
            }
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
            string dirPath = Path.GetDirectoryName(fullPath);
            string fileName = Path.GetFileName(fullPath);
            List<string> inFiles = new List<string>(Directory.GetFiles(dirPath, fileName));
            if (inFiles.Count > 0)
            {
                foreach (string inFile in inFiles)
                {
                    Converter converter = new Converter();
                    string condingFlag;
                    if (args.Length > 1 && !string.IsNullOrEmpty(args[1]))
                    {
                        condingFlag = args[1];
                    }
                    else
                    {
                        condingFlag = Path.GetExtension(inFile);
                        condingFlag = condingFlag.Remove(0,1);
                    }

                    // set Coding by commandline parameter / file extension
                    switch (condingFlag)
                    {
                        case "tw2cz":
                            converter.Coding = CodingType.tasword2CZ;
                            break;
                        case "tw2bcs":
                            converter.Coding = CodingType.tasword2BCS;
                            break;
                        case "dtcz":
                            converter.Coding = CodingType.dTextCZ;
                            break;
                    }
                    converter.Convert(inFile, inFile + ".txt");
                }
            }
        }
    }
}
