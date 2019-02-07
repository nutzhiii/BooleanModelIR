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
                Console.Write("Enter Your Boolean Query:");
                string query = Console.ReadLine().ToLower();
                List<string> result = new QueryInformation().ProcessingQuery(documentMatrix, query);
                //Display results
                Console.WriteLine("------------------------------");
                if (result.Count != 0)
                {
                    foreach (var item in result)
                    {
                        Console.WriteLine("Document Name: {0}", item);
                    }
                }
                else
                {
                    Console.WriteLine("*** Not found Result!!! ***");
                }
                Console.WriteLine("------------------------------");
            }
        }
    }
}
