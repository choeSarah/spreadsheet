// See https://aka.ms/new-console-template for more information
using SpreadsheetUtilities;

DependencyGraph t = new DependencyGraph();
t.AddDependency("x", "b");
t.AddDependency("a", "z");
t.ReplaceDependents("b", new HashSet<string>()); //nothing to remove, nothing to add
t.AddDependency("y", "b");
t.ReplaceDependents("a", new HashSet<string>() { "c" });
t.AddDependency("w", "d");
t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
t.ReplaceDependees("d", new HashSet<string>() { "b" });

var temp = t.GetDependents("b");

foreach (String s in temp)
{
    Console.WriteLine("hi, " + s);
}
