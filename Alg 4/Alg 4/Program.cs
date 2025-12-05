using System;
using System.Collections.Generic;

public class IterativeSolver
{
    private double[,] A;
    private double[] b;
    private double epsilon;
    private int maxIterations;

    public IterativeSolver(double[,] matrix, double[] vector, double eps = 1e-6, int maxIter = 1000)
    {
        A = matrix;
        b = vector;
        epsilon = eps;
        maxIterations = maxIter;
    }
    public (double[] solution, List<double> residuals, int iterations) Jacobi(double[] initialGuess)
    {
        int n = b.Length;
        double[] x = (double[])initialGuess.Clone();
        double[] xNew = new double[n];
        List<double> residuals = new List<double>();

        for (int iter = 0; iter < maxIterations; iter++)
        {
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    if (i != j) sum += A[i, j] * x[j];
                }
                xNew[i] = (b[i] - sum) / A[i, i];
            }

            double residual = CalculateResidual(xNew);
            residuals.Add(residual);

            if (residual < epsilon)
            {
                return (xNew, residuals, iter + 1);
            }

            Array.Copy(xNew, x, n);
        }

        return (xNew, residuals, maxIterations);
    }

    public (double[] solution, List<double> residuals, int iterations) Seidel(double[] initialGuess)
    {
        int n = b.Length;
        double[] x = (double[])initialGuess.Clone();
        List<double> residuals = new List<double>();

        for (int iter = 0; iter < maxIterations; iter++)
        {
            double[] xOld = (double[])x.Clone();

            for (int i = 0; i < n; i++)
            {
                double sum1 = 0;
                for (int j = 0; j < i; j++)
                {
                    sum1 += A[i, j] * x[j];
                }

                double sum2 = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum2 += A[i, j] * xOld[j];
                }

                x[i] = (b[i] - sum1 - sum2) / A[i, i];
            }

            double residual = CalculateResidual(x);
            residuals.Add(residual);

            if (residual < epsilon)
            {
                return (x, residuals, iter + 1);
            }
        }

        return (x, residuals, maxIterations);
    }

    private double CalculateResidual(double[] x)
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
            residual += Math.Pow(b[i] - sum, 2);
        }

        return Math.Sqrt(residual);
    }
}

public class Program
{
    static void Main()
    {

        double[,] A = {
            { 4, 1, -1 },
            { 2, 5, 1 },
            { 1, 2, 4 }
        };
        double[] b = { 3, 9, 12 };

        var solver = new IterativeSolver(A, b, eps: 1e-6, maxIter: 1000);


        double[][] initialGuesses = {
            new double[] { 0, 0, 0 },
            new double[] { 1, 1, 1 },
            new double[] { 10, 10, 10 },
            new double[] { -5, -5, -5 }
        };

        Console.WriteLine("Результаты решения СЛАУ итерационными методами");
        Console.WriteLine("Система уравнений:");
        Console.WriteLine("4x1 + x2 - x3 = 3");
        Console.WriteLine("2x1 + 5x2 + x3 = 9");
        Console.WriteLine("x1 + 2x2 + 4x3 = 12");
        Console.WriteLine($"Точность: {1e-6}");
        Console.WriteLine("=============================================\n");

        for (int i = 0; i < initialGuesses.Length; i++)
        {
            Console.WriteLine($"\n=== НАЧАЛЬНОЕ ПРИБЛИЖЕНИЕ {i + 1}: [{string.Join(", ", initialGuesses[i])}] ===");


            Console.WriteLine("\n--- МЕТОД ЯКОБИ ---");
            var (jacobiSolution, jacobiResiduals, jacobiIterations) = solver.Jacobi(initialGuesses[i]);
            PrintResults(jacobiSolution, jacobiResiduals, jacobiIterations);

            Console.WriteLine("\n--- МЕТОД ЗЕЙДЕЛЯ ---");
            var (seidelSolution, seidelResiduals, seidelIterations) = solver.Seidel(initialGuesses[i]);
            PrintResults(seidelSolution, seidelResiduals, seidelIterations);

            Console.WriteLine("\n" + new string('-', 60));
        }
    }

    static void PrintResults(double[] solution, List<double> residuals, int iterations)
    {
        Console.WriteLine($"Количество итераций: {iterations}");
        Console.WriteLine($"Достигнутая точность: {residuals[residuals.Count - 1]:E6}");
        Console.WriteLine("Решение:");
        for (int i = 0; i < solution.Length; i++)
        {
            Console.WriteLine($"  x{i + 1} = {solution[i]:F8}");
        }

        Console.WriteLine("\nЗависимость нормы невязки от номера итерации:");
        Console.WriteLine("Итер. | Норма невязки");
        Console.WriteLine("------|--------------");

        int printCount = Math.Min(10, residuals.Count);
        for (int i = 0; i < printCount; i++)
        {
            Console.WriteLine($" {i + 1,4} | {residuals[i]:E6}");
        }

        if (residuals.Count > 10)
        {
            Console.WriteLine(" ...  | ...");
            Console.WriteLine($" {residuals.Count,4} | {residuals[residuals.Count - 1]:E6}");
        }
    }
}