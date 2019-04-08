using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace F {
    class Program {
        static void Main(string[] args) {
            var map = ReadMap(5, 4);
            Console.WriteLine(CalculatePlaces(map, 5));
        }

        static int CalculatePlaces(short[,] map, int width) {
            int heigth = map.Length / width; 
            int direct = BuildDirectRoute(map, width);
            int reverse = BuildReverseRoute(map, width);
            if (direct == int.MinValue && reverse == int.MinValue) {
                throw new ArgumentException("Couldn't build route, map is invalid");
            } else if (direct == int.MinValue || reverse == int.MinValue) {
                return direct == int.MinValue ? reverse : direct;
            } else if (direct == 0 && reverse == 0) {
                return -1;
            } else {
                int firstValue = map[0, 0];
                int lastValue = map[width - 1, heigth -1];
                return direct + reverse - firstValue - lastValue;
            }
        }

        static int BuildDirectRoute(short[,] map, int width) {
            return CountValuesTopMap(map, width).Value;
        }

        static int? CountValuesTopMap(short[,] map, int width, int x = 0, int y = 0) {
            int heigth = map.Length / width; 
            if (x == width - 1 && y == heigth - 1) {
                return map[x, y];
            }
            int? rightRoute = 0;
            int? downRoute = 0;
            if (y <= GetYOfDiagonalMatrix(x, width, heigth) && y + 1 <= heigth - 1) {
                downRoute = CountValuesTopMap(map, width, x, y + 1);
            }
            if (x + 1 <= width - 1) {
                rightRoute = CountValuesTopMap(map, width, x + 1, y);
            }
            if (IsAvaliableCell((x, y), map)) {
                if (rightRoute == null && downRoute == null) {
                    return int.MinValue;
                }
                return map[x, y] + Math.Max(rightRoute ?? int.MinValue, downRoute ?? int.MinValue);
            } else {
                return null;
            }
        }

        static int BuildReverseRoute(short[,] map, int width) {
            int heigth = map.Length / width; 
            return CountValuesDownMap(map, width, width - 1, heigth - 1).Value;
        }

        static int? CountValuesDownMap(short[,] map, int width, int x, int y) {
            int heigth = map.Length / width; 
            if (x == 0 && y == 0) {
                return map[x, y];
            }
            int? leftRoute = 0;
            int? topRoute = 0;
            if (y > GetYOfDiagonalMatrix(x, width, heigth) && y - 1 >= 0) {
                topRoute = CountValuesDownMap(map, width, x, y - 1);
            }
            if (x - 1 >= 0) {
                leftRoute = CountValuesDownMap(map, width, x - 1, y);
            }
            if (IsAvaliableCell((x, y), map)) {
                if (leftRoute == null && topRoute == null) {
                    return int.MinValue;
                }
                return map[x, y] + Math.Max(leftRoute ?? int.MinValue, topRoute ?? int.MinValue);
            } else {
                return null;
            }
        }

        public static int GetYOfDiagonalMatrix(int x, int width, int heigth) {
            return (int)Math.Round((float)x * heigth / width, 0);
        }

        public static bool IsAvaliable(short place) => place != -1;

        public static bool IsTarget(short place) => place == 1;

        public static bool IsAvaliableCell((int x, int y) cell, short[,] map) {
            return IsAvaliable(map[cell.x, cell.y]);
        }

        static short[,] ReadMap(int width, int heigth) {
            var map = new short[width, heigth];
            using (var fileStream = File.OpenRead("input.txt")) {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true)) {
                    for (int j = 0; j < heigth; j++) {
                        string line = streamReader.ReadLine();
                        for (int i = 0; i < width; i++) {
                            char symbol = line[i];
                            short current;
                            switch (symbol) {
                                case '.': current = 0;  break;
                                case '*': current = 1;  break;
                                case '#': current = -1; break;
                                default: 
                                    throw new ArgumentOutOfRangeException(
                                        $"Unexpected symbol {symbol} on {j}:{i}"
                                    );
                            }
                            map[i, j] = current;
                        }
                    }
                }
            }
            return map;
        }
    }
}
