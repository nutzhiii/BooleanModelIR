using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanModelIR
{
    public class TermDocumentMatrix
    {
        public TermDocumentMatrixModel SettingTermDocumentMatrix(TermDocumentMatrixModel documentMatrix)
        {
            Dictionary<string, Dictionary<string, bool>> tempOfDocumentMatrix = new Dictionary<string, Dictionary<string, bool>>();
            foreach (var item in documentMatrix.IndexTermList)
            {
                Dictionary<string, bool> documentDict = new Dictionary<string, bool>();
                foreach (var indexdocterm in documentMatrix.DocumentList)
                {
                    if (indexdocterm.IndexTermDocuments.Contains(item))
                    {
                        documentDict.Add(indexdocterm.Name, true);
                    }
                    else
                    {
                        documentDict.Add(indexdocterm.Name, false);
                    }
                }
                tempOfDocumentMatrix.Add(item, documentDict);
            }
            documentMatrix.TermDocumentMatrixDic = tempOfDocumentMatrix;
            return documentMatrix;
        }
    }
}
