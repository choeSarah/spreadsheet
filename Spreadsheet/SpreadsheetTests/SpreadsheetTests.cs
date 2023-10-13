using System.Text.Json;
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

    [TestMethod()]
    public void TestSaveSingleStringZERO()
    {
        string sheet = "{\"Cells\":{\"A1\":{\"StringForm\":\"Hello\"}},\"Version\":\"default\"}";
        File.WriteAllText("sstest.txt", sheet);

        //Spreadsheet s1 = new Spreadsheet();
        //s1.SetContentsOfCell("A1", "Hello");

        //s1.Save("test1.txt");

        Spreadsheet anotherOne = new Spreadsheet("sstest.txt",
            s => true, s => s.ToUpper(), "default");

        List<string> expected = new List<string>();
        List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
        expected.Add("A1");

        Assert.AreEqual("Hello", (string)anotherOne.GetCellContents("A1"));
        CollectionAssert.AreEqual(expected, actual);

    }

    [TestMethod()]
    public void TestSaveSingleDoubleZERO()
    {
        //Spreadsheet s1 = new Spreadsheet();
        //s1.SetContentsOfCell("A1", "2");

        //s1.Save("test1.txt");

        string sheet = "{\"Cells\":{\"A1\":{\"StringForm\":\"2\"}},\"Version\":\"default\"}";
        File.WriteAllText("sstest.txt", sheet);

        Spreadsheet anotherOne = new Spreadsheet("sstest.txt",
            s => true, s => s.ToUpper(), "default");

        List<string> expected = new List<string>();
        List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
        expected.Add("A1");

        Assert.AreEqual(2.0, (double)anotherOne.GetCellContents("A1"));
        CollectionAssert.AreEqual(expected, actual);

    }

    [TestMethod()]
    public void TestSaveSingleFormulaNoVarNoOprZERO()
    {
        //Spreadsheet s1 = new Spreadsheet();
        //s1.SetContentsOfCell("A1", "=3");

        //s1.Save("test1.txt");

        string sheet = "{\"Cells\":{\"A1\":{\"StringForm\":\"=3\"}},\"Version\":\"default\"}";
        File.WriteAllText("sstest.txt", sheet);

        Spreadsheet anotherOne = new Spreadsheet("sstest.txt",
            s => true, s => s.ToUpper(), "default");

        List<string> expected = new List<string>();
        List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
        expected.Add("A1");

        Assert.AreEqual(new Formula("3"), (Formula)anotherOne.GetCellContents("A1"));
        Assert.AreEqual(3.0, anotherOne.GetCellValue("A1"));

        CollectionAssert.AreEqual(expected, actual);

    }

    [TestMethod()]
    public void TestSaveSingleFormulaNoVarYesOprZERO()
    {
        //Spreadsheet s1 = new Spreadsheet();
        //s1.SetContentsOfCell("A1", "=3+3");

        //s1.Save("test1.txt");

        string sheet = "{\"Cells\":{\"A1\":{\"StringForm\":\"=3+3\"}},\"Version\":\"default\"}";
        File.WriteAllText("sstest.txt", sheet);

        Spreadsheet anotherOne = new Spreadsheet("sstest.txt",
            s => true, s => s.ToUpper(), "default");

        List<string> expected = new List<string>();
        List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
        expected.Add("A1");

        Assert.AreEqual(new Formula("3+3"), (Formula)anotherOne.GetCellContents("A1"));
        Assert.AreEqual(6.0, anotherOne.GetCellValue("A1"));

        CollectionAssert.AreEqual(expected, actual);

    }

    [TestMethod()]
    public void TestSaveSingleFormulaYesVarNoOprZERO()
    {
        //Spreadsheet s1 = new Spreadsheet();
        //s1.SetContentsOfCell("A1", "3");
        //s1.SetContentsOfCell("A2", "=A1");

        //s1.Save("test1.txt");

        string sheet = "{\"Cells\":{\"A1\":{\"StringForm\":\"3\"},\"A2\":{\"StringForm\":\"=A1\"}},\"Version\":\"default\"}";
        File.WriteAllText("sstest.txt", sheet);

        Spreadsheet anotherOne = new Spreadsheet("sstest.txt",
            s => true, s => s.ToUpper(), "default");

        List<string> expected = new List<string>();
        List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
        expected.Add("A1");
        expected.Add("A2");


        Assert.AreEqual(new Formula("A1"), (Formula)anotherOne.GetCellContents("A2"));
        Assert.AreEqual(3.0, anotherOne.GetCellValue("A2"));

        CollectionAssert.AreEqual(expected, actual);

    }

    [TestMethod()]
    public void TestSaveSingleFormulaYesVarYesOprZERO()
    {
        //Spreadsheet s1 = new Spreadsheet();
        //s1.SetContentsOfCell("A1", "3");
        //s1.SetContentsOfCell("A2", "=A1+3");

        //s1.Save("test1.txt");

        string sheet = "{\"Cells\":{\"A1\":{\"StringForm\":\"3\"},\"A2\":{\"StringForm\":\"=A1+3\"}},\"Version\":\"default\"}";
        File.WriteAllText("sstest.txt", sheet);

        Spreadsheet anotherOne = new Spreadsheet("sstest.txt",
            s => true, s => s.ToUpper(), "default");

        List<string> expected = new List<string>();
        List<string> actual = anotherOne.GetNamesOfAllNonemptyCells().ToList();
        expected.Add("A1");
        expected.Add("A2");


        Assert.AreEqual(new Formula("A1+3"), (Formula)anotherOne.GetCellContents("A2"));
        Assert.AreEqual(6.0, anotherOne.GetCellValue("A2"));

        CollectionAssert.AreEqual(expected, actual);

    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestSaveDeserializeVersionZERO()
    {
        //Spreadsheet s1 = new Spreadsheet();
        //s1.SetContentsOfCell("A1", "Hello");

        //s1.Save("test1.txt");

        string sheet = "{\"Cells\":{\"A1\":{\"StringForm\":\"3\"},\"A2\":{\"StringForm\":\"=A1+3\"}},\"Version\":\"default\"}";
        File.WriteAllText("sstest.txt", sheet);

        Spreadsheet anotherOne = new Spreadsheet("sstest.txt",
            s => true, s => s.ToUpper(), "v2");

    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestSaveInvalidNameZERO()
    {

        string sheet = "{\"Cells\":{\"1A\":{\"StringForm\":\"3\"},\"A2\":{\"StringForm\":\"=A1+3\"}},\"Version\":\"default\"}";
        File.WriteAllText("sstest.txt", sheet);

        Spreadsheet anotherOne = new Spreadsheet("sstest.txt",
            s => true, s => s.ToUpper(), "default");
    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestSaveInvalidFormulaZERO()
    {
        string sheet = "{\"Cells\":{\"A1\":{\"StringForm\":\"=3++\"},\"A2\":{\"StringForm\":\"=A1+3\"}},\"Version\":\"default\"}";
        File.WriteAllText("sstest.txt", sheet);

        Spreadsheet anotherOne = new Spreadsheet("sstest.txt",
            s => true, s => s.ToUpper(), "default");
    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestSaveCircularDependenctZERO()
    {
        string sheet = "{\"Cells\":{\"A1\":{\"StringForm\":\"=A2\"},\"A2\":{\"StringForm\":\"=A1+3\"}},\"Version\":\"default\"}";
        File.WriteAllText("sstest.txt", sheet);

        Spreadsheet anotherOne = new Spreadsheet("sstest.txt",
            s => true, s => s.ToUpper(), "default");
    }

    [TestMethod()]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void TestDeserializePathDNEZERO()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        ss.Save("/some/nonsense/path.txt");

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

    //[TestMethod()]
    //[ExpectedException(typeof(SpreadsheetReadWriteException))]
    //public void TestDeserializeVersionTHREE()
    //{
    //    Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
    //    s1.SetContentsOfCell("A1", "Hello");

    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/SpreadsheetTests/bin/Debug/net7.0/test1.txt",
    //        s => true, s => s.ToUpper(), "v2");

    //}


    //[TestMethod()]
    //[ExpectedException(typeof(SpreadsheetReadWriteException))]
    //public void TestSaveInvalidNameTHREE()
    //{
    //    try
    //    {
    //        Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
    //        s1.SetContentsOfCell("1A", "Hello");
    //        s1.Save("test1.txt");
    //    }
    //    catch (InvalidNameException)
    //    {

    //    }

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
    //        s => true, s => s.ToUpper(), "v1");
    //}

    //[TestMethod()]
    //[ExpectedException(typeof(SpreadsheetReadWriteException))]
    //public void TestSaveInvalidFormulaTHREE()
    //{
    //    try
    //    {
    //        Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
    //        s1.SetContentsOfCell("A1", "=3++");
    //        s1.Save("test1.txt");
    //    }
    //    catch (FormulaFormatException)
    //    {

    //    }

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
    //        s => true, s => s.ToUpper(), "v1");
    //}

    //[TestMethod()]
    //[ExpectedException(typeof(SpreadsheetReadWriteException))]
    //public void TestSaveCircularDependenctTHREE()
    //{
    //    try
    //    {
    //        Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
    //        s1.SetContentsOfCell("A1", "=A2");
    //        s1.SetContentsOfCell("A2", "=A1");

    //        s1.Save("test1.txt");
    //    }
    //    catch (CircularException)
    //    {

    //    }

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
    //        s => true, s => s.ToUpper(), "v1");
    //}

    //[TestMethod()]
    //[ExpectedException(typeof(SpreadsheetReadWriteException))]
    //public void TestDeserializePathDNETHREE()
    //{
    //    Spreadsheet s1 = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
    //    s1.SetContentsOfCell("A1", "Hello");

    //    s1.Save("test1.txt");

    //    Spreadsheet anotherOne = new Spreadsheet("/Users/sarahchoe/Projects/spreadsheet-choeSarah/Spreadsheet/test1.txt",
    //        s => true, s => s.ToUpper(), "v1");

    //}

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



    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void VV(AbstractSpreadsheet sheet, params object[] constraints)
    {
        for (int i = 0; i < constraints.Length; i += 2)
        {
            if (constraints[i + 1] is double)
            {
                Assert.AreEqual((double)constraints[i + 1], (double)sheet.GetCellValue((string)constraints[i]), 1e-9);
            }
            else
            {
                Assert.AreEqual(constraints[i + 1], sheet.GetCellValue((string)constraints[i]));
            }
        }
    }


    // For setting a spreadsheet cell.
    public IEnumerable<string> Set(AbstractSpreadsheet sheet, string name, string contents)
    {
        List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
        return result;
    }

    // Tests IsValid
    [TestMethod, Timeout(2000)]
    [TestCategory("1")]
    public void IsValidTest1()
    {
        AbstractSpreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "x");
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("2")]
    [ExpectedException(typeof(InvalidNameException))]
    public void IsValidTest2()
    {
        AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
        ss.SetContentsOfCell("A1", "x");
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("3")]
    public void IsValidTest3()
    {
        AbstractSpreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("B1", "= A1 + C1");
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("4")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void IsValidTest4()
    {
        AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
        ss.SetContentsOfCell("B1", "= A1 + C1");
    }

    // Tests Normalize
    [TestMethod, Timeout(2000)]
    [TestCategory("5")]
    public void NormalizeTest1()
    {
        AbstractSpreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("B1", "hello");
        Assert.AreEqual("", s.GetCellContents("b1"));
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("6")]
    public void NormalizeTest2()
    {
        AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
        ss.SetContentsOfCell("B1", "hello");
        Assert.AreEqual("hello", ss.GetCellContents("b1"));
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("7")]
    public void NormalizeTest3()
    {
        AbstractSpreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("a1", "5");
        s.SetContentsOfCell("A1", "6");
        s.SetContentsOfCell("B1", "= a1");
        Assert.AreEqual(5.0, (double)s.GetCellValue("B1"), 1e-9);
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("8")]
    public void NormalizeTest4()
    {
        AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
        ss.SetContentsOfCell("a1", "5");
        ss.SetContentsOfCell("A1", "6");
        ss.SetContentsOfCell("B1", "= a1");
        Assert.AreEqual(6.0, (double)ss.GetCellValue("B1"), 1e-9);
    }

    // Simple tests
    [TestMethod, Timeout(2000)]
    [TestCategory("9")]
    public void EmptySheet()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        VV(ss, "A1", "");
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("10")]
    public void OneString()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        OneString(ss);
    }

    public void OneString(AbstractSpreadsheet ss)
    {
        Set(ss, "B1", "hello");
        VV(ss, "B1", "hello");
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("11")]
    public void OneNumber()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        OneNumber(ss);
    }

    public void OneNumber(AbstractSpreadsheet ss)
    {
        Set(ss, "C1", "17.5");
        VV(ss, "C1", 17.5);
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("12")]
    public void OneFormula()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        OneFormula(ss);
    }

    public void OneFormula(AbstractSpreadsheet ss)
    {
        Set(ss, "A1", "4.1");
        Set(ss, "B1", "5.2");
        Set(ss, "C1", "= A1+B1");
        VV(ss, "A1", 4.1, "B1", 5.2, "C1", 9.3);
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("13")]
    public void ChangedAfterModify()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        Assert.IsFalse(ss.Changed);
        Set(ss, "C1", "17.5");
        Assert.IsTrue(ss.Changed);
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("13b")]
    public void UnChangedAfterSave()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        Set(ss, "C1", "17.5");
        ss.Save("changed.txt");
        Assert.IsFalse(ss.Changed);
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("14")]
    public void DivisionByZero1()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        DivisionByZero1(ss);
    }

    public void DivisionByZero1(AbstractSpreadsheet ss)
    {
        Set(ss, "A1", "4.1");
        Set(ss, "B1", "0.0");
        Set(ss, "C1", "= A1 / B1");
        Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("15")]
    public void DivisionByZero2()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        DivisionByZero2(ss);
    }

    public void DivisionByZero2(AbstractSpreadsheet ss)
    {
        Set(ss, "A1", "5.0");
        Set(ss, "A3", "= A1 / 0.0");
        Assert.IsInstanceOfType(ss.GetCellValue("A3"), typeof(FormulaError));
    }



    [TestMethod, Timeout(2000)]
    [TestCategory("16")]
    public void EmptyArgument()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        EmptyArgument(ss);
    }

    public void EmptyArgument(AbstractSpreadsheet ss)
    {
        Set(ss, "A1", "4.1");
        Set(ss, "C1", "= A1 + B1");
        Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("17")]
    public void StringArgument()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        StringArgument(ss);
    }

    public void StringArgument(AbstractSpreadsheet ss)
    {
        Set(ss, "A1", "4.1");
        Set(ss, "B1", "hello");
        Set(ss, "C1", "= A1 + B1");
        Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("18")]
    public void ErrorArgument()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        ErrorArgument(ss);
    }

    public void ErrorArgument(AbstractSpreadsheet ss)
    {
        Set(ss, "A1", "4.1");
        Set(ss, "B1", "");
        Set(ss, "C1", "= A1 + B1");
        Set(ss, "D1", "= C1");
        Assert.IsInstanceOfType(ss.GetCellValue("D1"), typeof(FormulaError));
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("19")]
    public void NumberFormula1()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        NumberFormula1(ss);
    }

    public void NumberFormula1(AbstractSpreadsheet ss)
    {
        Set(ss, "A1", "4.1");
        Set(ss, "C1", "= A1 + 4.2");
        VV(ss, "C1", 8.3);
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("20")]
    public void NumberFormula2()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        NumberFormula2(ss);
    }

    public void NumberFormula2(AbstractSpreadsheet ss)
    {
        Set(ss, "A1", "= 4.6");
        VV(ss, "A1", 4.6);
    }


    // Repeats the simple tests all together
    [TestMethod, Timeout(2000)]
    [TestCategory("21")]
    public void RepeatSimpleTests()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        Set(ss, "A1", "17.32");
        Set(ss, "B1", "This is a test");
        Set(ss, "C1", "= A1+B1");
        OneString(ss);
        OneNumber(ss);
        OneFormula(ss);
        DivisionByZero1(ss);
        DivisionByZero2(ss);
        StringArgument(ss);
        ErrorArgument(ss);
        NumberFormula1(ss);
        NumberFormula2(ss);
    }

    // Four kinds of formulas
    [TestMethod, Timeout(2000)]
    [TestCategory("22")]
    public void Formulas()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        Formulas(ss);
    }

    public void Formulas(AbstractSpreadsheet ss)
    {
        Set(ss, "A1", "4.4");
        Set(ss, "B1", "2.2");
        Set(ss, "C1", "= A1 + B1");
        Set(ss, "D1", "= A1 - B1");
        Set(ss, "E1", "= A1 * B1");
        Set(ss, "F1", "= A1 / B1");
        VV(ss, "C1", 6.6, "D1", 2.2, "E1", 4.4 * 2.2, "F1", 2.0);
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("23")]
    public void Formulasa()
    {
        Formulas();
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("24")]
    public void Formulasb()
    {
        Formulas();
    }


    // Are multiple spreadsheets supported?
    [TestMethod, Timeout(2000)]
    [TestCategory("25")]
    public void Multiple()
    {
        AbstractSpreadsheet s1 = new Spreadsheet();
        AbstractSpreadsheet s2 = new Spreadsheet();
        Set(s1, "X1", "hello");
        Set(s2, "X1", "goodbye");
        VV(s1, "X1", "hello");
        VV(s2, "X1", "goodbye");
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("26")]
    public void Multiplea()
    {
        Multiple();
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("27")]
    public void Multipleb()
    {
        Multiple();
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("28")]
    public void Multiplec()
    {
        Multiple();
    }

    // Reading/writing spreadsheets
    [TestMethod, Timeout(2000)]
    [TestCategory("29")]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void SaveTest1()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        ss.Save(Path.GetFullPath("/missing/save.txt"));
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("30")]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void SaveTest2()
    {
        AbstractSpreadsheet ss = new Spreadsheet(Path.GetFullPath("/missing/save.txt"), s => true, s => s, "");
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("31")]
    public void SaveTest3()
    {
        AbstractSpreadsheet s1 = new Spreadsheet();
        Set(s1, "A1", "hello");
        s1.Save("save1.txt");
        s1 = new Spreadsheet("save1.txt", s => true, s => s, "default");
        Assert.AreEqual("hello", s1.GetCellContents("A1"));
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("32")]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void SaveTest4()
    {
        using (StreamWriter writer = new StreamWriter("save2.txt"))
        {
            writer.WriteLine("This");
            writer.WriteLine("is");
            writer.WriteLine("a");
            writer.WriteLine("test!");
        }
        AbstractSpreadsheet ss = new Spreadsheet("save2.txt", s => true, s => s, "");
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("33")]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void SaveTest5()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        ss.Save("save3.txt");
        ss = new Spreadsheet("save3.txt", s => true, s => s, "version");
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("35")]
    public void SaveTest7()
    {
        var sheet = new
        {
            Cells = new
            {
                A1 = new { StringForm = "hello" },
                A2 = new { StringForm = "5.0" },
                A3 = new { StringForm = "4.0" },
                A4 = new { StringForm = "= A2 + A3" }
            },
            Version = ""
        };

        File.WriteAllText("save5.txt", JsonSerializer.Serialize(sheet));


        AbstractSpreadsheet ss = new Spreadsheet("save5.txt", s => true, s => s, "");
        VV(ss, "A1", "hello", "A2", 5.0, "A3", 4.0, "A4", 9.0);
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("36")]
    public void SaveTest8()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        Set(ss, "A1", "hello");
        Set(ss, "A2", "5.0");
        Set(ss, "A3", "4.0");
        Set(ss, "A4", "= A2 + A3");
        ss.Save("save6.txt");

        string fileContents = File.ReadAllText("save6.txt");
        JsonDocument o = JsonDocument.Parse(fileContents);
        Assert.AreEqual("default", o.RootElement.GetProperty("Version").ToString());
        Assert.AreEqual("hello", o.RootElement.GetProperty("Cells").GetProperty("A1").GetProperty("StringForm").ToString());
        Assert.AreEqual(5.0, double.Parse(o.RootElement.GetProperty("Cells").GetProperty("A2").GetProperty("StringForm").ToString()), 1e-9);
        Assert.AreEqual(4.0, double.Parse(o.RootElement.GetProperty("Cells").GetProperty("A3").GetProperty("StringForm").ToString()), 1e-9);
        Assert.AreEqual("=A2+A3", o.RootElement.GetProperty("Cells").GetProperty("A4").GetProperty("StringForm").ToString().Replace(" ", ""));
    }


    // Fun with formulas
    [TestMethod, Timeout(2000)]
    [TestCategory("37")]
    public void Formula1()
    {
        Formula1(new Spreadsheet());
    }
    public void Formula1(AbstractSpreadsheet ss)
    {
        Set(ss, "a1", "= a2 + a3");
        Set(ss, "a2", "= b1 + b2");
        Assert.IsInstanceOfType(ss.GetCellValue("a1"), typeof(FormulaError));
        Assert.IsInstanceOfType(ss.GetCellValue("a2"), typeof(FormulaError));
        Set(ss, "a3", "5.0");
        Set(ss, "b1", "2.0");
        Set(ss, "b2", "3.0");
        VV(ss, "a1", 10.0, "a2", 5.0);
        Set(ss, "b2", "4.0");
        VV(ss, "a1", 11.0, "a2", 6.0);
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("38")]
    public void Formula2()
    {
        Formula2(new Spreadsheet());
    }
    public void Formula2(AbstractSpreadsheet ss)
    {
        Set(ss, "a1", "= a2 + a3");
        Set(ss, "a2", "= a3");
        Set(ss, "a3", "6.0");
        VV(ss, "a1", 12.0, "a2", 6.0, "a3", 6.0);
        Set(ss, "a3", "5.0");
        VV(ss, "a1", 10.0, "a2", 5.0, "a3", 5.0);
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("39")]
    public void Formula3()
    {
        Formula3(new Spreadsheet());
    }
    public void Formula3(AbstractSpreadsheet ss)
    {
        Set(ss, "a1", "= a3 + a5");
        Set(ss, "a2", "= a5 + a4");
        Set(ss, "a3", "= a5");
        Set(ss, "a4", "= a5");
        Set(ss, "a5", "9.0");
        VV(ss, "a1", 18.0);
        VV(ss, "a2", 18.0);
        Set(ss, "a5", "8.0");
        VV(ss, "a1", 16.0);
        VV(ss, "a2", 16.0);
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("40")]
    public void Formula4()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        Formula1(ss);
        Formula2(ss);
        Formula3(ss);
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("41")]
    public void Formula4a()
    {
        Formula4();
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("42")]
    public void MediumSheet()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        MediumSheet(ss);
    }

    public void MediumSheet(AbstractSpreadsheet ss)
    {
        Set(ss, "A1", "1.0");
        Set(ss, "A2", "2.0");
        Set(ss, "A3", "3.0");
        Set(ss, "A4", "4.0");
        Set(ss, "B1", "= A1 + A2");
        Set(ss, "B2", "= A3 * A4");
        Set(ss, "C1", "= B1 + B2");
        VV(ss, "A1", 1.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 3.0, "B2", 12.0, "C1", 15.0);
        Set(ss, "A1", "2.0");
        VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 4.0, "B2", 12.0, "C1", 16.0);
        Set(ss, "B1", "= A1 / A2");
        VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("43")]
    public void MediumSheeta()
    {
        MediumSheet();
    }


    [TestMethod, Timeout(2000)]
    [TestCategory("44")]
    public void MediumSave()
    {
        AbstractSpreadsheet ss = new Spreadsheet();
        MediumSheet(ss);
        ss.Save("save7.txt");
        ss = new Spreadsheet("save7.txt", s => true, s => s, "default");
        VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
    }

    [TestMethod, Timeout(2000)]
    [TestCategory("45")]
    public void MediumSavea()
    {
        MediumSave();
    }


    // A long chained formula. Solutions that re-evaluate 
    // cells on every request, rather than after a cell changes,
    // will timeout on this test.
    // This test is repeated to increase its scoring weight
    [TestMethod, Timeout(6000)]
    [TestCategory("46")]
    public void LongFormulaTest()
    {
        object result = "";
        LongFormulaHelper(out result);
        Assert.AreEqual("ok", result);
    }

    [TestMethod, Timeout(6000)]
    [TestCategory("47")]
    public void LongFormulaTest2()
    {
        object result = "";
        LongFormulaHelper(out result);
        Assert.AreEqual("ok", result);
    }

    [TestMethod, Timeout(6000)]
    [TestCategory("48")]
    public void LongFormulaTest3()
    {
        object result = "";
        LongFormulaHelper(out result);
        Assert.AreEqual("ok", result);
    }

    [TestMethod, Timeout(6000)]
    [TestCategory("49")]
    public void LongFormulaTest4()
    {
        object result = "";
        LongFormulaHelper(out result);
        Assert.AreEqual("ok", result);
    }

    [TestMethod, Timeout(6000)]
    [TestCategory("50")]
    public void LongFormulaTest5()
    {
        object result = "";
        LongFormulaHelper(out result);
        Assert.AreEqual("ok", result);
    }

    public void LongFormulaHelper(out object result)
    {
        try
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("sum1", "= a1 + a2");
            int i;
            int depth = 100;
            for (i = 1; i <= depth * 2; i += 2)
            {
                s.SetContentsOfCell("a" + i, "= a" + (i + 2) + " + a" + (i + 3));
                s.SetContentsOfCell("a" + (i + 1), "= a" + (i + 2) + "+ a" + (i + 3));
            }
            s.SetContentsOfCell("a" + i, "1");
            s.SetContentsOfCell("a" + (i + 1), "1");
            Assert.AreEqual(Math.Pow(2, depth + 1), (double)s.GetCellValue("sum1"), 1.0);
            s.SetContentsOfCell("a" + i, "0");
            Assert.AreEqual(Math.Pow(2, depth), (double)s.GetCellValue("sum1"), 1.0);
            s.SetContentsOfCell("a" + (i + 1), "0");
            Assert.AreEqual(0.0, (double)s.GetCellValue("sum1"), 0.1);
            result = "ok";
        }
        catch (Exception e)
        {
            result = e;
        }
    }
}
