// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to find all anagrams from an input text file and sort them based on count.
// ------------------------------------------------------------------------------------------------
using System.Reflection;
using static System.Console;

internal class Program {
   static void Main () {
      try {
         var words = LoadStrings ("A14.data.words.txt");
         var data = words.GroupBy (x => new string ([.. x.Order ()]))
                         .Where (x => x.Count () >= 2)
                         .Select (a => a.Order ().ToArray ())
                         .OrderByDescending (a => a.Length);
         foreach (var anagrams in data) WriteLine ($"{anagrams.Length} {string.Join (' ', anagrams)}");
      } catch (Exception e) {
         WriteLine (e.Message);
      }

      // Helper function to read a file 
      static string[] LoadStrings (string file) {
         using var stream = Assembly.GetExecutingAssembly ().GetManifestResourceStream (file);
         using var reader = new StreamReader (stream!);
         return reader.ReadToEnd ().Split ("\r\n");
      }
   }
}