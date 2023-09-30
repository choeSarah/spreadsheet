using SpreadsheetUtilities;
using SS;

namespace SpreadsheetTests;

[TestClass]
public class SpreadsheetTests
{

    // EMPTY SPREADSHEETS - ZERO PARAMETER CONSTRUCTOR
    [TestMethod()]
    [TestCategory("2")]
    [ExpectedException(typeof(InvalidNameException))]
    public void TestEmptyGetContentsZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.GetCellContents("1AA");
    }

    [TestMethod()]
    [TestCategory("3")]
    public void TestGetEmptyContentsZERO()
    {
        Spreadsheet s = new Spreadsheet();
        Assert.AreEqual("", s.GetCellContents("A2"));
    }

    // SETTING CELL TO A DOUBLE - ZERO PARAMETER CONSTRUCTOR
    [TestMethod()]
    [TestCategory("5")]
    [ExpectedException(typeof(InvalidNameException))]
    public void TestSetInvalidNameDoubleZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("1A1A", "1.5");
    }

    [TestMethod()]
    [TestCategory("6")]
    public void TestSimpleSetDoubleZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("Z7", "1.5");
        Assert.AreEqual(1.5, 1e-9, (double)s.GetCellContents("Z7"));
    }

    // SETTING CELL TO A STRING - ZERO PARAMETER CONSTRUCTOR
    [TestMethod()]
    [TestCategory("9")]
    [ExpectedException(typeof(InvalidNameException))]
    public void TestSetSimpleStringZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("1AZ", "hello");
    }

    [TestMethod()]
    [TestCategory("10")]
    public void TestSetGetSimpleStringZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("Z7", "hello");
        Assert.AreEqual("hello", s.GetCellContents("Z7"));
    }

    // SETTING CELL TO A FORMULA
    [TestMethod()]
    [TestCategory("13")]
    [ExpectedException(typeof(InvalidNameException))]
    public void TestSetSimpleFormZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("1AZ", "=2");
    }

    [TestMethod()]
    [TestCategory("14")]
    public void TestSetGetFormZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("Z7", "=3");
        Formula f = (Formula)s.GetCellContents("Z7");
        Assert.AreEqual(new Formula("3"), f);
        Assert.AreNotEqual(new Formula("2"), f);
    }

    [TestMethod()]
    public void TestGetValueEmptyZERO()
    {
        Spreadsheet s = new Spreadsheet();
        Assert.AreEqual("", s.GetCellValue("A3"));
    }

    [TestMethod()]
    public void TestGetValueDoubleZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A3", "5");
        Assert.AreEqual(5.0, s.GetCellValue("A3"));
    }

    [TestMethod()]
    public void TestGetValueStringZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A3", "hello");
        Assert.AreEqual("hello", s.GetCellValue("A3"));
    }

    [TestMethod()]
    public void TestGetValueFormulaNoOprNoVarZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A3", "=5");
        Assert.AreEqual(5.0, s.GetCellValue("A3"));
    }

    [TestMethod()]
    public void TestGetValueFormulaNoOprYesVarZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A3", "=5");
        s.SetContentsOfCell("A2", "=A3");
        Assert.AreEqual(5.0, s.GetCellValue("A2"));
    }

    [TestMethod()]
    public void TestGetValueFormulaYesOprYesVarZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A3", "=5");
        s.SetContentsOfCell("A2", "=5+A3");
        Assert.AreEqual(10.0, s.GetCellValue("A2"));
    }

    [TestMethod()]
    public void TestGetValueFormulaYesOprNoVarZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A2", "=5*9");
        Assert.AreEqual(45.0, s.GetCellValue("A2"));
    }

    // CIRCULAR FORMULA DETECTION - ZERO PARAMTER CONSTRUCTOR
    [TestMethod()]
    [TestCategory("15")]
    [ExpectedException(typeof(CircularException))]
    public void TestSimpleCircularZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "=A2");
        s.SetContentsOfCell("A2", "=A1");
    }

    [TestMethod()]
    [TestCategory("16")]
    [ExpectedException(typeof(CircularException))]
    public void TestComplexCircularZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "=A2+A3");
        s.SetContentsOfCell("A3", "=A4+A5");
        s.SetContentsOfCell("A5", "=A6+A7");
        s.SetContentsOfCell("A7", "=A1+A1");
    }

    [TestMethod()]
    [TestCategory("17")]
    [ExpectedException(typeof(CircularException))]
    public void TestUndoCircularZERO()
    {
        Spreadsheet s = new Spreadsheet();
        try
        {
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A2", "15");
            s.SetContentsOfCell("A3", "30");
            s.SetContentsOfCell("A2", "=A3*A1");
        }
        catch (CircularException e)
        {
            Assert.AreEqual(15, (double)s.GetCellContents("A2"));
            throw e;
        }
    }

    [TestMethod()]
    [TestCategory("17b")]
    [ExpectedException(typeof(CircularException))]
    public void TestUndoCellsCircularZERO()
    {
        Spreadsheet s = new Spreadsheet();
        try
        {
            s.SetContentsOfCell("A1", "=A2");
            s.SetContentsOfCell("A2", "=A1");
        }
        catch (CircularException e)
        {
            Assert.AreEqual("", s.GetCellContents("A2"));
            Assert.IsTrue(new HashSet<string> { "A1" }.SetEquals(s.GetNamesOfAllNonemptyCells()));
            throw e;
        }
    }

    // NONEMPTY CELLS - ZERO PARAMETER CONSTRUCTOR
    [TestMethod(), Timeout(2000)]
    [TestCategory("18")]
    public void TestEmptyNamesZERO()
    {
        Spreadsheet s = new Spreadsheet();
        Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
    }

    [TestMethod()]
    [TestCategory("19")]
    public void TestExplicitEmptySetZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("B1", "");
        Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
    }

    [TestMethod()]
    [TestCategory("20")]
    public void TestSimpleNamesStringZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("B1", "hello");
        Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
    }

    [TestMethod()]
    [TestCategory("21")]
    public void TestSimpleNamesDoubleZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("B1", "52.25");
        Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
    }

    [TestMethod()]
    [TestCategory("22")]
    public void TestSimpleNamesFormulaZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("B1", "=3.5");
        Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
    }

    [TestMethod()]
    [TestCategory("23")]
    public void TestMixedNamesZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "17.2");
        s.SetContentsOfCell("C1", "hello");
        s.SetContentsOfCell("B1", "=3.5");
        Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "A1", "B1", "C1" }));
    }

    // RETURN VALUE OF SET CELL CONTENTS
    [TestMethod()]
    [TestCategory("24")]
    public void TestSetSingletonDoubleZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("B1", "hello");
        s.SetContentsOfCell("C1", "=5");
        Assert.IsTrue(s.SetContentsOfCell("A1", "17.2").SequenceEqual(new List<string>() { "A1" }));
    }

    [TestMethod()]
    [TestCategory("25")]
    public void TestSetSingletonStringZEROZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "17.2");
        s.SetContentsOfCell("C1", "=5");
        Assert.IsTrue(s.SetContentsOfCell("B1", "hello").SequenceEqual(new List<string>() { "B1" }));
    }

    [TestMethod()]
    [TestCategory("26")]
    public void TestSetSingletonFormulaZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "17.2");
        s.SetContentsOfCell("B1", "hello");
        Assert.IsTrue(s.SetContentsOfCell("C1", "=5").SequenceEqual(new List<string>() { "C1" }));
    }

    [TestMethod()]
    [TestCategory("27")]
    public void TestSetChainZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "=A2+A3");
        s.SetContentsOfCell("A2", "6");
        s.SetContentsOfCell("A3", "=A2+A4");
        s.SetContentsOfCell("A4", "=A2+A5");
        Assert.IsTrue(s.SetContentsOfCell("A5", "82.5").SequenceEqual(new List<string>() { "A5", "A4", "A3", "A1" }));
    }

    // CHANGING CELLS
    [TestMethod()]
    [TestCategory("28")]
    public void TestChangeFtoDZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "=A2+A3");
        s.SetContentsOfCell("A1", "2.5");
        Assert.AreEqual(2.5, (double)s.GetCellContents("A1"), 1e-9);
    }

    [TestMethod()]
    [TestCategory("29")]
    public void TestChangeFtoSZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "=A2+A3");
        s.SetContentsOfCell("A1", "Hello");
        Assert.AreEqual("Hello", (string)s.GetCellContents("A1"));

    }

    [TestMethod()]
    [TestCategory("30")]
    public void TestChangeStoFZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "Hello");
        s.SetContentsOfCell("A1", "=23");
        Assert.AreEqual(new Formula("23"), (Formula)s.GetCellContents("A1"));
        Assert.AreNotEqual(new Formula("24"), (Formula)s.GetCellContents("A1"));

    }

    //[TestMethod()]
    //public void TestSaveSingleStringZERO()
    //{
    //    Spreadsheet s1 = new Spreadsheet();
    //    s1.SetContentsOfCell("A1", "Hello");

    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "default");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");

    //    Assert.AreEqual("Hello", (string)anotherOne.GetCellContents("A1"));
    //    CollectionAssert.AreEqual(expected, actual);
        
    //}

    //[TestMethod()]
    //public void TestSaveSingleDoubleZERO()
    //{
    //    Spreadsheet s1 = new Spreadsheet();
    //    s1.SetContentsOfCell("A1", "2");

    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "default");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");

    //    Assert.AreEqual(2.0, (double)anotherOne.GetCellContents("A1"));
    //    CollectionAssert.AreEqual(expected, actual);

    //}

    //[TestMethod()]
    //public void TestSaveSingleFormulaNoVarNoOprZERO()
    //{
    //    Spreadsheet s1 = new Spreadsheet();
    //    s1.SetContentsOfCell("A1", "=3");

    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "default");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");

    //    Assert.AreEqual(new Formula("3"), (Formula)anotherOne.GetCellContents("A1"));
    //    Assert.AreEqual(3.0, anotherOne.GetCellValue("A1"));

    //    CollectionAssert.AreEqual(expected, actual);

    //}

    //[TestMethod()]
    //public void TestSaveSingleFormulaNoVarYesOprZERO()
    //{
    //    Spreadsheet s1 = new Spreadsheet();
    //    s1.SetContentsOfCell("A1", "=3+3");

    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "default");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");

    //    Assert.AreEqual(new Formula("3+3"), (Formula)anotherOne.GetCellContents("A1"));
    //    Assert.AreEqual(6.0, anotherOne.GetCellValue("A1"));

    //    CollectionAssert.AreEqual(expected, actual);

    //}

    //[TestMethod()]
    //public void TestSaveSingleFormulaYesVarNoOprZERO()
    //{
    //    Spreadsheet s1 = new Spreadsheet();
    //    s1.SetContentsOfCell("A1", "3");
    //    s1.SetContentsOfCell("A2", "=A1");


    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "default");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");
    //    expected.Add("A2");


    //    Assert.AreEqual(new Formula("A1"), (Formula)anotherOne.GetCellContents("A2"));
    //    Assert.AreEqual(3.0, anotherOne.GetCellValue("A2"));

    //    CollectionAssert.AreEqual(expected, actual);

    //}

    //[TestMethod()]
    //public void TestSaveSingleFormulaYesVarYesOprZERO()
    //{
    //    Spreadsheet s1 = new Spreadsheet();
    //    s1.SetContentsOfCell("A1", "3");
    //    s1.SetContentsOfCell("A2", "=A1+3");


    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "default");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");
    //    expected.Add("A2");


    //    Assert.AreEqual(new Formula("A1+3"), (Formula)anotherOne.GetCellContents("A2"));
    //    Assert.AreEqual(6.0, anotherOne.GetCellValue("A2"));

    //    CollectionAssert.AreEqual(expected, actual);

    //}

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestSaveDeserializeVersionZERO()
    {
        Spreadsheet s1 = new Spreadsheet();
        s1.SetContentsOfCell("A1", "Hello");

        s1.Save("test1.txt");

        Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
            s => true, s => s.ToUpper(), "v2");

    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestSaveInvalidNameZERO()
    {
        try
        {
            Spreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("1A", "Hello");
            s1.Save("test1.txt");
        }
        catch (InvalidNameException)
        {

        }

        Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
            s => true, s => s.ToUpper(), "default");
    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestSaveInvalidFormulaZERO()
    {
        try
        {
            Spreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "=3++");
            s1.Save("test1.txt");
        }
        catch (FormulaFormatException)
        {

        }

        Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
            s => true, s => s.ToUpper(), "default");
    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestSaveCircularDependenctZERO()
    {
        try
        {
            Spreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", "=A2");
            s1.SetContentsOfCell("A2", "=A1");

            s1.Save("test1.txt");
        }
        catch (CircularException)
        {

        }

        Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
            s => true, s => s.ToUpper(), "default");
    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestDeserializePathDNEZERO()
    {
        Spreadsheet s1 = new Spreadsheet();
        s1.SetContentsOfCell("A1", "Hello");

        s1.Save("test1.txt");

        Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
            s => true, s => s.ToUpper(), "default");

    }

    // STRESS TESTS
    [TestMethod()]
    [TestCategory("31")]
    public void TestStress1ZERO()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "=B1+B2");
        s.SetContentsOfCell("B1", "=C1-C2");
        s.SetContentsOfCell("B2", "=C3*C4");
        s.SetContentsOfCell("C1", "=D1*D2");
        s.SetContentsOfCell("C2", "=D3*D4");
        s.SetContentsOfCell("C3", "=D5*D6");
        s.SetContentsOfCell("C4", "=D7*D8");
        s.SetContentsOfCell("D1", "=E1");
        s.SetContentsOfCell("D2", "=E1");
        s.SetContentsOfCell("D3", "=E1");
        s.SetContentsOfCell("D4", "=E1");
        s.SetContentsOfCell("D5", "=E1");
        s.SetContentsOfCell("D6", "=E1");
        s.SetContentsOfCell("D7", "=E1");
        s.SetContentsOfCell("D8", "=E1");
        IList<String> cells = s.SetContentsOfCell("E1", "0");
        Assert.IsTrue(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }.SetEquals(cells));
    }

    // Repeated for extra weight
    [TestMethod()]
    [TestCategory("32")]
    public void TestStress1aZERO()
    {
        TestStress1ZERO();
    }
    [TestMethod()]
    [TestCategory("33")]
    public void TestStress1bZERO()
    {
        TestStress1ZERO();
    }
    [TestMethod()]
    [TestCategory("34")]
    public void TestStress1cZERO()
    {
        TestStress1ZERO();
    }

    [TestMethod()]
    [TestCategory("35")]
    public void TestStress2ZERO()
    {
        Spreadsheet s = new Spreadsheet();
        ISet<String> cells = new HashSet<string>();
        for (int i = 1; i < 200; i++)
        {
            cells.Add("A" + i);
            Assert.IsTrue(cells.SetEquals(s.SetContentsOfCell("A" + i, "=A" + (i + 1))));
        }
    }
    [TestMethod()]
    [TestCategory("36")]
    public void TestStress2aZERO()
    {
        TestStress2ZERO();
    }
    [TestMethod()]
    [TestCategory("37")]
    public void TestStress2bZERO()
    {
        TestStress2ZERO();
    }
    [TestMethod()]
    [TestCategory("38")]
    public void TestStress2cZERO()
    {
        TestStress2ZERO();
    }

    [TestMethod()]
    [TestCategory("39")]
    public void TestStress3ZERO()
    {
        Spreadsheet s = new Spreadsheet();
        for (int i = 1; i < 200; i++)
        {
            s.SetContentsOfCell("A" + i, "=A" + (i + 1));
        }
        try
        {
            s.SetContentsOfCell("A150", "=A50");
            Assert.Fail();
        }
        catch (CircularException)
        {
        }
    }

    [TestMethod()]
    [TestCategory("40")]
    public void TestStress3aZERO()
    {
        TestStress3ZERO();
    }
    [TestMethod()]
    [TestCategory("41")]
    public void TestStress3bZERO()
    {
        TestStress3ZERO();
    }
    [TestMethod()]
    [TestCategory("42")]
    public void TestStress3cZERO()
    {
        TestStress3ZERO();
    }

    [TestMethod()]
    [TestCategory("43")]
    public void TestStress4ZERO()
    {
        Spreadsheet s = new Spreadsheet();
        for (int i = 0; i < 500; i++)
        {
            s.SetContentsOfCell("A1" + i, "=A1" + (i + 1));
        }
        LinkedList<string> firstCells = new LinkedList<string>();
        LinkedList<string> lastCells = new LinkedList<string>();
        for (int i = 0; i < 250; i++)
        {
            firstCells.AddFirst("A1" + i);
            lastCells.AddFirst("A1" + (i + 250));
        }
        Assert.IsTrue(s.SetContentsOfCell("A1249", "25.0").SequenceEqual(firstCells));
        Assert.IsTrue(s.SetContentsOfCell("A1499", "0").SequenceEqual(lastCells));
    }

    // EMPTY SPREADSHEETS - THREE PARAMETER CONSTRUCTOR////////////////////////////
    [TestMethod()]
    [TestCategory("2")]
    [ExpectedException(typeof(InvalidNameException))]
    public void TestEmptyGetContentsTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.GetCellContents("1AA");
    }

    [TestMethod()]
    [TestCategory("3")]
    public void TestGetEmptyContentsTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        Assert.AreEqual("", s.GetCellContents("A2"));
    }

    [TestMethod()]
    [TestCategory("3")]
    public void TestGetEmptyContentsNormalizerCheckTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A2", "hello");
        Assert.AreEqual("hello", s.GetCellContents("a2"));
    }

    // SETTING CELL TO A DOUBLE - ZERO PARAMETER CONSTRUCTOR
    [TestMethod()]
    [TestCategory("5")]
    [ExpectedException(typeof(InvalidNameException))]
    public void TestSetInvalidNameDoubleTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("1A1A", "1.5");
    }

    [TestMethod()]
    [TestCategory("6")]
    public void TestSimpleSetDoubleTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("Z7", "1.5");
        Assert.AreEqual(1.5, 1e-9, (double)s.GetCellContents("Z7"));
    }

    // SETTING CELL TO A STRING - THREE PARAMETER CONSTRUCTOR
    [TestMethod()]
    [TestCategory("9")]
    [ExpectedException(typeof(InvalidNameException))]
    public void TestSetSimpleStringTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("1AZ", "hello");
    }

    [TestMethod()]
    [TestCategory("10")]
    public void TestSetGetSimpleStringTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("Z7", "hello");
        Assert.AreEqual("hello", s.GetCellContents("Z7"));
    }

    // SETTING CELL TO A FORMULA
    [TestMethod()]
    [TestCategory("13")]
    [ExpectedException(typeof(InvalidNameException))]
    public void TestSetSimpleFormTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("1AZ", "=2");
    }

    [TestMethod()]
    [TestCategory("14")]
    public void TestSetGetFormTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("Z7", "=3");
        Formula f = (Formula)s.GetCellContents("Z7");
        Assert.AreEqual(new Formula("3"), f);
        Assert.AreNotEqual(new Formula("2"), f);
    }

    [TestMethod()]
    public void TestGetValueEmptyTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        Assert.AreEqual("", s.GetCellValue("A3"));
    }

    [TestMethod()]
    public void TestGetValueDoubleTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A3", "5");
        Assert.AreEqual(5.0, s.GetCellValue("A3"));
    }

    [TestMethod()]
    public void TestGetValueStringTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A3", "hello");
        Assert.AreEqual("hello", s.GetCellValue("A3"));
    }

    [TestMethod()]
    public void TestGetValueFormulaNoOprNoVarTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A3", "=5");
        Assert.AreEqual(5.0, s.GetCellValue("A3"));
    }

    [TestMethod()]
    public void TestGetValueFormulaNoOprYesVarTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A3", "=5");
        s.SetContentsOfCell("A2", "=A3");
        Assert.AreEqual(5.0, s.GetCellValue("A2"));
    }

    [TestMethod()]
    public void TestGetValueFormulaYesOprYesVarTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A3", "=5");
        s.SetContentsOfCell("A2", "=5+A3");
        Assert.AreEqual(10.0, s.GetCellValue("A2"));
    }

    [TestMethod()]
    public void TestGetValueFormulaYesOprNoVarTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A2", "=5*9");
        Assert.AreEqual(45.0, s.GetCellValue("A2"));
    }

    // CIRCULAR FORMULA DETECTION - THREE PARAMTER CONSTRUCTOR
    [TestMethod()]
    [TestCategory("15")]
    [ExpectedException(typeof(CircularException))]
    public void TestSimpleCircularTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A1", "=A2");
        s.SetContentsOfCell("A2", "=A1");
    }

    [TestMethod()]
    [TestCategory("16")]
    [ExpectedException(typeof(CircularException))]
    public void TestComplexCircularTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A1", "=A2+A3");
        s.SetContentsOfCell("A3", "=A4+A5");
        s.SetContentsOfCell("A5", "=A6+A7");
        s.SetContentsOfCell("A7", "=A1+A1");
    }

    [TestMethod()]
    [TestCategory("17")]
    [ExpectedException(typeof(CircularException))]
    public void TestUndoCircularTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        try
        {
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A2", "15");
            s.SetContentsOfCell("A3", "30");
            s.SetContentsOfCell("A2", "=A3*A1");
        }
        catch (CircularException e)
        {
            Assert.AreEqual(15, (double)s.GetCellContents("A2"));
            throw e;
        }
    }

    [TestMethod()]
    [TestCategory("17b")]
    [ExpectedException(typeof(CircularException))]
    public void TestUndoCellsCircularTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        try
        {
            s.SetContentsOfCell("A1", "=A2");
            s.SetContentsOfCell("A2", "=A1");
        }
        catch (CircularException e)
        {
            Assert.AreEqual("", s.GetCellContents("A2"));
            Assert.IsTrue(new HashSet<string> { "A1" }.SetEquals(s.GetNamesOfAllNonemptyCells()));
            throw e;
        }
    }


    // NONEMPTY CELLS - THREE PARAMETER CONSTRUCTOR
    [TestMethod(), Timeout(2000)]
    [TestCategory("18")]
    public void TestEmptyNamesTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
    }

    [TestMethod()]
    [TestCategory("19")]
    public void TestExplicitEmptySetTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("B1", "");
        Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
    }

    [TestMethod()]
    [TestCategory("20")]
    public void TestSimpleNamesStringTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("B1", "hello");
        Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
    }

    [TestMethod()]
    [TestCategory("21")]
    public void TestSimpleNamesDoubleTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("B1", "52.25");
        Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
    }

    [TestMethod()]
    [TestCategory("22")]
    public void TestSimpleNamesFormulaTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("B1", "=3.5");
        Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
    }

    [TestMethod()]
    [TestCategory("23")]
    public void TestMixedNamesTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A1", "17.2");
        s.SetContentsOfCell("C1", "hello");
        s.SetContentsOfCell("B1", "=3.5");
        Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "A1", "B1", "C1" }));
    }

    // RETURN VALUE OF SET CELL CONTENTS
    [TestMethod()]
    [TestCategory("24")]
    public void TestSetSingletonDoubleTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("B1", "hello");
        s.SetContentsOfCell("C1", "=5");
        Assert.IsTrue(s.SetContentsOfCell("A1", "17.2").SequenceEqual(new List<string>() { "A1" }));
    }

    [TestMethod()]
    [TestCategory("25")]
    public void TestSetSingletonStringTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A1", "17.2");
        s.SetContentsOfCell("C1", "=5");
        Assert.IsTrue(s.SetContentsOfCell("B1", "hello").SequenceEqual(new List<string>() { "B1" }));
    }

    [TestMethod()]
    [TestCategory("26")]
    public void TestSetSingletonFormulaTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A1", "17.2");
        s.SetContentsOfCell("B1", "hello");
        Assert.IsTrue(s.SetContentsOfCell("C1", "=5").SequenceEqual(new List<string>() { "C1" }));
    }

    [TestMethod()]
    [TestCategory("27")]
    public void TestSetChainTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A1", "=A2+A3");
        s.SetContentsOfCell("A2", "6");
        s.SetContentsOfCell("A3", "=A2+A4");
        s.SetContentsOfCell("A4", "=A2+A5");
        Assert.IsTrue(s.SetContentsOfCell("A5", "82.5").SequenceEqual(new List<string>() { "A5", "A4", "A3", "A1" }));
    }

    // CHANGING CELLS
    [TestMethod()]
    [TestCategory("28")]
    public void TestChangeFtoDTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A1", "=A2+A3");
        s.SetContentsOfCell("A1", "2.5");
        Assert.AreEqual(2.5, (double)s.GetCellContents("A1"), 1e-9);
    }

    [TestMethod()]
    [TestCategory("29")]
    public void TestChangeFtoSTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A1", "=A2+A3");
        s.SetContentsOfCell("A1", "Hello");
        Assert.AreEqual("Hello", (string)s.GetCellContents("A1"));
    }

    [TestMethod()]
    [TestCategory("30")]
    public void TestChangeStoFTHREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A1", "Hello");
        s.SetContentsOfCell("A1", "=23");
        Assert.AreEqual(new Formula("23"), (Formula)s.GetCellContents("A1"));
        Assert.AreNotEqual(new Formula("24"), (Formula)s.GetCellContents("A1"));
    }

    //[TestMethod()]
    //public void TestSaveSingleStringTHREE()
    //{
    //    Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
    //    s1.SetContentsOfCell("A1", "Hello");

    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "v1");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");

    //    Assert.AreEqual("Hello", (string)anotherOne.GetCellContents("A1"));
    //    CollectionAssert.AreEqual(expected, actual);

    //}

    //[TestMethod()]
    //public void TestSaveSingleDoubleTHREE()
    //{
    //    Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
    //    s1.SetContentsOfCell("A1", "2");

    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "v1");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");

    //    Assert.AreEqual(2.0, (double)anotherOne.GetCellContents("A1"));
    //    CollectionAssert.AreEqual(expected, actual);

    //}

    //[TestMethod()]
    //public void TestSaveSingleFormulaNoVarNoOprTHREE()
    //{
    //    Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
    //    s1.SetContentsOfCell("A1", "=3");

    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "v1");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");

    //    Assert.AreEqual(new Formula("3"), (Formula)anotherOne.GetCellContents("A1"));
    //    Assert.AreEqual(3.0, anotherOne.GetCellValue("A1"));

    //    CollectionAssert.AreEqual(expected, actual);

    //}

    //[TestMethod()]
    //public void TestSaveSingleFormulaNoVarYesOprTHREE()
    //{
    //    Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
    //    s1.SetContentsOfCell("A1", "=3+3");

    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "v1");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");

    //    Assert.AreEqual(new Formula("3+3"), (Formula)anotherOne.GetCellContents("A1"));
    //    Assert.AreEqual(6.0, anotherOne.GetCellValue("A1"));

    //    CollectionAssert.AreEqual(expected, actual);

    //}

    //[TestMethod()]
    //public void TestSaveSingleFormulaYesVarNoOprTHREE()
    //{
    //    Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
    //    s1.SetContentsOfCell("A1", "3");
    //    s1.SetContentsOfCell("A2", "=A1");


    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "v1");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");
    //    expected.Add("A2");


    //    Assert.AreEqual(new Formula("A1"), (Formula)anotherOne.GetCellContents("A2"));
    //    Assert.AreEqual(3.0, anotherOne.GetCellValue("A2"));

    //    CollectionAssert.AreEqual(expected, actual);

    //}

    //[TestMethod()]
    //public void TestSaveSingleFormulaYesVarYesOprTHREE()
    //{
    //    Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
    //    s1.SetContentsOfCell("A1", "3");
    //    s1.SetContentsOfCell("A2", "=A1+3");


    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "v1");

    //    List<string> expected = new List<string>();
    //    List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
    //    expected.Add("A1");
    //    expected.Add("A2");


    //    Assert.AreEqual(new Formula("A1+3"), (Formula)anotherOne.GetCellContents("A2"));
    //    Assert.AreEqual(6.0, anotherOne.GetCellValue("A2"));

    //    CollectionAssert.AreEqual(expected, actual);

    //}

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestDeserializeVersionTHREE()
    {
        Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s1.SetContentsOfCell("A1", "Hello");

        s1.Save("test1.txt");

        Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
            s => true, s => s.ToUpper(), "v2");

    }


    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestSaveInvalidNameTHREE()
    {
        try
        {
            Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
            s1.SetContentsOfCell("1A", "Hello");
            s1.Save("test1.txt");
        }
        catch (InvalidNameException)
        {

        }

        Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
            s => true, s => s.ToUpper(), "v1");
    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestSaveInvalidFormulaTHREE()
    {
        try
        {
            Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
            s1.SetContentsOfCell("A1", "=3++");
            s1.Save("test1.txt");
        }
        catch (FormulaFormatException)
        {

        }

        Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
            s => true, s => s.ToUpper(), "v1");
    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestSaveCircularDependenctTHREE()
    {
        try
        {
            Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
            s1.SetContentsOfCell("A1", "=A2");
            s1.SetContentsOfCell("A2", "=A1");

            s1.Save("test1.txt");
        }
        catch (CircularException)
        {

        }

        Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
            s => true, s => s.ToUpper(), "v1");
    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestDeserializePathDNETHREE()
    {
        Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s1.SetContentsOfCell("A1", "Hello");

        s1.Save("test1.txt");

        Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
            s => true, s => s.ToUpper(), "v1");

    }

    // STRESS TESTS
    [TestMethod()]
    [TestCategory("31")]
    public void TestStress1THREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        s.SetContentsOfCell("A1", "=B1+B2");
        s.SetContentsOfCell("B1", "=C1-C2");
        s.SetContentsOfCell("B2", "=C3*C4");
        s.SetContentsOfCell("C1", "=D1*D2");
        s.SetContentsOfCell("C2", "=D3*D4");
        s.SetContentsOfCell("C3", "=D5*D6");
        s.SetContentsOfCell("C4", "=D7*D8");
        s.SetContentsOfCell("D1", "=E1");
        s.SetContentsOfCell("D2", "=E1");
        s.SetContentsOfCell("D3", "=E1");
        s.SetContentsOfCell("D4", "=E1");
        s.SetContentsOfCell("D5", "=E1");
        s.SetContentsOfCell("D6", "=E1");
        s.SetContentsOfCell("D7", "=E1");
        s.SetContentsOfCell("D8", "=E1");
        IList<String> cells = s.SetContentsOfCell("E1", "0");
        Assert.IsTrue(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }.SetEquals(cells));
    }

    // Repeated for extra weight
    [TestMethod()]
    [TestCategory("32")]
    public void TestStress1aTHREE()
    {
        TestStress1THREE();
    }
    [TestMethod()]
    [TestCategory("33")]
    public void TestStress1bTHREE()
    {
        TestStress1THREE();
    }
    [TestMethod()]
    [TestCategory("34")]
    public void TestStress1cTHREE()
    {
        TestStress1THREE();
    }

    [TestMethod()]
    [TestCategory("35")]
    public void TestStress2THREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        ISet<String> cells = new HashSet<string>();
        for (int i = 1; i < 200; i++)
        {
            cells.Add("A" + i);
            Assert.IsTrue(cells.SetEquals(s.SetContentsOfCell("A" + i, "=A" + (i + 1))));
        }
    }
    [TestMethod()]
    [TestCategory("36")]
    public void TestStress2aTHREE()
    {
        TestStress2THREE();
    }
    [TestMethod()]
    [TestCategory("37")]
    public void TestStress2bTHREE()
    {
        TestStress2THREE();
    }
    [TestMethod()]
    [TestCategory("38")]
    public void TestStress2cTHREE()
    {
        TestStress2THREE();
    }

    [TestMethod()]
    [TestCategory("39")]
    public void TestStress3THREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        for (int i = 1; i < 200; i++)
        {
            s.SetContentsOfCell("A" + i, "=A" + (i + 1));
        }
        try
        {
            s.SetContentsOfCell("A150", "=A50");
            Assert.Fail();
        }
        catch (CircularException)
        {
        }
    }

    [TestMethod()]
    [TestCategory("40")]
    public void TestStress3aTHREE()
    {
        TestStress3THREE();
    }
    [TestMethod()]
    [TestCategory("41")]
    public void TestStress3bTHREE()
    {
        TestStress3THREE();
    }
    [TestMethod()]
    [TestCategory("42")]
    public void TestStress3cTHREE()
    {
        TestStress3THREE();
    }

    [TestMethod()]
    [TestCategory("43")]
    public void TestStress4THREE()
    {
        Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
        for (int i = 0; i < 1000; i++)
        {
            s.SetContentsOfCell("A1" + i, "=A1" + (i + 1));
        }
        LinkedList<string> firstCells = new LinkedList<string>();
        LinkedList<string> lastCells = new LinkedList<string>();
        for (int i = 0; i < 250; i++)
        {
            firstCells.AddFirst("A1" + i);
            lastCells.AddFirst("A1" + (i + 250));
        }
        Assert.IsTrue(s.SetContentsOfCell("A1249", "25.0").SequenceEqual(firstCells));
        Assert.IsTrue(s.SetContentsOfCell("A1499", "0").SequenceEqual(lastCells));
    }    
}
