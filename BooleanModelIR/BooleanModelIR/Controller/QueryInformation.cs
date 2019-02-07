using System.Collections.Generic;
using System.Linq;

namespace BooleanModelIR
{
    public class QueryInformation
    {
        public List<string> ProcessingQuery(TermDocumentMatrixModel termDocumentMatrix, string query)
        {
            string[] booleanOperator = new string[] { "and", "or", "not" };
            List<string> splitQuery = query.Split(' ').ToList();
            List<string> result = new List<string>();

            if (splitQuery.Count() == 1)
            {
                //have only single word query
                if (termDocumentMatrix.TermDocumentMatrixDic.Keys.Contains(splitQuery[0]))
                {
                    result = termDocumentMatrix.TermDocumentMatrixDic[splitQuery[0]].Where(e => e.IsTermInDocument).Select(x => x.Name).ToList();
                }
            }
            else
            {
                //more single word query
                List<DocumentDetail> tempResult = new List<DocumentDetail>();
                int count = 0;
                foreach (var item in splitQuery)
                {
                    if (booleanOperator.Contains(item))
                    {
                        if (item.Equals("and") && count != 0)
                        {
                            if (termDocumentMatrix.TermDocumentMatrixDic.Keys.Contains(splitQuery[count - 1]) && termDocumentMatrix.TermDocumentMatrixDic.Keys.Contains(splitQuery[count + 1]))
                            {
                                var indexPre = termDocumentMatrix.TermDocumentMatrixDic[splitQuery[count - 1]].Where(e => e.IsTermInDocument);
                                var indexNext = termDocumentMatrix.TermDocumentMatrixDic[splitQuery[count + 1]].Where(e => e.IsTermInDocument);
                                var resultOfOperationAnd = indexPre.Where(e1 => indexNext.Any(e2 => e1.Name.Equals(e2.Name))).ToList();
                                if (tempResult.Count() != 0)
                                {
                                    tempResult = tempResult.Where(t1 => resultOfOperationAnd.Any(t2 => t1.Name.Equals(t2.Name))).ToList();
                                }
                                else
                                {
                                    tempResult = resultOfOperationAnd;
                                }
                            }
                        }
                        else if (item.Equals("or") && count != 0)
                        {
                            if (termDocumentMatrix.TermDocumentMatrixDic.Keys.Contains(splitQuery[count - 1]) && termDocumentMatrix.TermDocumentMatrixDic.Keys.Contains(splitQuery[count + 1]))
                            {
                                var indexPre = termDocumentMatrix.TermDocumentMatrixDic[splitQuery[count - 1]].Where(e => e.IsTermInDocument);
                                var indexNext = termDocumentMatrix.TermDocumentMatrixDic[splitQuery[count + 1]].Where(e => e.IsTermInDocument);
                                if (tempResult.Count() != 0)
                                {
                                    foreach (var iP in indexNext)
                                    {
                                        if (tempResult.Where(e => e.Name.Equals(iP.Name)).Count() == 0) tempResult.Add(iP);
                                    }
                                }
                                else
                                {
                                    foreach (var iP in indexPre)
                                    {
                                        if (tempResult.Where(e => e.Name.Equals(iP.Name)).Count() == 0) tempResult.Add(iP);
                                    }

                                    foreach (var iN in indexNext)
                                    {
                                        if (tempResult.Where(e => e.Name.Equals(iN.Name)).Count() == 0) tempResult.Add(iN);
                                    }
                                }
                            }
                        }
                        else if (item.Equals("not"))
                        {
                            //enter not is first word 
                            if (count == 0)
                            {
                                if (termDocumentMatrix.TermDocumentMatrixDic.Keys.Contains(splitQuery[count + 1]))
                                {
                                    var indexNext = termDocumentMatrix.TermDocumentMatrixDic[splitQuery[count + 1]].Where(e => !e.IsTermInDocument);
                                    tempResult = indexNext.ToList();
                                }
                            }
                            else
                            {
                                if (termDocumentMatrix.TermDocumentMatrixDic.Keys.Contains(splitQuery[count - 1]) && termDocumentMatrix.TermDocumentMatrixDic.Keys.Contains(splitQuery[count + 1]))
                                {
                                    var indexPre = termDocumentMatrix.TermDocumentMatrixDic[splitQuery[count - 1]].Where(e => e.IsTermInDocument);
                                    var indexNext = termDocumentMatrix.TermDocumentMatrixDic[splitQuery[count + 1]].Where(e => !e.IsTermInDocument);
                                    if (tempResult.Count() != 0)
                                    {
                                        tempResult = tempResult.Where(e1 => indexNext.Any(e2 => e1.Name.Equals(e2.Name))).ToList();
                                    }
                                    else
                                    {
                                        tempResult = indexPre.Where(e1 => indexNext.Any(e2 => e1.Name.Equals(e2.Name))).ToList();
                                    }
                                }
                            }
                        }
                    }
                    count = count + 1;
                }
                result = tempResult.Select(e => e.Name).ToList();
            }
            return result;
        }
    }
}
