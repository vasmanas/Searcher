namespace Searcher.Core
{
    public class FoundLine
    {
        public FoundLine(string keyword, int lineNumber, string lineText)
        {
            this.Keyword = keyword;
            this.LineNumber = lineNumber;
            this.LineText = lineText;
        }

        public string Keyword { get; private set; }

        public int LineNumber { get; private set; }

        public string LineText { get; private set; }
    }
}
