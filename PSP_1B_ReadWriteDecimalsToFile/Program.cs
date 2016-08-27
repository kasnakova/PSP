using System;
using System.IO;

namespace PSP_1B_ReadWriteDecimalsToFile
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string fileName = GetFileName();
            string mode = GetMode();

            if (mode == "R")
            {
                Read(fileName);
            }
            else
            {
                Write(fileName);
            }
        }

        private static void Write(string fileName)
        {
            int quantity = GetQuantity();
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                Console.WriteLine("Enter the numbers, seperating them by hitting enter");
                for (int i = 0; i < quantity; i++)
                {
                    double number = GetNumber();
                    writer.Write(number);
                }

                Console.WriteLine("Numbers successfully written to file.");
            }
        }

        private static void Read(string fileName)
        {
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
                        Console.WriteLine(number);
                    }
                }
            }
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
            Console.WriteLine("Enter the mode you want:\nR - read\nW - write");
            string mode = Console.ReadLine();
            while (string.IsNullOrEmpty(mode) || (mode != "R" && mode != "W"))
            {
                DisplayErrorMessage("Please enter an existing mode in the format described!");
                mode = Console.ReadLine();
            }

            return mode;
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
