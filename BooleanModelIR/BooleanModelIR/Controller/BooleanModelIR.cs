using System;
using System.Collections.Generic;
using System.Linq;

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
            //Display header
            new DisplayResult().DisplayHead(documentMatrix);

            //Query 
            while (true)
            {
                Console.Write("Enter Boolean Query:");
                string query = Console.ReadLine().ToLower();
                List<string> result = new QueryInformation().ProcessingQuery(documentMatrix, query);

                //Display body result
                new DisplayResult().DisplayBodyResult(documentMatrix, result, query);   
            }
        }
    }
}
