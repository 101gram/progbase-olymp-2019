using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace A {
    class Program {
        static void Main(string[] args) {
            var memBefore = GC.GetTotalMemory(false);
            Console.WriteLine(getMinSteps(readInputSeq("input.txt")));
            Console.WriteLine($"Used {GC.GetTotalMemory(false) - memBefore} bytes");
        }

        static IEnumerable<byte> readInputSeq(string fileName) {
            using (var streamReader = new StreamReader(fileName)) {
                streamReader.ReadLine();
                while (!streamReader.EndOfStream) {
                    yield return (byte)(streamReader.Read() - '0');
                    streamReader.Read();
                }
            }
        }

        static int getMinSteps(IEnumerable<byte> nodes) {
            var amounts = new byte[6];
            foreach (var node in nodes) {
                ++amounts[node - 1];
            }
            var min = amounts.Min();
            return min + 2;
        }
    }
}
