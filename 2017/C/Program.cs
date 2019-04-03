using System;

namespace C {
    class Program {
        static readonly float G = 9.81f;

        static void Main(string[] args) {
            float[] result = getHeightsToShoot(1, new float[] {2, 1, 1.41f});
            foreach(var item in result) {
                Console.WriteLine(item.ToString());
            }
        }
        
        static float[] getHeightsToShoot(float velocity, float[] distances) {
            Array.Sort(distances);
            float[] heights = new float[distances.Length];
            for (short i = 0; i < heights.Length; i++) {
                heights[i] = calculateHeight(velocity, distances[i]);
            }
            return heights;
        }

        static float calculateHeight(float velocity, float distance) {
            return (float)Math.Round((G * Math.Pow(distance / velocity, 2)) / 2, 2);
        }
    }
}
