using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FormulaEvaluator;

/// <summary>
/// This class evaluates infix expressions using PEMDAS and includes all of the helper methods to do so
/// </summary>
public static class Evaluator
{
    public delegate int Lookup(String v); //helps evaluate the variables into their integer form

    /// <summary>
    /// This method takes an infix expressions using PEMDAS
    /// </summary>
    /// <param name="exp">String representation of the expression</param>
    /// <param name="variableEvaluator">The method that will evaluate the variable to an int</param>
    /// <returns>The result of the expression</returns>
    /// <exception cref="ArgumentException"></exception>
    public static int Evaluate(String exp, Lookup variableEvaluator)
    { 
        String noWhiteSpace = Regex.Replace(exp, @"\s", ""); //remove all the white space in the expression
        string[] substrings = Regex.Split(noWhiteSpace, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");  //turn the expression into tokens

        //Fields
        Stack<int> values = new Stack<int>();
        Stack<string> operations = new Stack<string>();

        foreach (string s in substrings)
        {
            if (Evaluator.IsInteger(s)) //s is an integer
            {
                if (operations.Count > 0 && (operations.Peek().Equals("*") || operations.Peek().Equals("/")))
                {
                    int result = Evaluator.StackOperation(values, operations, Int32.Parse(s));
                    values.Push(result);
                }
                else
                {
                    values.Push(Int32.Parse(s));
                }

            }
            else if (Evaluator.IsVar(s)) //s is a variable
            {
                if (operations.Count > 0 && (operations.Peek().Equals("*") || operations.Peek().Equals("/")))
                {
                    int result = Evaluator.StackOperation(values, operations, variableEvaluator(s));
                    values.Push(result);
                }
                else
                {
                    values.Push(variableEvaluator(s));
                }
            } else if (s.Equals("+") || s.Equals("-")) //s is + or -
            {
                if (operations.Count > 0 && (operations.Peek().Equals("+") || operations.Peek().Equals("-")))
                {
                    int result = Evaluator.StackOperation(values, operations);
                    values.Push(result);
                }

                    operations.Push(s);
            } else if (s.Equals("*") || s.Equals("/")) //s is * or /
            {
                operations.Push(s);
            } else if (s.Equals("(")) //s is (
            {
                operations.Push(s);
            } else if (s.Equals(")")) { //s is )
                if (operations.Count > 0)
                {
                    if (operations.Peek().Equals("+") || operations.Peek().Equals("-"))
                    {
                        int result = Evaluator.StackOperation(values, operations);
                        values.Push(result);
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
                        int result = Evaluator.StackOperation(values, operations);
                        values.Push(result);
                    }
                } else
                {
                    throw new ArgumentException();
                }

            } else if (string.IsNullOrEmpty(s))
            {
                continue;
            } else
            {
                throw new ArgumentException();
            }
       }

        if (operations.Count>0) //operations stack still has something
        {
            if (operations.Count == 1 && values.Count == 2)
            {
                if (operations.Peek().Equals("+") || operations.Peek().Equals("-"))
                {
                    return Evaluator.StackOperation(values, operations);
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
        } else //operations stack is empty
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

    private static int StackOperation (Stack<int> v, Stack <String> o, int a)
    {
        try
        {
            int value1 = v.Pop();
            int value2 = a;
            string operation = o.Pop();

            int result = Evaluator.DoOperation(value1, value2, operation);

            return result;
        } catch (InvalidOperationException)
        {
            throw new ArgumentException();
        }
        
        //pop the value stack, pop the operator stack, and apply the popped
       //operator to the popped number and t. Push the result onto the value stack.
    }

    private static int StackOperation(Stack<int> v, Stack<String> o)
    {
        try
        {
            int value2 = v.Pop();
            int value1 = v.Pop();
            string operation = o.Pop();

            int result = Evaluator.DoOperation(value1, value2, operation);

            return result;
        } catch (InvalidOperationException)
        {
            throw new ArgumentException();
        }
    }

    private static int DoOperation (int a, int b, string op)
    {
        int result = -1;
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

    private static Boolean IsInteger(String s)
    {
        int value;
        bool isNumeric = int.TryParse(s, out value);

        return isNumeric;
    }

    private static Boolean IsVar(String s)
    {
        int letterCount = 0;
        int digitCount = 0;

        for (int i=0; i<s.Length; i++)
        {
            if (Char.IsLetter(s[i]))
            {
                letterCount++;

                if (i > 0 && Char.IsDigit(s[i-1]))
                {
                    return false;
                }
            }

            if (Char.IsDigit(s[i]))
            {
                digitCount++;
              
            }
        }

        if (letterCount == 0 || digitCount == 0)
        {
            return false;
        }

        return true;

    }

}
