using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MultiThreading
{
    class Program
    {
        static int runCount = 0;

        private static string folderPath;

        //creat output folder, each run will have a new folder
        static void InitializeOutputFolder()
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string targetPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(rootPath).FullName).FullName).FullName).FullName).FullName;

            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            folderPath = Path.Combine(targetPath, $"Program_Output_{timeStamp}");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine($"Created output folder at: {folderPath}");
            }
        }

        static void Main(string[] args)
        {
            InitializeOutputFolder();

            while (true)
            {
                runCount++;

                int[] numbers = randomArray(10, 1, 100);

                Thread thread1 = new Thread(() => TotalSum(numbers));
                Thread thread2 = new Thread(() => FindDivisibleBy(numbers, 6));
                Thread thread3 = new Thread(() => StoreInFile(numbers, Path.Combine(folderPath, $"output_run_{runCount}.txt")));

                thread1.Start();
                thread2.Start();
                thread3.Start();

                //wait for all threads to complete
                thread1.Join();
                thread2.Join();
                thread3.Join();

                Console.WriteLine("All threads are done." +
                    "\n\nPress any key to rerun the program or close the console to exit...");

                Console.ReadKey();
            }
        }

        static int[] randomArray(int length, int minValue, int maxValue)
        {
            Random random = new Random();
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = random.Next(minValue, maxValue);
            }
            return array;
        }

        //sum
        static void TotalSum(int[] nums)
        {
            int sum = 0;
            foreach (var num in nums)
            {
                sum += num;
            }
            Console.WriteLine($"\nTotal sum: {sum}");
        }

        static void FindDivisibleBy(int[] nums, int divisor)
        {
            List<int> divisibleNumbers = new List<int>();
            foreach (var num in nums)
            {
                if (num % divisor == 0)
                {
                    divisibleNumbers.Add(num);
                }
            }

            if (divisibleNumbers.Count == 1)
            {
                Console.WriteLine($"{divisibleNumbers[0]} is divisible by {divisor}");
            }
            else
            {
                Console.WriteLine($"{string.Join(" ", divisibleNumbers)} are divisible by {divisor}");
            }
        }

        //output
        static void StoreInFile(int[] nums, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var num in nums)
                {
                    writer.WriteLine(num);
                }
            }
            Console.WriteLine($"All stored in file: {fileName}");
        }
    }
}
