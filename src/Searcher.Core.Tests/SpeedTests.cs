using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Searcher.Core.Tests
{
    [TestClass]
    public class SpeedTests
    {
        //[TestMethod]
        public void Old_Way_Speed_Test()
        {
            var filePath = @"C:\logs\20170915\Service-Error.log";

            var timer = new Stopwatch();

            timer.Restart();

            var allLines = File.ReadAllLines(filePath);

            timer.Stop();
            Console.WriteLine(timer.Elapsed);

            timer.Restart();
            var searchForKeys = new List<string> { "101", "102" };

            timer.Stop();
            Console.WriteLine(timer.Elapsed);

            timer.Restart();
            var foundKeys = new List<string>();

            for (int i = 0; i < allLines.Length; i++)
            {
                var line = allLines[i];

                var localFoundKeys = searchForKeys.Where(k => line.Contains(k)).ToList();

                if (!localFoundKeys.Any())
                {
                    continue;
                }

                var removedCount = searchForKeys.RemoveAll(k => localFoundKeys.Contains(k));

                if (removedCount != localFoundKeys.Count)
                {
                    throw new Exception("Not matches");
                }

                foundKeys.AddRange(localFoundKeys);
            }

            timer.Stop();
            Console.WriteLine(timer.Elapsed);
        }
    }
}
