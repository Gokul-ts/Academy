// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to find all anagrams from an input text file and sort them based on count.
// ------------------------------------------------------------------------------------------------
using static System.Console;

internal class Program {
   static void Main () {
      try {
         var words = LoadStrings (@"data\words.txt");
         var data = words.GroupBy (x => new string ([.. x.Order ()]))
                         .Where (x => x.Count () >= 2)
                         .Select (a => a.Order ().ToArray ())
                         .OrderByDescending (a => a.Length);
         foreach (var anagrams in data) WriteLine ($"{anagrams.Length} {string.Join (' ', anagrams)}");
      } catch (Exception) {
         WriteLine ("Couldn't read file!!");
      }

      // Helper function to read a file 
      string[] LoadStrings (string filename) => File.ReadAllLines (filename);
   }
}