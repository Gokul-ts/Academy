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
         Dictionary<string, List<string>> data = [];
         foreach (var word in File.ReadAllLines (@"data\words.txt")) {
            string key = new ([.. word.Order ()]);
            if (data.TryGetValue (key, out var wList))
               wList.Add (word);
            else data.Add (key, []);
         }
         data = data.Where (a => a.Value.Count > 1).OrderByDescending (a => a.Value.Count).ToDictionary ();
         foreach (var val in data.Values) WriteLine ($"{val.Count} {string.Join (' ', val)}");
      } catch (Exception) {
         WriteLine ("Couldn't read file!!");
      }
   }
}