using System;

class NonlinearEquationSolver
{
    static double Function(double x)
    {
        return x * x - 4 * Math.Sin(x);
    }
    static void SolveByBisection()
    {
        Console.WriteLine("\n=== МЕТОД ДЕЛЕНИЯ ОТРЕЗКА ПОПОЛАМ ===");
        Console.Write("Введите левую границу (a): ");
        double a = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введите правую границу (b): ");
        double b = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введите точность (например 0.001): ");
        double epsilon = Convert.ToDouble(Console.ReadLine());

        Console.Write("Максимальное число итераций: ");
        int maxIterations = Convert.ToInt32(Console.ReadLine());
        if (Function(a) * Function(b) >= 0)
        {
            Console.WriteLine("Ошибка: На концах отрезка функция имеет одинаковые знаки!");
            Console.WriteLine($"F({a}) = {Function(a)}, F({b}) = {Function(b)}");
            return;
        }
        Console.WriteLine("\nВыполнение метода...");
        Console.WriteLine("Итер. |     a     |     b     |     c     |   F(c)   ");
        Console.WriteLine("---------------------------------------------------");

        int iteration = 0;
        double c = 0;
        double error = 0;

        while (iteration < maxIterations)
        {
            iteration++;
            c = (a + b) / 2;
            error = Math.Abs(b - a) / 2;
            Console.WriteLine($"{iteration,5} | {a,9:F6} | {b,9:F6} | {c,9:F6} | {Function(c),8:E3}");

            if (Math.Abs(Function(c)) < epsilon || error < epsilon)
            {
                Console.WriteLine("\nТочность достигнута!");
                break;
            }

            if (Function(a) * Function(c) < 0)
                b = c;
            else
                a = c;
        }
        Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ===");
        Console.WriteLine($"Найденный корень: x = {c:F8}");
        Console.WriteLine($"Значение функции: F(x) = {Function(c):E8}");
        Console.WriteLine($"Абсолютная погрешность: {error:E8}");
        Console.WriteLine($"Количество итераций: {iteration}");
        Console.WriteLine($"Проверка: F({c:F6}) = {Function(c):E8}");
    }

    static void SolveBySecant()
    {
        Console.WriteLine("\n=== МЕТОД СЕКУЩИХ ===");
        Console.Write("Введите первое приближение (x0): ");
        double x0 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введите второе приближение (x1): ");
        double x1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введите точность (например 0.001): ");
        double epsilon = Convert.ToDouble(Console.ReadLine());

        Console.Write("Максимальное число итераций: ");
        int maxIterations = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("\nВыполнение метода...");
        Console.WriteLine("Итер. |     x     |   F(x)    |  Разность ");
        Console.WriteLine("-------------------------------------------");

        int iteration = 0;
        double x2 = 0;
        double prevX = x1;

        while (iteration < maxIterations)
        {
            iteration++;
            double denominator = Function(x1) - Function(x0);
            if (Math.Abs(denominator) < 1e-15)
            {
                Console.WriteLine("Ошибка: Деление на ноль!");
                return;
            }
            x2 = x1 - Function(x1) * (x1 - x0) / denominator;
            double difference = Math.Abs(x2 - prevX);
            Console.WriteLine($"{iteration,5} | {x2,9:F6} | {Function(x2),8:E3} | {difference,8:E3}");

            if (Math.Abs(Function(x2)) < epsilon || difference < epsilon)
            {
                Console.WriteLine("\nТочность достигнута!");
                break;
            }
            x0 = x1;
            x1 = x2;
            prevX = x2;
        }

        Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ===");
        Console.WriteLine($"Найденный корень: x = {x2:F8}");
        Console.WriteLine($"Значение функции: F(x) = {Function(x2):E8}");
        Console.WriteLine($"Количество итераций: {iteration}");
        Console.WriteLine($"Проверка: F({x2:F6}) = {Function(x2):E8}");
    }

    static void ShowEquationInfo()
    {
        Console.WriteLine("\n=== ТЕКУЩЕЕ УРАВНЕНИЕ ===");
        Console.WriteLine("F(x) = x² - 4·sin(x)");
        Console.WriteLine("Примерные корни: x ≈ 1.93375 и x ≈ 0");

        Console.WriteLine("\nЧтобы изменить уравнение, отредактируйте");
        Console.WriteLine("метод Function() в исходном коде.");

        Console.WriteLine("\nПримеры других уравнений:");
        Console.WriteLine("1) x³ - 2x - 5 = 0");
        Console.WriteLine("2) cos(x) - x = 0");
        Console.WriteLine("3) e^x - 3x = 0");
        Console.WriteLine("4) ln(x) + x² - 3 = 0");
    }

    static void TestMethods()
    {
        Console.WriteLine("\n=== ТЕСТИРОВАНИЕ МЕТОДОВ ===");
        Console.WriteLine("Для уравнения x² - 4·sin(x) = 0");
        Console.WriteLine("Известный корень: x ≈ 1.93375");
        Console.WriteLine();

        Console.WriteLine("1. Метод деления отрезка пополам:");
        Console.WriteLine("   a = 1.5, b = 2.5, ε = 0.0001");

        Console.WriteLine("\n2. Метод секущих:");
        Console.WriteLine("   x0 = 1.5, x1 = 2.5, ε = 0.0001");

        Console.WriteLine("\nНажмите Enter для возврата в меню...");
        Console.ReadLine();
    }
    static void Main()
    {
        Console.WriteLine("РЕШЕНИЕ НЕЛИНЕЙНЫХ УРАВНЕНИЙ");
        Console.WriteLine("============================");

        while (true)
        {
            Console.WriteLine("\nГЛАВНОЕ МЕНЮ:");
            Console.WriteLine("1. Метод деления отрезка пополам");
            Console.WriteLine("2. Метод секущих");
            Console.WriteLine("3. Информация об уравнении");
            Console.WriteLine("4. Тестирование методов");
            Console.WriteLine("5. Выход");
            Console.Write("\nВыберите пункт: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SolveByBisection();
                    break;

                case "2":
                    SolveBySecant();
                    break;

                case "3":
                    ShowEquationInfo();
                    break;

                case "4":
                    TestMethods();
                    break;

                case "5":
                    Console.WriteLine("\nВыход из программы...");
                    return;

                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }

            Console.WriteLine("\nНажмите Enter для продолжения...");
            Console.ReadLine();
            Console.Clear();
        }
    }
}