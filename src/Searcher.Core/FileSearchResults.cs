using System.Collections.Generic;

namespace Searcher.Core
{
    public class FileSearchResults
    {
        public FileSearchResults(string filePath)
        {
            this.FilePath = filePath;
            this.FoundedLines = new List<FoundLine>();
        }

        public string FilePath { get; private set; }

        public List<FoundLine> FoundedLines { get; private set; }
    }
}
