using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;


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

        //check if the expression is valid

        for (int i = 0; i < substrings.Length; i++)
        {
            if (string.IsNullOrEmpty(substrings[i])) //if there is an empty token, just continue
            {
                continue;
            }
            if (!(Evaluator.IsInt(substrings[i]) || Evaluator.IsVar(substrings[i]) || Evaluator.IsOperator(substrings[i]))) //checking if the token is valid
            {
                throw new ArgumentException();
            }

            if ((substrings[i].Equals("-") || substrings[i].Equals("+") || substrings[i].Equals("*") || substrings[i].Equals("/")) && (i == 0)) //checking against leading operators
            {
                throw new ArgumentException();
            }

            if ((substrings[i].Equals("-")) && (Evaluator.IsOperator(substrings[i - 1]))) //checking against negative numbers
            {
                throw new ArgumentException();
            }
        }


        //Fields
        Stack<int> values = new Stack<int>();
        Stack<string> operations = new Stack<string>();

        //For each token
        for (int i = 0; i < substrings.Length; i++)
        {
            //Parantheses
            if (substrings[i].Equals("("))
            {
                operations.Push(substrings[i]);
            }

            if (substrings[i].Equals(")"))
            {
                if ((operations.Count != 0) && ((operations.Peek().Equals("+")) || (operations.Peek().Equals("-"))))
                {
                    try
                    {
                        //pop the value stack twice and the operator stack once.
                        int value2 = values.Pop();
                        int value1 = values.Pop();
                        string op = operations.Pop();

                        //Apply the popped operator to the popped numbers.

                        int result = Evaluator.DoOperator(value1, value2, op);

                        //Push the result onto the value stack.
                        values.Push(result);
                    }
                    catch (InvalidOperationException e)
                    {
                        throw new ArgumentException();
                    }
                }

                operations.Pop();

                if ((operations.Count != 0) && ((operations.Peek().Equals("*")) || (operations.Peek().Equals("/"))))
                {
                    try
                    {
                        //pop the value stack twice and the operator stack once.
                        int value2 = values.Pop();
                        int value1 = values.Pop();
                        string op = operations.Pop();

                        //Apply the popped operator to the popped numbers.

                        int result = Evaluator.DoOperator(value1, value2, op);

                        //Push the result onto the value stack.
                        values.Push(result);
                    }
                    catch (InvalidOperationException e)
                    {
                        throw new ArgumentException();
                    }
                }
            }

            //Operations
            if ((substrings[i].Equals("*")) || (substrings[i].Equals("/")))
            {
                operations.Push(substrings[i]);
            }

            if ((substrings[i].Equals("+")) || (substrings[i].Equals("-")))
            {
                if ((operations.Count != 0) && ((operations.Peek().Equals("+")) || (operations.Peek().Equals("-"))))
                {
                    try
                    {
                        //pop the value stack twice and the operator stack once.
                        int value2 = values.Pop();
                        int value1 = values.Pop();
                        string op = operations.Pop();

                        //Apply the popped operator to the popped numbers.

                        int result = Evaluator.DoOperator(value1, value2, op);

                        //Push the result onto the value stack.
                        values.Push(result);
                    } catch (InvalidOperationException e)
                    {
                        throw new ArgumentException();
                    }
                }

                operations.Push(substrings[i]);

            }

            //Operands

            if ((Evaluator.IsInt(substrings[i])) || (Evaluator.IsVar(substrings[i])))
            {
                if ((operations.Count != 0) && ((operations.Peek().Equals("*")) || (operations.Peek().Equals("/"))))
                {
                    int value2 = -1;
                    if (Evaluator.IsInt(substrings[i]))
                    {
                        value2 = Int32.Parse(substrings[i]);
                    }
                    else if (Evaluator.IsInt(substrings[i]))
                    {
                        value2 = variableEvaluator(substrings[i]);
                    }
                    //pop the value stack once and the operator stack once.
                    int value1 = values.Pop();
                    string op = operations.Pop();

                    //Apply the popped operator to the popped numbers.

                    int result = Evaluator.DoOperator(value1, value2, op);

                    //Push the result onto the value stack.
                    values.Push(result);
                }
                else
                {

                    if (Evaluator.IsInt(substrings[i]))
                    {
                        values.Push(int.Parse(substrings[i]));
                    }
                    else if (Evaluator.IsVar(substrings[i]))
                    {
                        values.Push(variableEvaluator(substrings[i]));
                    }

                }

            }

        }

        //after the last token has been processed
        if (operations.Count == 0) //if there is nothing in the operations stack
        {
            if (values.Count == 1)
            {
                return values.Pop();

            } else
            {
                throw new ArgumentException();
            }

        }
        else //if there is something in the operations stack
        {
            if (operations.Count !=1 || values.Count !=2 ||
                ((!operations.Peek().Equals("+")) || !(operations.Peek().Equals("-"))))
            {
                throw new ArgumentException();
            } else
            {
                try
                {
                    //pop the value stack twice and the operator stack once.
                    int value2 = values.Pop();
                    int value1 = values.Pop();
                    string op = operations.Pop();

                    //Apply the popped operator to the popped numbers.

                    int result = Evaluator.DoOperator(value1, value2, op);

                    //Push the result onto the value stack.
                    return result;
                }
                catch (InvalidOperationException e)
                {
                    throw new ArgumentException();
                }
            }

        }

    }

    /// <summary>
    /// Given 2 ints and an operator, it applies the operator to the two ints
    /// </summary>
    /// <param name="a">The first int</param>
    /// <param name="b">The second int</param>
    /// <param name="c">The operator</param>
    /// <returns>Returns the answer</returns>
    public static int DoOperator(int a, int b, string c)
    {
        int result = -1;
        switch (c)
        {
            case "+": //if the operator is +
                result = a + b;
                break;

            case "-"://if the operator is -
                result = a - b; 
                break;


            case "*"://if the operator is *
                result = a * b;
                break;

            case "/"://if the operator is /
                if (b == 0)
                {
                    throw new DivideByZeroException();
                }
                result = a / b;
                break;
        }

        return result;
    }

    /// <summary>
    /// Checks if the token is an operator
    /// </summary>
    /// <param name="s">The given token</param>
    /// <returns>Returns a boolean; true if it is an operator; false otherwise</returns>
    public static Boolean IsOperator(string s)
    {
        if (s.Equals("(") || s.Equals(")") ||
            s.Equals("+") || s.Equals("-") ||
            s.Equals("*") || s.Equals("/"))
        {
            //Console.WriteLine("issue1: true");
            return true;
        }
        else
        {
            // Console.WriteLine("issue1: false");

            return false;
        }

    }

    /// <summary>
    /// Checks if the token is a number
    /// </summary>
    /// <param name="s">The given token</param>
    /// <returns>Returns a boolean; true if its a number; false otherwise</returns>
    public static Boolean IsInt(string s)
    {
        try
        {
            int result = Int32.Parse(s);
        }
        catch (FormatException E)
        {

            return false;
        }

        return true;
    }

    /// <summary>
    /// This method checks if the given token is a valid variable
    /// </summary>
    /// <param name="s">Token to be checked</param>
    /// <returns>Returns a boolean; true if it is a valid variable, false otherwise</returns>
    public static Boolean IsVar(string s)
    {
        //if the token has less than 2 characters or if it starts with a digit
        if (s.Length < 2 || Char.IsDigit(s[0]))
        {

            return false;
        }

        //checks if there is any instance where a letter comes after a digit
        for (int i = 1; i < s.Length; i++)
        {

            if (Char.IsLetter(s[i]) && Char.IsDigit(s[i - 1]))
            {

                return false;
            }

            if (Char.IsLetter(s[i]) && Evaluator.IsOperator(s[i].ToString()))
            {
                return false;
            }
        }

        return true;
    }


}