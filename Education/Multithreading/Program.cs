using System;
using System.Threading;
using System.IO;

namespace Multithreading
{
    public delegate string DisplayHandler(string path);
    class Program
    {
        static Timer timer;
        static string previousText, currentText, result;
        static void Main(string[] args)
        {
            previousText = "";
            currentText = "";
            string pathToFile = "test.txt";
            using (StreamReader sr = new StreamReader(pathToFile))
            {
                currentText = sr.ReadToEnd();
            }
            DisplayHandler handler = new DisplayHandler(StartTimer);
            Console.WriteLine("Changes check is on");
            IAsyncResult resultObj = handler.BeginInvoke(pathToFile, new AsyncCallback(AsyncCompleted), null);            
            Console.ReadLine();
        }
        public static void Check(object obj)
        {
            previousText = currentText;
            string path = (string)obj;
            using (StreamReader sr = new StreamReader(path))
            {
                currentText = sr.ReadToEnd();
                if (currentText != previousText)
                {
                    result = currentText;
                    Console.WriteLine("File was changed " + DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString());
                    Console.WriteLine("Last saved text:" + Environment.NewLine + previousText);
                    Console.WriteLine("Current text:");
                    Console.WriteLine(result + Environment.NewLine);
                }
            }
        }
        static string StartTimer(string path)
        {
            TimerCallback tm = new TimerCallback(Check);
            timer = new Timer(tm, path, 0, 1000);
            return result;
        }
        static void AsyncCompleted(IAsyncResult resObj)
        {
            string message = (string)resObj.AsyncState;
            Console.WriteLine(message);
        }
    }
}
