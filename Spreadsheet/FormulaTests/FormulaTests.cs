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
    public void TestUnknownVariable()
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
    public void TestDivideByZero()
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
    public void TestRepeatedVar()
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

}
