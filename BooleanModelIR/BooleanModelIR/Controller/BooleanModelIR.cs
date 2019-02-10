using System;
using System.Collections.Generic;
using System.Linq;

namespace BooleanModelIR
{
    public class BooleanModelIR
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("==[ Boolean Model ]==");
            Console.WriteLine("\nInformation: We use these 5 articles from Medium.com for demonstrating the Boolean Model");
            Console.WriteLine("(1) How To Teach A Cyborg");
            Console.WriteLine("(2) It’s Time To Fix Testing");
            Console.WriteLine("(3) Nobody Knows How To Learn A Language");
            Console.WriteLine("(4) The Cost of Working in Education");
            Console.WriteLine("(5) Why We Need More Testing, Not Less\n");
            Console.WriteLine("The result will show the retrieved document and similarity of Boolean Model");
            Console.WriteLine("\n------------------------------\n");

            //document detail contain readfile and filter index term document 
            TermDocumentMatrixModel documentMatrix = new Files().ReadFile();
            //setting term document matrix
            documentMatrix = new TermDocumentMatrix().SettingTermDocumentMatrix(documentMatrix);
            
            //Query 
            while (true)
            {
                Console.Write("Enter Boolean Query:");
                string query = Console.ReadLine().ToLower();
                List<string> result = new QueryInformation().ProcessingQuery(documentMatrix, query);
                
                //Display retrieved documents
                Console.WriteLine("\n\n=[ Retrieved Document ]=");
                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        Console.WriteLine("{0}", item);
                    }
                }
                else
                {
                    Console.WriteLine("*** Result Not Found ***");
                }

                //Display similarity
                Console.WriteLine("\n\n=[ Similarity ]=\n");

                for (int i = 1; i <= 5; i++)
                {
                    Console.Write("\tDoc " + i + "\t");
                }

                Console.Write("\n" + query);

                foreach(var item in documentMatrix.DocumentList)
                {
                    if (result.Contains(item.Name))
                    {
                        Console.Write("\t1\t");
                    }
                    else
                    {
                        Console.Write("\t0\t");
                    }
                }
                
                Console.WriteLine("\n\n------------------------------");
            }
        }
    }
}
