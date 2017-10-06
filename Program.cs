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
                Console.WriteLine("Convert ZX Spectrum CODE block with Tasword or D-TEXT(SpectralWriter) text to UTF-8 plain text file");
                Console.WriteLine("CIDSOFT (C)2017, version 1.0");
                Console.WriteLine("");
                Console.WriteLine("Usage: ZXt2txt file(s) [coding]");
                Console.WriteLine("file(s): file path name or path with mask");
                Console.WriteLine("coding:  tw2cz, tw2bcs, dtcz");
                Console.WriteLine("If coding isn't set, converter use coding by file extension (same asi coding).");
                return;    
            }
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
            string dirPath = Path.GetDirectoryName(fullPath);
            string fileName = Path.GetFileName(fullPath);
            List<string> inFiles = new List<string>(Directory.GetFiles(dirPath, fileName));
            if (inFiles.Count > 0)
            {
                Converter converter = new Converter();
                CodingType coding = CodingType.zxGraphics; // default

                if (args.Length > 1 && !string.IsNullOrEmpty(args[1]))
                {
                    // set CodingType by commandline parameter
                    switch (args[1])
                    {
                        case "tw2cz":
                            coding = CodingType.tasword2CZ;
                            break;
                        case "tw2bcs":
                            coding = CodingType.tasword2BCS;
                            break;
                        case "dtcz":
                            coding = CodingType.dTextCZ;
                            break;
                    }
                }
                else
                {
                    // auto set CodingType by file extension
                    foreach (string inFile in inFiles)
                    {
                        string fileExtension = Path.GetExtension(inFile);
                        if (fileExtension == ".twt")
                            coding = CodingType.tasword2CZ;
                        else if (fileExtension == ".tbt")
                            coding = CodingType.tasword2BCS;
                        else if (fileExtension == ".dtt")
                            coding = CodingType.dTextCZ;

                        converter.Convert(inFile, inFile + ".txt", coding);
                    }
                }
            }
        }
    }
}
