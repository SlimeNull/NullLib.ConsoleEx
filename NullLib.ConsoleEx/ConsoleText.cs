using System;
using System.Collections;
using System.Collections.Generic;

namespace NullLib.ConsoleEx
{
    public static class ConsoleText
    {
        public static bool IsFullWidthChar(char c)
        {
            return (c >= 0x1100 &&
                (c <= 0x115f ||  // Hangul Jamo init. consonants
                 c == 0x2329 || c == 0x232a ||
                 (c >= 0x2e80 && c <= 0xa4cf &&
                  c != 0x303f) || // CJK ... Yi
                 (c >= 0xac00 && c <= 0xd7a3) || // Hangul Syllables
                 (c >= 0xf900 && c <= 0xfaff) || // CJK Compatibility Ideographs
                 (c >= 0xfe10 && c <= 0xfe19) || // Vertical forms
                 (c >= 0xfe30 && c <= 0xfe6f) || // CJK Compatibility Forms
                 (c >= 0xff00 && c <= 0xff60) || // Fullwidth Forms
                 (c >= 0xffe0 && c <= 0xffe6) ||
                 (c >= 0x20000 && c <= 0x2fffd) ||
                 (c >= 0x30000 && c <= 0x3fffd)));
        }

        public static int CalcCharLength(char c)
        {
            return c switch
            {
                '\0' => 0,
                _ => IsFullWidthChar(c) ? 2 : 1
            };
        }

        public static int CalcStringLength(string str)
        {
            int len = 0;
            foreach (char c in str)
                len += CalcCharLength(c);
            return len;
        }

    }
}
