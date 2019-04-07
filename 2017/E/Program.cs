using System;

namespace E {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine(countAllWays(4));
        }

        static long countAllWays(int length) {
            return fibonacci(length).last;
        }

        static (long penultimate, long last) fibonacci(
            int count, long penultimate = 1, long last = 2, int i = 3
        ) {
            if (i == count) {
                return (penultimate, last);
            }
            return fibonacci(count, last, penultimate + last, ++i);
        }
    }
}
