using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Core.Tests
{
    [TestClass]
    public class OrderedFileWriteDumperTests
    {
        [TestMethod]
        public void OrderTest()
        {
            var allResults = new List<FileSearchResults>();

            var singleResult = new FileSearchResults("C:\\MocFile.txt");
            singleResult.FoundedLines.Add(new FoundLine("123", 1, "2017-09-27 18:16:04,180 [41] TRACE e470.Im"));
            singleResult.FoundedLines.Add(new FoundLine("123", 2, "2017-09-27 17:18:04,140 [43] TRACE e470.Ima"));
            singleResult.FoundedLines.Add(new FoundLine("123", 3, "2017-09-27 17:16:05,140 [44] TRACE e470.Im"));
            singleResult.FoundedLines.Add(new FoundLine("123", 4, "2017-09-27 17:18:04,180 [41] TRACE e470.Imb"));
            singleResult.FoundedLines.Add(new FoundLine("123", 5, "2017-09-27 18:16:04,140 [44] TRACE e470.Im"));

            allResults.Add(singleResult);

            var ordered = allResults
                .SelectMany(r => r.FoundedLines, (r, s) => new { r.FilePath, s.Keyword, s.LineNumber, s.LineText })
                .OrderBy(r => r.Keyword)
                .ThenBy(r => r.LineText)
                .ToList();

            Assert.AreEqual(3, ordered[0].LineNumber);
            Assert.AreEqual(2, ordered[1].LineNumber);
            Assert.AreEqual(4, ordered[2].LineNumber);
            Assert.AreEqual(5, ordered[3].LineNumber);
            Assert.AreEqual(1, ordered[4].LineNumber);
        }
    }
}
