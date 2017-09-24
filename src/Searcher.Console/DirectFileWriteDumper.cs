using Searcher.Core;
using System.IO;
using System.Collections.Generic;

namespace Searcher.Console
{
    public class DirectFileWriteDumper : IDumper
    {
        private readonly StreamWriter writer;
        private readonly bool writeFoundLine;
        private bool isDisposing = false;

        public DirectFileWriteDumper(string logFileName, bool writeFoundLine)
        {
            this.writer = new StreamWriter(logFileName, false);
            this.writeFoundLine = writeFoundLine;
        }

        public void Write(FileSearchResults results)
        {
            var filePath = results.FilePath;

            foreach (var foundLine in results.FoundedLines)
            {
                this.writer.WriteLine($"{filePath}:{foundLine.Keyword}:{foundLine.LineNumber}");

                if (this.writeFoundLine)
                {
                    this.writer.WriteLine(foundLine.LineText);
                }
            }
        }

        public void WriteRange(List<FileSearchResults> foundLines)
        {
            foreach (var foundLine in foundLines)
            {
                this.Write(foundLine);
            }
        }

        public void Dispose()
        {
            if (this.isDisposing)
            {
                return;
            }

            this.isDisposing = true;

            if (this.writer != null)
            {
                this.writer.Dispose();
            }
        }
    }
}
