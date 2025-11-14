// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to build a frequency table for alphabets in a word list.
// ------------------------------------------------------------------------------------------------
using static System.Console;

internal class Program {
   private static void Main () {
      var words = File.ReadAllLines (@"C:\etc\words.txt");
      if (words.Length == 0) {
         WriteLine ("No words found in the file.");
         return;
      }
      var fTable = BuildTable (words);
      WriteLine ("Letter | Occurrences\n--------------------");
      int i = 0;
      foreach (var item in fTable) {
         if (i < 7) { ForegroundColor = ConsoleColor.Green; }
         WriteLine ($"{item.Key,-6} | {item.Value}");
         ResetColor ();
         i++;
      }
   }

   static Dictionary<char, int> BuildTable (string[] words) {
      foreach (var word in words)
         foreach (var ch in word)
            if (ch >= 'A' && ch <= 'Z')
               if (freqTable.TryGetValue (ch, out int value))
                  freqTable[ch] = ++value;
               else freqTable.Add (ch, 1);
      freqTable = freqTable.OrderByDescending (a => a.Value).ToDictionary (a => a.Key, a => a.Value);
      return freqTable;
   }

   static Dictionary<char, int> freqTable = [];
}