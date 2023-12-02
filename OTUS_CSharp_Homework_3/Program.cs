using System.Collections;


var data = new Dictionary<string, string?>();
Console.WriteLine($"a * x^2 + b * x + c = 0");
do
{
    data.Clear();
    Console.Write($"a: ");
    data.Add("a", Console.ReadLine());
    Console.Write($"b: ");
    data.Add("b", Console.ReadLine());
    Console.Write($"c: ");
    data.Add("c", Console.ReadLine());
} while (!CorrectUserDataParsed(data));

try
{
    DoMath(data);
}
catch
{
    FormatData("Вещественных значений не найдено", Severity.Warning, null);
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
    catch (FormatException e)
    {
        FormatData("Неверный формат параметра", Severity.Error, formatCastException);
        return false;
    }

    return true;
}


void DoMath(IDictionary data)
{
    var a = int.Parse(data["a"].ToString());
    var b = int.Parse(data["b"].ToString());
    var c = int.Parse(data["c"].ToString());

    var  D = Math.Pow(b, 2) - 4 * a * c;

    switch (D)
    {
        case var expression when D > 0:
            var x1 = (-b + Math.Sqrt(D)) / (2 * a);
            var x2 = (-b - Math.Sqrt(D)) / (2 * a);
            Console.WriteLine($"x1= {x1}\nx2= {x2}");
            Console.ReadKey();
            break;

        case var expression when D == 0:
            var x = (-b + Math.Sqrt(D)) / (2 * a);
            Console.WriteLine($"x= {x}\n");
            Console.ReadKey();
            break;

        default:
            throw new Exception("Вещественных значений не найдено");
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



enum Severity
{
    Warning,
    Error
}

