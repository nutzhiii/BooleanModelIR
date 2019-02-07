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
            Dictionary<string, List<DocumentDetail>> tempOfDocumentMatrix = new Dictionary<string, List<DocumentDetail>>();
            foreach (var item in documentMatrix.IndexTermList)
            {
                List<DocumentDetail> documentsDetail = new List<DocumentDetail>();
                foreach (var indexdocterm in documentMatrix.DocumentList)
                {
                    if (indexdocterm.IndexTermDocuments.Contains(item))
                    {
                        documentsDetail.Add(new DocumentDetail { IsTermInDocument = true, Name = indexdocterm.Name });
                    }
                    else
                    {
                        documentsDetail.Add(new DocumentDetail { IsTermInDocument = false, Name = indexdocterm.Name });
                    }
                }
                tempOfDocumentMatrix.Add(item, documentsDetail);
            }
            documentMatrix.TermDocumentMatrixDic = tempOfDocumentMatrix;
            return documentMatrix;
        }
    }
}
