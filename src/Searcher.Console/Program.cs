using CommandLine;
using Searcher.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /// -r ".\debug_results.txt" -f "C:\work\e470ProdLogs\original\20170914" -s ".\missingWorkflowIds_20170914.txt" -t 16 -v

            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                // consume Options instance properties
                if (options.Verbose)
                {
                    System.Console.WriteLine(options.ResultFileName);
                    System.Console.WriteLine(options.SearchablesFileName);
                    System.Console.WriteLine(options.SreachInFolder);
                    System.Console.WriteLine(options.ThreadCount);
                }
                else
                {
                    System.Console.WriteLine("working ...");
                }
            }
            else
            {
                //// Display the default usage information
                //System.Console.WriteLine(options.GetUsage());

                return;
            }

            var logFileStorageFolderPath = options.SreachInFolder;

            if (!Directory.Exists(logFileStorageFolderPath))
            {
                System.Console.WriteLine($"Folder doies not exitst `{logFileStorageFolderPath}`");

                System.Console.ReadKey();

                return;
            }

            var allSearchables = File.ReadAllLines(options.SearchablesFileName);

            var search = new SlidingSearch(allSearchables);

            var logFilePaths = Directory.EnumerateFiles(logFileStorageFolderPath, "*.*", SearchOption.AllDirectories).ToList();

            var logFileName = options.ResultFileName;

            using (var resultsFile = new StreamWriter(logFileName, false))
            {
                var tasks = new List<Task<FoundKeyResponse>>();

                foreach (var logFilePath in logFilePaths)
                {
                    System.Console.WriteLine($"Working on: {logFilePath}");

                    if (tasks.Count > options.ThreadCount)
                    {
                        Task.WaitAny(tasks.ToArray());
                    }

                    WriteResult(tasks, resultsFile);

                    var runTask = search.Go(logFilePath);
                    tasks.Add(runTask);
                }

                while (tasks.Any())
                {
                    Task.WaitAny(tasks.ToArray());

                    WriteResult(tasks, resultsFile);
                }
            }

            System.Console.WriteLine($"The end");

            System.Console.ReadKey();
        }

        public static void WriteResult(List<Task<FoundKeyResponse>> tasks, StreamWriter resultsFile)
        {
            foreach (var task in tasks.Where(t => t.IsCompleted).ToList())
            {
                var taskResult = task.Result;
                var filePath = taskResult.FilePath;

                foreach (var key in taskResult.Keys)
                {
                    resultsFile.WriteLine($"{filePath}:{key}");
                }

                tasks.Remove(task);
            }
        }
    }
}
