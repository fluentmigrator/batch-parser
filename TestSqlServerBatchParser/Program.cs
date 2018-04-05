using System;
using System.Collections.Generic;

namespace TestSqlServerBatchParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<(int key, string value)>();
            list.Add((-1, "a"));
            list.Add((-1, "b"));
            list.Add((1, "c"));
            list.Add((5, "d"));
            list.Add((2, "e"));
            list.Sort(new StringIndexComparer());
            foreach (var item in list)
            {
                Console.WriteLine(item.value);
            }
        }

        private class StringIndexComparer : IComparer<(int key, string value)>
        {
            public int Compare((int key, string value) x, (int key, string value) y)
            {
                if (x.key == -1 && y.key == -1)
                    return 0;
                if (x.key == -1)
                    return 1;
                if (y.key == -1)
                    return -1;
                return x.key.CompareTo(y.key);
            }
        }
    }
}
