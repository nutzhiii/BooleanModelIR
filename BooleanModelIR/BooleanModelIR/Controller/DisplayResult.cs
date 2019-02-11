using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanModelIR
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
                ConsoleTableBuilder.From(GetDataTableOfDocumentList(result))
                    .WithFormat(ConsoleTableBuilderFormat.Alternative).ExportAndWriteLine();
            }
            else
            {
                Console.WriteLine("*** Result Not Found ***\n");
            }

            //Display similarity
            Console.WriteLine("=[ Similarity ]=");

            ConsoleTableBuilder.From(GetDataTableOfSimilarity(result, documentMatrix, query))
                .WithFormat(ConsoleTableBuilderFormat.Alternative).ExportAndWriteLine();

            Console.WriteLine("------------------------------");
        }

        public DataTable GetDataTableOfDocumentList(List<string> documentList)
        {
            DataTable table = new DataTable();
            table.Columns.Add("***** Document List *****", typeof(string));

            foreach (var item in documentList)
            {
                table.Rows.Add(item);
            }
            return table;
        }

        public DataTable GetDataTableOfSimilarity(List<string> documentList, TermDocumentMatrixModel documentMatrix, string query)
        {
            DataTable table = new DataTable();
            //column
            table.Columns.Add("Query", typeof(string));
            string[] flagdocument = new string[documentMatrix.DocumentList.Count + 1];
            flagdocument[0] = query;
            for (int i = 0; i < documentMatrix.DocumentList.Count; i++)
            {
                table.Columns.Add(string.Format("Doc{0}", i + 1), typeof(string));
                if (documentList.Contains(documentMatrix.DocumentList[i].Name))
                {
                    flagdocument[i + 1] = "1";
                }
                else
                {
                    flagdocument[i + 1] = "0";
                }
            }
            table.Rows.Add(flagdocument);
            return table;
        }
    }
}
