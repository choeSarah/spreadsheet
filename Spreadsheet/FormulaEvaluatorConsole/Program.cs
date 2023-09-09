using FormulaEvaluator;

namespace FormulaEvaluatorConsole;


class Program
{
    static int NoKnownVars(String s)
    {
        return 3;
    }


    static void Main(string[] args)
    {

        int result = Evaluator.Evaluate("a4-a4*a4/a4", NoKnownVars);
        Console.WriteLine(result);

    }
}

