using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ZXt2txt
{

    class TAPUtils
    {
        public Dictionary<int, string> zxTokens = new Dictionary<int, string>
        {
          {0xA0,"(Q)"},
          {0xA1,"(R)"},
          {0xA2,"(S)"},
          {0xA3,"(T)"},
          {0xA4,"(U)"},
          {0xA5,"RND"},
          {0xA6,"INKEY$"},
          {0xA7,"PI"},
          {0xA8,"FN"},
          {0xA9,"POINT"},
          {0xAA,"SCREEN$"},
          {0xAB,"ATTR"},
          {0xAC,"AT"},
          {0xAD,"TAB"},
          {0xAE,"VAL$"},
          {0xAF,"CODE"},
          {0xB0,"VAL"},
          {0xB1,"LEN"},
          {0xB2,"SIN"},
          {0xB3,"COS"},
          {0xB4,"TAN"},
          {0xB5,"ASN"},
          {0xB6,"ACS"},
          {0xB7,"ATN"},
          {0xB8,"LN"},
          {0xB9,"EXP"},
          {0xBA,"INT"},
          {0xBB,"SQR"},
          {0xBC,"SGN"},
          {0xBD,"ABS"},
          {0xBE,"PEEK"},
          {0xBF,"IN"},
          {0xC0,"USR"},
          {0xC1,"STR$"},
          {0xC2,"CHR$"},
          {0xC3,"NOT"},
          {0xC4,"BIN"},
          {0xC5,"OR"},
          {0xC6,"AND"},
          {0xC7,"<="},
          {0xC8,">="},
          {0xC9,"<>"},
          {0xCA,"LINE"},
          {0xCB,"THEN"},
          {0xCC,"TO"},
          {0xCD,"STEP"},
          {0xCE,"DEF FN"},
          {0xCF,"CAT"},
          {0xD0,"FORMAT"},
          {0xD1,"MOVE"},
          {0xD2,"ERASE"},
          {0xD3,"OPEN #"},
          {0xD4,"CLOSE #"},
          {0xD5,"MERGE"},
          {0xD6,"VERIFY"},
          {0xD7,"BEEP"},
          {0xD8,"CIRCLE"},
          {0xD9,"INK"},
          {0xDA,"PAPER"},
          {0xDB,"FLASH"},
          {0xDC,"BRIGHT"},
          {0xDD,"INVERSE"},
          {0xDE,"OVER"},
          {0xDF,"OUT"},
          {0xE0,"LPRINT"},
          {0xE1,"LLIST"},
          {0xE2,"STOP"},
          {0xE3,"READ"},
          {0xE4,"DATA"},
          {0xE5,"RESTORE"},
          {0xE6,"NEW"},
          {0xE7,"BORDER"},
          {0xE8,"CONTINUE"},
          {0xE9,"DIM"},
          {0xEA,"REM"},
          {0xEB,"FOR"},
          {0xEC,"GO TO"},
          {0xED,"GO SUB"},
          {0xEE,"INPUT"},
          {0xEF,"LOAD"},
          {0xF0,"LIST"},
          {0xF1,"LET"},
          {0xF2,"PAUSE"},
          {0xF3,"NEXT"},
          {0xF4,"POKE"},
          {0xF5,"PRINT"},
          {0xF6,"PLOT"},
          {0xF7,"RUN"},
          {0xF8,"SAVE"},
          {0xF9,"RANDOMIZE"},
          {0xFA,"IF"},
          {0xFB,"CLS"},
          {0xFC,"DRAW"},
          {0xFD,"CLEAR"},
          {0xFE,"RETURN"},
          {0xFF,"COPY"}
        };
        private class FileHeader {
            byte flag;
            byte dataType;
            //byte[] fileName = new byte[10];
            string fileName;
            UInt16 dataLength;
            UInt16 dataExtens;
            //various for header type (for CODE = start)
            UInt16 unused;
            byte checksum;

            UInt16 DataExtens {
                get {return dataExtens;}
                set {dataExtens = value;}
            }

            public FileHeader()
            {}
            public FileHeader(byte[] headBytes)
            {
                flag = headBytes[0];
                dataType = headBytes[1];
                //Array.Copy(headBytes,2,fileName,0,10);

                StringBuilder sb = new StringBuilder();
                for (int i = 2; i < 12; i++)
                {
                    if (headBytes[i] >= 0xA0)
                    {
                    sb.Append(zxTokens[headBytes[i]]);


                    }
                    sb.Append(Encoding.UTF8.GetString(headBytes,i,1));

                }

                //fileName = Encoding.UTF8.GetString(headBytes,2,10);
                dataLength = BitConverter.ToUInt16(headBytes,12);
                dataExtens = BitConverter.ToUInt16(headBytes,14);
                checksum = headBytes[18];
            }

        }
        private FileStream input;
        private FileStream output;
        byte[] tapWord = new byte[2];
        byte[] tapFileName = new byte[10];
        byte[] tapHeaderBlock = new byte[19];
        byte[] tapDataBlock;
        byte tapDataBlockFlag;
        int tapHeaderCodeLength;
        int tapHeaderCodeStart;
        int tapBlockLength;
        int tapCheckSum;
        int ret;

        public void ExportCodeBlocks(string tapFileNamePath)
        {
            input = File.OpenRead(tapFileNamePath);

            while ((ret = input.Read(tapWord, 0, 2)) == 2)
            {
                tapBlockLength = BitConverter.ToUInt16(tapWord,0);
                ReadTapBlock(tapBlockLength);

                // check last block type 
                if (tapDataBlockFlag == 255)
                {
                    // last block was CODE/SCREEN$ 
                    string fileName = SaveDataBlock();

#region Converting
                    // Converter converter = new Converter();
                    // CodingType coding = CodingType.zxGraphics;
                    // switch (tapHeaderCodeStart)
                    // {
                    //     case 32768: 
                    //         coding = CodingType.dTextCZ;
                    //         break;
                    //     case 32000:
                    //         coding = CodingType.tasswordCZ;
                    //         break;
                    // }
                    // converter.Convert(
                    //     Path.Combine("files",fileName),
                    //     Path.Combine("files",fileName + ".txt"),
                    //     coding);
#endregion                        
                    
                }
            }
        }

        private void ReadTapBlock(int tapBlockLength)
        {
                
                // First byte from data block = flag
                tapDataBlockFlag = (byte)input.ReadByte(); 
                if (tapDataBlockFlag == 0 && tapBlockLength == 19)
                {
                    // header 
                    tapHeaderBlock[0] = tapDataBlockFlag;
                   
                    // read rest header values (without tapDataBlockFlag )
                    input.Read(tapHeaderBlock,1,tapBlockLength - 1);

                    tapHeaderCodeLength = BitConverter.ToUInt16(tapHeaderBlock,12);
                    if (tapHeaderBlock[1] == 3) // CODE and SCREEN data block
                        tapHeaderCodeStart = BitConverter.ToUInt16(tapHeaderBlock,14);

                    FileHeader header = new FileHeader(tapHeaderBlock);
                }

                // flag of header data block (data for last header)
                if (tapDataBlockFlag == 0xFF && ((tapBlockLength -2) == tapHeaderCodeLength))
                {
                    tapDataBlock = new byte[tapHeaderCodeLength];
                    input.Read(tapDataBlock,0,tapHeaderCodeLength);
                    tapCheckSum = input.ReadByte();
                }


        }

        private string SaveDataBlock()
        {
            // ZX file name conversion (codes > 0x7F)
            Array.Copy(tapHeaderBlock,2,tapFileName,0,10);

            for (int i = 0; i < 10; i++)
            {
                if (tapFileName[i] > 0x7F || (tapFileName[i] < 0x20))
                {
                    // replace CODES from BASIC TOKENS etc.
                    tapFileName[i] = (byte)'_';
                }
            }

            string fileExtension  = ".xxx"; // unknown
            if (tapHeaderCodeStart == 32000)
            {
                fileExtension  = ".twt"; // tasword
            }
            else if  (tapHeaderCodeStart == 32768)
            {
                fileExtension  = ".dtt"; // d-text (spectral writer)
            }

            string fileName = Path.Combine(Encoding.UTF8.GetString(tapFileName) + fileExtension);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            output = File.OpenWrite(Path.Combine("files",fileName));
            output.Write(tapDataBlock, 0, tapDataBlock.Length);
            output.Flush();
            output.Dispose();

            Console.WriteLine("Saved: " + fileName);

            return fileName;

        }

    }

}