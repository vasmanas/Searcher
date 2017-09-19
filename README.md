# Searcher
Application for searching many strings in many files

# How to use

-r - result file path and name. Deafult - results.txt
-f - folder to search keys in
-s - search key file, each line is a separate key
-t - thread count to run in parallel. Default - 4
-v - verbose

Examples:
Searcher.Console.exe -r ".\debug_results.txt" -f "C:\logs\20170914" -s ".\searchables.txt" -t 16 -v
Searcher.Console.exe -f "C:\logs\20170914" -s ".\searchables.txt"