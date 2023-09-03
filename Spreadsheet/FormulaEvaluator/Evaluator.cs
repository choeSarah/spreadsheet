using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;


namespace FormulaEvaluator;

public static class Evaluator
{
    public delegate int Lookup(String v);

    public static int Evaluate(String exp, Lookup variableEvaluator)
    {
        //remove all the white space in the expression
        String noWhiteSpace = Regex.Replace(exp, @"\s", "");
        //Console.WriteLine(noWhiteSpace);
        //turn the expression into tokens
        string[] substrings = Regex.Split(noWhiteSpace, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

        //check if the expression is valid

        for (int i=0; i<substrings.Length; i++)
        {
            if (string.IsNullOrEmpty(substrings[i]))
            {
                continue;
            }
            if (!(Evaluator.IsInt(substrings[i]) || Evaluator.IsVar(substrings[i]) || Evaluator.IsOperator(substrings[i])))
            {
                throw new ArgumentException();
            }

            if ((substrings[i].Equals("-")) && (i == 0))
            {
                throw new ArgumentException();
            }

            if ((substrings[i].Equals("-")) && (Evaluator.IsOperator(substrings[i-1])))
            {
                throw new ArgumentException();
            }
        }



        //stack values  (int)
        Stack<int> values = new Stack<int>();
        //stack operations (char)
        Stack<string> operations = new Stack<string>();
        //for every token:

        for (int i=0; i<substrings.Length; i++)
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
                    //pop the value stack twice and the operator stack once.
                    int value2 = values.Pop();
                    int value1 = values.Pop();
                    string op = operations.Pop();

                    //Apply the popped operator to the popped numbers.

                    int result = Evaluator.DoOperator(value1, value2, op);

                    //Push the result onto the value stack.
                    values.Push(result);
                }

                operations.Pop();

                if ((operations.Count !=0)&&((operations.Peek().Equals("*")) || (operations.Peek().Equals("/"))))
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
            }

            //Operations
            if ((substrings[i].Equals("*")) || (substrings[i].Equals("/"))) {
                operations.Push(substrings[i]);
            }

            if ((substrings[i].Equals("+")) || (substrings[i].Equals("-")))
            {
                if ((operations.Count != 0) && ((operations.Peek().Equals("+")) || (operations.Peek().Equals("-"))))
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

                operations.Push(substrings[i]);

            }

            //Operands

            if ((Evaluator.IsInt(substrings[i])) || (Evaluator.IsVar(substrings[i])))
            {
                if ( (operations.Count != 0) && ((operations.Peek().Equals("*")) || (operations.Peek().Equals("/"))))
                {
                    int value2 = -1;
                    if (Evaluator.IsInt(substrings[i]))
                    {
                        value2 = Int32.Parse(substrings[i]);
                    } else if (Evaluator.IsInt(substrings[i]))
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
                } else
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
        if (operations.Count == 0)
        {
            return values.Pop();

        } else
        {
            //pop the value stack once and the operator stack once.
            int value2 = values.Pop();

            int value1 = values.Pop();
            string op = operations.Pop();

            //Apply the popped operator to the popped numbers.

            int result = Evaluator.DoOperator(value1, value2, op);
            return result;

        }

    }

    public static int DoOperator(int a, int b, string c)
    {
        int result = -1;
        switch(c)
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
                result = a / b;
                break;
        }

        return result;
    }

    public static Boolean IsOperator(string s)
    {
        if (s.Equals("(") || s.Equals(")") ||
            s.Equals("+") || s.Equals("-") ||
            s.Equals("*") || s.Equals("/"))
        {
            //Console.WriteLine("issue1: true");
            return true;
        } else
        {
           // Console.WriteLine("issue1: false");

            return false;
        }

    }


    public static Boolean IsInt (string s)
    {
        try
        {
            int result = Int32.Parse(s);
        } catch (FormatException E) {
            //Console.WriteLine("issue2: false");

            return false;
        }
        //Console.WriteLine("issue2: true");

        return true;
    }

    public static Boolean IsVar(string s)
    {
        if (s.Length < 2)
        {
            //Console.WriteLine("issue3: false");

            return false;
        }

        if (Char.IsDigit(s[0]))
        {
            //Console.WriteLine("issue3: false");

            return false;
        }

        for (int i = 1; i < s.Length; i++)
        {

            if (Char.IsLetter(s[i]) && Char.IsDigit(s[i - 1]))
            {
                //Console.WriteLine("issue3: false");

                return false;
            }
        }
        //Console.WriteLine("issue3: true");

        return true;
    }


}

