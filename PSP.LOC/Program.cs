/********************************************************************************************************************/
/* Program Assignment:  2A                                                                                          */
/* Name:                Elizaveta Kasnakova                                                                         */
/* Date:                24.08.2016                                                                                  */
/* Description:         Counts the logical lines of code in a specified by the user source file of a C# program     */
/********************************************************************************************************************/

using System;
using System.IO;

namespace PSP.LOC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var filePath = GetFilePath();
            var sourceCode = GetFileContents(filePath);
            var loc = GetLogicalLinesOfCode(sourceCode);
            Console.WriteLine("The count of logical lines of the specified source code is: ");
            Console.WriteLine(loc);
        }

        private static int GetLogicalLinesOfCode(string sourceCode)
        {
            int loc = 0;
            for (int i = 0; i < sourceCode.Length; i++)
            {
                var currSymbol = sourceCode[i];
                if(currSymbol == '/')
                {
                    i++;
                    if(sourceCode[i] == '/')
                    {
                        do
                        {
                            i++;
                        } while (sourceCode[i] != '\n');
                    }
                    else if(sourceCode[i] == '*')
                    {
                        do
                        {
                            i++;
                        } while (!(sourceCode[i] == '*' && sourceCode[i + 1] == '/'));
                        i++; //consume the '/'
                    }
                }
                else if(currSymbol == '"')
                {
                    do
                    {
                        i++;
                    } while (!(sourceCode[i] == '"' && sourceCode[i - 1] != '\\')); //loop until the end of the string, ignoring escaped double quotes (\") in it
                }
                else if(currSymbol == '\'')
                {
                    i += 2; //skip the char
                    if(sourceCode[i] != '\'')
                    {
                        i++; //if an escape sequence is used in the char it will be 2 physical symbols instead of one
                    }
                }
                else if(currSymbol == 'f' && sourceCode[i + 1] == 'o' && sourceCode[i + 2] == 'r' && !Char.IsLetter(sourceCode[i + 3])) 
                {
                    do
                    {
                        i++;
                    } while (sourceCode[i] != '{');
                    loc++; //we've reached a { so we count it //we've reached a { so we count it
                }
                else if (currSymbol == '{' || currSymbol == ';')
                {
                    loc++;
                }
            }

            return loc;
        }

        private static string GetFileContents(string filePath)
        {
            var contents = string.Empty;

            try
            {
                contents = File.ReadAllText(filePath);
            }
            catch
            {
                DisplayErrorMessage("The specified source code could not be read!");
            }

            return contents;
        }

        private static string GetFilePath()
        {
            Console.WriteLine("Please, enter the source code's path");
            var filePath = Console.ReadLine();
            while (!File.Exists(filePath))
            {
                DisplayErrorMessage("Please enter an existing path!");
                filePath = Console.ReadLine();
            }

            return filePath;
        }

        private static void DisplayErrorMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
