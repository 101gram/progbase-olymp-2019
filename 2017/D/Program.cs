using System;

namespace D {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine(GetNumberSubarrays(3, new int[] {1,2,3,4,5}));
        }

        static int GetNumberSubarrays(int height, int[] heights) {
            int result = 0;
            for (int begin = 0; begin < heights.Length; begin++) {
                for (int end = begin; end < heights.Length; end++) {
                    if (IsValidHeigth(ref heights, begin, end, height)) {
                        result++;
                    }
                }
            }
            return result;
        }

        static bool IsValidHeigth(ref int[] array, int begin, int end, int val) {
            return array[begin + ((end - begin + 2) / 2 - 1)] >= val;
        }
    }
}
