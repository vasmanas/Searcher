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
            // -r ".\debug_results.txt" -f "C:\work\original" -s ".\find_keys.txt" -t 16 -l -v

            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                // consume Options instance properties
                if (options.Verbose)
                {
                    System.Console.WriteLine($"{nameof(options.ResultFileName)} - {options.ResultFileName}");
                    System.Console.WriteLine($"{nameof(options.SreachInFolder)} - {options.SreachInFolder}");
                    System.Console.WriteLine($"{nameof(options.SearchablesFileName)} - {options.SearchablesFileName}");
                    System.Console.WriteLine($"{nameof(options.ThreadCount)} - {options.ThreadCount}");
                    System.Console.WriteLine($"{nameof(options.WriteFoundLine)} - {options.WriteFoundLine}");
                    System.Console.WriteLine($"{nameof(options.WriteOrdered)} - {options.WriteOrdered}");                    
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

            var scanFileStorageFolderPath = options.SreachInFolder;

            if (!Directory.Exists(scanFileStorageFolderPath))
            {
                System.Console.WriteLine($"Folder doies not exitst `{scanFileStorageFolderPath}`");

                System.Console.ReadKey();

                return;
            }

            var allSearchables = File.ReadAllLines(options.SearchablesFileName);
            var search = new SlidingSearch(allSearchables);

            var scanFilePaths = Directory.EnumerateFiles(scanFileStorageFolderPath, "*.*", SearchOption.AllDirectories).ToList();
            var tasks = new List<Task<FileSearchResults>>();

            using (IDumper resultWriter =
                options.WriteOrdered ?
                (IDumper)new OrderedFileWriteDumper(options.ResultFileName, options.WriteFoundLine) :
                (IDumper)new DirectFileWriteDumper(options.ResultFileName, options.WriteFoundLine))
            {
                foreach (var scanFilePath in scanFilePaths)
                {
                    System.Console.WriteLine($"Working on: {scanFilePath}");

                    if (tasks.Count > options.ThreadCount)
                    {
                        Task.WaitAny(tasks.ToArray());
                    }

                    var r = CloseTasksGetResult(tasks);
                    resultWriter.WriteRange(r);

                    var runTask = search.Go(scanFilePath);
                    tasks.Add(runTask);
                }

                /// Wait for all tasks to finish
                while (tasks.Any())
                {
                    Task.WaitAny(tasks.ToArray());

                    var r = CloseTasksGetResult(tasks);
                    resultWriter.WriteRange(r);
                }
            }

            System.Console.WriteLine($"Finished");
            System.Console.ReadKey();
        }

        internal static List<FileSearchResults> CloseTasksGetResult(List<Task<FileSearchResults>> tasks)
        {
            var results = new List<FileSearchResults>();

            foreach (var task in tasks.Where(t => t.IsCompleted).ToList())
            {
                var taskResult = task.Result;

                if (taskResult.FoundedLines.Any())
                {
                    results.Add(taskResult);
                }

                tasks.Remove(task);
            }

            return results;
        }
    }
}
