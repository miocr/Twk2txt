using System;
using System.IO;
using System.Collections.Generic;

namespace ZXt2txt
{
    class Program
    {

        static void Main(string[] args)
        {

            TAPUtils tapUtils = new TAPUtils();
            tapUtils.ExportCodeBlocks("CC-TXT2S1.tap");


            Converter converter = new Converter();

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(),args[0]);
            string dirPath = Path.GetDirectoryName(fullPath);
            string fileName = Path.GetFileName(fullPath);
            List<string> inFiles = new List<string>(Directory.GetFiles(dirPath,fileName));
            foreach (string inFile in inFiles)
            {
                string fileExtension = Path.GetExtension(inFile);
                CodingType coding = CodingType.zxGraphics;
                if (fileExtension == ".twt")
                    coding = CodingType.tasswordCZ;
                else if (fileExtension == ".dtt")
                    coding = CodingType.dTextCZ;

                converter.Convert(inFile, inFile + ".txt", coding);

            }

        }


    }
}
