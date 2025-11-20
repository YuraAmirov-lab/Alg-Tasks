using System;
using System.Linq;

class LinearSolver
{
    static void Main()
    {
        Console.WriteLine("РЕШЕНИЕ СИСТЕМ ЛИНЕЙНЫХ УРАВНЕНИЙ");

        double[,] A = {
            { 4, 1, 1 },
            { 1, 6, -1 },
            { 1, 2, 5 }
        };
        double[] f = { 9, 10, 20 };

        Console.WriteLine("Матрица A:");
        PrintMatrix(A);
        Console.WriteLine("\nВектор f: [" + string.Join(", ", f) + "]");

        Console.WriteLine("\n1. МЕТОД ГАУССА");
        double[] xGauss = SolveGauss(A, f);
        Console.WriteLine("Решение: [" + string.Join(", ", xGauss.Select(val => val.ToString("F4"))) + "]");
        PrintR(A, f, xGauss);

        Console.WriteLine("\n2. МЕТОД ХОЛЕЦКОГО");
        double[] xCholesky = SolveCholesky(A, f);
        Console.WriteLine("Решение: [" + string.Join(", ", xCholesky.Select(val => val.ToString("F4"))) + "]");
        PrintR(A, f, xCholesky);
    }

    static double[] SolveGauss(double[,] A, double[] f)
    {
        int n = f.Length;
        double[,] matrix = (double[,])A.Clone();
        double[] vector = (double[])f.Clone();
        for (int i = 0; i < n; i++)
        {
            double div = matrix[i, i];
            for (int j = i; j < n; j++)
                matrix[i, j] /= div;
            vector[i] /= div;
            for (int k = i + 1; k < n; k++)
            {
                double factor = matrix[k, i];
                for (int j = i; j < n; j++)
                    matrix[k, j] -= factor * matrix[i, j];
                vector[k] -= factor * vector[i];
            }
        }

        double[] x = new double[n];
        for (int i = n - 1; i >= 0; i--)
        {
            x[i] = vector[i];
            for (int j = i + 1; j < n; j++)
                x[i] -= matrix[i, j] * x[j];
        }

        return x;
    }

    static double[] SolveCholesky(double[,] A, double[] f)
    {
        int n = f.Length;
        double[,] L = new double[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                double sum = 0;
                for (int k = 0; k < j; k++)
                    sum += L[i, k] * L[j, k];

                if (i == j)
                    L[i, j] = Math.Sqrt(A[i, i] - sum);
                else
                    L[i, j] = (A[i, j] - sum) / L[j, j];
            }
        }

        double[] y = new double[n];
        for (int i = 0; i < n; i++)
        {
            double sum = 0;
            for (int j = 0; j < i; j++)
                sum += L[i, j] * y[j];
            y[i] = (f[i] - sum) / L[i, i];
        }

        double[] x = new double[n];
        for (int i = n - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < n; j++)
                sum += L[j, i] * x[j];
            x[i] = (y[i] - sum) / L[i, i];
        }

        return x;
    }

    
    static void PrintR(double[,] A, double[] f, double[] x)
    {
        int n = f.Length;
        double[] r = new double[n];

        for (int i = 0; i < n; i++)
        {
            double sum = 0;
            for (int j = 0; j < n; j++)
                sum += A[i, j] * x[j];
            r[i] = sum - f[i];
        }

        Console.WriteLine("Вектор невязки: [" + string.Join(", ", r.Select(val => val.ToString("E4"))) + "]");

        double norm = 0;
        for (int i = 0; i < n; i++)
            norm += r[i] * r[i];
        norm = Math.Sqrt(norm);
        Console.WriteLine("Норма невязки: " + norm.ToString("E4"));
    }

    static void PrintMatrix(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            Console.Write("[ ");
            for (int j = 0; j < n; j++)
                Console.Write(matrix[i, j].ToString("F1") + " ");
            Console.WriteLine("]");
        }
    }
}