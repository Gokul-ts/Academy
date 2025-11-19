// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to build a frequency table for alphabets in a word list.
// ------------------------------------------------------------------------------------------------
using static System.Console;

#region class Program -----------------------------------------------------------------------------
internal class Program {
   #region Implementation -------------------------------------------
   static void Main () {
      try {
         var chars = File.ReadAllText (@"TData\words.txt");
         if (chars.Length == 0) {
            WriteLine ("No words found in the file.");
            return;
         }
         WriteLine ("Letter | Occurrences\n--------------------");
         int i = 0;
         foreach (var (ch, occur) in BuildTable (chars)) {
            if (i < 7) ForegroundColor = ConsoleColor.Green;
            WriteLine ($"{ch,-6} | {occur}");
            ResetColor ();
            i++;
         }
      } catch {
         WriteLine ("Error reading file!");
      }
   }
   #endregion

   #region Methods --------------------------------------------------
   // Builds a frequency table for alphabetic characters
   static Dictionary<char, int> BuildTable (string chars) {
      foreach (var ch in chars.ToUpper ())
         if (ch is >= 'A' and <= 'Z')
            if (!sFreqTable.TryAdd (ch, 1)) sFreqTable[ch]++;
      return sFreqTable.OrderByDescending (a => a.Value).ToDictionary ();
   }
   #endregion

   #region Private data ---------------------------------------------
   static Dictionary<char, int> sFreqTable = [];
   #endregion
}
#endregion