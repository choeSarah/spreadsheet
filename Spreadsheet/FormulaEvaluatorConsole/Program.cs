using FormulaEvaluator;

namespace FormulaEvaluatorConsole;


class Program
{
    static int NoKnownVars(String s)
    {
        return 0;
    }


    static void Main(string[] args)
    {

        int result = Evaluator.Evaluate("2+5*7)", NoKnownVars);
        Console.WriteLine(result);

    }
}

