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
        Formula f1 = new Formula("5+a1");
    }
}

