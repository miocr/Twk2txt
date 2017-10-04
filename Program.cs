using System;
//using System.IO;

namespace Twk2txt
{
    class Program
    {

        static void Main(string[] args)
        {
           Converter converter = new Converter(args[0],args[1]);
           converter.Convert();
        }

    
    }
}
