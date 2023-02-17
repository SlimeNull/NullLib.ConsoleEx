using System;
using System.Linq;
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
        
        static async Task Main(string[] args)
        {
            while(true)
            {
                string qwq = await ConsoleSc.ReadLineAsync();
                int size = qwq.Select(c => ConsoleText.IsFullWidthChar(c) ? 2 : 1).Sum();
                Console.WriteLine(size);
            }
        }
    }
}
