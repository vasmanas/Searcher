using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Searcher.Core.Tests
{
    [TestClass]
    public class SlidingSearchTests
    {
        //[TestMethod]
        public void Speed_Test()
        {
            //var filePath = @"C:\work\e470ProdLogs20170915\original\SFH1\E470-DMVManRequestAPI-Error.log";
            //var filePath = @"C:\work\e470ProdLogs20170915\original\SFH1\E470-DMVManRequestDiscovery-Info.log.3";
            var filePath = @"C:\work\e470ProdLogs\original\20170914\SFH3\E470-V1SensResponseAPI-Error.log";

            var timer = new Stopwatch();

            timer.Restart();

            //var search = new SlidingSearch(V1MissingWorkflowIds.Ids);
            // TODO: Search is case sensitive
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
