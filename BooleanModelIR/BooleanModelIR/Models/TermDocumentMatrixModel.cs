using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanModelIR
{
    public class TermDocumentMatrixModel
    {
        public Dictionary<string, Dictionary<string, bool>> TermDocumentMatrixDic = new Dictionary<string, Dictionary<string, bool>>();
        public List<string> IndexTermList { get; set; }
        public List<DocumentModel> DocumentList { get; set; }
    }
}
