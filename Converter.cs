using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Twk2txt
{
  class Converter
  {


    Dictionary<int,char> taswordCZ = new Dictionary<int, char>
    {
      {0x80,'é'},
      {0x81,'ě'},
      {0x82,'š'},
      {0x83,'č'},
      {0x84,'ř'},
      {0x85,'ý'},
      {0x86,'á'},
      {0x87,'í'},
      {0x88,'ů'},
      {0x89,'ú'},
      {0x8A,'ó'},
      {0x8B,'ď'},
      {0x8C,' '},
      {0x8D,'ň'},
      {0x8E,' '},
      {0x8F,'ž'},

      {0x90,' '},
      {0x91,' '},
      {0x92,' '},
      {0x93,' '},
      {0x94,' '},
      {0x95,' '},
      {0x96,' '},
      {0x97,' '},
      {0x98,' '},
      {0x99,' '},
      {0x9A,' '},
      {0x9B,' '},
      {0x9C,' '},
      {0x9D,' '},
      {0x9E,' '},
      {0x9F,' '},      
    };
     Dictionary<int,char> dtextCZ = new Dictionary<int, char>
    {
      {0x80,'é'},
      {0x81,'ě'},
      {0x82,'š'},
      {0x83,'č'},
      {0x84,'ř'},
      {0x85,'ý'},
      {0x86,'á'},
      {0x87,'í'},
      {0x88,'ů'},
      {0x89,'ú'},
      {0x8A,'ó'},
      {0x8B,'ď'},
      {0x8C,' '},
      {0x8D,'ň'},
      {0x8E,' '},
      {0x8F,'ž'},

      {0x90,' '},
      {0x91,' '},
      {0x92,' '},
      {0x93,' '},
      {0x94,' '},
      {0x95,' '},
      {0x96,' '},
      {0x97,' '},
      {0x98,' '},
      {0x99,' '},
      {0x9A,' '},
      {0x9B,' '},
      {0x9C,' '},
      {0x9D,' '},
      {0x9E,' '},
      {0x9F,' '},      
    };

    private byte[] textLine = new byte[64];
    private String stextLine = String.Empty;

    private StringBuilder sbTextLine = new StringBuilder();
    //private int linePos = 0;

    private FileStream input;
    private FileStream output;


    

    public Converter(string inputFilePath, string outputFilePath)
    {
      input = File.OpenRead(inputFilePath); //@"text.twk");
      output = File.OpenWrite(outputFilePath);      
    }
    public void Convert()
    {
      int repeat = 0;
      int lastByte = 32;

      int inpByte;

      while ((inpByte = input.ReadByte()) != -1)
      {
        if (inpByte >= 0xA5)
        {
          // count total repeat lastchar
          repeat = repeat + (inpByte - 0xA5 + 1);
        }
        else
        {
          // if (inpByte > 0x7F) // && inpByte < 0xA5)
          // {
          //   // translate national char > 0x7F
          //   inpByte = (int)'?';
          // }

          // unpack last character (repeat x)
          // continue, if repeat is 0
          while (repeat > 0)
          {
            WriteChar(lastByte);
            repeat--; 
            // repeat = 0;
          }
         

          WriteChar(inpByte);

          lastByte = inpByte;
        }

      }
      
      if (sbTextLine.Length > 0 )
      {
        Console.WriteLine(sbTextLine.ToString());
        output.Write(Encoding.UTF8.GetBytes(sbTextLine.ToString()),0,sbTextLine.Length);
      }

      //if (linePos > 0)
      // for (int i=0;i < linePos;i++)
      //   WriteChar(textLine[i]);
      //Console.WriteLine(String.Concat(textLine).Substring(0, linePos));
        
       output.Flush();
        
    }

    private void WriteChar(int charInt)
    {
      if (charInt > 0x7F)
        //textLine[linePos++] = System.Convert.ToByte(taswordCZ[character]);
        //stextLine = String.Concat(stextLine,taswordCZ[character]);
        sbTextLine.Append(taswordCZ[charInt]);
      else
        //textLine[linePos++] = (byte)character;
        //stextLine = String.Concat(stextLine,System.Convert.ToChar(character));
        sbTextLine.Append(System.Convert.ToChar(charInt));

      //if (++linePos == 64)
      if (sbTextLine.Length == 64)
      {
        //linePos = 0;
        //Console.WriteLine(System.Text.Encoding.ASCII.GetString(textLine));
        //Console.WriteLine(stextLine);
        //stextLine = string.Empty;
        
        Console.WriteLine(sbTextLine.ToString());


        output.Write(Encoding.UTF8.GetBytes(sbTextLine.ToString()),0,64);
        output.WriteByte(0x0D);
        output.WriteByte(0x0A);

        sbTextLine.Clear();
      }
    }

  }
}