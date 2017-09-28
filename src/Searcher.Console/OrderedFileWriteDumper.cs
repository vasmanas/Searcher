using System.Collections.Generic;
using System.Linq;
using Searcher.Core;
using System.IO;

namespace Searcher.Console
{
    public class OrderedFileWriteDumper : IDumper
    {
        private readonly string logFileName;
        private readonly bool writeFoundLine;
        private readonly List<FileSearchResults> allResults;
        private bool isDisposing = false;

        public OrderedFileWriteDumper(string logFileName, bool writeFoundLine)
        {
            this.logFileName = logFileName;
            this.writeFoundLine = writeFoundLine;
            this.allResults = new List<FileSearchResults>();
        }

        public void Write(FileSearchResults foundLines)
        {
            this.allResults.Add(foundLines);
        }

        public void WriteRange(List<FileSearchResults> foundLines)
        {
            this.allResults.AddRange(foundLines);
        }

        public void Dispose()
        {
            if (this.isDisposing)
            {
                return;
            }

            this.isDisposing = true;

            using (var resultsFile = new StreamWriter(logFileName, false))
            {
                var ordered = this.allResults
                    .SelectMany(r => r.FoundedLines, (r, s) => new { r.FilePath, s.Keyword, s.LineNumber, s.LineText })
                    .OrderBy(r => r.Keyword)
                    .ThenBy(r => r.LineText)
                    .ToList();

                foreach (var foundLine in ordered)
                {
                    resultsFile.WriteLine($"{foundLine.FilePath}:{foundLine.Keyword}:{foundLine.LineNumber}");

                    if (this.writeFoundLine)
                    {
                        resultsFile.WriteLine(foundLine.LineText);
                    }
                }
            }
        }
    }
}
