using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NonUtfDetector
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = ".";
            string[] fileTypes = new string[0];


            if(args.Length < 2)
            {
                Console.WriteLine("Usage:\r\nNonUtfDetector.exe [path] [filetypes]");
                Console.WriteLine("Example:\r\nNonUtfDetector.exe D:\\project php,txt,xml,dist");
                Environment.Exit(-1);
            }
            else
            {
                path = args[0];
                string[] fileTypesCandidates = args[1].Split(',');
                fileTypes = new string[fileTypesCandidates.Length];
                for (int i = 0; i < fileTypesCandidates.Length; i++)
                {
                    fileTypes[i] = "." + fileTypesCandidates[i];
                }

            }

            if(Directory.Exists(path) == false) {
                Console.WriteLine("[ERROR] Provided directory does not exist.");
                Environment.Exit(-1);
            }

            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            foreach (string filePath in files)
            {
                if(fileTypes.Contains(Path.GetExtension(filePath)) == false) {
                    continue;
                }
                byte[] source = File.ReadAllBytes(filePath);

                Encoding utf16 = Encoding.GetEncoding("macintosh");

                //expect utf8 and convert to utf16
                byte[] bytes16 = Encoding.Convert(Encoding.UTF8, utf16, source);

                //convert to string
                string string16 = utf16.GetString(bytes16);

                //convert back to UTF-8
                byte[] newBytes16 = utf16.GetBytes(string16);
                byte[] newBytes8 = Encoding.Convert(utf16, Encoding.UTF8, newBytes16);

                //try to get string out of it (just for testing)
                string newString8 = Encoding.UTF8.GetString(newBytes8);
                string oldString8 = Encoding.UTF8.GetString(source);

                //now check if original code matches the new one
                bool valid = true; //TODO

                StringReader newFileReader = new StringReader(newString8);
                Dictionary<int, string> fileByLines = new Dictionary<int, string>();
                {
                    int i = 0;
                    while (newFileReader.Peek() != -1)
                    {
                        fileByLines.Add(i++, newFileReader.ReadLine());
                    }
                    newFileReader.Close();
                }
                {
                    StringReader oldFileReader = new StringReader(oldString8);
                    int i = 0;
                    string buffer = filePath;
                    bool showBuffer = false;
                    while (oldFileReader.Peek() != -1)
                    {


                        string line = oldFileReader.ReadLine();
                        char[] newChars = fileByLines[i++].ToArray();
                        char[] characters = line.ToArray();
                        for(int j=0;j<characters.Length;j++)
                        {
                            if (characters[j] != newChars[j])
                            {
                                buffer += "\r\n[" + i + ":" + j + "] " + line;// +newChars[j] + "  :  " + characters[j];
                                showBuffer = true;
                            }
                        }


                    }
                    if (showBuffer)
                    {
                        Console.WriteLine(buffer);
                        Console.WriteLine();
                    }
                    oldFileReader.Close();
                }





                //Console.WriteLine(filePath + "\r\n" + );



                /*
                byte[] fileBytes = File.ReadAllBytes(filePath);
                string converted = "";
                try
                {
                    converted = Encoding.UTF8.GetString(fileBytes);

                    if (XmlConvert.VerifyXmlChars(converted) == null)
                    {
                        Console.WriteLine("E: " + filePath);
                    }


                } catch(Exception e)
                {
                    Console.WriteLine("E: " + filePath);
                }
                */
            }
        }
    }
}

