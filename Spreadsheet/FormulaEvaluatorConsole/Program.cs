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

        //int result = Evaluator.Evaluate("-)", NoKnownVars);
        //Console.WriteLine(result);

        Console.WriteLine(9.0 / 0.0);

    }
}

