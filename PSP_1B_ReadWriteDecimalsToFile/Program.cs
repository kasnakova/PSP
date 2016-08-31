/********************************************************************************************************************/
/* Program Assignment:  1B - 4B                                                                                     */
/* Name:                Elizaveta Kasnakova                                                                         */
/* Date:                22.08.2016 - 31.08.2016                                                                     */
/* Description:         Collects and validates user input from the console                                          */
/********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace PSP_1B_ReadWriteDecimalsToFile
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //TODO: ask what tool has to be used for the modified, etc. lines
            string fileName = GetFileName();
            string mode = GetMode();

            if (mode == "R")
            {
                var numbers = Read(fileName);
                PrintNumbers(numbers);
            }
            else if(mode == "W")
            {
                var numbers = GetNumbersFromUser();
                Write(fileName, numbers);
            } 
            else if(mode == "M")
            {
                var numbers = Read(fileName);
                Modify(numbers, fileName);
            }
        }

        private static void PrintNumbers(List<double> numbers)
        {
            foreach (var num in numbers)
            {
                Console.WriteLine(num);
            }
        }

        private static void Modify(List<double> numbers, string fileName)
        {
            Console.WriteLine("For every number please choose one of the following actions:");
            Console.WriteLine("A - accept");
            Console.WriteLine("R - replace");
            Console.WriteLine("D - delete");
            Console.WriteLine("IA - insert after the current number");
            Console.WriteLine("IB - insert before the current number");
            Console.WriteLine("AA - accept this number and all the remaining numbers");
            var modifiedNumbers = new List<double>();
           
            for(int i = 0; i < numbers.Count; i++)
            {
                var num = numbers[i];
                Console.WriteLine(num);
                var action = GetModifyAction();
                switch (action)
                {
                    case "A":
                        modifiedNumbers.Add(num);
                        break;
                    case "R":
                        Console.WriteLine("Enter the number with which you want to replace:");
                        var replacedNum = GetNumber();
                        modifiedNumbers.Add(replacedNum);
                        break;
                    case "D":
                        //we just don't add it to the list of modified values
                        break;
                    case "IA":
                        Console.WriteLine("Enter the new number");
                        var newNumAfter = GetNumber();
                        modifiedNumbers.Add(num);
                        modifiedNumbers.Add(newNumAfter);
                        break;
                    case "IB":
                        Console.WriteLine("Enter the new number");
                        var newNumBefore = GetNumber();
                        modifiedNumbers.Add(newNumBefore);
                        modifiedNumbers.Add(num);
                        break;
                    case "AA":
                        while(i < numbers.Count)
                        {
                            modifiedNumbers.Add(numbers[i]);
                            i++;
                        }

                        break;
                    default:
                        DisplayErrorMessage("Invalid action!");
                        break;
                }
            }

            Console.WriteLine("If you want to save the modified numbers to a new file, enter its name. If not just press enter");
            var newFileName = Console.ReadLine();
            if (string.IsNullOrEmpty(newFileName))
            {
                newFileName = fileName;
            }
            else
            {
                newFileName += ".bin";
            }

            Write(newFileName, modifiedNumbers);
        }

        private static void Write(string fileName, List<double> numbers)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                foreach (var num in numbers)
                {
                    writer.Write(num);
                }

                Console.WriteLine("Numbers successfully written to file.");
            }
        }

        private static List<double> Read(string fileName)
        {
            var numbers = new List<double>();
            while (!File.Exists(fileName))
            {
                DisplayErrorMessage("The file you entered doesn't exist!");
                fileName = GetFileName();
            }

            FileInfo info = new FileInfo(fileName);
            long quantity = info.Length / sizeof(double);
            if (quantity == 0)
            {
                DisplayErrorMessage("File is empty!");
            }
            else
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    for (int i = 0; i < quantity; i++)
                    {
                        double number = reader.ReadDouble();
                        numbers.Add(number);
                    }
                }
            }

            return numbers;
        }

        private static List<double> GetNumbersFromUser()
        {
            int quantity = GetQuantity();
            var numbers = new List<double>();
            Console.WriteLine("Enter the numbers, seperating them by hitting enter");
            for (int i = 0; i < quantity; i++)
            {
                double number = GetNumber();
                numbers.Add(number);
            }

            return numbers;
        } 

        private static double GetNumber()
        {
            double num;
            while (!double.TryParse(Console.ReadLine(), out num))
            {
                DisplayErrorMessage("Enter a valid real number!");
            }

            return num;
        }

        private static int GetQuantity()
        {
            Console.WriteLine("Enter the quantity of numbers to be written to the file");
            int quantity = 0;
            while (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                DisplayErrorMessage("Please, enter a valid, positive and greater than 0 integer number for quantity!");
            }

            return quantity;
        }
        
        private static string GetMode()
        {
            Console.WriteLine("Enter the mode you want:\nR - read\nW - write\nM - modify");
            string mode = Console.ReadLine().ToUpper();
            while (string.IsNullOrEmpty(mode) || (mode != "R" && mode != "W" && mode != "M"))
            {
                DisplayErrorMessage("Please enter an existing mode in the format described!");
                mode = Console.ReadLine().ToUpper();
            }

            return mode;
        }

        private static string GetModifyAction()
        {
            var actions = new List<string>(new string[]{ "A", "R", "D", "IA", "IB", "AA"});
            string action = Console.ReadLine().ToUpper();
            while (string.IsNullOrEmpty(action) || !actions.Contains(action))
            {
                DisplayErrorMessage("Please enter an existing action in the format described!");
                action = Console.ReadLine().ToUpper();
            }

            return action;
        }

        private static string GetFileName()
        {
            Console.WriteLine("Please, enter the file name");
            string fileName = Console.ReadLine();
            while (string.IsNullOrEmpty(fileName))
            {
                DisplayErrorMessage("Please enter a file name!");
                fileName = Console.ReadLine();
            }

            return fileName + ".bin";
        }
    
        private static void DisplayErrorMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
