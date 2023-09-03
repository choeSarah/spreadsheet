using FormulaEvaluator;

namespace FormulaEvaluatorConsole;


class Program
{
    static int NoKnownVars(String s)
    {
        Console.WriteLine("Inside delegate");
        return 0;
    }


    static void Main(string[] args)
    {

        int result = Evaluator.Evaluate("2#4", NoKnownVars);
        Console.WriteLine(result);
    }
}

