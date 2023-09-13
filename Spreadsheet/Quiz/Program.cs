public class Thing
{
    public string? X
    {
        get; set;
    }

    public Thing(string x)
    {
        X = x;
    }

    public static void Main(string[] args)
    {
        Thing thing = new Thing("a");
    }
}

