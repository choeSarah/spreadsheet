// See https://aka.ms/new-console-template for more information
using System;
using System.Xml.Linq;
using SpreadsheetUtilities;
using SS;

Spreadsheet ss = new Spreadsheet();

//f1: a1 = 10 -> 11; a2 = 5 -> 6
ss.SetContentsOfCell("a1", "= a2 + a3");
ss.SetContentsOfCell("a2", "= b1 + b2");
ss.SetContentsOfCell("a3", "5.0");
ss.SetContentsOfCell("b1", "2.0");
ss.SetContentsOfCell("b2", "3.0");
ss.SetContentsOfCell("b2", "4.0");

//f2
ss.SetContentsOfCell("a1", "= a2 + a3"); // ends with 5+5
ss.SetContentsOfCell("a2", "= a3"); // ends with 5
ss.SetContentsOfCell("a3", "6.0");
ss.SetContentsOfCell("a3", "5.0");

//f3
ss.SetContentsOfCell("a1", "= a3 + a5"); //18
ss.SetContentsOfCell("a2", "= a5 + a4"); //18
ss.SetContentsOfCell("a3", "= a5"); //9
ss.SetContentsOfCell("a4", "= a5"); //9
ss.SetContentsOfCell("a5", "9.0");
//ss.SetContentsOfCell("a5", "8.0");



Console.WriteLine(ss.GetCellValue("a1"));

//ss.SetContentsOfCell("A3", "=5");
//ss.SetContentsOfCell("A2", "=5+A3");

//Console.WriteLine(ss.GetCellValue("A2"));
