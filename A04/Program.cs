// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to build a frequency table for alphabets in a word list.
// ------------------------------------------------------------------------------------------------
using static System.Console;

internal class Program {
   static void Main () {
      var chars = File.ReadAllText (@"TData\words.txt");
      if (chars.Length == 0) {
         WriteLine ("No words found in the file.");
         return;
      }
      var fTable = BuildTable (chars);
      WriteLine ("Letter | Occurrences\n--------------------");
      int i = 0;
      foreach (var item in fTable) {
         if (i < 7) ForegroundColor = ConsoleColor.Green;
         WriteLine ($"{item.Key,-6} | {item.Value}");
         ResetColor ();
         i++;
      }
   }

   static Dictionary<char, int> BuildTable (string chars) {
      foreach (var ch in chars.ToUpper ())
         if (ch is >= 'A' and <= 'Z')
            if (!freqTable.TryAdd (ch, 1)) freqTable[ch]++;
      return freqTable.OrderByDescending (a => a.Value).ToDictionary ();
   }

   static Dictionary<char, int> freqTable = [];
}