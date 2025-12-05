using System;

class EigenMethods
{
    public static (double lambda, int iterations) PowerMethod(double[,] A, double eps)
    {
        int n = A.GetLength(0);
        double[] x = new double[n];
        double[] y = new double[n];

        for (int i = 0; i < n; i++)
            x[i] = 1.0;

        double lambdaOld = 0;
        int iter = 0;

        while (true)
        {
            iter++;

            for (int i = 0; i < n; i++)
            {
                y[i] = 0;
                for (int j = 0; j < n; j++)
                {
                    y[i] += A[i, j] * x[j];
                }
            }
            double num = 0;
            double den = 0; 

            for (int i = 0; i < n; i++)
            {
                num += y[i] * x[i];
                den += x[i] * x[i];
            }

            double lambda = num / den;

            if (Math.Abs(lambda - lambdaOld) < eps)
            {
                return (lambda, iter);
            }

            double norm = 0;
            for (int i = 0; i < n; i++)
                norm += y[i] * y[i];
            norm = Math.Sqrt(norm);

            for (int i = 0; i < n; i++)
                x[i] = y[i] / norm;

            lambdaOld = lambda;

            if (iter > 1000) break; 
        }

        return (lambdaOld, iter);
    }


    public static (double[] lambdas, int iterations) JacobiMethod(double[,] A, double eps)
    {
        int n = A.GetLength(0);

        double[,] B = new double[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                B[i, j] = A[i, j];

        int iter = 0;

        while (true)
        {
            iter++;


            double maxVal = 0;
            int p = 0, q = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++) 
                {
                    if (Math.Abs(B[i, j]) > Math.Abs(maxVal))
                    {
                        maxVal = B[i, j];
                        p = i;
                        q = j;
                    }
                }
            }


            if (Math.Abs(maxVal) < eps)
                break;


            double theta;
            if (Math.Abs(B[p, p] - B[q, q]) < 1e-10)
                theta = Math.PI / 4.0;
            else
                theta = 0.5 * Math.Atan2(2 * B[p, q], B[q, q] - B[p, p]);

            double cosT = Math.Cos(theta);
            double sinT = Math.Sin(theta);


            for (int j = 0; j < n; j++)
            {
                if (j != p && j != q)
                {
                    double bpj = B[p, j];
                    double bqj = B[q, j];
                    B[p, j] = bpj * cosT + bqj * sinT;
                    B[q, j] = -bpj * sinT + bqj * cosT;
                    B[j, p] = B[p, j]; 
                    B[j, q] = B[q, j];
                }
            }

            double bpp = B[p, p];
            double bpq = B[p, q];
            double bqq = B[q, q];

            B[p, p] = bpp * cosT * cosT + 2 * bpq * cosT * sinT + bqq * sinT * sinT;
            B[q, q] = bpp * sinT * sinT - 2 * bpq * cosT * sinT + bqq * cosT * cosT;
            B[p, q] = B[q, p] = 0; 

            if (iter > 1000) break;
        }

        double[] lambdas = new double[n];
        for (int i = 0; i < n; i++)
            lambdas[i] = B[i, i];

        return (lambdas, iter);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("ПОИСК СОБСТВЕННЫХ ЧИСЕЛ МАТРИЦЫ\n");


        double[,] A1 = {
            { 3, 1 },
            { 1, 2 }
        };

        double[,] A2 = {
            { 4, 1, 2 },
            { 1, 3, 0 },
            { 2, 0, 5 }
        };

        double eps = 1e-6;

        Console.WriteLine("Матрица 1 (2x2):");
        Console.WriteLine($"[{A1[0, 0]}, {A1[0, 1]}]");
        Console.WriteLine($"[{A1[1, 0]}, {A1[1, 1]}]\n");

        var (lambda1, iter1) = EigenMethods.PowerMethod(A1, eps);
        Console.WriteLine($"1. Метод прямой итерации:");
        Console.WriteLine($"   Наибольшее собственное число: {lambda1:F8}");
        Console.WriteLine($"   Количество итераций: {iter1}\n");

        var (lambdas1, iterJacobi1) = EigenMethods.JacobiMethod(A1, eps);
        Console.WriteLine($"2. Метод вращений Якоби:");
        Console.WriteLine($"   Собственные числа: {lambdas1[0]:F8}, {lambdas1[1]:F8}");
        Console.WriteLine($"   Количество итераций: {iterJacobi1}\n");

        Console.WriteLine("------------------------------------------------\n");

        Console.WriteLine("Матрица 2 (3x3):");
        for (int i = 0; i < 3; i++)
        {
            Console.Write("[");
            for (int j = 0; j < 3; j++)
                Console.Write($"{A2[i, j]}, ");
            Console.WriteLine("]");
        }
        Console.WriteLine();

        var (lambda2, iter2) = EigenMethods.PowerMethod(A2, eps);
        Console.WriteLine($"1. Метод прямой итерации:");
        Console.WriteLine($"   Наибольшее собственное число: {lambda2:F8}");
        Console.WriteLine($"   Количество итераций: {iter2}\n");

        var (lambdas2, iterJacobi2) = EigenMethods.JacobiMethod(A2, eps);
        Console.WriteLine($"2. Метод вращений Якоби:");
        Console.WriteLine($"   Собственные числа: {lambdas2[0]:F8}, {lambdas2[1]:F8}, {lambdas2[2]:F8}");
        Console.WriteLine($"   Количество итераций: {iterJacobi2}");
    }
}