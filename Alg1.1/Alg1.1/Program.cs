using System;
using System.Collections.Generic;
using System.Linq;
class Program
{
    static void Main()
    {
        Console.WriteLine("ИНТЕРПОЛЯЦИОННЫЙ ПОЛИНОМ ЛАГРАНЖА");
        while (true)
        {

            Console.WriteLine("Выберите функцию:");
            Console.WriteLine("1. f(x) = x^2 ");
            Console.WriteLine("2. f(x) = 1/(1+25*x*x)");
            Console.WriteLine("3. f(x) = |x|");
            Console.WriteLine("4. Выход");
            Console.Write("Ваш выбор (1-4): ");
            string choice = Console.ReadLine();
            if (choice == "4") break;
            Console.Write("Введите количество узлов n (2-20): ");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine("Выберите тип узлов:");
            Console.WriteLine("1. Равноотстоящие");
            Console.WriteLine("2. Чебышевские");
            Console.Write("Ваш выбор (1-2): ");
            string nodeType = Console.ReadLine();
            Func<double, double> function = GetFunction(choice);
            double[] nodes;
            if (nodeType == "1")
            {
                nodes = GetNodes(n); 
            }
            else
            {
                nodes = GetChebyshevNodes(n); 
            }
            double[] values = new double[n + 1];
            for (int i = 0; i <= n; i++)
            {
                values[i] = function(nodes[i]);
            }
            AnalyzeDeviation(function, nodes, values, n);
            Console.WriteLine();
        }
    }
    static Func<double, double> GetFunction(string choice)
    {
        if (choice == "1")
        {
            return QuadraticFunction; 
        }
        else if (choice == "2")
        {
            return ExponentialFunction; 
        }
        else if (choice == "3")
        {
            return AbsoluteFunction; 
        }
        else
        {
            return QuadraticFunction; 
        }
    }
    static double QuadraticFunction(double x)
    {
        return x * x;
    }
    static double ExponentialFunction(double x)
    {
        return 1 / (1 + 25 * x * x);
    }
    static double AbsoluteFunction(double x)
    {
        return Math.Abs(x);
    }
    static double[] GetNodes(int n)
    {
        double[] nodes = new double[n + 1];
        for (int i = 0; i <= n; i++)
        {
            nodes[i] = -1 + 2.0 * i / n;
        }
        return nodes;
    }
    static double[] GetChebyshevNodes(int n)
    {
        double[] nodes = new double[n + 1];
        for (int i = 0; i <= n; i++)
        {
            nodes[i] = Math.Cos(Math.PI * (2 * i + 1) / (2 * (n + 1)));
        }
        return nodes;
    }
    static double LagrangePolynomial(double x, double[] nodes, double[] values)
    {
        double result = 0;
        int n = nodes.Length - 1;

        for (int i = 0; i <= n; i++)
        {
            double term = values[i];
            for (int j = 0; j <= n; j++)
            {
                if (j != i)
                {
                    term = term * (x - nodes[j]) / (nodes[i] - nodes[j]);
                }
            }
            result = result + term;
        }
        return result;
    }
    static void AnalyzeDeviation(Func<double, double> function, double[] nodes, double[] values, int n)
    {
        Console.WriteLine("\nАНАЛИЗ ОТКЛОНЕНИЯ");
        Console.WriteLine("================\n");
        Console.WriteLine($"Узлы интерполяции (n = {n}):");
        for (int i = 0; i <= n; i++)
        {
            Console.WriteLine($"  x{i} = {nodes[i]:F4}, f(x{i}) = {values[i]:F4}");
        }
        int Points = 100;
        double maxDeviation = 0;
        double averageDeviation = 0;
        double maxDeviationX = 0;
        List<(double x, double func, double poly, double dev)> results = new List<(double, double, double, double)>();

        for (int i = 0; i <= Points; i++)
        {
            double x = -1 + 2.0 * i / Points;
            double funcValue = function(x);
            double polyValue = LagrangePolynomial(x, nodes, values);
            double deviation = Math.Abs(funcValue - polyValue);

            results.Add((x, funcValue, polyValue, deviation));

            averageDeviation = averageDeviation + deviation;

            if (deviation > maxDeviation)
            {
                maxDeviation = deviation;
                maxDeviationX = x;
            }
        }

        averageDeviation = averageDeviation / (Points + 1);
        Console.WriteLine("\nОтклонение в ключевых точках:");
        Console.WriteLine(" x\t\tf(x)\t\tP(x)\t\t|f(x)-P(x)|");
        Console.WriteLine("------------------------------------------------");
        int[] keyPoints = { 0, Points / 4, Points / 2, 3 * Points / 4, Points };
        foreach (int point in keyPoints)
        {
            var result = results[point];
            Console.WriteLine($"{result.x:F4}\t\t{result.func:F4}\t\t{result.poly:F4}\t\t{result.dev:F6}");
        }
        Console.WriteLine("\nСТАТИСТИКА ОТКЛОНЕНИЙ:");
        Console.WriteLine($"Максимальное отклонение: {maxDeviation:F6} (при x = {maxDeviationX:F4})");
        Console.WriteLine($"Среднее отклонение: {averageDeviation:F6}");
    }
}