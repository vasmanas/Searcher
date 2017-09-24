using Searcher.Core;
using System;
using System.Collections.Generic;

namespace Searcher.Console
{
    public interface IDumper : IDisposable
    {
        void Write(FileSearchResults foundLines);

        void WriteRange(List<FileSearchResults> foundLines);
    }
}
