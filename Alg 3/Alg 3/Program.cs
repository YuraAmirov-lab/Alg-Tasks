using System;
using System.Linq;

class LinearEquationsSolver
{
    static void Main()
    {
        Console.WriteLine("РЕШЕНИЕ СИСТЕМ ЛИНЕЙНЫХ УРАВНЕНИЙ");
        Console.WriteLine("=================================\n");

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

        Console.WriteLine("\n3. МЕТОД ПРОГОНКИ");

        double[] a = { 1, 2 };    
        double[] b = { 3, 4, 5 };   
        double[] c = { 2, 1 };  
        double[] d = { 8, 18, 22 };

        double[,] A_tridiag = {
            { 3, 2, 0 },
            { 1, 4, 1 },
            { 0, 2, 5 }
        };

        Console.WriteLine("Трехдиагональная матрица:");
        PrintMatrix(A_tridiag);
        Console.WriteLine("Вектор f: [" + string.Join(", ", d) + "]");

        double[] xTridiag = SolveTridiagonal(a, b, c, d);
        Console.WriteLine("Решение: [" + string.Join(", ", xTridiag.Select(val => val.ToString("F4"))) + "]");
        PrintR(A_tridiag, d, xTridiag);
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

    static double[] SolveTridiagonal(double[] a, double[] b, double[] c, double[] d)
    {
        int n = b.Length;
        double[] x = new double[n];
        double[] alpha = new double[n - 1];
        double[] beta = new double[n];

        alpha[0] = -c[0] / b[0];
        beta[0] = d[0] / b[0];

        for (int i = 1; i < n - 1; i++)
        {
            double denominator = b[i] + a[i - 1] * alpha[i - 1];
            alpha[i] = -c[i] / denominator;
            beta[i] = (d[i] - a[i - 1] * beta[i - 1]) / denominator;
        }

        beta[n - 1] = (d[n - 1] - a[n - 2] * beta[n - 2]) / (b[n - 1] + a[n - 2] * alpha[n - 2]);

        x[n - 1] = beta[n - 1];
        for (int i = n - 2; i >= 0; i--)
        {
            x[i] = alpha[i] * x[i + 1] + beta[i];
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