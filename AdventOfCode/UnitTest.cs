using Xunit.Abstractions;
using System.Collections;
using System.Reflection;

namespace AoC
{
    public class UnitTest
    {
        private readonly ITestOutputHelper output;

        public UnitTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void TestPuzzle(TestScenario scenario)
        {
            string input = File.OpenText($"{scenario.Path}/testInput.txt").ReadToEnd();
            var result = scenario.Day.GetPuzzle(input);
            Assert.Equal(scenario.ExpectedResult, result);

            input = File.OpenText($"{scenario.Path}/input.txt").ReadToEnd();
            result = scenario.Day.GetPuzzle(input);
            output.WriteLine(scenario.Day.GetType().Name);
            output.WriteLine(result);
        }
    }

    public class TestScenario
    {
        public TestScenario(IDay day, string path, string expectedResult, string name)
        {
            Day = day;
            Path = path;
            ExpectedResult = expectedResult;
            Name = name;
        }

        public IDay Day { get; init; }
        public string Path { get; init; }
        public string ExpectedResult { get; init; }
        public string Name { get; init; }

        public override string ToString()
        {
            return Name;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DayAttribute : Attribute
    {
        public string? ExpectedValue { get; set; }
    }

    public class TestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var day = typeof(IDay);
            var days = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => day.IsAssignableFrom(t));
            foreach (var item in days.OrderBy(d => d.Name))
            {
                if (item == day)
                {
                    continue;
                }

                var attribute = item.GetCustomAttribute<DayAttribute>();
                if (attribute == null)
                {
                    throw new InvalidDataException("attribute");
                }
                if (item.Namespace == null)
                {
                    throw new InvalidDataException("namespace");
                }

                yield return new object[]
                {
                    new TestScenario(
                         GetDay(item),
                        item.Namespace.Split('.').Last().ToString()!,
                         attribute.ExpectedValue!,
                         item.Name)
                };
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static IDay GetDay(Type iday)
        {
            try
            {
                var day = Activator.CreateInstance(iday) as IDay;
                if (day == null)
                {
                    throw new InvalidDataException("day");
                }
                return day;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ex : {ex.ToString()}");
            }
        }
    }
}