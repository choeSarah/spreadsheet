using SpreadsheetUtilities;
using SS;

namespace SpreadsheetTests;

[TestClass]
public class SpreadsheetTests
{
    //EMPTY TESTS
    [TestMethod]
    public void EmptyGetCellContents()
    {
        Spreadsheet ss = new Spreadsheet();
        var ssActual = ss.GetCellContents("a3");
        String expected = "";

        Assert.AreEqual(expected, ssActual);
    }

    [TestMethod]
    public void EmptyGetNamesOfAllNonEmptyCells()
    {
        Spreadsheet ss = new Spreadsheet();
        List<string> ssActual = ss.GetNamesOfAllNonemptyCells().ToList();
        List<string> ssExpected = new List<string>();

        CollectionAssert.AreEqual(ssExpected, ssActual);
    }

    //SINGULAR CELLS///////////////////////////////
    //DOUBLES
    [TestMethod]
    public void SingularDoubleGetCellContents()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", 3);
        var ssActual = ss.GetCellContents("a3");
        double expected = 3.0;

        Assert.AreEqual(expected, ssActual);
    }

    [TestMethod]
    public void SingularGetNamesOfAllNonEmptyCellsDouble()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", 2.0);

        List<string> ssActual = ss.GetNamesOfAllNonemptyCells().ToList();
        List<string> ssExpected = new List<string>();
        ssExpected.Add("a3");

        CollectionAssert.AreEqual(ssExpected, ssActual);
    }
    //STRINGS
    [TestMethod]
    public void SingularStringGetCellContents()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", "hi");
        var ssActual = ss.GetCellContents("a3");
        String expected = "hi";

        Assert.AreEqual(expected, ssActual);
    }

    [TestMethod]
    public void SingularGetNamesOfAllNonEmptyCellsString()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", "hi");

        List<string> ssActual = ss.GetNamesOfAllNonemptyCells().ToList();
        List<string> ssExpected = new List<string>();
        ssExpected.Add("a3");

        CollectionAssert.AreEqual(ssExpected, ssActual);
    }

    //FORMULAS

    [TestMethod]
    public void SingularFormulaGetCellContents()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", new Formula("a4+a2"));
        var ssActual = ss.GetCellContents("a3");
        Formula expected = new Formula("a4+a2");

        Assert.AreEqual(expected, ssActual);
    }

    [TestMethod]
    public void SingularGetNamesOfAllNonEmptyCellsFormula() 
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", new Formula("a4+a2"));

        List<string> ssActual = ss.GetNamesOfAllNonemptyCells().ToList();
        List<string> ssExpected = new List<string>();
        ssExpected.Add("a3");

        CollectionAssert.AreEqual(ssExpected, ssActual);
    }

    //Medium Tests
    //DOUBLE
    [TestMethod]
    public void MediumDoubleGetCellContents()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", 3);
        ss.SetCellContents("a4", 4);
        ss.SetCellContents("a5", 7);
        ss.SetCellContents("a6", 9);
        ss.SetCellContents("a7", 13);

        Assert.AreEqual(ss.GetCellContents("a3"), 3.0);
        Assert.AreEqual(ss.GetCellContents("a4"), 4.0);
        Assert.AreEqual(ss.GetCellContents("a5"), 7.0);
        Assert.AreEqual(ss.GetCellContents("a6"), 9.0);
        Assert.AreEqual(ss.GetCellContents("a7"), 13.0);

        List<string> ssExpected = new List<string>();
        ssExpected.Add("a3");
        ssExpected.Add("a4");
        ssExpected.Add("a5");
        ssExpected.Add("a6");
        ssExpected.Add("a7");

        CollectionAssert.AreEqual(ssExpected, ss.GetNamesOfAllNonemptyCells().ToList());
    }

    public void MediumStringGetCellContents()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", "a");
        ss.SetCellContents("a4", "b");
        ss.SetCellContents("a5", "c");
        ss.SetCellContents("a6", "d");
        ss.SetCellContents("a7", "e");

        Assert.AreEqual(ss.GetCellContents("a3"), "a");
        Assert.AreEqual(ss.GetCellContents("a4"), "b");
        Assert.AreEqual(ss.GetCellContents("a5"), "c");
        Assert.AreEqual(ss.GetCellContents("a6"), "d");
        Assert.AreEqual(ss.GetCellContents("a7"), "e");

        List<string> ssExpected = new List<string>();
        ssExpected.Add("a3");
        ssExpected.Add("a4");
        ssExpected.Add("a5");
        ssExpected.Add("a6");
        ssExpected.Add("a7");

        CollectionAssert.AreEqual(ssExpected, ss.GetNamesOfAllNonemptyCells().ToList());
    }

    public void MediumFormulaGetCellContents()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", new Formula("1+2"));
        ss.SetCellContents("a4", new Formula("a1+2"));
        ss.SetCellContents("a5", new Formula("3+5"));
        ss.SetCellContents("a6", new Formula("a2*8"));
        ss.SetCellContents("a7", new Formula("4/2"));

        Assert.AreEqual(ss.GetCellContents("a3"), new Formula("1+2"));
        Assert.AreEqual(ss.GetCellContents("a4"), new Formula("a1+2"));
        Assert.AreEqual(ss.GetCellContents("a5"), new Formula("3+5"));
        Assert.AreEqual(ss.GetCellContents("a6"), new Formula("a2*8"));
        Assert.AreEqual(ss.GetCellContents("a7"), new Formula("4/2"));

        List<string> ssExpected = new List<string>();
        ssExpected.Add("a3");
        ssExpected.Add("a4");
        ssExpected.Add("a5");
        ssExpected.Add("a6");
        ssExpected.Add("a7");
        ssExpected.Add("a1");
        ssExpected.Add("a2");

        CollectionAssert.AreEqual(ssExpected, ss.GetNamesOfAllNonemptyCells().ToList());
    }

    //Mixed Cases
    [TestMethod]
    public void SwitchingContentTypes()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", "hi");
        Assert.AreEqual(ss.GetCellContents("a3"), "hi");
        ss.SetCellContents("a3", 3);
        Assert.AreEqual(ss.GetCellContents("a3"), 3.0);
        ss.SetCellContents("a3", new Formula("a4+1"));
        Assert.AreEqual(ss.GetCellContents("a3"), new Formula("a4+1"));
    }

    [TestMethod]
    public void SwitchingContentTypesToEmpty()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", "hi");
        Assert.AreEqual(ss.GetCellContents("a3"), "hi");
        ss.SetCellContents("a3", 3);
        Assert.AreEqual(ss.GetCellContents("a3"), 3.0);
        ss.SetCellContents("a3", new Formula("a4+1"));
        Assert.AreEqual(ss.GetCellContents("a3"), new Formula("a4+1"));

        ss.SetCellContents("a3", "");
        Assert.AreEqual(ss.GetCellContents("a3"), "");

        List<string> ssExpected = new List<string>();
        CollectionAssert.AreEqual(ssExpected, ss.GetNamesOfAllNonemptyCells().ToList());

    }

    [TestMethod]
    public void SwitchingFormulas()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", new Formula("a2+1"));
        ss.SetCellContents("a3", new Formula("a1*7"));

        Assert.AreEqual(ss.GetCellContents("a3"), new Formula("a1*7"));

        List<string> ssExpected = new List<string>();
        ssExpected.Add("a3");

        CollectionAssert.AreEqual(ssExpected, ss.GetNamesOfAllNonemptyCells().ToList());

    }

    //Testing Set Lists
    [TestMethod]
    public void SetFormula1Dependent()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", new Formula("b1 + c3"));
        ss.SetCellContents("b2", new Formula("4+ c3"));
        ss.SetCellContents("b2", 3);


        List<string> ssActual = ss.SetCellContents("c3", 4).ToList();
        List<string> ssExpected = new List<string>();
        ssExpected.Add("c3");
        ssExpected.Add("a3");

        CollectionAssert.AreEqual(ssExpected, ssActual);
    }

    //Testing Set Lists
    [TestMethod]
    public void SetFormula3Dependent()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a1", new Formula("b1 + b1"));
        ss.SetCellContents("b1", new Formula("d1+1"));
        ss.SetCellContents("c1", new Formula("4+ b1"));


        List<string> ssActual =ss.SetCellContents("b1", 4).ToList();
        List<string> ssExpected = new List<string>();
        ssExpected.Add("b1");
        ssExpected.Add("c1");
        ssExpected.Add("a1");


        CollectionAssert.AreEqual(ssExpected, ssActual);
    }

    //Special Cases
    [TestMethod]
    public void AddEmptyGetNamesOfAllNonEmptyCellsString()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", "");

        List<string> ssActual = ss.GetNamesOfAllNonemptyCells().ToList();
        List<string> ssExpected = new List<string>();

        CollectionAssert.AreEqual(ssExpected, ssActual);
    }

    [TestMethod]
    public void AddNonEmptySetEmptyGetNamesOfAllNonEmptyCellsString()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", "three");
        ss.SetCellContents("a3", "");


        List<string> ssActual = ss.GetNamesOfAllNonemptyCells().ToList();
        List<string> ssExpected = new List<string>();

        CollectionAssert.AreEqual(ssExpected, ssActual);
    }

    //Testing Dependencies

    [TestMethod]
    public void IndirectDependentsSetCellContentsString()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a4", new Formula("a3+2"));
        ss.SetCellContents("a5", new Formula("a4*7"));
        List<string> dependentsA3 =  ss.SetCellContents("a3", 4).ToList();

        List<string> ssActual = new List<string>();
        ssActual.Add("a3");
        ssActual.Add("a4");
        ssActual.Add("a5");

        CollectionAssert.AreEqual(dependentsA3, ssActual);
    }

    [TestMethod]
    public void IndirectDependentsSetCellContentsStringToEmpty()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a4", new Formula("a3+2"));
        ss.SetCellContents("a5", new Formula("a4*7"));
        ss.SetCellContents("a3", 4).ToList();
        List<string> dependentsA3 = ss.SetCellContents("a3", "").ToList();
        
        List<string> ssActual = new List<string>();
        ssActual.Add("a3");
        ssActual.Add("a4");
        ssActual.Add("a5");

        CollectionAssert.AreEqual(dependentsA3, ssActual);
    }

    //EXCEPTIONS

    //FORMULA EXCEPTIONS
    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void ParsingError1()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", new Formula("%5+3"));
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void ParsingError2()
    {
        Formula f1 = new Formula("5$+3");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void ParsingError3()
    {
        Formula f1 = new Formula("5^+3");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    //no token error
    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void NoTokenConstructor()
    {
        Formula f1 = new Formula("");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    //Right Parentheses Rule
    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void MoreClosingParanthesesConstructor()
    {
        Formula f1 = new Formula("(5+3))");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    //Balanced Parentheses Rule
    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void MoreOpeningParanthesesConstructor()
    {
        Formula f1 = new Formula("((5+3)");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    //Starting Token Rule
    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void BeginsWithOperatorConstructorAdd()
    {
        Formula f1 = new Formula("+3+5");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void BeginsWithOperatorConstructorMinus()
    {
        Formula f1 = new Formula("-3+5");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void BeginsWithOperatorConstructorMultiply()
    {
        Formula f1 = new Formula("*3+5");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void BeginsWithOperatorConstructorDivide()
    {
        Formula f1 = new Formula("/3+5");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void BeginsWithOperatorConstructorClosingParanthesis()
    {
        Formula f1 = new Formula(")3+5");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    //Ending Token Rule
    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void EndsWithOperatorConstructorAdd()
    {
        Formula f1 = new Formula("3+5+");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void EndsWithOperatorConstructorMinus()
    {
        Formula f1 = new Formula("3+5-");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void EndsWithOperatorConstructorMultiply()
    {
        Formula f1 = new Formula("3+5*");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void EndsWithOperatorConstructorDivide()
    {
        Formula f1 = new Formula("3+5/");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    //Ending Token Rule
    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOpeningParanthesisAdd()
    {
        Formula f1 = new Formula("(+3+7)");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    //Parenthesis/Operator Following Rule
    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOpeningParanthesisSubtract()
    {
        Formula f1 = new Formula("(-3+7)");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOpeningParanthesisMultiply()
    {
        Formula f1 = new Formula("(*3+7)");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOpeningParanthesisDivide()
    {
        Formula f1 = new Formula("(/3+7)");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOpeningParanthesisClosingParanthesis()
    {
        Formula f1 = new Formula("()3+7)");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOperatorAdd()
    {
        Formula f1 = new Formula("3++7");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOperatorSubtract()
    {
        Formula f1 = new Formula("3+-7");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOperatorMultiply()
    {
        Formula f1 = new Formula("3+*7");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOperatorDivide()
    {
        Formula f1 = new Formula("3+/7");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOperatorClosingParanthesis()
    {
        Formula f1 = new Formula("3+)7");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    //Extra Following Rule
    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void NumFollowingNum()
    {
        Formula f1 = new Formula("3 7");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void VarFollowingNum()
    {
        Formula f1 = new Formula("x3 7");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OpeningParanthesisFollowingNum()
    {
        Formula f1 = new Formula("3(");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OpeningParanthesisFollowingVar()
    {
        Formula f1 = new Formula("c3(");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void NumFollowingClosingParanthesis()
    {
        Formula f1 = new Formula("(4+3)3");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void VarFollowingClosingParanthesis()
    {
        Formula f1 = new Formula("(4+3)c3");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OpeningParanthesisFollowingClosingParanthesis()
    {
        Formula f1 = new Formula("(4+3)(");
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", f1);
    }
    //INVALIDNAMEEXCEPTION
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void InvalidFormula()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", new Formula("4a+1"));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void InvalidNameTestString()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("3a", "hi");
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void InvalidNameTestFormula()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("3a", new Formula("a4+a2"));
    }

    //CIRCULAREXCEPTION
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void CircularExceptionTest1()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", new Formula("a4+a2"));
        ss.SetCellContents("a4", new Formula("a3+a2"));

        List<string> ssExpected = new List<string>();
        ssExpected.Add("a3");

        CollectionAssert.AreEqual(ssExpected, ss.GetNamesOfAllNonemptyCells().ToList());

    }

    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void CircularExceptionTest2()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", new Formula("a3+a2"));

        List<string> ssExpected = new List<string>();

        CollectionAssert.AreEqual(ssExpected, ss.GetNamesOfAllNonemptyCells().ToList());

    }

    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void CircularExceptionTest3()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a3", new Formula("a4 + 7"));
        ss.SetCellContents("a4", new Formula("a5*13"));
        ss.SetCellContents("a5", new Formula("a3/2"));

        List<string> ssExpected = new List<string>();
        ssExpected.Add("a3");
        ssExpected.Add("a4");


        CollectionAssert.AreEqual(ssExpected, ss.GetNamesOfAllNonemptyCells().ToList());
    }

    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void CircularExceptionTest4()
    {
        Spreadsheet ss = new Spreadsheet();
        ss.SetCellContents("a1", new Formula("a2 + 1"));
        ss.SetCellContents("a2", new Formula("a3 + 1"));
        ss.SetCellContents("a3", new Formula("a4 + 1"));
        ss.SetCellContents("a4", new Formula("a5 + 1"));
        ss.SetCellContents("a5", new Formula("a6 + 1"));
        ss.SetCellContents("a6", new Formula("a1 + 1"));

        List<string> ssExpected = new List<string>();
        ssExpected.Add("a1");
        ssExpected.Add("a2");
        ssExpected.Add("a3");
        ssExpected.Add("a4");
        ssExpected.Add("a5");


        CollectionAssert.AreEqual(ssExpected, ss.GetNamesOfAllNonemptyCells().ToList());
    }
}
