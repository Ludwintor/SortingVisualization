using System;
using System.Linq;
using NUnit.Framework;

namespace Sorting.Tests
{
    public class IntroSortTests
    {
        private static object[] Numbers = new object[]
        {
            Array.Empty<int>(),
            new int[] { 1 },
            new int[] { 1, 2, 3, 4, 5 },
            new int[] { 0, 0, 0, 55, 55, 60 },
            new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 },
            new int[] { 8, 0, 42, 3, 4, 8, 0, 45, 50, 9999, 7 },
            new int[] { -5, 0, 9, -999, 874, 35, -4, -5, 0 },
            new int[] { 1, 1, 1 }
        };

        private static object[] Strings = new object[]
        {
            Array.Empty<string>(),
            new string[] { "a" },
            new string[] { "a", "b", "c", "d", "e" },
            new string[] { "aa", "aa", "aa", "ab", "ac", "b" },
            new string[] { "e", "d", "c", "b", "a" },
            new string[] { "abc", "a", "foo", "bar", "booz", "baz", "spam", "love" },
            new string[] { "abc", "abc", "abc" },
            new string[] { "" }
        };

        [Test]
        [TestCaseSource(nameof(Numbers))]
        public void Sort_Ascend_Numbers(int[] data)
        {
            int[] expected = new int[data.Length];
            data.CopyTo(expected, 0);
            new IntroSort().Sort(data);
            Array.Sort(expected);
            Assert.True(expected.SequenceEqual(data));
        }

        [Test]
        [TestCaseSource(nameof(Numbers))]
        public void Sort_Descend_Numbers(int[] data)
        {
            int[] expected = new int[data.Length];
            data.CopyTo(expected, 0);
            new IntroSort().Sort(data, true);
            Array.Sort(expected);
            Array.Reverse(expected);
            Assert.True(expected.SequenceEqual(data));
        }

        [Test]
        [TestCaseSource(nameof(Strings))]
        public void Sort_Ascend_Strings(string[] data)
        {
            string[] expected = new string[data.Length];
            data.CopyTo(expected, 0);
            new IntroSort().Sort(data);
            Array.Sort(expected);
            Assert.True(expected.SequenceEqual(data));
        }

        [Test]
        [TestCaseSource(nameof(Strings))]
        public void Sort_Descend_Strings(string[] data)
        {
            string[] expected = new string[data.Length];
            data.CopyTo(expected, 0);
            new IntroSort().Sort(data, true);
            Array.Sort(expected);
            Array.Reverse(expected);
            Assert.True(expected.SequenceEqual(data));
        }
    }
}