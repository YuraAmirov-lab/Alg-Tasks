using System;

class MatrixInverseGauss
{
    static void Main()
    {
        double[,] matrix = {
            { 1, 2, 3 },
            { 0, 1, 4 },
            { 5, 6, 0 }
        };

        Console.WriteLine("Исходная матрица:");
        PrintMatrix(matrix);

        try
        {
            double[,] inverse = InverseMatrix(matrix);
            Console.WriteLine("\nОбратная матрица:");
            PrintMatrix(inverse);

            Console.WriteLine("\nПроверка (A * A^-1):");
            double[,] check = MultiplyMatrices(matrix, inverse);
            PrintMatrix(check);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    static double[,] InverseMatrix(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        int m = matrix.GetLength(1);

        if (n != m)
        {
            throw new ArgumentException("Матрица должна быть квадратной!");
        }

        double[,] augmented = new double[n, 2 * n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                augmented[i, j] = matrix[i, j];
            }
            augmented[i, n + i] = 1.0;
        }

        for (int i = 0; i < n; i++)
        {
            int maxRow = i;
            double maxVal = Math.Abs(augmented[i, i]);

            for (int k = i + 1; k < n; k++)
            {
                if (Math.Abs(augmented[k, i]) > maxVal)
                {
                    maxVal = Math.Abs(augmented[k, i]);
                    maxRow = k;
                }
            }

            if (maxVal < 1e-10)
            {
                throw new InvalidOperationException("Матрица вырождена!");
            }

            if (maxRow != i)
            {
                for (int j = 0; j < 2 * n; j++)
                {
                    double temp = augmented[i, j];
                    augmented[i, j] = augmented[maxRow, j];
                    augmented[maxRow, j] = temp;
                }
            }

            double pivot = augmented[i, i];
            for (int j = 0; j < 2 * n; j++)
            {
                augmented[i, j] /= pivot;
            }

            for (int k = 0; k < n; k++)
            {
                if (k != i)
                {
                    double factor = augmented[k, i];
                    for (int j = 0; j < 2 * n; j++)
                    {
                        augmented[k, j] -= factor * augmented[i, j];
                    }
                }
            }
        }

        double[,] inverse = new double[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                inverse[i, j] = augmented[i, n + j];
            }
        }

        return inverse;
    }

    static double[,] MultiplyMatrices(double[,] a, double[,] b)
    {
        int n = a.GetLength(0);
        int m = a.GetLength(1);
        int p = b.GetLength(1);

        double[,] result = new double[n, p];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < p; j++)
            {
                double sum = 0;
                for (int k = 0; k < m; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                result[i, j] = sum;
            }
        }

        return result;
    }

    static void PrintMatrix(double[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{matrix[i, j],12:F6} ");
            }
            Console.WriteLine();
        }
    }
}