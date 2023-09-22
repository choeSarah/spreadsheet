namespace FormulaTests;

using System.Text;
using SpreadsheetUtilities;

[TestClass]
public class FormulaTests
{
    //parsing errors
    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void ParsingError1()
    {
        Formula f1 = new Formula("%5+3");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void ParsingError2()
    {
        Formula f1 = new Formula("5$+3");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void ParsingError3()
    {
        Formula f1 = new Formula("5^+3");
    }

    //no token error
    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void NoTokenConstructor()
    {
        Formula f1 = new Formula("");
    }

    //Right Parentheses Rule
    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void MoreClosingParanthesesConstructor()
    {
        Formula f1 = new Formula("(5+3))");
    }

    //Balanced Parentheses Rule
    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void MoreOpeningParanthesesConstructor()
    {
        Formula f1 = new Formula("((5+3)");
    }

    //Starting Token Rule
    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void BeginsWithOperatorConstructorAdd()
    {
        Formula f1 = new Formula("+3+5");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void BeginsWithOperatorConstructorMinus()
    {
        Formula f1 = new Formula("-3+5");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void BeginsWithOperatorConstructorMultiply()
    {
        Formula f1 = new Formula("*3+5");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void BeginsWithOperatorConstructorDivide()
    {
        Formula f1 = new Formula("/3+5");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void BeginsWithOperatorConstructorClosingParanthesis()
    {
        Formula f1 = new Formula(")3+5");
    }

    //Ending Token Rule
    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void EndsWithOperatorConstructorAdd()
    {
        Formula f1 = new Formula("3+5+");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void EndsWithOperatorConstructorMinus()
    {
        Formula f1 = new Formula("3+5-");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void EndsWithOperatorConstructorMultiply()
    {
        Formula f1 = new Formula("3+5*");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void EndsWithOperatorConstructorDivide()
    {
        Formula f1 = new Formula("3+5/");
    }

    //Ending Token Rule
    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOpeningParanthesisAdd()
    {
        Formula f1 = new Formula("(+3+7)");
    }

    //Parenthesis/Operator Following Rule
    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOpeningParanthesisSubtract()
    {
        Formula f1 = new Formula("(-3+7)");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOpeningParanthesisMultiply()
    {
        Formula f1 = new Formula("(*3+7)");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOpeningParanthesisDivide()
    {
        Formula f1 = new Formula("(/3+7)");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOpeningParanthesisClosingParanthesis()
    {
        Formula f1 = new Formula("()3+7)");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOperatorAdd()
    {
        Formula f1 = new Formula("3++7");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOperatorSubtract()
    {
        Formula f1 = new Formula("3+-7");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOperatorMultiply()
    {
        Formula f1 = new Formula("3+*7");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOperatorDivide()
    {
        Formula f1 = new Formula("3+/7");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OperatorFollowingAnOperatorClosingParanthesis()
    {
        Formula f1 = new Formula("3+)7");
    }

    //Extra Following Rule
    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void NumFollowingNum()
    {
        Formula f1 = new Formula("3 7");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void VarFollowingNum()
    {
        Formula f1 = new Formula("x3 7");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OpeningParanthesisFollowingNum()
    {
        Formula f1 = new Formula("3(");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OpeningParanthesisFollowingVar()
    {
        Formula f1 = new Formula("c3(");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void NumFollowingClosingParanthesis()
    {
        Formula f1 = new Formula("(4+3)3");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void VarFollowingClosingParanthesis()
    {
        Formula f1 = new Formula("(4+3)c3");
    }

    [TestMethod(), Timeout(5000)]
    [ExpectedException(typeof(FormulaFormatException))]
    public void OpeningParanthesisFollowingClosingParanthesis()
    {
        Formula f1 = new Formula("(4+3)(");
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("1")]
    public void TestSingleNumber()
    {
        Formula f1 = new Formula("5");
        Assert.AreEqual("5", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("2")]
    public void TestSingleVariable()
    {
        Formula f1 = new Formula("X5");
        Assert.AreEqual("13", f1.Evaluate(s => 13).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("3")]
    public void TestAddition()
    {
        Formula f1 = new Formula("5+3");
        Assert.AreEqual("8", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    public void TestAdditionEvaluator()
    {
        Formula f1 = new Formula("5 + A2", s => s.ToLower(), s => s == "a2");
        Assert.AreEqual("7", f1.Evaluate(s => 2).ToString());
    }


    [TestMethod(), Timeout(5000)]
    [TestCategory("4")]
    public void TestSubtraction()
    {
        Formula f1 = new Formula("18-10");
        Assert.AreEqual("8", f1.Evaluate(s => 0).ToString());
    }


    [TestMethod(), Timeout(5000)]
    public void TestSubtractionEvaluator()
    {
        Formula f1 = new Formula("5 - A2", s => s.ToLower(), s => s == "a2");
        Assert.AreEqual("3", f1.Evaluate(s => 2).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("5")]
    public void TestMultiplication()
    {
        Formula f1 = new Formula("2*4");
        Assert.AreEqual("8", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    public void TestMultiplicationEvaluator()
    {
        Formula f1 = new Formula("5 * A2", s => s.ToLower(), s => s == "a2");
        Assert.AreEqual("10", f1.Evaluate(s => 2).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("6")]
    public void TestDivision()
    {
        Formula f1 = new Formula("16/2");
        Assert.AreEqual("8", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    public void TestDivisionEvaluator()
    {
        Formula f1 = new Formula("5 / A2", s => s.ToLower(), s => s == "a2");
        Assert.AreEqual("2.5", f1.Evaluate(s => 2).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("8")]
    public void TestUnknownVariable1()
    {
        Formula f1 = new Formula("2+X1");
        Assert.IsTrue(f1.Evaluate(s => { throw new ArgumentException("Unknown variable"); }) is FormulaError);
    }


    [TestMethod(), Timeout(5000)]
    [TestCategory("9")]
    public void TestLeftToRight()
    {
        Formula f1 = new Formula("2*6+3");
        Assert.AreEqual("15", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("10")]
    public void TestOrderOperations()
    {
        Formula f1 = new Formula("2+6*3");
        Assert.AreEqual("20", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("11")]
    public void TestParenthesesTimes()
    {
        Formula f1 = new Formula("(2+6)*3");
        Assert.AreEqual("24", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("12")]
    public void TestTimesParentheses()
    {
        Formula f1 = new Formula("2*(3+5)");
        Assert.AreEqual("16", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("13")]
    public void TestPlusParentheses()
    {
        Formula f1 = new Formula("2+(3+5)");
        Assert.AreEqual("10", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("14")]
    public void TestPlusComplex()
    {
        Formula f1 = new Formula("2+(3+5*9)");
        Assert.AreEqual("50", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("15")]
    public void TestOperatorAfterParens()
    {
        Formula f1 = new Formula("(1*1)-2/2");
        Assert.AreEqual("0", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("16")]
    public void TestComplexTimesParentheses()
    {
        Formula f1 = new Formula("2+3*(3+5)");
        Assert.AreEqual("26", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("17")]
    public void TestComplexAndParentheses()
    {
        Formula f1 = new Formula("2+3*5+(3+4*8)*5+2");
        Assert.AreEqual("194", f1.Evaluate(s => 0).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("18")]
    public void TestDivideByZero1()
    {
        Formula f1 = new Formula("5/0");
        Assert.IsTrue(f1.Evaluate(s => 0) is FormulaError);
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("22")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestInvalidVariable()
    {
        Formula f1 = new Formula("2z");
        f1.Evaluate(s => 0);
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("23")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestPlusInvalidVariable()
    {
        Formula f1 = new Formula("5+ 2z");
        f1.Evaluate(s => 0);
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("26")]
    public void TestComplexMultiVar()
    {
        Formula f1 = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
        Assert.AreEqual("5.142857142857142", f1.Evaluate(s => (s == "x7") ? 1 : 4).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("27")]
    public void TestComplexNestedParensRight()
    {
        Formula f1 = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
        Assert.AreEqual("6", f1.Evaluate(s => 1).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("28")]
    public void TestComplexNestedParensLeft()
    {
        Formula f1 = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
        Assert.AreEqual("12", f1.Evaluate(s => 2).ToString());
    }

    [TestMethod(), Timeout(5000)]
    [TestCategory("29")]
    public void TestRepeatedVar1()
    {
        Formula f1 = new Formula("a4-a4*a4/a4");
        Assert.AreEqual("0", f1.Evaluate(s => 3).ToString());
    }

    [TestMethod(), Timeout(5000)]
    public void GetNoVariables()
    {
        Formula f1 = new Formula("5");
        IEnumerable<string> varIEnumerable = f1.GetVariables();
        List<string> varList = varIEnumerable.ToList();

        Assert.AreEqual(0, varList.Count);
    }

    [TestMethod(), Timeout(5000)]
    public void GetOneValidVariable()
    {
        Formula f1 = new Formula("5+a4");
        IEnumerable<string> varIEnumerable = f1.GetVariables();
        List<string> varList = varIEnumerable.ToList();

        Assert.AreEqual(1, varList.Count);
        Assert.AreEqual("a4", varList.ElementAt(0));
    }

    [TestMethod(), Timeout(5000)]
    public void NormalizedExtraction()
    {
        Formula f1 = new Formula("x+B*a", s=> s.ToUpper(), s=> true);
        IEnumerable<string> varIEnumerable = f1.GetVariables();
        List<string> varList = varIEnumerable.ToList();

        Assert.AreEqual(3, varList.Count);
        Assert.AreEqual("X", varList.ElementAt(0));
        Assert.AreEqual("B", varList.ElementAt(1));
        Assert.AreEqual("A", varList.ElementAt(2));
    }

    [TestMethod(), Timeout(5000)]
    public void BasicVariableExtraction()
    {
        Formula f1 = new Formula("x + y*z");
        IEnumerable<string> varIEnumerable = f1.GetVariables();
        List<string> varList = varIEnumerable.ToList();

        Assert.AreEqual(3, varList.Count);
        Assert.AreEqual("x", varList.ElementAt(0));
        Assert.AreEqual("y", varList.ElementAt(1));
        Assert.AreEqual("z", varList.ElementAt(2));
    }

    [TestMethod(), Timeout(5000)]
    public void WhiteSpaceTest()
    {
        Formula f1 = new Formula("x     +   y   *z");
        IEnumerable<string> varIEnumerable = f1.GetVariables();
        List<string> varList = varIEnumerable.ToList();

        Assert.AreEqual(3, varList.Count);
        Assert.AreEqual("x", varList.ElementAt(0));
        Assert.AreEqual("y", varList.ElementAt(1));
        Assert.AreEqual("z", varList.ElementAt(2));
    }

    [TestMethod(), Timeout(5000)]
    public void LongExtraction()
    {
        Formula f1 = new Formula("v1 + _var12*a9 / b_12");
        IEnumerable<string> varIEnumerable = f1.GetVariables();
        List<string> varList = varIEnumerable.ToList();

        Assert.AreEqual(4, varList.Count);
        Assert.AreEqual("v1", varList.ElementAt(0));
        Assert.AreEqual("_var12", varList.ElementAt(1));
        Assert.AreEqual("a9", varList.ElementAt(2));
        Assert.AreEqual("b_12", varList.ElementAt(3));
    }

    [TestMethod(), Timeout(5000)]
    public void ToStringSingleNumber()
    {
        Formula f1 = new Formula("5");
        Assert.AreEqual("5", f1.ToString());
    }

    [TestMethod(), Timeout(5000)]
    public void ToStringSingleVar()
    {
        Formula f1 = new Formula("X5");
        Assert.AreEqual("X5", f1.ToString());
    }

    [TestMethod(), Timeout(5000)]
    public void ToStringSimpleEquation()
    {
        Formula f1 = new Formula("x5 + 3");
        Assert.AreEqual("x5+3", f1.ToString());
    }

    [TestMethod(), Timeout(5000)]
    public void ToStringNormalized()
    {
        Formula f1 = new Formula("x5 + 3", s=> s.ToUpper(), s=> true);
        Assert.AreEqual("X5+3", f1.ToString());
    }

    [TestMethod(), Timeout(5000)]
    public void ToStringMixedNormalized()
    {
        Formula f1 = new Formula("x5 + Y3", s => s.ToUpper(), s => true);
        Assert.AreEqual("X5+Y3", f1.ToString());
    }

    [TestMethod(), Timeout(5000)]
    public void SimpleEqualNull ()
    {
        Formula f1 = new Formula("x5 + 3");
        Formula ? f2 = null;

        Assert.IsFalse(f1.Equals(f2));
    }

    [TestMethod(), Timeout(5000)]
    public void SimpleEqualNotNull()
    {
        Formula f1 = new Formula("x5 + 3");
        Formula? f2 = new Formula("x5+3"); ;

        Assert.IsTrue(f1.Equals(f2));
    }

    [TestMethod(), Timeout(5000)]
    public void SimpleNotEqualNotNull()
    {
        Formula f1 = new Formula("x5 + 3");
        Formula? f2 = new Formula("x7+3"); ;

        Assert.IsFalse(f1.Equals(f2));
    }

    public void DoubleEqualNotNull()
    {
        Formula f1 = new Formula("3.0+5.0");
        Formula? f2 = new Formula("3+5.0"); ;

        Assert.IsFalse(f1.Equals(f2));
    }

    [TestMethod(), Timeout(5000)]
    public void SimpleEqual()
    {
        Formula f1 = new Formula("x5 + 3");
        Formula f2 = new Formula("x5+3");

        Assert.IsTrue(f1 == f2);
    }

    public void DoubleEqual()
    {
        Formula f1 = new Formula("3.0+5.0");
        Formula? f2 = new Formula("3+5.0"); ;

        Assert.IsFalse(f1 == f2);
    }

    [TestMethod(), Timeout(5000)]
    public void MixedEqualitesTestEqual()
    {
        Formula f1 = new Formula("3.0+5.0");
        Formula f2 = new Formula("4+5");
        Formula f3 = new Formula("3+5");

        Assert.IsTrue(f1.Equals(f3));
        Assert.IsFalse(f1.Equals(f2));

    }
    [TestMethod(), Timeout(5000)]
    public void SimpleNotEqual()
    {
        Formula f1 = new Formula("x5 + 3");
        Formula f2 = new Formula("x7+3");

        Assert.IsTrue(f1 != f2);
    }

    [TestMethod(), Timeout(5000)]
    public void DoubleNotEqual()
    {
        Formula f1 = new Formula("3.0+5.0");
        Formula? f2 = new Formula("4+5.0"); 

        Assert.IsTrue(f1 != f2);
    }

    [TestMethod(), Timeout(5000)]
    public void MixedEqualityTest()
    {
        Formula f1 = new Formula("3.0+5.0");
        Formula f2 = new Formula("4+5");
        Formula f3 = new Formula("3+5");

        Assert.IsTrue(f1 == f3);
        Assert.IsTrue(f1 != f2);

    }
    [TestMethod(), Timeout(5000)]
    public void SimpleGetHashCodeEqual()
    {
        Formula f1 = new Formula("x5 + 3");
        Formula f2 = new Formula("x5+3");

        Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
    }

    public void SimpleGetHashCodeNotEqual()
    {
        Formula f1 = new Formula("x5 + 3");
        Formula f2 = new Formula("x7+3");

        Assert.AreNotEqual(f1.GetHashCode(), f2.GetHashCode());
    }

    //////////
    ///
        // Normalizer tests
    [TestMethod(), Timeout(2000)]
    [TestCategory("1")]
    public void TestNormalizerGetVars()
    {
        Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
        HashSet<string> vars = new HashSet<string>(f.GetVariables());

        Assert.IsTrue(vars.SetEquals(new HashSet<string> { "X1" }));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("2")]
    public void TestNormalizerEquals()
    {
        Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
        Formula f2 = new Formula("2+X1", s => s.ToUpper(), s => true);

        Assert.IsTrue(f.Equals(f2));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("3")]
    public void TestNormalizerToString()
    {
        Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
        Formula f2 = new Formula(f.ToString());

        Assert.IsTrue(f.Equals(f2));
    }

    // Validator tests
    [TestMethod(), Timeout(2000)]
    [TestCategory("4")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestValidatorFalse()
    {
        Formula f = new Formula("2+x1", s => s, s => false);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("5")]
    public void TestValidatorX1()
    {
        Formula f = new Formula("2+x", s => s, s => (s == "x"));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("6")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestValidatorX2()
    {
        Formula f = new Formula("2+y1", s => s, s => (s == "x"));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("7")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestValidatorX3()
    {
        Formula f = new Formula("2+x1", s => s, s => (s == "x"));
    }


    // Simple tests that return FormulaErrors
    [TestMethod(), Timeout(2000)]
    [TestCategory("8")]
    public void TestUnknownVariable()
    {
        Formula f = new Formula("2+X1");
        Assert.IsInstanceOfType(f.Evaluate(s => { throw new ArgumentException("Unknown variable"); }), typeof(FormulaError));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("9")]
    public void TestDivideByZero()
    {
        Formula f = new Formula("5/0");
        Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("10")]
    public void TestDivideByZeroVars()
    {
        Formula f = new Formula("(5 + X1) / (X1 - 3)");
        Assert.IsInstanceOfType(f.Evaluate(s => 3), typeof(FormulaError));
    }


    // Tests of syntax errors detected by the constructor
    [TestMethod(), Timeout(2000)]
    [TestCategory("11")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestSingleOperator()
    {
        Formula f = new Formula("+");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("12")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraOperator()
    {
        Formula f = new Formula("2+5+");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("13")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraCloseParen()
    {
        Formula f = new Formula("2+5*7)");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("14")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraOpenParen()
    {
        Formula f = new Formula("((3+5*7)");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("15")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator()
    {
        Formula f = new Formula("5x");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("16")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator2()
    {
        Formula f = new Formula("5+5x");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("17")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator3()
    {
        Formula f = new Formula("5+7+(5)8");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("18")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator4()
    {
        Formula f = new Formula("5 5");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("19")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestDoubleOperator()
    {
        Formula f = new Formula("5 + + 3");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("20")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestEmpty()
    {
        Formula f = new Formula("");
    }

    // Some more complicated formula evaluations
    [TestMethod(), Timeout(2000)]
    [TestCategory("21")]
    public void TestComplex1()
    {
        Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
        Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("22")]
    public void TestRightParens()
    {
        Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
        Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("23")]
    public void TestLeftParens()
    {
        Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
        Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("53")]
    public void TestRepeatedVar()
    {
        Formula f = new Formula("a4-a4*a4/a4");
        Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
    }

    // Test of the Equals method
    [TestMethod(), Timeout(2000)]
    [TestCategory("24")]
    public void TestEqualsBasic()
    {
        Formula f1 = new Formula("X1+X2");
        Formula f2 = new Formula("X1+X2");
        Assert.IsTrue(f1.Equals(f2));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("25")]
    public void TestEqualsWhitespace()
    {
        Formula f1 = new Formula("X1+X2");
        Formula f2 = new Formula(" X1  +  X2   ");
        Assert.IsTrue(f1.Equals(f2));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("26")]
    public void TestEqualsDouble()///////FIX
    {
        Formula f1 = new Formula("2+X1*3.00");
        Formula f2 = new Formula("2.00+X1*3.0");
        Assert.IsTrue(f1.Equals(f2));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("27")]
    public void TestEqualsComplex() ////////FIX
    {
        Formula f1 = new Formula("1e-2 + X5 + 17.00 * 19 ");
        Formula f2 = new Formula("   0.0100  +     X5+ 17 * 19.00000 ");
        Assert.IsTrue(f1.Equals(f2));
    }


    [TestMethod(), Timeout(2000)]
    [TestCategory("28")]
    public void TestEqualsNullAndString()
    {
        Formula f = new Formula("2");
        Assert.IsFalse(f.Equals(null));
        Assert.IsFalse(f.Equals(""));
    }


    // Tests of == operator
    [TestMethod(), Timeout(2000)]
    [TestCategory("29")]
    public void TestEq()
    {
        Formula f1 = new Formula("2");
        Formula f2 = new Formula("2");
        Assert.IsTrue(f1 == f2);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("30")]
    public void TestEqFalse()
    {
        Formula f1 = new Formula("2");
        Formula f2 = new Formula("5");
        Assert.IsFalse(f1 == f2);
    }


    // Tests of != operator
    [TestMethod(), Timeout(2000)]
    [TestCategory("32")]
    public void TestNotEq()
    {
        Formula f1 = new Formula("2");
        Formula f2 = new Formula("2");
        Assert.IsFalse(f1 != f2);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("33")]
    public void TestNotEqTrue()
    {
        Formula f1 = new Formula("2");
        Formula f2 = new Formula("5");
        Assert.IsTrue(f1 != f2);
    }


    // Test of ToString method
    [TestMethod(), Timeout(2000)]
    [TestCategory("34")]
    public void TestString()
    {
        Formula f = new Formula("2*5");
        Assert.IsTrue(f.Equals(new Formula(f.ToString())));
    }


    // Tests of GetHashCode method
    [TestMethod(), Timeout(2000)]
    [TestCategory("35")]
    public void TestHashCode()
    {
        Formula f1 = new Formula("2*5");
        Formula f2 = new Formula("2*5");
        Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
    }

    // Technically the hashcodes could not be equal and still be valid,
    // extremely unlikely though. Check their implementation if this fails.
    [TestMethod(), Timeout(2000)]
    [TestCategory("36")]
    public void TestHashCodeFalse()
    {
        Formula f1 = new Formula("2*5");
        Formula f2 = new Formula("3/8*2+(7)");
        Assert.IsTrue(f1.GetHashCode() != f2.GetHashCode());
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("37")]
    public void TestHashCodeComplex()
    {
        Formula f1 = new Formula("2 * 5 + 4.00 - _x"); /////FIXX
        Formula f2 = new Formula("2*5+4-_x");
        Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
    }


    // Tests of GetVariables method
    [TestMethod(), Timeout(2000)]
    [TestCategory("38")]
    public void TestVarsNone()
    {
        Formula f = new Formula("2*5");
        Assert.IsFalse(f.GetVariables().GetEnumerator().MoveNext());
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("39")]
    public void TestVarsSimple()
    {
        Formula f = new Formula("2*X2");
        List<string> actual = new List<string>(f.GetVariables());
        HashSet<string> expected = new HashSet<string>() { "X2" };
        Assert.AreEqual(actual.Count, 1);
        Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("40")]
    public void TestVarsTwo()
    {
        Formula f = new Formula("2*X2+Y3");
        List<string> actual = new List<string>(f.GetVariables());
        HashSet<string> expected = new HashSet<string>() { "Y3", "X2" };
        Assert.AreEqual(actual.Count, 2);
        Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("41")]
    public void TestVarsDuplicate()
    {
        Formula f = new Formula("2*X2+X2");
        List<string> actual = new List<string>(f.GetVariables());
        HashSet<string> expected = new HashSet<string>() { "X2" };
        Assert.AreEqual(actual.Count, 1);
        Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("42")]
    public void TestVarsComplex()
    {
        Formula f = new Formula("X1+Y2*X3*Y2+Z7+X1/Z8");
        List<string> actual = new List<string>(f.GetVariables());
        HashSet<string> expected = new HashSet<string>() { "X1", "Y2", "X3", "Z7", "Z8" };
        Assert.AreEqual(actual.Count, 5);
        Assert.IsTrue(expected.SetEquals(actual));
    }

    // Tests to make sure there can be more than one formula at a time
    [TestMethod(), Timeout(2000)]
    [TestCategory("43")]
    public void TestMultipleFormulae()
    {
        Formula f1 = new Formula("2 + a1");
        Formula f2 = new Formula("3");
        Assert.AreEqual(2.0, f1.Evaluate(x => 0));
        Assert.AreEqual(3.0, f2.Evaluate(x => 0));
        Assert.IsFalse(new Formula(f1.ToString()) == new Formula(f2.ToString()));
        IEnumerator<string> f1Vars = f1.GetVariables().GetEnumerator();
        IEnumerator<string> f2Vars = f2.GetVariables().GetEnumerator();
        Assert.IsFalse(f2Vars.MoveNext());
        Assert.IsTrue(f1Vars.MoveNext());
    }

    // Repeat this test to increase its weight
    [TestMethod(), Timeout(2000)]
    [TestCategory("44")]
    public void TestMultipleFormulaeB()
    {
        TestMultipleFormulae();
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("45")]
    public void TestMultipleFormulaeC()
    {
        TestMultipleFormulae();
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("46")]
    public void TestMultipleFormulaeD()
    {
        TestMultipleFormulae();
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("47")]
    public void TestMultipleFormulaeE()
    {
        TestMultipleFormulae();
    }

    // Stress test for constructor
    [TestMethod(), Timeout(2000)]
    [TestCategory("48")]
    public void TestConstructor()
    {
        Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    // This test is repeated to increase its weight
    [TestMethod(), Timeout(2000)]
    [TestCategory("49")]
    public void TestConstructorB()
    {
        Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("50")]
    public void TestConstructorC()
    {
        Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("51")]
    public void TestConstructorD()
    {
        Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    // Stress test for constructor
    [TestMethod(), Timeout(2000)]
    [TestCategory("52")]
    public void TestConstructorE()
    {
        Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

}
