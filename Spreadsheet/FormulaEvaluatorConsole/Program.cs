using FormulaEvaluator;

namespace FormulaEvaluatorConsole;


class Program
{
    static int NoKnownVars(String s)
    {
        return -2;
    }


    static void Main(string[] args)
    {

        int result = Evaluator.Evaluate("2-A2", NoKnownVars);
        Console.WriteLine(result);
    }
}

