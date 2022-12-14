namespace AdventOfCode
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

        public static IEnumerable<T> Include<T>(this IEnumerable<T> values, T value)
        {
            foreach (var item in values)
            {
                yield return item;
            }
            yield return value;
        }

        public static ulong Product(this IEnumerable<int> values)
        {
            ulong result = 1;
            foreach(int value in values)
            {
                result *= (ulong)value;
            }
            return result;
        }

        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (var value in values)
            {
                action(value);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> values, Action<(T item, T? previous)> action)
        {
            T? previous = default;
            foreach (var value in values)
            {
                action((value, previous));
                previous = value;
            }
        }

        public static IEnumerable<string> GetLines(this string input, string extraSplit = "")
        {
            foreach (var item in input.Split(Environment.NewLine + extraSplit, StringSplitOptions.RemoveEmptyEntries))
            {
                yield return item;
            }
        }
    }
}
