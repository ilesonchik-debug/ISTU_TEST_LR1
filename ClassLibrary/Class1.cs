
// 4 вариант

namespace ClassLibrary
{
    public static class ArrayAnalyzer
    {
        public static (int Index, double Value)[] Analyze(double[] numbers)
        {
            var result = new System.Collections.Generic.List<(int Index, double Value)>();

            for (int i = 0; i < numbers.Length; i++)
            {
                double number = numbers[i];
                if (number >= 0) continue;

                string digits = Math.Abs(number).ToString("0.################", System.Globalization.CultureInfo.InvariantCulture);
                int sum = 0;

                foreach (char c in digits)
                {
                    if (char.IsDigit(c))
                        sum += c - '0';
                }

                if (sum % 2 == 0)
                    result.Add((i, number));
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