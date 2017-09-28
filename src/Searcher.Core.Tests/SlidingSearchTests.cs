using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Searcher.Core.Tests
{
    [TestClass]
    public class SlidingSearchTests
    {
        [TestMethod]
        [Ignore]
        public void Speed_Test()
        {
            var filePath = @"C:\logs\20170914\Service-Error.log";

            var timer = new Stopwatch();

            timer.Restart();

            var search = new SlidingSearch(new string[] { "TimeOut", "Timeout" });

            timer.Stop();
            Console.WriteLine(timer.Elapsed);

            timer.Restart();

            var task = search.Go(filePath);
            task.Wait();

            timer.Stop();
            Console.WriteLine(timer.Elapsed);
        }
    }
}
