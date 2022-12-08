﻿namespace AdventOfCode
{
    public static class MyEnumerable
    {
        public static IEnumerable<(int x, int y)> GetTableValues(int maxX, int maxY, int startX = 0, int startY = 0)
        {
            for (int y = startX; y < maxY; y++)
            {
                for (int x = startY; x < maxX; x++)
                {
                    yield return (x, y);
                }
            }
        }
    }
}
