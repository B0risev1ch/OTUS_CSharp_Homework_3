﻿using System.Collections;


var data = new Dictionary<string, string?>();
Console.WriteLine("a * x^2 + b * x + c = 0");
do
{
    data.Clear();
    Console.Write("a: ");
    data.Add("a", Console.ReadLine());
    Console.Write("b: ");
    data.Add("b", Console.ReadLine());
    Console.Write("c: ");
    data.Add("c", Console.ReadLine());
} while (!CorrectUserDataParsed(data));

try
{
    DoMath(data);
}
catch (NoAnswerException e)
{
    FormatData($"{e.Message}", Severity.Warning, null);
}

static bool CorrectUserDataParsed(IDictionary data)
{
    var formatCastException = new Dictionary<string, string>();

    try
    {
        foreach (DictionaryEntry dataItem in data)
        {
            try
            {
                int.Parse(dataItem.Value.ToString());
            }
            catch (FormatException)
            {
                formatCastException.Add(dataItem.Key.ToString(), dataItem.Value.ToString());
            }
        }

        if (formatCastException.Count > 0)
            throw new FormatException();
    }
    catch (FormatException)
    {
        FormatData("Неверный формат параметра", Severity.Error, formatCastException);
        return false;
    }

    return true;
}

static void DoMath(IDictionary data)
{
    try
    {
        var a = int.Parse(data["a"].ToString());
        var b = int.Parse(data["b"].ToString());
        var c = int.Parse(data["c"].ToString());

        var D = Math.Pow(b, 2) - 4 * a * c;
        const double Epsilon = 0.00001;

        switch (D)
        {
            case > 0:
                var x1 = (-b + Math.Sqrt(D)) / (2 * a);
                var x2 = (-b - Math.Sqrt(D)) / (2 * a);
                Console.WriteLine($"x1= {x1}\nx2= {x2}");
                Console.ReadKey();
                break;

            case var expression when Math.Abs(D) < Epsilon:
                var x = (-b + Math.Sqrt(D)) / (2 * a);
                Console.WriteLine($"x= {x}\n");
                Console.ReadKey();
                break;

            default:
                throw new NoAnswerException();
        }
    }
    catch (NoAnswerException)
    {
        throw new NoAnswerException();
    }
    catch (Exception)
    {
        throw new ArithmeticException("Ошибка при расчёте!");
    }
}

static void FormatData(string message, Severity severity, IDictionary data)
{
    switch (severity)
    {
        case Severity.Warning:
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine(message);
            Console.WriteLine("-------------------------------------------------------");
            if (data != null)
                foreach (DictionaryEntry failedDataEntry in data)
                    Console.WriteLine($"{failedDataEntry.Key} = {failedDataEntry.Value}", severity);
            break;

        case Severity.Error:
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine(message);
            Console.WriteLine("-------------------------------------------------------");

            if (data != null)
                foreach (DictionaryEntry failedDataEntry in data)
                    Console.WriteLine($"{failedDataEntry.Key} = {failedDataEntry.Value}", severity);
            break;
    }
    Console.ResetColor();
}

class NoAnswerException : Exception
{
    public NoAnswerException() : base($"Вещественных корней не найдено!") { }

    }

enum Severity
{
    Warning,
    Error
}

