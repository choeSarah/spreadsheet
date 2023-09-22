// See https://aka.ms/new-console-template for more information
using SpreadsheetUtilities;
using SS;
Spreadsheet ss = new Spreadsheet();
ss.SetCellContents("a1", new Formula("b1 + b1"));
ss.SetCellContents("b1", new Formula("d1+1"));
ss.SetCellContents("c1", new Formula("4+ b1"));


List<string> ssActual = ss.SetCellContents("b1", 4).ToList();
List<string> ssExpected = new List<string>();
ssExpected.Add("b1");
ssExpected.Add("a1");
ssExpected.Add("c1");

foreach (string s in ssActual)
{
    Console.WriteLine(s);
}
