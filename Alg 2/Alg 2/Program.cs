using System;

class Program
{
    static double F(double x)
    {
        return Math.Sin(x);
    }
    static double LeftRectangles(double a, double b, int n)
    {
        double h = (b - a) / n;
        double sum = 0.0;
        for (int i = 0; i < n; i++)
            sum += F(a + i * h);
        return h * sum;
    }

    static double RightRectangles(double a, double b, int n)
    {
        double h = (b - a) / n;
        double sum = 0.0;
        for (int i = 1; i <= n; i++)
            sum += F(a + i * h);
        return h * sum;
    }

    static void Main()
    {
        double a = 1.0;          
        double b = Math.PI;      
        double epsil = 0.00000001;   
        int n = 1;               

        double old_left = LeftRectangles(a, b, n);
        double new_left = LeftRectangles(a, b, 2 * n);
        while (Math.Abs(new_left - old_left) > epsil)
        {
            n *= 2;
            old_left = new_left;
            new_left = LeftRectangles(a, b, n);
        }
        Console.WriteLine("Левые прямоугольники:");
        Console.WriteLine($"Интеграл = {new_left}");
        Console.WriteLine($"Шаг = {(b - a) / (2 * n)}");
        Console.WriteLine($"Разбиений = {2 * n}");

        n = 1;
        double old_right = RightRectangles(a, b, n);
        double new_right = RightRectangles(a, b, 2 * n);
        while (Math.Abs(new_right - old_right) > epsil)
        {
            n *= 2;
            old_right = new_right;
            new_right = RightRectangles(a, b, n);
        }
        Console.WriteLine("\nПравые прямоугольники:");
        Console.WriteLine($"Интеграл = {new_right}");
        Console.WriteLine($"Шаг = {(b - a) / (2 * n)}");
        Console.WriteLine($"Разбиений = {2 * n}");
    }
}