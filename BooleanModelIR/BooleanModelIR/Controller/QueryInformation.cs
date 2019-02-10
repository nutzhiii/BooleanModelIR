using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BooleanModelIR
{
    public class QueryInformation
    {
        Stack<List<string>> NotValuesStack = new Stack<List<string>>();
        Stack<List<string>> TempValuesStack = new Stack<List<string>>();

        public List<string> ProcessingQuery(TermDocumentMatrixModel termDocumentMatrix, string query)
        {
            List<string> result = BooleanExpressionStackProcessing(termDocumentMatrix, query);
            return result;
        }

        public List<string> BooleanExpressionStackProcessing(TermDocumentMatrixModel termDocumentMatrix, string query)
        {
            //case single word
            var splitQuery = query.Split(' ').ToList();
            if (splitQuery.Count() == 1)
            {
                List<string> resultSingleWord = new List<string>();
                if (query.ToCharArray().Contains('~'))
                {
                    string splitOperator = query.Replace("~", string.Empty);
                    if (termDocumentMatrix.TermDocumentMatrixDic.ContainsKey(splitOperator))
                    {
                        resultSingleWord = termDocumentMatrix.TermDocumentMatrixDic[splitOperator].Where(e => !e.Value).Select(e => e.Key).ToList();
                    }
                }
                else
                {
                    if (termDocumentMatrix.TermDocumentMatrixDic.ContainsKey(query))
                    {
                        resultSingleWord = termDocumentMatrix.TermDocumentMatrixDic[query].Where(e => e.Value).Select(e => e.Key).ToList();
                    }
                }
                return resultSingleWord;
            }
            //case multi word
            char[] tokens = query.ToCharArray();
            Stack<string> values = new Stack<string>();
            Stack<string> operation = new Stack<string>();
            Regex regex = new Regex(@"^[a-zA-Z0-9_-]*$");

            for (int i = 0; i < tokens.Length; i++)
            {
                //space
                if (tokens[i] == ' ')
                {
                    continue;
                }

                //value
                if (regex.IsMatch(tokens[i].ToString()))
                {
                    StringBuilder sbuf = new StringBuilder();
                    while (i < tokens.Length && regex.IsMatch(tokens[i].ToString()))
                    {
                        sbuf.Append(tokens[i++]);
                    }
                    if (!String.IsNullOrEmpty(sbuf.ToString()))
                    {
                        values.Push(sbuf.ToString().Trim());
                    }
                    i--;
                }
                else if (tokens[i] == '(')
                {
                    operation.Push(tokens[i].ToString());
                }
                else if (tokens[i] == ')')
                {
                    while (operation.Peek() != "(")
                    {
                        var operationPop = operation.Pop();
                        if (operationPop == "~")
                        {
                            values.Push(ComputeExpressionOfNot(values.Pop(), termDocumentMatrix));
                            //if (values.Count > 2) values.Push(ComputeExpression(operation.Pop(), values.Pop(), values.Pop(), termDocumentMatrix));
                        }
                        else
                        {
                            values.Push(ComputeExpression(operationPop, values.Pop(), values.Pop(), termDocumentMatrix));
                        }
                    }
                    operation.Pop();
                }
                else if (tokens[i] == '&' || tokens[i] == '|' || tokens[i] == '~')
                {
                    while (operation.Count > 0 && hasPrecedence(tokens[i], operation.Peek()))
                    {
                        var operationPop = operation.Pop();
                        if (operationPop == "~")
                        {
                            values.Push(ComputeExpressionOfNot(values.Pop(), termDocumentMatrix));
                            // if (values.Count > 2) values.Push(ComputeExpression(operation.Pop(), values.Pop(), values.Pop(), termDocumentMatrix));
                        }
                        else
                        {
                            values.Push(ComputeExpression(operationPop, values.Pop(), values.Pop(), termDocumentMatrix));
                        }
                    }
                    operation.Push(tokens[i].ToString());
                }
            }

            while (operation.Count > 0)
            {
                var operationPop = operation.Pop();
                if (operationPop == "~")
                {
                    values.Push(ComputeExpressionOfNot(values.Pop(), termDocumentMatrix));
                    //if (operation.Count > 2) values.Push(ComputeExpression(operation.Pop(), values.Pop(), values.Pop(), termDocumentMatrix));
                }
                else if (operationPop == "(")
                {
                    continue;
                }
                else
                {
                    values.Push(ComputeExpression(operationPop, values.Pop(), values.Pop(), termDocumentMatrix));
                }
            }
            return TempValuesStack.Pop();
        }

        //check op1 has high operation
        public bool hasPrecedence(char op1, string op2)
        {
            if (op2 == "(" || op2 == ")")
            {
                return false;
            }
            if ((op1 == '~' || op1 == '&') && (op2 == "|"))
            {
                return false;
            }
            if ((op2 == "&" || op2 == "|") && (op1 == '~'))
            {
                return false;
            }
            return true;
        }

        public string ComputeExpression(string operation, string value1, string value2, TermDocumentMatrixModel termDocumentMatrix)
        {
            string result = "yes";
            List<string> TempResult1 = new List<string>();
            List<string> TempResult2 = new List<string>();
            List<string> TempValue = new List<string>();

            //setting value for compute
            if (operation == "&" || operation == "|")
            {
                if (value1 == "~")
                {
                    TempResult1 = NotValuesStack.Pop();
                }
                else
                {
                    if (termDocumentMatrix.TermDocumentMatrixDic.ContainsKey(value1))
                    {
                        TempResult1 = termDocumentMatrix.TermDocumentMatrixDic[value1].Where(e => e.Value).Select(e => e.Key).ToList();
                    }
                    else if (value1 == "&" || value1 == "|")
                    {
                        TempResult1 = TempValuesStack.Pop();
                    }
                }

                if (value2 == "~")
                {
                    TempResult2 = NotValuesStack.Pop();
                }
                else
                {
                    if (termDocumentMatrix.TermDocumentMatrixDic.ContainsKey(value2))
                    {
                        TempResult2 = termDocumentMatrix.TermDocumentMatrixDic[value2].Where(e => e.Value).Select(e => e.Key).ToList();
                    }
                    else if (value2 == "&" || value2 == "|")
                    {
                        TempResult2 = TempValuesStack.Pop();
                    }
                }
                //end
            }

            //operation
            if (operation == "&")
            {
                TempValue = TempResult1.Intersect(TempResult2).ToList();
                TempValuesStack.Push(TempValue);
                result = "&";
            }
            else if (operation == "|")
            {
                TempValue = TempResult1.Union(TempResult2).ToList();
                TempValuesStack.Push(TempValue);
                result = "|";
            }
            //end 
            return result;
        }

        public string ComputeExpressionOfNot(string value, TermDocumentMatrixModel termdocumentmatrix)
        {
            if (value != "~")
            {
                if (termdocumentmatrix.TermDocumentMatrixDic.ContainsKey(value))
                {
                    NotValuesStack.Push(termdocumentmatrix.TermDocumentMatrixDic[value].Where(e => !e.Value).Select(e => e.Key).ToList());
                }
                else
                {
                    NotValuesStack.Push(null);
                }
            }
            return "~";
        }
    }
}
