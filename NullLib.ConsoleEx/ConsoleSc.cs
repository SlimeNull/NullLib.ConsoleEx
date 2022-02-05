using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NullLib.ConsoleEx
{
    public static class ConsoleSc
    {
        private const ConsoleKey readlineUntilKey = ConsoleKey.Enter;
        static bool isReading;
        static bool notIntercept;
        static bool overwriteMode = false;
        static int readIndex = 0;
        static int readHistoryIndex = 0;
        static string readStr;
        static readonly List<string> readHistory = new List<string>();
        static readonly StringBuilder readBuffer = new StringBuilder();
        static ConsoleKey readUntil;

        static readonly object textWriteLock = new object();
        static string prompt;

        static int
            r_startLeft, r_startTop,
            r_cursorLeft, r_cursorTop,
            r_endLeft, r_endTop,
            w_cursorLeft, w_cursorTop,
            w_endLeft, w_endTop;

        public static string Prompt { get => prompt; set => prompt = value; }

        public static StringBuilder ReadBuffer => readBuffer;
        public static bool IsReading => isReading;
        public static bool IsOverwriteMode => overwriteMode;
        public static int ReadStartLeft => r_startLeft;
        public static int ReadStartTop => r_startTop;

        private static void SwitchInputHistory(int offset)
        {
            readHistoryIndex += offset;
            int inputHistoryCount = readHistory.Count;
            if (readHistoryIndex < 0)
                readHistoryIndex = 0;
            else if (readHistoryIndex < inputHistoryCount)
            {
                readBuffer.Clear();
                readBuffer.Append(readHistory[readHistoryIndex]);
                readIndex = readBuffer.Length;
            }
            else if (readHistoryIndex >= inputHistoryCount)
            {
                readBuffer.Clear();
                readHistoryIndex = inputHistoryCount;
                readIndex = 0;
            }

        }
        private static void MoveReadIndex(int offset)
        {
            readIndex += offset;
            ReCorrectInputIndex();
        }
        private static void SetReadIndex(int index)
        {
            readIndex = index;
            ReCorrectInputIndex();
        }
        private static void ProcBackspaceKey(bool ctrl = false)
        {
            try
            {
                if (ctrl)
                {
                    readBuffer.Remove(0, readIndex);
                    SetReadIndex(0);
                }
                else
                {
                    readBuffer.Remove(readIndex - 1, 1);
                    MoveReadIndex(-1);
                }
            }
            catch { }
        }
        private static void ProcDeleteKey(bool ctrl = false)
        {
            try
            {
                if (ctrl)
                {
                    readBuffer.Remove(readIndex, readBuffer.Length - readIndex);
                }
                else
                {
                    readBuffer.Remove(readIndex, 1);
                }
            }
            catch { }
        }
        private static void ClearInput()
        {
            readBuffer.Clear();
            SetReadIndex(0);
        }
        private static void FlushInput()
        {
            if (readHistoryIndex == readHistory.Count)
                readHistoryIndex++;

            readStr = readBuffer.ToString();
            readHistory.Add(readStr);
            isReading = false;
        }
        private static void SetCursorVisible(bool value)
        {
            try
            {
                Console.CursorVisible = value;
            }
            catch { }
        }
        private static void SetCursorSize(int size)
        {
            try
            {
                Console.CursorSize = size;
            }
            catch { }
        }
        private static void RenderReadText()
        {
            TextWriter stdout = Console.Out;
            lock (textWriteLock)
            {
                string before = readBuffer.ToString(0, readIndex);
                string after = readBuffer.ToString(readIndex, readBuffer.Length - readIndex);

                SetCursorVisible(false);
                Console.SetCursorPosition(r_startLeft, r_startTop);

                int
                    r_lastEndLeft = r_endLeft,
                    r_lastEndTop = r_endTop;


                stdout.Write(prompt);
                stdout.Write(before);
                r_cursorLeft = Console.CursorLeft;
                r_cursorTop = Console.CursorTop;

                stdout.Write(after);
                r_endLeft = Console.CursorLeft;
                r_endTop = Console.CursorTop;

                int spaceEx = MeasureSpace(r_endLeft, r_endTop, r_lastEndLeft, r_lastEndTop);

                if (spaceEx > 0)
                    stdout.Write(new string(' ', spaceEx));

                if (overwriteMode)
                    SetCursorSize(100);
                else
                    SetCursorSize(25);

                Console.SetCursorPosition(r_cursorLeft, r_cursorTop);
                SetCursorVisible(true);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>if not append the current char to buffer</returns>
        private static bool DealSpecialKey(ConsoleKeyInfo keyInfo)
        {
            ConsoleModifiers modifiers = keyInfo.Modifiers;
            ConsoleKey key = keyInfo.Key;

            if (key.Equals(readUntil))
                FlushInput();
            else
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        SwitchInputHistory(-1);
                        break;
                    case ConsoleKey.DownArrow:
                        SwitchInputHistory(1);
                        break;
                    case ConsoleKey.LeftArrow:
                        MoveReadIndex(-1);
                        break;
                    case ConsoleKey.RightArrow:
                        MoveReadIndex(1);
                        break;
                    case ConsoleKey.Backspace:
                        ProcBackspaceKey(modifiers.PrsControl());
                        break;
                    case ConsoleKey.Delete:
                        ProcDeleteKey(modifiers.PrsControl());
                        break;
                    case ConsoleKey.Insert:
                        overwriteMode ^= true;
                        break;
                    case ConsoleKey.Home:
                        SetReadIndex(0);
                        break;
                    case ConsoleKey.End:
                        SetReadIndex(readBuffer.Length);
                        break;
                    case ConsoleKey.Escape:
                        ClearInput();
                        break;
                    default:
                        return false;
                }

            return true;
        }
        private static void WaitForInput()
        {
            while (isReading) ;
        }
        private static void PutInputChar(char c)
        {
            if (overwriteMode && readIndex < readBuffer.Length)
                readBuffer[readIndex] = c;
            else
                readBuffer.Insert(readIndex, c);
            MoveReadIndex(1);
        }
        private static int MeasureSpace(int fromLeft, int fromTop, int toLeft, int toTop)
        {
            return Console.BufferWidth * (toTop - fromTop) + (toLeft - fromLeft);
        }

        private static bool PrsControl(this ConsoleModifiers self) => self.HasFlag(ConsoleModifiers.Control);

        private static void ReCorrectInputIndex()
        {
            if (readIndex < 0)
                readIndex = 0;
            if (readIndex > readBuffer.Length)
                readIndex = readBuffer.Length;
        }
        public static async Task<string> ReadAsync(ConsoleKey until, bool intercept)
        {
            await Task.Run(WaitForInput);

            readIndex = 0;
            isReading = true;
            readUntil = until;
            r_startLeft = Console.CursorLeft;
            r_startTop = Console.CursorTop;
            readBuffer.Clear();
            notIntercept = !intercept;

            if (notIntercept)
                RenderReadText();

            await Task.Run(() =>
            {
                while (isReading)
                {
                    ConsoleKeyInfo readKey = Console.ReadKey(true);
                    if (!DealSpecialKey(readKey))
                        PutInputChar(readKey.KeyChar);

                    if (notIntercept)
                        RenderReadText();
                }
            });

            Console.WriteLine();
            return readBuffer.ToString();
        }
        public static Task<string>  ReadLineAsync(bool intercept) => ReadAsync(readlineUntilKey, intercept);
        public static Task<string> ReadLineAsync() => ReadAsync(readlineUntilKey, false);
        public static string ReadLine(bool intercept) => ReadAsync(readlineUntilKey, intercept).Result;
        public static string ReadLine() => ReadAsync(readlineUntilKey, false).Result;
        public static int Read() => Console.Read();
        public static char ReadChar() => ReadChar(false);
        public static char ReadChar(bool intercept)
        {
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(intercept);
            }
            while (char.IsControl(keyInfo.KeyChar));
            return keyInfo.KeyChar;
        }

        public static async Task WriteAsync(string text, string end, bool renderRead)
        {
            TextWriter stdout = Console.Out;

            Monitor.Enter(textWriteLock);                                      // 文本写入锁

            SetCursorVisible(false);                                           // 隐藏光标

            if (isReading)
                Console.SetCursorPosition(r_startLeft, r_startTop);            // 重置光标位置到 ReadLine 时的起始位置
            await stdout.WriteAsync(text);                                     // 将要写的内容输出

            w_cursorLeft = Console.CursorLeft;                                 // 记录末尾坐标
            w_cursorTop = Console.CursorTop;

            int spaceEx = MeasureSpace(w_cursorLeft, w_cursorTop, r_endLeft, r_endTop);
            if (spaceEx > 0)
                await stdout.WriteAsync(new string(' ', spaceEx));             // 填充剩余内容

            Console.SetCursorPosition(w_cursorLeft, w_cursorTop);              // 回到记录的末尾坐标
            await stdout.WriteAsync(end);

            w_endLeft = Console.CursorLeft; 
            w_endTop = Console.CursorTop;

            r_startLeft = w_endLeft;
            r_startTop = w_endTop;

            SetCursorVisible(true);

            Monitor.Exit(textWriteLock);

            if (renderRead && isReading && notIntercept)
                RenderReadText();
        }
        public static Task WriteLineAsync(string text, bool renderRead) => WriteAsync(text, Console.Out.NewLine, renderRead);
        public static Task WriteLineAsync(string text) => WriteAsync(text, Console.Out.NewLine, true);
        public static Task WriteAsync(string text, bool renderRead) => WriteAsync(text, null, renderRead);
        public static Task WriteAsync(string text) => WriteAsync(text, null, true);
        public static void WriteLine(string text, bool renderRead) => WriteAsync(text, Console.Out.NewLine, renderRead).Wait();
        public static void WriteLine(string text) => WriteAsync(text, Console.Out.NewLine, true).Wait();
        public static void WriteLine() => WriteAsync(null, null, true).Wait();
        public static void Write(string text, bool renderRead) => WriteAsync(text, null, renderRead).Wait();
        public static void Write(string text) => WriteAsync(text, null, true).Wait();
        public static ConsoleKeyInfo ReadKey() => Console.ReadKey();
        public static ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);
    }
}
