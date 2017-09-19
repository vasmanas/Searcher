﻿using CommandLine;

namespace Searcher.Console
{
    internal class Options
    {
        [Option('r', "resultFileName", DefaultValue = "Results.txt", Required = true, HelpText = "Results file to write.")]
        public string ResultFileName { get; set; }

        [Option('f', "sreachInFolder", Required = true, HelpText = "Folder in which located files for search.")]
        public string SreachInFolder { get; set; }

        [Option('s', "searchablesFileName", Required = true, HelpText = "File from which search keys to take.")]
        public string SearchablesFileName { get; set; }

        [Option('t', "threadCount", DefaultValue = 4, Required = true, HelpText = "Number of threads to run in parallel.")]
        public int ThreadCount { get; set; }

        [Option('v', null, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
    }
}