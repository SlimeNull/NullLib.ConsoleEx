using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NullLib.ConsoleEx.DynamicText
{
    public static class ConsoleSc
    {
        private const ConsoleKey defaultReadUntilKey = ConsoleKey.Enter;
        private static string newLine = Console.Out.NewLine;
        static bool isReading;
        static bool notIntercept;
        static bool insertMode = false;
        static int readIndex = 0;
        static int readHistoryIndex = 0;
        static string readStr;
        static List<string> readHistory = new List<string>();
        static StringBuilder readBuffer = new StringBuilder();
        static ConsoleKey readUntil;

        static object textWriteLock = new object();

        static int
            r_startLeft, r_startTop,
            r_cursorLeft, r_cursorTop,
            r_endLeft, r_endTop,
            w_cursorLeft, w_cursorTop,
            w_endLeft, w_endTop;

        public static StringBuilder ReadBuffer => readBuffer;
        public static bool IsReading => isReading;
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
        private static void FlushInput()
        {
            if (readHistoryIndex == readBuffer.Length)
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
        private static void RenderReadText()
        {
            lock (textWriteLock)
            {
                string before = readBuffer.ToString(0, readIndex);
                string after = readBuffer.ToString(readIndex, readBuffer.Length - readIndex);

                SetCursorVisible(false);
                Console.SetCursorPosition(r_startLeft, r_startTop);

                Console.Write(before);
                r_cursorLeft = Console.CursorLeft;
                r_cursorTop = Console.CursorTop;

                Console.Write(after);
                int spaceEx = MeasureSpace(r_cursorLeft, r_cursorTop, r_endLeft, r_endTop);
                r_endLeft = Console.CursorLeft;
                r_endTop = Console.CursorTop;

                if (spaceEx > 0)
                    Console.Write(new string(' ', spaceEx));

                if (insertMode)
                    Console.CursorSize = 100;
                else
                    Console.CursorSize = 10;

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
                        insertMode ^= true;
                        break;
                    case ConsoleKey.Home:
                        SetReadIndex(0);
                        break;
                    case ConsoleKey.End:
                        SetReadIndex(readBuffer.Length);
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
            if (insertMode && readIndex < readBuffer.Length)
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
        public static async Task<string> ReadLineAsync(ConsoleKey until, bool intercept)
        {
            await Task.Run(WaitForInput);

            readIndex = 0;
            isReading = true;
            readUntil = until;
            r_startLeft = Console.CursorLeft;
            r_startTop = Console.CursorTop;
            readBuffer.Clear();
            notIntercept = !intercept;

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
        public static Task<string> ReadLineAsync(bool intercept) => ReadLineAsync(defaultReadUntilKey, intercept);
        public static Task<string> ReadLineAsync() => ReadLineAsync(defaultReadUntilKey, false);
        public static string ReadLine(ConsoleKey until, bool intercept) => ReadLineAsync(until, intercept).Result;
        public static string ReadLine(bool intercept) => ReadLineAsync(defaultReadUntilKey, intercept).Result;
        public static string ReadLine() => ReadLineAsync(defaultReadUntilKey, false).Result;
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
        public static void Write(string text, string end, bool renderRead)
        {
            lock (textWriteLock)
            {
                SetCursorVisible(false);
                Console.SetCursorPosition(r_startLeft, r_startTop);
                Console.Write(text);

                w_cursorLeft = Console.CursorLeft;
                w_cursorTop = Console.CursorTop;

                int spaceEx = MeasureSpace(w_cursorLeft, w_cursorTop, r_endLeft, r_endTop);
                if (spaceEx > 0)
                    Console.Write(new string(' ', spaceEx));

                Console.SetCursorPosition(w_cursorLeft, w_cursorTop);
                Console.Write(end);

                w_endLeft = Console.CursorLeft;
                w_endTop = Console.CursorTop;

                r_startLeft = w_endLeft;
                r_startTop = w_endTop;

                SetCursorVisible(true);
            }

            if (renderRead && isReading && notIntercept)
                RenderReadText();
        }
        public static void WriteLine(string text, bool renderRead) => Write(text, newLine, renderRead);
        public static void Write(string text, bool renderRead) => Write(text, null, renderRead);
        public static void WriteLine(string text) => Write(text, newLine, true);
        public static void Write(string text) => Write(text, null, true);
        public static ConsoleKeyInfo ReadKey() => Console.ReadKey();
        public static ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);
    }
}
