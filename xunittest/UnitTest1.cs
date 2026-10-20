




namespace ClassLibrary1;

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






