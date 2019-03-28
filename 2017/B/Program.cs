using System;
using System.Collections.Generic;
using System.Linq;

namespace B {
    class Program {

        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            MeasurePreformance(() => Lcm(new uint[] {6, 9, 11}));
        }


        static uint Lcm(IEnumerable<uint> seq) {
            return seq.Aggregate(Lcm);
        }

        static uint Lcm(uint a, uint b) => a * b / GcdBinary(a, b);

        // O(n^2), where n is the amount of bits in the greatest number
        static uint Gcd(uint a, uint b) {
            while (a != 0 && b != 0) {
                if (a > b) {
                    a %= b;
                } else {
                    b %= a;
                }
            }
            return a + b;
        }

        static void Swap(ref uint a, ref uint b) {
            var temp = a;
            a = b;
            b = temp;
        }
        static void BisectUntilOdd(ref uint a) {
            while (IsEven(a)) {
                a /= 2;
            }
        }
        static bool IsEven(uint suspect) => (suspect & 1) == 0;
        
        // O(n^2)
        // https://uk.wikipedia.org/wiki/%D0%94%D0%B2%D1%96%D0%B9%D0%BA%D0%BE%D0%B2%D0%B8%D0%B9_%D0%B0%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC_%D0%BE%D0%B1%D1%87%D0%B8%D1%81%D0%BB%D0%B5%D0%BD%D0%BD%D1%8F_%D0%BD%D0%B0%D0%B9%D0%B1%D1%96%D0%BB%D1%8C%D1%88%D0%BE%D0%B3%D0%BE_%D1%81%D0%BF%D1%96%D0%BB%D1%8C%D0%BD%D0%BE%D0%B3%D0%BE_%D0%B4%D1%96%D0%BB%D1%8C%D0%BD%D0%B8%D0%BA%D0%B0
        static uint GcdBinary(uint a, uint b) {
            if (a == 0) return b;
            if (b == 0) return a;

            int shift = 0;
            while (IsEven(a | b)) {
                a /= 2;
                b /= 2;
                ++shift;
            }

            BisectUntilOdd(ref a);           // a is always odd from now on

            do {
                BisectUntilOdd(ref b);
                if (a > b) Swap(ref a, ref b);
                b -= a;                   // odd - odd == even
            } while (b != 0);

            return a << shift;    // right operand must be only of `int` type
        }


        static void MeasurePreformance<T>(Func<T> routine) {
            Console.WriteLine("Invoking routine");

            var watch     = System.Diagnostics.Stopwatch.StartNew();
            var memBefore = GC.GetTotalMemory(false);

            var output = routine();
            watch.Stop();

            var elapsedTicks = watch.ElapsedTicks;
            var elapsedMs    = watch.ElapsedMilliseconds;

            var memUsed   = GC.GetTotalMemory(false) - memBefore;
            
            Console.WriteLine(
                $"Returned '{output}'.\nUsed memory {memUsed} within {elapsedMs}ms ({elapsedTicks} ticks)"
            );
        }
    
    }
}
