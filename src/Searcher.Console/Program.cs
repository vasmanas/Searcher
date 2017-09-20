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
            /// -r ".\debug_results.txt" -f "C:\logs\20170914" -s ".\searchables.txt" -t 16 -v

            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                // consume Options instance properties
                if (options.Verbose)
                {
                    System.Console.WriteLine($"{nameof(options.ResultFileName)} - {options.ResultFileName}");
                    System.Console.WriteLine($"{nameof(options.SearchablesFileName)} - {options.SearchablesFileName}");
                    System.Console.WriteLine($"{nameof(options.SearchablesFileName)} - {options.SearchablesFileName}");
                    System.Console.WriteLine($"{nameof(options.ThreadCount)} - {options.ThreadCount}");
                    System.Console.WriteLine($"{nameof(options.WriteFoundLine)} - {options.WriteFoundLine}");
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
                var tasks = new List<Task<FileSearchResults>>();

                foreach (var logFilePath in logFilePaths)
                {
                    System.Console.WriteLine($"Working on: {logFilePath}");

                    if (tasks.Count > options.ThreadCount)
                    {
                        Task.WaitAny(tasks.ToArray());
                    }

                    WriteResult(tasks, resultsFile, options);

                    var runTask = search.Go(logFilePath);
                    tasks.Add(runTask);
                }

                while (tasks.Any())
                {
                    Task.WaitAny(tasks.ToArray());

                    WriteResult(tasks, resultsFile, options);
                }
            }

            System.Console.WriteLine($"The end");

            System.Console.ReadKey();
        }

        internal static void WriteResult(List<Task<FileSearchResults>> tasks, StreamWriter resultsFile, Options options)
        {
            foreach (var task in tasks.Where(t => t.IsCompleted).ToList())
            {
                var taskResult = task.Result;
                var filePath = taskResult.FilePath;

                if (!taskResult.FoundedLines.Any())
                {
                    tasks.Remove(task);
                }

                foreach (var foundLine in taskResult.FoundedLines)
                {
                    resultsFile.WriteLine($"{filePath}:{foundLine.Keyword}:{foundLine.LineNumber}");

                    if (options.WriteFoundLine)
                    {
                        resultsFile.WriteLine(foundLine.LineText);
                    }
                }

                resultsFile.Flush();

                tasks.Remove(task);
            }
        }
    }
}
