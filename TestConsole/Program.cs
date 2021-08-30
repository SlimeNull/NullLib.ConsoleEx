using System;
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


            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(2000);
                    ConsoleSc.WriteLine("嘿嘿");
                }
            });
            while(true)
            {
                string instr = ConsoleSc.ReadLine();
                ConsoleSc.WriteLine(instr);
            }
        }
    }
}
