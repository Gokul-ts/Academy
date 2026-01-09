// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement the wordle game.
// ------------------------------------------------------------------------------------------------
using System.Reflection;
using System.Text;
using static EState;
using static System.Console;
using static System.ConsoleColor;
using static System.ConsoleKey;

#region class Program -----------------------------------------------------------------------------
internal class Program {
   static void Main () => Wordle.Run ();
}
#endregion

#region class Wordle ------------------------------------------------------------------------------
/// <summary>Class to implement the wordle game</summary>
static class Wordle {
   #region Methods --------------------------------------------------
   /// <summary>Runs the wordle game</summary>
   public static void Run () {
      OutputEncoding = Encoding.UTF8;
      CursorVisible = false;
      Initialize ();
      Display ();
      while (!siGameOver) {
         ConsoleKeyInfo key = ReadKey (true);
         UpdateGame (key);
         Display ();
      }
      PrintResult ();
   }
   #endregion

   #region Implementation -------------------------------------------
   // Initializes the words for seed and valid words
   static void Initialize () {
      try {
         var (seedWords, valWords) = (LoadStrings ("puzzle.txt"), LoadStrings ("dict.txt"));
         (sSeed, sValidWords) = (seedWords[new Random ().Next (0, seedWords.Length - 1)], valWords);
      } catch (Exception ex) {
         Write ($"Error reading files!! {ex.Message}"); Environment.Exit (0);
      }
   }

   // Collects words from text files
   static string[] LoadStrings (string file) {
      using var stream = Assembly.GetExecutingAssembly ().GetManifestResourceStream ($"A12.data.{file}");
      using var reader = new StreamReader (stream!);
      return reader.ReadToEnd ().Split ("\r\n");
   }

   // Reads and returns the data from a file
   static string[] GetWords (string path) => File.ReadAllLines (path);

   // Displays the interface to the console
   static void Display () {
      Clear ();
      int total = 6 * LEN, alpha = 26, rowSize = 7;
      for (int i = 0; i < total; i++) {
         if (i % LEN == 0) Write ("\n\t");
         if (i < sInputs.Count) {
            var (ch, type) = sInputs[i];
            if (i < sColored) ForegroundColor = type switch {
               Exact => Green,
               Misplaced => Blue,
               _ => DarkGray,
            };
            Write ($"{ch} ");
            ResetColor ();
         } else Write ($"{(i == sInputs.Count ? '\u25cc' : '.')} ");
      }
      Line ();
      var temp = sInputs.Take (sColored);
      for (int j = 1; j <= alpha; j++) {
         char c = (char)(j + 64);
         ForegroundColor = temp switch {
            _ when temp.Contains ((c, Exact)) => Green,
            _ when temp.Contains ((c, Misplaced)) => Blue,
            _ when temp.Contains ((c, Absent)) => DarkGray,
            _ => White
         };
         Write ($"{c}   ");
         if (j % rowSize == 0) WriteLine ();
         ResetColor ();
      }
   }

   // Draws a separation line
   static void Line () => WriteLine ($"\n{new string ('\u2500', 25)}\n");

   // Updates the game state
   static void UpdateGame (ConsoleKeyInfo info) {
      char ch = char.ToUpper (info.KeyChar);
      if (ch is >= 'A' and <= 'Z' && sWord.Length != LEN) {
         sInputs.Add ((ch, Hold));
         sWord += ch;
         return;
      }
      if (info.Key is Enter && sWord.Length == LEN) {
         if (sValidWords.Contains (sWord)) {
            Restructure ();
            sColored += LEN;
            siFound = sWord == sSeed;
            siGameOver = siFound || sColored / LEN == 6;
            if (siGameOver) return;
         } else {
            PrintMsg (sWord);
            sInputs.RemoveRange (sInputs.Count - LEN, LEN);
         }
         sWord = string.Empty;
         return;
      }
      if (info.Key is Backspace or Delete && sWord.Length > 0) {
         sInputs.RemoveAt (sInputs.Count - 1);
         sWord = sWord[..^1];
         return;
      }
   }

   // Restructures the list
   static void Restructure () {
      var rem = new Dictionary<char, int> ();
      var result = new EState[LEN];
      for (int i = 0; i < LEN; i++) {
         var s = sSeed[i];
         if (sWord[i] == s) result[i] = Exact;
         else rem[s] = rem.GetValueOrDefault (s) + 1;
      }
      for (int i = 0; i < LEN; i++) {
         var w = sWord[i];
         ref var r = ref result[i];
         if (r == EState.None)
            if (rem.GetValueOrDefault (w) > 0) {
               r = Misplaced;
               rem[w]--;
            } else r = Absent;
      }
      sInputs.RemoveRange (sInputs.Count - LEN, LEN);
      sInputs.AddRange (sWord.Select ((c, i) => (c, result[i])));
   }

   // Prints result to the console
   static void PrintResult () {
      Line ();
      if (siFound) {
         ForegroundColor = Green;
         WriteLine ($"You found the word in {sColored / LEN} tries");
         ResetColor ();
      } else WriteLine ($"Sorry - the word was {sSeed}");
   }

   // Prints a message to the console
   static void PrintMsg (string s) {
      Line ();
      ForegroundColor = Yellow;
      WriteLine ($"   {s} is not a word");
      ResetColor ();
      WriteLine ("Enter any key to continue");
      ReadKey (true);
   }
   #endregion

   #region Private data ---------------------------------------------
   static List<(char, EState)> sInputs = [];
   static int sColored;
   static bool siGameOver, siFound;
   static string sSeed = string.Empty, sWord = string.Empty;
   static string[] sValidWords = [];
   #endregion

   #region Constants ------------------------------------------------
   const int LEN = 5;
   #endregion
}
#endregion

#region enum EState -------------------------------------------------------------------------------
enum EState { None, Exact, Misplaced, Absent, Hold }
#endregion