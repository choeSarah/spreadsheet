// See https://aka.ms/new-console-template for more information
using System;
using System.Xml.Linq;
using SpreadsheetUtilities;
using SS;

Spreadsheet s1 = new Spreadsheet();
s1.SetContentsOfCell("A1", "3");
s1.SetContentsOfCell("A2", "=A1");

s1.Save("save1.txt");