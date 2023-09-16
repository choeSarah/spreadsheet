using SpreadsheetUtilities;

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
        Formula f1 = new Formula("v1 + _var12*a9 / b_12");
        Console.WriteLine(f1.Evaluate(s => (s == "x7") ? 1 : 4).ToString());

    }
}

