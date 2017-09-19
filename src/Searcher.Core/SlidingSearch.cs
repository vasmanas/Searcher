using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Searcher.Core
{
    public class SlidingSearch
    {
        private TreePart _conditionTree;

        public SlidingSearch(IEnumerable<string> keywords)
        {
            this._conditionTree = null;

            var builder = new TreeBuilder();
            foreach (var keyword in keywords)
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    continue;
                }

                this._conditionTree = builder.Create(this._conditionTree, keyword);
            }
        }

        public Task<FoundKeyResponse> Go(string filePath)
        {
            return Task<FoundKeyResponse>.Factory.StartNew(() =>
            {
                var allLines = File.ReadAllLines(filePath);

                var foundKeys = new List<string>();

                for (int i = 0; i < allLines.Length; i++)
                {
                    var line = allLines[i];

                    for (int j = 0; j < line.Length; j++)
                    {
                        // TODO: improve line substring is not good for performance
                        var startLength = this._conditionTree.IsSatisfiedDeepBy(line.Substring(j));

                        if (startLength == -1)
                        {
                            continue;
                        }

                        foundKeys.Add(line.Substring(j, startLength));

                        j += (startLength - 1);
                    }
                }

                var result = new FoundKeyResponse
                {
                    FilePath = filePath,
                    Keys = foundKeys.Distinct().ToList()
                };

                return result;
            });
        }
    }
}
