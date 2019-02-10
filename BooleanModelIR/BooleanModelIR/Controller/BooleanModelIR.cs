using System;
using System.Collections.Generic;
namespace BooleanModelIR
{
    public class BooleanModelIR
    {
        public static void Main(string[] args)
        {
            //document detail contain readfile and filter index term document 
            TermDocumentMatrixModel documentMatrix = new Files().ReadFile();
            //setting term document matrix
            documentMatrix = new TermDocumentMatrix().SettingTermDocumentMatrix(documentMatrix);
            //Query 
            while (true)
            {
                Console.Write("enter your boolean query:");
                string query = Console.ReadLine().ToLower();
                List<string> result = new QueryInformation().ProcessingQuery(documentMatrix, query);
                //display results
                Console.WriteLine("------------------------------");
                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        Console.WriteLine("document name: {0}", item);
                    }
                }
                else
                {
                    Console.WriteLine("*** not found result!!! ***");
                }
                Console.WriteLine("------------------------------");
            }
        }
    }
}
