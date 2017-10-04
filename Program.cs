using System;
using System.IO;

namespace Twk2txt
{
    class Program
    {
    
        static void Main(string[] args)
        {
            char[] textLine = new char[64];
            int linePosition = 0;
            int repeatCounter = 0;
            int lastChar = 32;

            Console.WriteLine("Hello World!");
            FileStream input = File.OpenRead(@"text.twk");
            int inpByte;
            while((inpByte = input.ReadByte()) != -1)
            {
                if (inpByte >= 0xA5)
                {
                    // while (inpByte == 0xFF)
                    // {
                    //     repeatCounter = repeatCounter + 255;
                    //     inpByte = input.ReadByte();
                    // }
                    repeatCounter = repeatCounter + inpByte - 0xA5 + 1;
                    while (repeatCounter-- > 0)
                    {
                        textLine[linePosition++] = (char)lastChar;
                        if (linePosition == 64)
                        {
                            linePosition = 0;
                            Console.WriteLine(String.Concat(textLine));
                        }
                    }
                }
                else
                {
                    textLine[linePosition++] = (char)inpByte;
                    if (linePosition == 64)
                    {
                        linePosition = 0;
                        Console.WriteLine(String.Concat(textLine));
                    }
                }
                lastChar = inpByte;
            } 

        }
    }
}
