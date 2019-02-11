using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanModelIR.Controller
{
    class DisplayResult
    {
        public void DisplayHead(TermDocumentMatrixModel documentMatrixModel)
        {
            Console.WriteLine("==[ Boolean Model ]==");
            Console.WriteLine(string.Format("\nInformation: We use these {0} articles from Medium.com for demonstrating the Boolean Model", documentMatrixModel.DocumentList.Count));
            for (int i = 0; i < documentMatrixModel.DocumentList.Count; i++)
            {
                Console.WriteLine(string.Format("({0}) {1}", i + 1, documentMatrixModel.DocumentList[i].Name));
            }
            Console.WriteLine("The result will show the retrieved document and similarity of Boolean Model");
            Console.WriteLine("\n------------------------------\n");
        }

        public void DisplayBodyResult(TermDocumentMatrixModel documentMatrix, List<string> result, string query)
        {
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
            Console.Write("\t\t");

            for (int i = 0; i < documentMatrix.DocumentList.Count; i++)
            {
                Console.Write("\tDoc " + (i + 1) + "\t");
            }

            Console.WriteLine("\n" + query);
            Console.Write("\t");

            foreach (var item in documentMatrix.DocumentList)
            {
                if (result.Contains(item.Name))
                {
                    Console.Write("\t\t1");
                }
                else
                {
                    Console.Write("\t\t0");
                }
            }

            Console.WriteLine("\n\n------------------------------\n");
        }
    }
}
