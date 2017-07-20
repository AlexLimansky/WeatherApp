using System;
using System.Threading;
using System.IO;

namespace Multithreading
{
    public delegate void DisplayHandler(string path);
    class Program
    {
        static string previousText;
        static object locker = new object();
        static void Main(string[] args)
        {
            string pathToFile = "test.txt";
            using (StreamReader sr = new StreamReader(pathToFile))
            {
                previousText = sr.ReadToEnd();
            }           
            TimerCallback tm = new TimerCallback(Check);
            Timer timer = new Timer(tm, pathToFile, 0, 1000);          
            Console.ReadLine();
        }
        public static void Check(object obj)
        {
            string path = (string)obj;
            DisplayHandler handler = new DisplayHandler(DisplayDifference);
            IAsyncResult resultObj = handler.BeginInvoke(path, null, null);
        }
        static void DisplayDifference(string path)
        {
            lock(locker)
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string currentText = sr.ReadToEnd();
                    if (currentText != previousText)
                    {
                        Console.WriteLine("File was changed " + DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString());
                        Console.WriteLine("Last saved text:" + Environment.NewLine + previousText);
                        Console.WriteLine("Current text:");
                        Console.WriteLine(currentText + Environment.NewLine);
                    }
                    previousText = currentText;
                }
            }           
        }
    }
}
