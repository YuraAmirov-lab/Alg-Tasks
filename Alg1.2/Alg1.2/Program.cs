using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("КУБИЧЕСКИЙ СПЛАЙН");
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

    static double CubicSpline(double x, double[] nodes, double[] values)
    {
        int n = nodes.Length - 1;
        int interval = 0;
        for (int i = 0; i < n; i++)
        {
            if (x >= nodes[i] && x <= nodes[i + 1])
            {
                interval = i;
                break;
            }
        }
        double[] a = new double[n + 1];
        double[] b = new double[n];
        double[] c = new double[n + 1];
        double[] d = new double[n];
        for (int i = 0; i <= n; i++)
        {
            a[i] = values[i];
        }
        double[] h = new double[n];
        for (int i = 0; i < n; i++)
        {
            h[i] = nodes[i + 1] - nodes[i];
        }

        double[] alpha = new double[n];
        for (int i = 1; i < n; i++)
        {
            alpha[i] = 3.0 / h[i] * (a[i + 1] - a[i]) - 3.0 / h[i - 1] * (a[i] - a[i - 1]);
        }

        double[] l = new double[n + 1];
        double[] mu = new double[n + 1];
        double[] z = new double[n + 1];

        l[0] = 1;
        mu[0] = 0;
        z[0] = 0;

        for (int i = 1; i < n; i++)
        {
            l[i] = 2 * (nodes[i + 1] - nodes[i - 1]) - h[i - 1] * mu[i - 1];
            mu[i] = h[i] / l[i];
            z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
        }

        l[n] = 1;
        z[n] = 0;
        c[n] = 0;

        for (int j = n - 1; j >= 0; j--)
        {
            c[j] = z[j] - mu[j] * c[j + 1];
            b[j] = (a[j + 1] - a[j]) / h[j] - h[j] * (c[j + 1] + 2 * c[j]) / 3.0;
            d[j] = (c[j + 1] - c[j]) / (3.0 * h[j]);
        }

        double dx = x - nodes[interval];
        return a[interval] + b[interval] * dx + c[interval] * dx * dx + d[interval] * dx * dx * dx;
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
            double polyValue = CubicSpline(x, nodes, values);
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