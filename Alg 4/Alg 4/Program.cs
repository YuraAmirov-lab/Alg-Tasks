using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {

        double[,] A = {
            { 4, 1, 1 },
            { 1, 4, 1 },
            { 1, 1, 4 }
        };

        double[] b = { 4, 4, 4 };
        double epsilon = 1e-6;


        double[][] initialGuesses = {
            new double[] { 0, 0, 0 },
            new double[] { 10, 10, 10 },
            new double[] { -5, 2, 8 }
        };

        for (int i = 0; i < initialGuesses.Length; i++)
        {
            Console.WriteLine($"\n=== Начальное приближение: [{string.Join(", ", initialGuesses[i])}] ===");

            Console.WriteLine("\n--- Метод Якоби ---");
            var (solution, residuals, iterations) = JacobiMethod(A, b, initialGuesses[i], epsilon);
            PrintResults(solution, residuals, iterations);

            Console.WriteLine("\n--- Метод Зейделя ---");
            (solution, residuals, iterations) = SeidelMethod(A, b, initialGuesses[i], epsilon);
            PrintResults(solution, residuals, iterations);
        }
    }

    static (double[], List<double>, int) JacobiMethod(double[,] A, double[] b, double[] initial, double epsilon)
    {
        int n = b.Length;
        double[] x_old = (double[])initial.Clone();
        double[] x_new = new double[n];
        List<double> residuals = new List<double>();

        int iteration = 0;
        double residual;

        do
        {

            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        sum += A[i, j] * x_old[j]; 
                    }
                }
                x_new[i] = (b[i] - sum) / A[i, i];
            }

  
            residual = CalculateResidual(A, b, x_new);
            residuals.Add(residual);

  
            Array.Copy(x_new, x_old, n);
            iteration++;

            if (iteration > 1000) break; 
        }
        while (residual > epsilon);

        return (x_new, residuals, iteration);
    }


    static (double[], List<double>, int) SeidelMethod(double[,] A, double[] b, double[] initial, double epsilon)
    {
        int n = b.Length;
        double[] x = (double[])initial.Clone();
        List<double> residuals = new List<double>();

        int iteration = 0;
        double residual;

        do
        {

            for (int i = 0; i < n; i++)
            {
                double sum = 0;


                for (int j = 0; j < i; j++)
                {
                    sum += A[i, j] * x[j];  
                }


                for (int j = i + 1; j < n; j++)
                {
                    sum += A[i, j] * x[j];  
                }

                x[i] = (b[i] - sum) / A[i, i];
            }


            residual = CalculateResidual(A, b, x);
            residuals.Add(residual);

            iteration++;

            if (iteration > 1000) break; 
        }
        while (residual > epsilon);

        return (x, residuals, iteration);
    }


    static double CalculateResidual(double[,] A, double[] b, double[] x)
    {
        int n = b.Length;
        double residual = 0;

        for (int i = 0; i < n; i++)
        {
            double sum = 0;
            for (int j = 0; j < n; j++)
            {
                sum += A[i, j] * x[j];
            }
            residual += Math.Pow(sum - b[i], 2);
        }

        return Math.Sqrt(residual);
    }

    static void PrintResults(double[] solution, List<double> residuals, int iterations)
    {
        Console.WriteLine($"Количество итераций: {iterations}");
        Console.WriteLine("Решение: [" + string.Join(", ", solution) + "]");
        Console.WriteLine("Нормы невязок по итерациям:");
        for (int i = 0; i < Math.Min(residuals.Count, 10); i++) 
        {
            Console.WriteLine($"  Итерация {i + 1}: {residuals[i]:E6}");
        }
        if (residuals.Count > 10)
        {
            Console.WriteLine($"  ... (пропущено {residuals.Count - 10} итераций)");
            Console.WriteLine($"  Итерация {residuals.Count}: {residuals[residuals.Count - 1]:E6}");
        }
        Console.WriteLine($"Достигнутая точность: {residuals[residuals.Count - 1]:E6}");
    }
}