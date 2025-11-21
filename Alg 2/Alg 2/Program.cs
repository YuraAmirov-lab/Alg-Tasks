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

    static double Trapezoidal(double a, double b, int n)
    {
        double h = (b - a) / n;
        double sum = (F(a) + F(b)) / 2;

        for (int i = 1; i < n; i++)
        {
            sum += F(a + i * h);
        }

        return h * sum;
    }

    static double Simpson(double a, double b, int n)
    {
        if (n % 2 != 0) n++;
        double h = (b - a) / n;
        double sum = F(a) + F(b);

        for (int i = 1; i < n; i++)
        {
            double x = a + i * h;
            if (i % 2 == 0)
            {
                sum += 2 * F(x);
            }
            else
            {
                sum += 4 * F(x);
            }
        }

        return h * sum / 3;
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
        Console.WriteLine("Правые прямоугольники:");
        Console.WriteLine($"Интеграл = {new_right}");
        Console.WriteLine($"Шаг = {(b - a) / (2 * n)}");
        Console.WriteLine($"Разбиений = {2 * n}");

        n = 1;
        double old_trap = Trapezoidal(a, b, n);
        double new_trap = Trapezoidal(a, b, 2 * n);
        while (Math.Abs(new_trap - old_trap) > epsil)
        {
            n *= 2;
            old_trap = new_trap;
            new_trap = Trapezoidal(a, b, n);
        }
        Console.WriteLine("Метод трапеций:");
        Console.WriteLine($"Интеграл = {new_trap}");
        Console.WriteLine($"Шаг = {(b - a) / (2 * n)}");
        Console.WriteLine($"Разбиений = {2 * n}");

        n = 2;
        double old_simp = Simpson(a, b, n);
        double new_simp = Simpson(a, b, 2 * n);
        while (Math.Abs(new_simp - old_simp) > epsil)
        {
            n *= 2;
            old_simp = new_simp;
            new_simp = Simpson(a, b, n);
        }
        Console.WriteLine("Метод Симпсона:");
        Console.WriteLine($"Интеграл = {new_simp}");
        Console.WriteLine($"Шаг = {(b - a) / (2 * n)}");
        Console.WriteLine($"Разбиений = {2 * n}");
    }
}