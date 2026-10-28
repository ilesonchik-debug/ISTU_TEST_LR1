
// 29 вариант

namespace ClassLibrary
{
    public static class ArrayAnalyzer
    {
        public static (int Index1, double Value1, int Index2, double Value2)[] Analyze(double[] numbers)
        {
            var result = new System.Collections.Generic.List<(int Index1, double Value1, int Index2, double Value2)>();

            for (int i = 0; i < numbers.Length - 1; i++)
            {
                double first = numbers[i];
                double second = numbers[i + 1];

                if (first % 3 == 0 && second < 0)
                    result.Add((i, first, i + 1, second));
            }

            return result.ToArray();
        }
    }
}

/*

 // 7 вариант

namespace ClassLibrary
{
    public static class ArrayAnalyzer
    {
        public static (int Count, double Sum) Analyze(double[] numbers)
        {
            int count = 0;
            double sum = 0;

            foreach (double number in numbers)
            {
                if (number % 5 == 0 && number % 7 != 0)
                {
                    count++;
                    sum += number;
                }
            }

            return (count, sum);
        }
    }
}


*/