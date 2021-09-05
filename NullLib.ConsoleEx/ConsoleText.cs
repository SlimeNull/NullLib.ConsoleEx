using System;
using System.Collections;
using System.Collections.Generic;

namespace NullLib.ConsoleEx
{
    public static class ConsoleText
    {
        private struct CharLenSeg : IComparable<CharLenSeg>
        {
            public int Start;
            public int End;
            public int Length;

            public CharLenSeg(int start, int end, int length)
            {
                Start = start;
                End = end;
                Length = length;
            }

            public int InRange(int c)
            {
                if (c < Start)
                    return c - Start;
                else if (c >= End)
                    return End - c + 1;
                else
                    return 0;
            }

            public int CompareTo(CharLenSeg other)
            {
                return Start.CompareTo(other.Start);
            }
        }
        private class CharLenSegs : IReadOnlyList<CharLenSeg>
        {
            CharLenSeg[] collection;
            public CharLenSegs(CharLenSeg[] collection)
            {
                this.collection = collection;
                Array.Sort(collection);
            }

            public CharLenSeg this[int index] => collection[index];

            public int Count => collection.Length;

            public CharLenSeg BinarySearch(char c)
            {
                int cIndex = c;
                int
                    low = 0,
                    high = Count,
                    middle;

                while (low <= high)
                {
                    middle = (low + high) / 2;

                    CharLenSeg curSeg = this[middle];

                    if (cIndex < curSeg.Start)
                    {
                        high = middle - 1;
                    }
                    else if (cIndex < curSeg.End)
                    {
                        return curSeg;
                    }
                    else
                    {
                        low = middle + 1;
                    }
                }

                return default;
            }

            public IEnumerator<CharLenSeg> GetEnumerator()
            {
                foreach (CharLenSeg each in collection)
                    yield return each;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return collection.GetEnumerator();
            }
        }

        private static readonly CharLenSegs AllCharLenSegs = new CharLenSegs(
            new CharLenSeg[]
            {
                new CharLenSeg(0, 7, 1),
                new CharLenSeg(7, 9, 0),
                new CharLenSeg(9, 10, 8),
                new CharLenSeg(10, 11, 0),
                new CharLenSeg(11, 13, 1),
                new CharLenSeg(13, 14, 0),
                new CharLenSeg(14, 162, 1),
                new CharLenSeg(162, 166, 2),
                new CharLenSeg(166, 167, 1),
                new CharLenSeg(167, 169, 2),
                new CharLenSeg(169, 175, 1),
                new CharLenSeg(175, 178, 2),
                new CharLenSeg(178, 180, 1),
                new CharLenSeg(180, 182, 2),
                new CharLenSeg(182, 183, 1),
                new CharLenSeg(183, 184, 2),
                new CharLenSeg(184, 215, 1),
                new CharLenSeg(215, 216, 2),
                new CharLenSeg(216, 247, 1),
                new CharLenSeg(247, 248, 2),
                new CharLenSeg(248, 449, 1),
                new CharLenSeg(449, 450, 2),
                new CharLenSeg(450, 711, 1),
                new CharLenSeg(711, 712, 2),
                new CharLenSeg(712, 713, 1),
                new CharLenSeg(713, 716, 2),
                new CharLenSeg(716, 729, 1),
                new CharLenSeg(729, 730, 2),
                new CharLenSeg(730, 913, 1),
                new CharLenSeg(913, 930, 2),
                new CharLenSeg(930, 931, 1),
                new CharLenSeg(931, 938, 2),
                new CharLenSeg(938, 945, 1),
                new CharLenSeg(945, 962, 2),
                new CharLenSeg(962, 963, 1),
                new CharLenSeg(963, 970, 2),
                new CharLenSeg(970, 1025, 1),
                new CharLenSeg(1025, 1026, 2),
                new CharLenSeg(1026, 1040, 1),
                new CharLenSeg(1040, 1104, 2),
                new CharLenSeg(1104, 1105, 1),
                new CharLenSeg(1105, 1106, 2),
                new CharLenSeg(1106, 8208, 1),
                new CharLenSeg(8208, 8209, 2),
                new CharLenSeg(8209, 8211, 1),
                new CharLenSeg(8211, 8215, 2),
                new CharLenSeg(8215, 8216, 1),
                new CharLenSeg(8216, 8218, 2),
                new CharLenSeg(8218, 8220, 1),
                new CharLenSeg(8220, 8222, 2),
                new CharLenSeg(8222, 8229, 1),
                new CharLenSeg(8229, 8231, 2),
                new CharLenSeg(8231, 8240, 1),
                new CharLenSeg(8240, 8241, 2),
                new CharLenSeg(8241, 8242, 1),
                new CharLenSeg(8242, 8244, 2),
                new CharLenSeg(8244, 8245, 1),
                new CharLenSeg(8245, 8246, 2),
                new CharLenSeg(8246, 8251, 1),
                new CharLenSeg(8251, 8252, 2),
                new CharLenSeg(8252, 8254, 1),
                new CharLenSeg(8254, 8255, 2),
                new CharLenSeg(8255, 8364, 1),
                new CharLenSeg(8364, 8365, 2),
                new CharLenSeg(8365, 8451, 1),
                new CharLenSeg(8451, 8452, 2),
                new CharLenSeg(8452, 8453, 1),
                new CharLenSeg(8453, 8454, 2),
                new CharLenSeg(8454, 8457, 1),
                new CharLenSeg(8457, 8458, 2),
                new CharLenSeg(8458, 8470, 1),
                new CharLenSeg(8470, 8471, 2),
                new CharLenSeg(8471, 8481, 1),
                new CharLenSeg(8481, 8482, 2),
                new CharLenSeg(8482, 8544, 1),
                new CharLenSeg(8544, 8556, 2),
                new CharLenSeg(8556, 8560, 1),
                new CharLenSeg(8560, 8570, 2),
                new CharLenSeg(8570, 8592, 1),
                new CharLenSeg(8592, 8596, 2),
                new CharLenSeg(8596, 8598, 1),
                new CharLenSeg(8598, 8602, 2),
                new CharLenSeg(8602, 8712, 1),
                new CharLenSeg(8712, 8713, 2),
                new CharLenSeg(8713, 8719, 1),
                new CharLenSeg(8719, 8720, 2),
                new CharLenSeg(8720, 8721, 1),
                new CharLenSeg(8721, 8722, 2),
                new CharLenSeg(8722, 8725, 1),
                new CharLenSeg(8725, 8726, 2),
                new CharLenSeg(8726, 8728, 1),
                new CharLenSeg(8728, 8729, 2),
                new CharLenSeg(8729, 8730, 1),
                new CharLenSeg(8730, 8731, 2),
                new CharLenSeg(8731, 8733, 1),
                new CharLenSeg(8733, 8737, 2),
                new CharLenSeg(8737, 8739, 1),
                new CharLenSeg(8739, 8740, 2),
                new CharLenSeg(8740, 8741, 1),
                new CharLenSeg(8741, 8742, 2),
                new CharLenSeg(8742, 8743, 1),
                new CharLenSeg(8743, 8748, 2),
                new CharLenSeg(8748, 8750, 1),
                new CharLenSeg(8750, 8751, 2),
                new CharLenSeg(8751, 8756, 1),
                new CharLenSeg(8756, 8760, 2),
                new CharLenSeg(8760, 8764, 1),
                new CharLenSeg(8764, 8766, 2),
                new CharLenSeg(8766, 8776, 1),
                new CharLenSeg(8776, 8777, 2),
                new CharLenSeg(8777, 8780, 1),
                new CharLenSeg(8780, 8781, 2),
                new CharLenSeg(8781, 8786, 1),
                new CharLenSeg(8786, 8787, 2),
                new CharLenSeg(8787, 8800, 1),
                new CharLenSeg(8800, 8802, 2),
                new CharLenSeg(8802, 8804, 1),
                new CharLenSeg(8804, 8808, 2),
                new CharLenSeg(8808, 8814, 1),
                new CharLenSeg(8814, 8816, 2),
                new CharLenSeg(8816, 8853, 1),
                new CharLenSeg(8853, 8854, 2),
                new CharLenSeg(8854, 8857, 1),
                new CharLenSeg(8857, 8858, 2),
                new CharLenSeg(8858, 8869, 1),
                new CharLenSeg(8869, 8870, 2),
                new CharLenSeg(8870, 8895, 1),
                new CharLenSeg(8895, 8896, 2),
                new CharLenSeg(8896, 8978, 1),
                new CharLenSeg(8978, 8979, 2),
                new CharLenSeg(8979, 9312, 1),
                new CharLenSeg(9312, 9322, 2),
                new CharLenSeg(9322, 9332, 1),
                new CharLenSeg(9332, 9372, 2),
                new CharLenSeg(9372, 9632, 1),
                new CharLenSeg(9632, 9634, 2),
                new CharLenSeg(9634, 9650, 1),
                new CharLenSeg(9650, 9652, 2),
                new CharLenSeg(9652, 9660, 1),
                new CharLenSeg(9660, 9662, 2),
                new CharLenSeg(9662, 9670, 1),
                new CharLenSeg(9670, 9672, 2),
                new CharLenSeg(9672, 9675, 1),
                new CharLenSeg(9675, 9676, 2),
                new CharLenSeg(9676, 9678, 1),
                new CharLenSeg(9678, 9680, 2),
                new CharLenSeg(9680, 9698, 1),
                new CharLenSeg(9698, 9702, 2),
                new CharLenSeg(9702, 9733, 1),
                new CharLenSeg(9733, 9735, 2),
                new CharLenSeg(9735, 9737, 1),
                new CharLenSeg(9737, 9738, 2),
                new CharLenSeg(9738, 9792, 1),
                new CharLenSeg(9792, 9793, 2),
                new CharLenSeg(9793, 9794, 1),
                new CharLenSeg(9794, 9795, 2),
                new CharLenSeg(9795, 12288, 1),
                new CharLenSeg(12288, 12292, 2),
                new CharLenSeg(12292, 12293, 1),
                new CharLenSeg(12293, 12312, 2),
                new CharLenSeg(12312, 12317, 1),
                new CharLenSeg(12317, 12319, 2),
                new CharLenSeg(12319, 12321, 1),
                new CharLenSeg(12321, 12330, 2),
                new CharLenSeg(12330, 12353, 1),
                new CharLenSeg(12353, 12436, 2),
                new CharLenSeg(12436, 12443, 1),
                new CharLenSeg(12443, 12447, 2),
                new CharLenSeg(12447, 12449, 1),
                new CharLenSeg(12449, 12535, 2),
                new CharLenSeg(12535, 12540, 1),
                new CharLenSeg(12540, 12543, 2),
                new CharLenSeg(12543, 12549, 1),
                new CharLenSeg(12549, 12586, 2),
                new CharLenSeg(12586, 12690, 1),
                new CharLenSeg(12690, 12704, 2),
                new CharLenSeg(12704, 12832, 1),
                new CharLenSeg(12832, 12868, 2),
                new CharLenSeg(12868, 12928, 1),
                new CharLenSeg(12928, 12958, 2),
                new CharLenSeg(12958, 12959, 1),
                new CharLenSeg(12959, 12964, 2),
                new CharLenSeg(12964, 12969, 1),
                new CharLenSeg(12969, 12977, 2),
                new CharLenSeg(12977, 13198, 1),
                new CharLenSeg(13198, 13200, 2),
                new CharLenSeg(13200, 13212, 1),
                new CharLenSeg(13212, 13215, 2),
                new CharLenSeg(13215, 13217, 1),
                new CharLenSeg(13217, 13218, 2),
                new CharLenSeg(13218, 13252, 1),
                new CharLenSeg(13252, 13253, 2),
                new CharLenSeg(13253, 13262, 1),
                new CharLenSeg(13262, 13263, 2),
                new CharLenSeg(13263, 13265, 1),
                new CharLenSeg(13265, 13267, 2),
                new CharLenSeg(13267, 13269, 1),
                new CharLenSeg(13269, 13270, 2),
                new CharLenSeg(13270, 19968, 1),
                new CharLenSeg(19968, 40870, 2),
                new CharLenSeg(40870, 55296, 1),
                new CharLenSeg(55296, 55297, 0),
                new CharLenSeg(55297, 56320, 1),
                new CharLenSeg(56320, 56321, 2),
                new CharLenSeg(56321, 57344, 1),
                new CharLenSeg(57344, 59335, 2),
                new CharLenSeg(59335, 59337, 1),
                new CharLenSeg(59337, 59493, 2),
                new CharLenSeg(59493, 63733, 1),
                new CharLenSeg(63733, 63734, 2),
                new CharLenSeg(63734, 63744, 1),
                new CharLenSeg(63744, 64046, 2),
                new CharLenSeg(64046, 65072, 1),
                new CharLenSeg(65072, 65074, 2),
                new CharLenSeg(65074, 65075, 1),
                new CharLenSeg(65075, 65093, 2),
                new CharLenSeg(65093, 65097, 1),
                new CharLenSeg(65097, 65107, 2),
                new CharLenSeg(65107, 65108, 1),
                new CharLenSeg(65108, 65112, 2),
                new CharLenSeg(65112, 65113, 1),
                new CharLenSeg(65113, 65127, 2),
                new CharLenSeg(65127, 65128, 1),
                new CharLenSeg(65128, 65132, 2),
                new CharLenSeg(65132, 65281, 1),
                new CharLenSeg(65281, 65375, 2),
                new CharLenSeg(65375, 65504, 1),
                new CharLenSeg(65504, 65510, 2),
                new CharLenSeg(65510, 65536, 1),
            }
        );

        public static bool IsWideChar(char c)
        {
            return CalcCharLength(c) > 1;
        }
        public static int CalcCharLength(char c)
        {
            return AllCharLenSegs.BinarySearch(c).Length;
        }
        public static int CalcStringLength(string str)
        {
            int total = 0;
            foreach (char c in str)
                total += CalcCharLength(c);
            return total;
        }

    }
}
