using System;
using System.IO;

namespace Find
{
    class Program
    {
        //파일 대상 : HWP, DOC, PPT, PPTX, DOCX, PDF
        public static byte[] HWP_DOC_PPT_File_Header = new byte[8] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
        public static byte[] PPTX_DOCX_File_Header = new byte[4] { 0x50, 0x4B, 0x03, 0x04 };
        public static byte[] PDF_File_Header = new byte[4] { 0x25, 0x50, 0x44, 0x46 };

        static void Main(string[] args)
        {

            String file_path = @"C:\Users\admin\Downloads\testtt.doc";

            file_check(ref file_path);

        }
        public static void file_check(ref String filepath)
        {
            bool isfile= true;
            byte[] searchedFileHeaderByte;

            searchedFileHeaderByte = File.ReadAllBytes(filepath);

            if (HWP_DOC_PPT_File_Header.Length >= searchedFileHeaderByte.Length ||
                PPTX_DOCX_File_Header.Length >= searchedFileHeaderByte.Length ||
                PDF_File_Header.Length >= searchedFileHeaderByte.Length)
            {
                isfile = false;
            }
            else
            {
                for (int i = 0; i < HWP_DOC_PPT_File_Header.Length; i++)
                {
                    if (HWP_DOC_PPT_File_Header[i] != searchedFileHeaderByte[i])
                    {
                        isfile = false;
                        break;
                    }
                    isfile = true;
                }
                if (!isfile)
                {
                    for (int i = 0; i < PPTX_DOCX_File_Header.Length; i++)
                    {
                        if (PPTX_DOCX_File_Header[i] != searchedFileHeaderByte[i])
                        {
                            isfile = false;
                            break;
                        }
                        isfile = true;
                    }
                }
                if (!isfile)
                {
                    for (int i = 0; i < PDF_File_Header.Length; i++)
                    {
                        if (PDF_File_Header[i] != searchedFileHeaderByte[i])
                        {
                            isfile = false;
                            break;
                        }
                        isfile = true;
                    }
                }
            }
            if (isfile)
            {
                Console.WriteLine(filepath + "는 jpg입니다.\n");
            }
            else
            {
                Console.WriteLine(filepath + "는 jpg가 아닙니다.\n");
            }

        }


        
    }
}
