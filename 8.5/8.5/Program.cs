using System;

class SimpleFFT
{
    static void Main()
    {
        Console.WriteLine("=== БЫСТРОЕ ПРЕОБРАЗОВАНИЕ ФУРЬЕ (БПФ) ===\n");

        double[] signal = { 1, 2, 3, 4, 4, 3, 2, 1 };

        Console.WriteLine("Исходный сигнал:");
        for (int i = 0; i < signal.Length; i++)
        {
            Console.WriteLine($"x[{i}] = {signal[i]:F2}");
        }

        Complex[] spectrum = FFT(signal);

        Console.WriteLine("\nСпектр (прямое БПФ):");
        for (int i = 0; i < spectrum.Length; i++)
        {
            Console.WriteLine($"F[{i}] = {spectrum[i].Real:F2} + {spectrum[i].Imag:F2}i");
        }

        Console.WriteLine("\nАмплитуды:");
        for (int i = 0; i < spectrum.Length; i++)
        {
            double amplitude = Math.Sqrt(spectrum[i].Real * spectrum[i].Real +
                                        spectrum[i].Imag * spectrum[i].Imag);
            Console.WriteLine($"A[{i}] = {amplitude:F2}");
        }
    }

    struct Complex
    {
        public double Real;
        public double Imag;

        public Complex(double real, double imag)
        {
            Real = real;
            Imag = imag;
        }
    }

    static Complex ComplexAdd(Complex a, Complex b)
    {
        return new Complex(a.Real + b.Real, a.Imag + b.Imag);
    }

    static Complex ComplexSubtract(Complex a, Complex b)
    {
        return new Complex(a.Real - b.Real, a.Imag - b.Imag);
    }

    static Complex ComplexMultiply(Complex a, Complex b)
    {
        return new Complex(
            a.Real * b.Real - a.Imag * b.Imag,
            a.Real * b.Imag + a.Imag * b.Real
        );
    }

    static Complex[] FFT(double[] signal)
    {
        int N = signal.Length;

        if ((N & (N - 1)) != 0)
        {
            Console.WriteLine("Предупреждение: длина сигнала должна быть степенью двойки!");
            return null;
        }

        Complex[] x = new Complex[N];
        for (int i = 0; i < N; i++)
        {
            x[i] = new Complex(signal[i], 0);
        }

        return FFT_Recursive(x);
    }

    static Complex[] FFT_Recursive(Complex[] x)
    {
        int N = x.Length;

        if (N == 1)
        {
            return new Complex[] { x[0] };
        }

        Complex[] even = new Complex[N / 2];
        Complex[] odd = new Complex[N / 2];

        for (int i = 0; i < N / 2; i++)
        {
            even[i] = x[2 * i];
            odd[i] = x[2 * i + 1];
        }

        Complex[] evenFFT = FFT_Recursive(even);
        Complex[] oddFFT = FFT_Recursive(odd);

        Complex[] result = new Complex[N];

        for (int k = 0; k < N / 2; k++)
        {
            double angle = -2 * Math.PI * k / N;
            Complex w = new Complex(Math.Cos(angle), Math.Sin(angle));

            Complex t = ComplexMultiply(w, oddFFT[k]);
            result[k] = ComplexAdd(evenFFT[k], t);
            result[k + N / 2] = ComplexSubtract(evenFFT[k], t);
        }

        return result;
    }
}