


using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ClassLibrary;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        public static IEnumerable<object[]> CsvCases
        {
            get
            {
                string path = Path.Combine(
                    AppContext.BaseDirectory,
                    "TestData",
                    "test_results.csv");

                if (!File.Exists(path))
                    throw new FileNotFoundException(
                        "Не найден CSV-файл с тестовыми данными.", path);

                foreach (string line in File.ReadLines(path).Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] fields = line.Split(';');

                    double[] numbers = ParseDoubles(fields[2]);
                    int[] expectedIndices = ParseInts(fields[3]);
                    double[] expectedValues = ParseDoubles(fields[4]);

                    yield return new object[]
                    {
                        fields[0],
                        numbers,
                        expectedIndices,
                        expectedValues
                    };
                }
            }
        }

        private static double[] ParseDoubles(string text)
        {
            string values = text.Trim()
                .TrimStart('[').TrimEnd(']');

            if (string.IsNullOrWhiteSpace(values))
                return Array.Empty<double>();

            return values.Split(',')
                .Select(x => double.Parse(
                    x.Trim(), CultureInfo.InvariantCulture))
                .ToArray();
        }

        private static int[] ParseInts(string text)
        {
            string values = text.Trim()
                .TrimStart('[').TrimEnd(']');

            if (string.IsNullOrWhiteSpace(values))
                return Array.Empty<int>();

            return values.Split(',')
                .Select(x => int.Parse(
                    x.Trim(), CultureInfo.InvariantCulture))
                .ToArray();
        }

        [Theory]
        [MemberData(nameof(CsvCases))]
        public void Analyze_CsvCases_ReturnsExpectedResults(
            string testId,
            double[] numbers,
            int[] expectedIndices,
            double[] expectedValues)
        {
            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.True(
                expectedIndices.SequenceEqual(
                    result.Select(x => x.Index)),
                $"Тест {testId}: неверные индексы. " +
                $"Ожидалось [{string.Join(", ", expectedIndices)}], " +
                $"получено [{string.Join(", ", result.Select(x => x.Index))}]");

            Assert.True(
                expectedValues.SequenceEqual(
                    result.Select(x => x.Value)),
                $"Тест {testId}: неверные значения. " +
                $"Ожидалось [{string.Join(", ", expectedValues)}], " +
                $"получено [{string.Join(", ", result.Select(x => x.Value))}]");
        }
    }
}


/*

// убираю предупреждение проверки csv 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ClassLibrary;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        public static IEnumerable<object[]> CsvCases
        {
            get
            {
                string path = Path.Combine(
                    AppContext.BaseDirectory,
                    "TestData",
                    "test_results.csv");

                if (!File.Exists(path))
                    throw new FileNotFoundException(
                        "Не найден CSV-файл с тестовыми данными.", path);

                foreach (string line in File.ReadLines(path).Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] fields = line.Split(';');

                    string testId = fields[0];
                    string group = fields[1];
                    string values = fields[2].Trim()
                        .TrimStart('[').TrimEnd(']');

                    double[] numbers = string.IsNullOrWhiteSpace(values)
                        ? Array.Empty<double>()
                        : values.Split(',')
                            .Select(x => double.Parse(
                                x.Trim(), CultureInfo.InvariantCulture))
                            .ToArray();

                    int expectedCount = int.Parse(
                        fields[3], CultureInfo.InvariantCulture);

                    double expectedSum = double.Parse(
                        fields[4], CultureInfo.InvariantCulture);

                    yield return new object[]
                    {
                        testId, group, numbers, expectedCount, expectedSum
                    };
                }
            }
        }

        [Theory]
        [MemberData(nameof(CsvCases))]
        public void Analyze_CsvCases_ReturnsExpectedResults(
            string testId,
            string group,
            double[] numbers,
            int expectedCount,
            double expectedSum)
        {
            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(expectedCount, result.Count);
            Assert.Equal(expectedSum, result.Sum);
        }
    }
}

*/


/*
 * 
// файл юнит тестов до того как мы добавили тестовые значения из csv python стороннего файлика

using System.Collections.Generic;
using ClassLibrary;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        // Группа 1. Граничные значения типа double.
        public static IEnumerable<object[]> BoundaryCases =>
            new List<object[]>
            {
                new object[] { new double[] { double.MinValue }, 0, 0.0 },
                new object[] { new double[] { -1.0 }, 0, 0.0 },
                new object[] { new double[] { 0.0 }, 0, 0.0 },
                new object[] { new double[] { 1.0 }, 0, 0.0 },
                new object[] { new double[] { double.MaxValue }, 0, 0.0 }
            };

        [Theory]
        [MemberData(nameof(BoundaryCases))]
        public void Analyze_EquivalenceClasses_Boundaries(
            double[] numbers, int expectedCount, double expectedSum)
        {
            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(expectedCount, result.Count);
            Assert.Equal(expectedSum, result.Sum);
        }

        // Группа 2. Ключевой элемент в начале, середине и конце.
        public static IEnumerable<object[]> KeyElementPositionCases =>
            new List<object[]>
            {
                new object[] { new double[] { 5, 1, 2 }, 1, 5.0 },
                new object[] { new double[] { 1, 5, 2 }, 1, 5.0 },
                new object[] { new double[] { 1, 2, 5 }, 1, 5.0 }
            };

        [Theory]
        [MemberData(nameof(KeyElementPositionCases))]
        public void Analyze_KeyElementPosition_FirstMiddleLast(
            double[] numbers, int expectedCount, double expectedSum)
        {
            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(expectedCount, result.Count);
            Assert.Equal(expectedSum, result.Sum);
        }

        // Группа 3. Массивы без подходящих элементов.
        public static IEnumerable<object[]> NoKeyElementCases =>
            new List<object[]>
            {
                new object[] { System.Array.Empty<double>(), 0, 0.0 },
                new object[] { new double[] { 1, 2, 3, 4, 6, 7, 8, 9 }, 0, 0.0 },
                new object[] { new double[] { 35, 70, 105 }, 0, 0.0 },
                new object[] { new double[] { 7, 14, 21, 28 }, 0, 0.0 },
                new object[] { new double[] { 14, 35, 70 }, 0, 0.0 },
                new object[] { new double[] { 2.5, 15.5 }, 0, 0.0 }
            };

        [Theory]
        [MemberData(nameof(NoKeyElementCases))]
        public void Analyze_NoKeyElements_ReturnsZero(
            double[] numbers, int expectedCount, double expectedSum)
        {
            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(expectedCount, result.Count);
            Assert.Equal(expectedSum, result.Sum);
        }

        // Группа 4. Один подходящий элемент и отрицательные значения.
        public static IEnumerable<object[]> SingleAndNegativeCases =>
            new List<object[]>
            {
                new object[] { new double[] { 10 }, 1, 10.0 },
                new object[] { new double[] { 5 }, 1, 5.0 },
                new object[] { new double[] { -5 }, 1, -5.0 },
                new object[] { new double[] { 14 }, 0, 0.0 },
                new object[] { new double[] { -5, -10, -15, -35 }, 3, -30.0 },
                new object[] { new double[] { -10, 5, -20, 15 }, 4, -10.0 },
                new object[] { new double[] { -70, -65, -60 }, 2, -125.0 }
            };

        [Theory]
        [MemberData(nameof(SingleAndNegativeCases))]
        public void Analyze_SingleAndNegativeValues_ReturnsExpectedResults(
            double[] numbers, int expectedCount, double expectedSum)
        {
            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(expectedCount, result.Count);
            Assert.Equal(expectedSum, result.Sum);
        }

        // Группа 5. Смешанные данные, повторения и десятичные числа.
        public static IEnumerable<object[]> MixedCases =>
            new List<object[]>
            {
                new object[] { new double[] { 10, 15, 20 }, 3, 45.0 },
                new object[] { new double[] { 10, 14, 15, 35, 20, 3, 70, -25 }, 4, 20.0 },
                new object[] { new double[] { 5, 7, 10 }, 2, 15.0 },
                new object[] { new double[] { 25, 30, 40 }, 3, 95.0 },
                new object[] { new double[] { 5, 35, 10, 70, 15 }, 3, 30.0 },
                new object[] { new double[] { 5, 10, 15, 20, 25 }, 5, 75.0 },
                new object[] { new double[] { 49, 50, 51 }, 1, 50.0 },
                new object[] { new double[] { 34, 35, 36 }, 0, 0.0 },
                new object[] { new double[] { 69, 70, 71 }, 0, 0.0 },
                new object[] { new double[] { 100, 105, 110 }, 2, 210.0 },
                new object[] { new double[] { 999, 1000, 1001 }, 1, 1000.0 },
                new object[] { new double[] { 10, 10, 35, 35, 15, 15 }, 4, 50.0 },
                new object[] { new double[] { 5, 7, 10, 14, 15, 21, 20, 28, 25, 35 }, 5, 75.0 },
                new object[] { new double[] { 2.5, 5.0, 10.0, 15.5 }, 2, 15.0 },
                new object[] { new double[] { 0.0, 5.0, 7.0, 10.0, 35.0 }, 2, 15.0 },
                new object[] { new double[] { 5, 35, 10 }, 2, 15.0 }
            };

        [Theory]
        [MemberData(nameof(MixedCases))]
        public void Analyze_MixedValues_ReturnsExpectedResults(
            double[] numbers, int expectedCount, double expectedSum)
        {
            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(expectedCount, result.Count);
            Assert.Equal(expectedSum, result.Sum);
        }
    }
}

*/



/*
// до группировки тестов

using ClassLibrary;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(0, 0.0)]
        public void Analyze_EmptyArray_ReturnsZero(int expectedCount, double expectedSum)
        {
            double[] numbers = { };

            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(expectedCount, result.Count);
            Assert.Equal(expectedSum, result.Sum);
        }

        [Theory]
        [InlineData(1, 0, 0.0)]
        [InlineData(2, 0, 0.0)]
        [InlineData(3, 1, 10.0)]
        [InlineData(4, 0, 0.0)]
        [InlineData(5, 0, 0.0)]
        [InlineData(6, 1, 5.0)]
        [InlineData(7, 1, -5.0)]
        [InlineData(8, 0, 0.0)]
        [InlineData(9, 3, 45.0)]
        [InlineData(10, 0, 0.0)]
        [InlineData(11, 3, -30.0)]
        [InlineData(12, 4, 20.0)]
        [InlineData(13, 2, 15.0)]
        [InlineData(14, 0, 0.0)]
        [InlineData(15, 3, 95.0)]
        [InlineData(16, 3, 30.0)]
        [InlineData(17, 1, 5.0)]
        [InlineData(18, 1, 5.0)]
        [InlineData(19, 1, 5.0)]
        [InlineData(20, 1, 5.0)]
        [InlineData(21, 5, 75.0)]
        [InlineData(22, 4, -10.0)]
        [InlineData(23, 1, 50.0)]
        [InlineData(24, 0, 0.0)]
        [InlineData(25, 0, 0.0)]
        [InlineData(26, 2, 210.0)]
        [InlineData(27, 2, -125.0)]
        [InlineData(28, 1, 1000.0)]
        [InlineData(29, 1, 5.0)]
        [InlineData(30, 4, 50.0)]
        [InlineData(31, 5, 75.0)]
        [InlineData(32, 2, 15.0)]
        [InlineData(33, 2, 15.0)]
        public void Analyze_TestCases_ReturnExpectedResults(
            int testCaseId,
            int expectedCount,
            double expectedSum)
        {
            double[][] inputs =
            {
                Array.Empty<double>(),
                new double[] { 1, 2, 3, 4, 6, 7, 8, 9 },
                new double[] { 10 },
                new double[] { 14 },
                new double[] { 35 },
                new double[] { 5 },
                new double[] { -5 },
                new double[] { 0 },
                new double[] { 10, 15, 20 },
                new double[] { 35, 70, 105 },
                new double[] { -5, -10, -15, -35 },
                new double[] { 10, 14, 15, 35, 20, 3, 70, -25 },
                new double[] { 5, 7, 10 },
                new double[] { 7, 14, 21, 28 },
                new double[] { 25, 30, 40 },
                new double[] { 5, 35, 10, 70, 15 },
                new double[] { 5, 1, 2 },
                new double[] { 1, 5, 2 },
                new double[] { 1, 2, 5 },
                new double[] { 7, 5, 14 },
                new double[] { 5, 10, 15, 20, 25 },
                new double[] { -10, 5, -20, 15 },
                new double[] { 49, 50, 51 },
                new double[] { 34, 35, 36 },
                new double[] { 69, 70, 71 },
                new double[] { 100, 105, 110 },
                new double[] { -70, -65, -60 },
                new double[] { 999, 1000, 1001 },
                new double[] { 2147483647, -2147483648, 5 },
                new double[] { 10, 10, 35, 35, 15, 15 },
                new double[] { 5, 7, 10, 14, 15, 21, 20, 28, 25, 35 },
                new double[] { 2.5, 5.0, 10.0, 15.5 },
                new double[] { 0.0, 5.0, 7.0, 10.0, 35.0 }
            };

            var result = ArrayAnalyzer.Analyze(inputs[testCaseId - 1]);

            Assert.Equal(expectedCount, result.Count);
            Assert.Equal(expectedSum, result.Sum);
        }
    }
}

*/


/*

// до добавление сторонних скриптов 
//рабочий вариант 2

using ClassLibrary;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Analyze_EmptyArray_ReturnsZero()
        {
            double[] numbers = { };

            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(0, result.Count);
            Assert.Equal(0.0, result.Sum);
        }

        [Fact]
        public void Analyze_NoMatchingElements_ReturnsZero()
        {
            double[] numbers = { 1, 2, 3, 4, 7, 14 };

            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(0, result.Count);
            Assert.Equal(0.0, result.Sum);
        }

        [Fact]
        public void Analyze_OneMatchingElement_ReturnsCorrectResult()
        {
            double[] numbers = { 10 };

            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(1, result.Count);
            Assert.Equal(10.0, result.Sum);
        }

        [Fact]
        public void Analyze_MultipleMatchingElements_ReturnsCorrectResult()
        {
            double[] numbers = { 10, 15, 20 };

            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(3, result.Count);
            Assert.Equal(45.0, result.Sum);
        }

        [Fact]
        public void Analyze_MultiplesOfBothFiveAndSeven_AreExcluded()
        {
            double[] numbers = { 35, 70, 105 };

            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(0, result.Count);
            Assert.Equal(0.0, result.Sum);
        }

        [Fact]
        public void Analyze_NegativeMatchingElements_ReturnsCorrectResult()
        {
            double[] numbers = { -5, -10, -15, -35 };

            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(3, result.Count);
            Assert.Equal(-30.0, result.Sum);
        }

        [Fact]
        public void Analyze_MixedElements_ReturnsCorrectResult()
        {
            double[] numbers = { 10, 14, 15, 35, 20, 3, 70, -25 };

            var result = ArrayAnalyzer.Analyze(numbers);

            Assert.Equal(4, result.Count);
            Assert.Equal(20.0, result.Sum);
        }
    }
}

*/


/*
 // рабочий вариант
using ClassLibrary;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Analyze_ReturnsCorrectCountAndSum()
        {
            // Arrange: подготавливаем входные данные
            double[] numbers = { 10, 14, 15, 35, 20, 3, 70, -25 };

            // Act: вызываем проверяемый метод
            var result = ArrayAnalyzer.Analyze(numbers);

            // Assert: сравниваем с ожидаемым результатом
            Assert.Equal(4, result.Count);
            Assert.Equal(20.0, result.Sum);
        }
    }
}

*/








