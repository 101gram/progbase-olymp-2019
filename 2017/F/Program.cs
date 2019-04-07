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
            int direct = BuildDirectRoute(map, width);
            int reverse = BuildReverseRoute(map, width);
            if (direct == int.MinValue && reverse == int.MinValue) {
                throw new ArgumentException("Couldn't build route, map is invalid");
            } else if (direct == int.MinValue || reverse == int.MinValue) {
                return direct == int.MinValue ? reverse : direct;
            } else {
                return direct + reverse;
            }
        }

        static int BuildDirectRoute(short[,] map, int width) {
            int heigth = map.Length / width; 
            int x = 0, y = 0;
            int value = 0;
            int currentRowValue = CalculateRowValue((x, y), map, width);
            while (x != width - 1 || y != heigth - 1) {
                int currentValue = IsTarget(map[x, y]) ? 1 : 0;
                currentRowValue -= currentValue;
                value += currentValue;
                if (y < GetYOfDiagonalMatrix(x, width, heigth) || x == width - 1) {
                    if (IsAvaliableRow((x, y + 1), map, width)) {
                        int newRow = CalculateRowValue((x, y + 1), map, width);
                        if (newRow >= currentRowValue) {
                            currentRowValue = newRow;
                            y++;
                        }
                    } else if (x == width - 1) {
                        return int.MinValue;
                    }
                }
                if (x + 1 < width && IsAvaliable(map[x + 1, y])) {
                   x++;
                } 
            }
            return value;
        }

        static int BuildReverseRoute(short[,] map, int width) {
            int heigth = map.Length / width; 
            int x = width - 1, y = heigth - 1;
            int value = 0;
            int currentRowValue = CalculateRowValueInverse((x, y), map, width);
            while (x != 0 || y != 0) {
                int currentValue = IsTarget(map[x, y]) ? 1 : 0;
                currentRowValue -= currentValue;
                value += currentValue;
                if (y > GetYOfDiagonalMatrix(x, width, heigth) || x == 0) {
                    if (IsAvaliableRowInverse((x, y - 1), map, width)) {
                        int newRow = CalculateRowValueInverse((x, y - 1), map, width);
                        if (newRow >= currentRowValue) {
                            currentRowValue = newRow;
                            y--;
                        }
                    } else if (x == 0) {
                        return int.MinValue;
                    }
                }
                if (x - 1 >= 0 && IsAvaliable(map[x - 1, y])) {
                   x--;
                } 
            }
            return value;
        }

        public static int GetYOfDiagonalMatrix(int x, int width, int heigth) {
            return (int)Math.Round((float)x * heigth / width, 0);
        }

        public static bool IsAvaliable(short place) => place != -1;

        public static bool IsTarget(short place) => place == 1;

        public static bool IsAvaliableRow((int x, int y) start, short[,] map, int width) {
            while(start.x < width) {
                if (!IsAvaliable(map[start.x++, start.y])) {
                    return false;
                }
            }
            return true;
        }

        public static bool IsAvaliableRowInverse((int x, int y) start, short[,] map, int width) {
            while(start.x > 0) {
                if (!IsAvaliable(map[start.x--, start.y])) {
                    return false;
                }
            }
            return true;
        }

        public static int CalculateRowValue((int x, int y) start, short[,] map, int width) {
            int value = 0;
            while(start.x < width) {
                if (!IsAvaliable(map[start.x, start.y])) {
                    return int.MinValue;
                }
                value += map[start.x++, start.y];
            }
            return value;
        }

        public static int CalculateRowValueInverse((int x, int y) start, short[,] map, int width) {
            int value = 0;
            while(start.x > 0) {
                if (!IsAvaliable(map[start.x, start.y])) {
                    return int.MinValue;
                }
                value += map[start.x--, start.y];
            }
            return value;
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
