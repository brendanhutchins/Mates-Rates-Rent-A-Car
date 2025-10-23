using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    /// <summary>
    ///
    /// Menu driven program that allows the user to navigate between 
    /// customer, fleet, and rental operations. The program can read
    /// CSV files and save instances of CRM and Fleet lists.
    /// 
    /// This class handles the search functionality of the program
    /// and includes three methods: ParseText, ShuntingYard, and 
    /// SearchVehicles. If all three methods are used, this class can
    /// take a string of text representing a user query and output
    /// a searched list of vehicles from the nonrented vehicles list
    /// that satisfy conditions for the user's query.
    /// 
    /// The user can use the symbols "(", ")", "\"" and the operators
    /// "AND" and "OR" to search. If the query item is more than one
    /// word, enclose that string with quotation marks. And example is
    /// the query "Fiero 2M4", where the user types out the quotations.
    ///
    /// This class is inspired by the AMS week 10 SimpleCalculator 
    /// activity as well as Shlomo Geva's RPN demonstration from the 
    /// week 9 lectures.
    /// 
    /// Author Brendan Hutchins June 2020
    ///
    ///
    /// </summary>
    class Search
    {
        // This method takes in user input and outputs a list of tokens representing infix
        // notation. This method is inspired largely by AMS week 10 SimpleCalculator
        public static List<Token> ParseText(string text)
        {
            List<Token> infixTokens = new List<Token>();
            string fixedText = "";
            string[] textArray;
            bool isQuote = false;

            if (text.Length > 0)
            {
                // Fix formatting of given string
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i].Equals('(') || text[i].Equals(')'))
                    {
                        fixedText += " ";
                        fixedText += text[i];
                        fixedText += " ";
                    }
                    else if (text[i].Equals('"'))
                    {
                        if (isQuote == false)
                        {
                            isQuote = true;
                        }
                        else if (isQuote == true)
                        {
                            isQuote = false;
                        }
                    }
                    else
                    {
                        if (text == "-1")
                        {
                            Menu.RentalMenu();
                        }
                        else if (isQuote)
                        {
                            if (!text[i].Equals(' '))
                            {
                                fixedText += text[i];
                            }
                        }
                        else
                        {
                            fixedText += text[i];
                        }
                    }
                }
                if (isQuote == true)
                {
                    throw new FormatException("Error: Mismatched quotation marks in query.");
                }
            }
            // From Shlomo's RPN Demonstration, replace quotation marks and extra spaces
            fixedText = fixedText.ToUpper();
            fixedText = fixedText.Replace("\"", "");
            textArray = fixedText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Output each phrase as a token, AND having higher precedence
            foreach (string phrase in textArray)
            {
                switch (phrase)
                {
                    case "(":
                        infixTokens.Add(new LeftParenthesisToken());
                        break;
                    case ")":
                        infixTokens.Add(new RightParenthesisToken());
                        break;
                    case "AND":
                        infixTokens.Add(new OperatorToken(phrase, 2));
                        break;
                    case "OR":
                        infixTokens.Add(new OperatorToken(phrase, 1));
                        break;
                    default:
                        infixTokens.Add(new OperandToken(phrase));
                        break;
                }
            }
            return infixTokens;
        }

        // Convert infix tokens to postfix format using ShuntingYard method
        // This method is inspired largely by AMS week 10 SimpleCalculator
        public static List<Token> ShuntingYard(List<Token> infixTokens)
        {
            List<Token> postfixTokens = new List<Token>();
            Stack<Token> operatorStack = new Stack<Token>();

            foreach (Token token in infixTokens)
            {
                if (token is OperandToken)
                {
                    postfixTokens.Add(token);
                }
                else if (token is OperatorToken)
                {
                    // Check if operatorStack token is of higher precedence
                    while (operatorStack.Any() && operatorStack.Peek() is OperatorToken
                        && ((OperatorToken)operatorStack.Peek()).Precedence >= ((OperatorToken)token).Precedence)
                    {
                        postfixTokens.Add(operatorStack.Pop());
                    }
                    operatorStack.Push(token);
                }
                else if (token is LeftParenthesisToken)
                {
                    operatorStack.Push(token);
                }
                else if (token is RightParenthesisToken)
                {
                    while (operatorStack.Count > 0 && !(operatorStack.Peek() is LeftParenthesisToken))
                    {
                        postfixTokens.Add(operatorStack.Pop());
                        if (operatorStack.Count == 0)
                            throw new ShuntingYardException("Error: Mismatched parenthesis in expression.");
                    }
                    if (operatorStack.Count == 0)
                        throw new ShuntingYardException("Error: Mismatched parenthesis in expression.");
                    operatorStack.Pop();
                }
                else
                    throw new Exception("Unrecognized token " + token.ToString());
            }
            while (operatorStack.Any())
            {
                if (operatorStack.Peek() is RightParenthesisToken)
                    throw new ShuntingYardException("Error: Mismatched parenthesis in expression.");
                postfixTokens.Add(operatorStack.Pop());
            }

            return postfixTokens;
        }

        // Search method from list of unrented vehicles to return list of matching vehicles.
        // This uses hashsets to track which vehicle registrations match the user's query.
        // This method is inspired largely from Shlomo Geva's RPN demonstration from the 
        // week 9 lectures.
        public static List<Vehicle> SearchVehicles(List<Token> postfixTokens, Fleet allVehicles)
        {
            // Create and instantiate a new empty Stack for result sets.
            Stack<Token> operatorStack = new Stack<Token>();
            OperandToken t1;
            OperandToken t2;
            OperandToken op;
            HashSet<string> hs;
            int idx;
            SortedList attributeSets = allVehicles.attributeSets;
            String[] temp = new string[] { };
            List<Vehicle> unrentedList = allVehicles.GetFleet(false);
            List<Vehicle> searchedList = new List<Vehicle>();

            foreach (Token token in postfixTokens)
            {
                if (token is OperatorToken)
                {
                    if (token.ToString() == "AND")
                    {
                        // pop two sets off the stack and apply Intersect
                        t1 = (OperandToken)operatorStack.Pop();
                        t2 = (OperandToken)operatorStack.Pop();
                        temp = t1.Attributes.ToArray<string>(); // copy the elements of the set t1.Attributes
                        hs = new HashSet<string>(temp); // make a deep copy of hs1
                        hs.IntersectWith(t2.Attributes);// apply the Intersect to the new set
                        op = t1;
                        op.Attributes = hs;
                        operatorStack.Push(op); // push a reference to a new set
                    }
                    else if (token.ToString() == "OR")
                    {
                        // pop two sets off the stack and apply Union
                        t1 = (OperandToken)operatorStack.Pop();
                        t2 = (OperandToken)operatorStack.Pop();
                        temp = t1.Attributes.ToArray<string>(); // copy the elements of the set hs1
                        hs = new HashSet<string>(temp); // make a deep copy of hs1
                        hs.UnionWith(t2.Attributes); // apply the Union to the new set
                        op = t1;
                        op.Attributes = hs;
                        operatorStack.Push(op); // push a reference to a new set
                    }
                }
                else
                {
                    // here if an operand
                    idx = attributeSets.IndexOfKey(token.ToString()); // identify attribute set
                    if (idx >= 0)
                    {
                        hs = (HashSet<string>)attributeSets.GetByIndex(idx);
                        ((OperandToken)token).Attributes = hs;
                        operatorStack.Push(token);
                    }
                    else
                    {
                        throw new FormatException("Invalid attribute " + token);
                    }
                }
            }
            if (operatorStack.Count == 1)
            {
                t1 = (OperandToken)operatorStack.Pop();

                // For each vehicle registration in final OperandToken attributes add vehicle to final list
                foreach(string item in t1.Attributes)
                {
                    Vehicle foundVehicle = allVehicles.GetVehicle(item);
                    // Only add vehicle to list if vehicle is unrented
                    if (unrentedList.Contains(foundVehicle))
                    {
                        searchedList.Add(foundVehicle);
                    }
                }
            }
            // Give full unrented vehicles list if user enters nothing
            else if (postfixTokens.Count == 0)
            {
                searchedList = unrentedList;
            }
            else
            {
                throw new Exception("Invalid query");
            }

            return searchedList;
        }
    }
}
