// Skeleton written by Profs Zachary, Kopta and Martin for CS 3500
// Read the entire skeleton carefully and completely before you
// do anything else!
// Last updated: August 2023 (small tweak to API)

using System.Text.RegularExpressions;

namespace SpreadsheetUtilities;

/// <summary>
/// Represents formulas written in standard infix notation using standard precedence
/// rules.  The allowed symbols are non-negative numbers written using double-precision
/// floating-point syntax (without unary preceeding '-' or '+');
/// variables that consist of a letter or underscore followed by
/// zero or more letters, underscores, or digits; parentheses; and the four operator
/// symbols +, -, *, and /.
///
/// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
/// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
/// and "x 23" consists of a variable "x" and a number "23".
///
/// Associated with every formula are two delegates: a normalizer and a validator.  The
/// normalizer is used to convert variables into a canonical form. The validator is used to
/// add extra restrictions on the validity of a variable, beyond the base condition that
/// variables must always be legal: they must consist of a letter or underscore followed
/// by zero or more letters, underscores, or digits.
/// Their use is described in detail in the constructor and method comments.
/// </summary>
public class Formula
{
    private readonly string formulaExp;
    private readonly Func<string, string> normalizer;
    private readonly Func<string, bool> validator;

    /// <summary>
    /// Creates a Formula from a string that consists of an infix expression written as
    /// described in the class comment.  If the expression is syntactically invalid,
    /// throws a FormulaFormatException with an explanatory Message.
    ///
    /// The associated normalizer is the identity function, and the associated validator
    /// maps every string to true.
    /// </summary>
    public Formula(string formula) :
        this(formula, s => s, s => true)
    {

    }

    /// <summary>
    /// Creates a Formula from a string that consists of an infix expression written as
    /// described in the class comment.  If the expression is syntactically incorrect,
    /// throws a FormulaFormatException with an explanatory Message.
    ///
    /// The associated normalizer and validator are the second and third parameters,
    /// respectively.
    ///
    /// If the formula contains a variable v such that normalize(v) is not a legal variable,
    /// throws a FormulaFormatException with an explanatory message.
    ///
    /// If the formula contains a variable v such that isValid(normalize(v)) is false,
    /// throws a FormulaFormatException with an explanatory message.
    ///
    /// Suppose that N is a method that converts all the letters in a string to upper case, and
    /// that V is a method that returns true only if a string consists of one letter followed
    /// by one digit.  Then:
    ///
    /// new Formula("x2+y3", N, V) should succeed
    /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
    /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
    /// </summary>
    public Formula(string formula, Func<string, string> normalize, Func<string, bool> isValid)
    {

        IEnumerable<string> equation = Formula.GetTokens(formula);
        List<string> equationList = equation.ToList();
        formulaExp = "";
        //normalizing and validating variables and turning all numbers into doubles

        for (int i=0; i<equationList.Count; i++)
        {
            if (!(IsLegalVar(equationList[i]) || IsNum(equationList[i]) || IsSymbol(equationList[i]))) //Parsing Rule
            {
                throw new FormulaFormatException("Invalid token entered. Please check again.");
            }

            if (IsLegalVar(equationList[i])) //If there is a variable
            {

                if (!IsLegalVar(normalize(equationList[i]))) //And when normalized, it is not a legal variable,
                {
                    throw new FormulaFormatException(equationList[i] + " is not a legal variable.");
                } else if (!isValid(normalize(equationList[i])))
                {
                    throw new FormulaFormatException(equationList[i] + " is not a valid variable.");
                } else
                {
                    equationList[i] = normalize(equationList[i]);
                }
            }
        }

        //Instantiating fields
        foreach(String s in equationList)
        {
            formulaExp += s;
        }

        normalizer = normalize;
        validator = isValid;

        //Checking for Syntax and Syntax errors
        if (equationList.Count == 0) //One Token Rule
        {
            throw new FormulaFormatException("Nothing has been passed; please try again.");
        }

        int openParan = 0;
        int closeParan = 0;

        for (int i=0; i<equationList.Count; i++)
        {
            //Balanced Parentheses Rule
            if (equationList[i].Equals("(")) 
            {
                openParan++;
            } else if (equationList[i].Equals(")"))
            {
                closeParan++;
            }

            if (closeParan > openParan)
            {
                throw new FormulaFormatException("There is an unbalanced number of opening and closing parantheses."); 
            }

            //Starting Token Rule
            if (i==0)
            {
                if (!(IsLegalVar(equationList[i]) || IsNum(equationList[i]) || equationList[i].Equals("("))) {
                    throw new FormulaFormatException("Invalid start"); //suggest a change
                }
            }

            //Ending Token Rule
            if (i == equationList.Count-1)
            {
                if (!(IsLegalVar(equationList[i]) || IsNum(equationList[i]) || equationList[i].Equals(")")))
                {
                    throw new FormulaFormatException("Your expression cannot end in anything other than a number, variable, or a closing paranthesis."); //suggest a change
                }
            }

            //Parenthesis/Operator Following Rule
            if (i > 0 && (IsOperator(equationList[i - 1]) || equationList[i - 1].Equals("("))) 
            {
                if (!(equationList[i].Equals("(") || IsLegalVar(equationList[i]) || IsNum(equationList[i])))
                {
                    throw new FormulaFormatException("Opening parantheses and operators must only be followed by a number, variable, or an opening paranthesis.");
                }
                
            }

            //Extra Following Rule
            if (i > 0 && (IsNum(equationList[i - 1]) || IsLegalVar(equationList[i - 1]) || equationList[i - 1].Equals(")"))) 
            {
                if (!(equationList[i].Equals(")") || IsOperator(equationList[i])))
                {
                    throw new FormulaFormatException("Numbers, variables, and closing parantheses must be followed by an operator or a closing paranthesis only."); //suggest a change
                }
                
            }

            //Preceeding Negatives and Positives
            if ((equationList[i].Equals("-") || equationList[i].Equals("+")) &&
                !(IsLegalVar(equationList[i+1]) || IsNum(equationList[i + 1]) || equationList[i + 1].Equals("(")))
            {
                throw new FormulaFormatException("Detected a preceeeding unary operator");
            }
        }

        if (openParan != closeParan) //Balanced Parentheses Rule
        {
            throw new FormulaFormatException("There is an unbalanced number of opening and closing parantheses."); //suggest a change
        }

    }

    /// <summary>
    /// Evaluates this Formula, using the lookup delegate to determine the values of
    /// variables.  When a variable symbol v needs to be determined, it should be looked up
    /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to
    /// the constructor.)
    ///
    /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters
    /// in a string to upper case:
    ///
    /// new Formula("x+7", N, s => true).Evaluate(L) is 11
    /// new Formula("x+7").Evaluate(L) is 9
    ///
    /// Given a variable symbol as its parameter, lookup returns the variable's value
    /// (if it has one) or throws an ArgumentException (otherwise).
    ///
    /// If no undefined variables or divisions by zero are encountered when evaluating
    /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.
    /// The Reason property of the FormulaError should have a meaningful explanation.
    ///
    /// This method should never throw an exception.
    /// </summary>
    public object Evaluate(Func<string, double> lookup)
    {
        String noWhiteSpace = this.ToString();
        string[] substrings = Regex.Split(noWhiteSpace, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");  //turn the expression into tokens

        //Fields
        Stack<double> values = new Stack<double>();
        Stack<string> operations = new Stack<string>();

        foreach (string s in substrings)
        {
            if (Formula.IsNum(s)) //s is a double
            {
                if (operations.Count > 0 && (operations.Peek().Equals("*") || operations.Peek().Equals("/")))
                {
                    try
                    {
                        double result = Formula.StackOperation(values, operations, double.Parse(s));
                        values.Push(result);
                    } catch (Exception)
                    {
                        return new FormulaError("You tried to divide by zero.");
                    }

                }
                else
                {
                    values.Push(double.Parse(s));
                }

            }
            else if (Formula.IsLegalVar(s)) //s is a variable
            {
                if (operations.Count > 0 && (operations.Peek().Equals("*") || operations.Peek().Equals("/")))
                {
                    try
                    {
                        double result = Formula.StackOperation(values, operations, lookup(normalizer(s)));
                        values.Push(result);
                    } catch (Exception)
                    {
                        return new FormulaError("Either you tried to divide by zero or there is no value for your variable");
                    }
                }
                else
                {
                    try
                    {
                        values.Push(lookup(normalizer(s)));
                    }
                    catch (Exception)
                    {
                        return new FormulaError("Lookup failed.");
                    }
                }
            }
            else if (s.Equals("+") || s.Equals("-")) //s is + or -
            {
                if (operations.Count > 0 && (operations.Peek().Equals("+") || operations.Peek().Equals("-")))
                {
                    try
                    {
                        double result = Formula.StackOperation(values, operations);
                        values.Push(result);
                    } catch (Exception)
                    {
                        return new FormulaError("You tried to divide by zero.");
                    }
                }

                operations.Push(s);
            }
            else if (s.Equals("*") || s.Equals("/")) //s is * or /
            {
                operations.Push(s);
            }
            else if (s.Equals("(")) //s is (
            {
                operations.Push(s);
            }
            else if (s.Equals(")"))
            { //s is )
                if (operations.Count > 0)
                {
                    if (operations.Peek().Equals("+") || operations.Peek().Equals("-"))
                    {
                        try
                        {
                            double result = Formula.StackOperation(values, operations);
                            values.Push(result);
                        } catch (Exception)
                        {
                            return new FormulaError("You tried to divide by zero.");
                        }

                    }

                    if (operations.Count > 0 && operations.Peek().Equals("("))
                    {
                        operations.Pop();
                    }
                    else
                    {
                        throw new ArgumentException();
                    }

                    if (operations.Count > 0 && (operations.Peek().Equals("*") || operations.Peek().Equals("/")))
                    {
                        try
                        {
                            double result = Formula.StackOperation(values, operations);
                            values.Push(result);
                        } catch (Exception)
                        {
                            return new FormulaError("You tried to divide by zero.");

                        }

                    }
                }
                else
                {
                    throw new ArgumentException();
                }

            }
            else if (string.IsNullOrEmpty(s))
            {
                continue;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        if (operations.Count > 0) //operations stack still has something
        {
            if (operations.Count == 1 && values.Count == 2)
            {
                if (operations.Peek().Equals("+") || operations.Peek().Equals("-"))
                {
                    return Formula.StackOperation(values, operations);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }
        else //operations stack is empty
        {

            if (values.Count == 1)
            {
                return values.Pop();
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }

    /// <summary>
    ///Pops the value stack once, pops the operator stack, and apply the popped
       //operator to the popped number and a. Push the result onto the value stack.
    /// </summary>
    /// <param name="v">value1</param>
    /// <param name="o">value2</param>
    /// <param name="a">operator</param>
    /// <returns>result</returns>
    /// <exception cref="ArgumentException"></exception>
    private static double StackOperation(Stack<double> v, Stack<String> o, double a)
    {
        try
        {
            double value1 = v.Pop();
            double value2 = a;
            string operation = o.Pop();

            double result = Formula.DoOperation(value1, value2, operation);

            return result;
        }
        catch (InvalidOperationException)
        {
            throw new ArgumentException();
        }
    }

    /// <summary>
    ///Pops the value stack twice, pops the operator stack, and apply the popped
    //operator to the popped number and a. Push the result onto the value stack.
    /// </summary>
    /// <param name="v">value1</param>
    /// <param name="o">value2</param>
    /// <param name="a">operator</param>
    /// <returns>result</returns>
    /// <exception cref="ArgumentException"></exception>
    private static double StackOperation(Stack<double> v, Stack<String> o)
    {
        try
        {
            double value2 = v.Pop();
            double value1 = v.Pop();
            string operation = o.Pop();

            double result = Formula.DoOperation(value1, value2, operation);

            return result;
        }
        catch (InvalidOperationException)
        {
            throw new ArgumentException();
        }
    }

    /// <summary>
    /// Does the operation, given two values and an operator. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="op"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static double DoOperation(double a, double b, string op)
    {
        double result = -1;
        switch (op)
        {
            case "+":
                result = a + b;
                break;
            case "-":
                result = a - b;
                break;
            case "*":
                result = a * b;
                break;
            case "/":
                if (b == 0)
                {
                    throw new ArgumentException();
                }
                result = a / b;
                break;
        }

        return result;
    }

    /// <summary>
    /// Enumerates the normalized versions of all of the variables that occur in this
    /// formula.  No normalization may appear more than once in the enumeration, even
    /// if it appears more than once in this Formula.
    ///
    /// For example, if N is a method that converts all the letters in a string to upper case:
    ///
    /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
    /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
    /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
    /// </summary>
    public IEnumerable<string> GetVariables()
    {
        IEnumerable<string> equation = Formula.GetTokens(formulaExp);
        HashSet<string> varList = new HashSet<string>();

        foreach (String s in equation)
        {
            if (IsLegalVar(s))
            {
                varList.Add(s);
            }
        }

        return varList;
    }

    /// <summary>
    /// Returns a string containing no spaces which, if passed to the Formula
    /// constructor, will produce a Formula f such that this.Equals(f).  All of the
    /// variables in the string should be normalized.
    ///
    /// For example, if N is a method that converts all the letters in a string to upper case:
    ///
    /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
    /// new Formula("x + Y").ToString() should return "x+Y"
    /// </summary>
    public override string ToString()
    {
        String noWhiteSpace = Regex.Replace(formulaExp, @"\s", ""); //remove all the white space in the expression
        return noWhiteSpace;
    }

    /// <summary>
    /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
    /// whether or not this Formula and obj are equal.
    ///
    /// Two Formulae are considered equal if they consist of the same tokens in the
    /// same order.  To determine token equality, all tokens are compared as strings
    /// except for numeric tokens and variable tokens.
    /// Numeric tokens are considered equal if they are equal after being "normalized" by
    /// using C#'s standard conversion from string to double (and optionally back to a string).
    /// Variable tokens are considered equal if their normalized forms are equal, as
    /// defined by the provided normalizer.
    ///
    /// For example, if N is a method that converts all the letters in a string to upper case:
    ///
    /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
    /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
    /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
    /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Formula))
        {
            return false;
        } else
        {
            Formula f1 = this;
            Formula ? f2 = obj as Formula;

            if (f2 is not null)
            {
                List<string> f1List = Formula.GetTokens(f1.ToString()).ToList(); ;
                List<string> f2List = Formula.GetTokens(f2.ToString()).ToList();

                if (f1List.Count != f2List.Count)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < f1List.Count; i++)
                    {
                        if (IsNum(f1List[i]))
                        {
                            f1List[i] = Double.Parse(f1List[i]).ToString();
                        }
                        else if (IsNum(f2List[i]))
                        {
                            f2List[i] = Double.Parse(f2List[i]).ToString();

                        }

                        if (!f1List[i].Equals(f2List[i]))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Reports whether f1 == f2, using the notion of equality from the Equals method.
    /// Note that f1 and f2 cannot be null, because their types are non-nullable
    /// </summary>
    public static bool operator ==(Formula f1, Formula f2)
    {
        List<string> f1List = Formula.GetTokens(f1.ToString()).ToList(); ;
        List<string> f2List = Formula.GetTokens(f2.ToString()).ToList();

        if (f1List.Count != f2List.Count)
        {
            return false;
        } else
        {
            for (int i=0; i<f1List.Count; i++)
            {
                if (IsNum(f1List[i]))
                {
                    f1List[i] = Double.Parse(f1List[i]).ToString();
                } else if (IsNum(f2List[i]))
                {
                    f2List[i] = Double.Parse(f2List[i]).ToString();

                }
                if (!f1List[i].Equals(f2List[i]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Reports whether f1 != f2, using the notion of equality from the Equals method.
    /// Note that f1 and f2 cannot be null, because their types are non-nullable
    /// </summary>
    public static bool operator !=(Formula f1, Formula f2)
    {
        List<string> f1List = Formula.GetTokens(f1.ToString()).ToList(); ;
        List<string> f2List = Formula.GetTokens(f2.ToString()).ToList();

        if (f1List.Count != f2List.Count)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < f1List.Count; i++)
            {
                if (IsNum(f1List[i]))
                {
                    f1List[i] = Double.Parse(f1List[i]).ToString();
                }
                else if (IsNum(f2List[i]))
                {
                    f2List[i] = Double.Parse(f2List[i]).ToString();

                }
                if (!f1List[i].Equals(f2List[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
    /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two
    /// randomly-generated unequal Formulae have the same hash code should be extremely small.
    /// </summary>
    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }

    /// <summary>
    /// Returns whether the token is a legal variable
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static Boolean IsLegalVar(String s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        //Variables must consist of a letter or underscore
        if (s[0] == '_' || Char.IsLetter(s[0])) { }
        else
        {
            return false;
        }

        for (int i = 1; i < s.Length; i++)
        {
            if (!(Char.IsLetterOrDigit(s[i]) || s[i] == '_'))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns whether the token is a number
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static Boolean IsNum(String s)
    {
        double value;
        bool isNumeric = double.TryParse(s, out value);
        return isNumeric;
    }

    /// <summary>
    /// Returns whether the token is one of the 4 operators or ()
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static Boolean IsSymbol(String s)
    {
        if (s.Equals("+") || s.Equals("-") || s.Equals("*") || s.Equals("/") || s.Equals("(") || s.Equals(")"))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns whether the token is one of the 4 operators
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static Boolean IsOperator(String s)
    {
        if (s.Equals("+") || s.Equals("-") || s.Equals("*") || s.Equals("/"))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
    /// right paren; one of the four operator symbols; a legal variable token;
    /// a double literal; and anything that doesn't match one of those patterns.
    /// There are no empty tokens, and no token contains white space.
    /// </summary>
    private static IEnumerable<string> GetTokens(string formula)
    {
        // Patterns for individual tokens
        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*/]";
        string varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
        string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        // Overall pattern
        string pattern = string.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

        // Enumerate matching tokens that don't consist solely of white space.
        foreach (string s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
        {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
                yield return s;
            }
        }

    }

}

/// <summary>
/// Used to report syntactic errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{
    /// <summary>
    /// Constructs a FormulaFormatException containing the explanatory message.
    /// </summary>
    public FormulaFormatException(string message) : base(message)
    {

    }
}

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public struct FormulaError
{
    /// <summary>
    /// Constructs a FormulaError containing the explanatory reason.
    /// </summary>
    /// <param name="reason"></param>
    public FormulaError(string reason) : this()
    {
        Reason = reason;
    }

    /// <summary>
    ///  The reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}