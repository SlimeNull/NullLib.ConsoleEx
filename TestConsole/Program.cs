using System;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using NullLib.ConsoleEx;

namespace TestConsole
{
    class Program
    {
        static Program()
        {
            ConsoleSc.Prompt = ">>> ";
        }
        static int GetCharLen(char c)
        {
            UnicodeRanges.As    //qwq
            UnicodeRange combiningHalfMarks = UnicodeRanges.CombiningHalfMarks;
            if (combiningHalfMarks.FirstCodePoint <= c && combiningHalfMarks.Length < c - combiningHalfMarks.FirstCodePoint)
                return 1;
            return 2;
        }
        static int GetStrLen(string str)
        {
            int rst = 0;
            foreach (var c in str)
                rst += GetCharLen(c);
            return rst;
            
        }
        static async Task Main(string[] args)
        {
            _ = Task.Run(() =>
            {
                while (true)
                {
                    ConsoleSc.WriteLine("QWQ\tQAQ");
                    Thread.Sleep(3000);
                }
            });
            while(true)
            {
                
            }
        }
    }
}
