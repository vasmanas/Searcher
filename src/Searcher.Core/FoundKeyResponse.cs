using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Core
{
    public class FoundKeyResponse
    {
        public string FilePath { get; set; }

        public List<string> Keys { get; set; }
    }
}
