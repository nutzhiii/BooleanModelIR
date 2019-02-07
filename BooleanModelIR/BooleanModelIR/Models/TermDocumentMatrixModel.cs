using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanModelIR
{
    public class TermDocumentMatrixModel
    {
        public Dictionary<string, List<DocumentDetail>> TermDocumentMatrixDic = new Dictionary<string, List<DocumentDetail>>();
        public List<string> IndexTermList { get; set; }
        public List<DocumentModel> DocumentList { get; set; }
    }


    public class DocumentDetail
    {
        public string Name { get; set; }
        public bool IsTermInDocument { get; set; }
    }
}
