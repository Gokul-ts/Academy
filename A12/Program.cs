// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement the wordle game.
// ------------------------------------------------------------------------------------------------
using System.Text;
using static EState;
using static System.Console;
using static System.ConsoleColor;
using static System.ConsoleKey;

#region Class program -----------------------------------------------------------------------------
internal class Program {
   static void Main () => Wordle.Run ();
}
#endregion

#region Class Wordle ------------------------------------------------------------------------------
/// <summary>Class to implement the wordle game</summary>
static class Wordle {
   #region Methods --------------------------------------------------
   /// <summary>Runs the wordle game</summary>
   public static void Run () {
      OutputEncoding = Encoding.UTF8;
      CursorVisible = false;
      Initialize ();
      Display ();
      while (!sGameOver) {
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
         var (seedWords, valWords) = (GetWords (@"data\puzzle.txt"), GetWords (@"data\dict.txt"));
         if (seedWords.Length == 0 || valWords.Length == 0) throw new InvalidDataException ("File empty!");
         (sSeed, sValidWords) = (seedWords[new Random ().Next (0, seedWords.Length - 1)], valWords);
      } catch (Exception ex) {
         Write (ex.Message); ReadKey (true);
      }
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
               EXACT => Green,
               MISPLACED => Blue,
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
            _ when temp.Contains ((c, EXACT)) => Green,
            _ when temp.Contains ((c, MISPLACED)) => Blue,
            _ when temp.Contains ((c, ABSENT)) => DarkGray,
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
         sInputs.Add ((ch, HOLD));
         sWord += ch;
         return;
      }
      if (info.Key is Enter && sWord.Length == LEN) {
         if (sValidWords.Contains (sWord)) {
            Restructure ();
            sColored += LEN;
            sFound = sWord == sSeed;
            sGameOver = sFound || sColored / LEN == 6;
            if (sGameOver) return;
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
      for (int i = 0; i < LEN; i++)
         if (sWord[i] == sSeed[i]) result[i] = EXACT;
         else rem[sSeed[i]] = rem.GetValueOrDefault (sSeed[i]) + 1;
      for (int i = 0; i < LEN; i++)
         if (result[i] == default)
            if (rem.GetValueOrDefault (sWord[i]) > 0) {
               result[i] = MISPLACED;
               rem[sWord[i]]--;
            } else result[i] = ABSENT;
      sInputs.RemoveRange (sInputs.Count - LEN, LEN);
      sInputs.AddRange (sWord.Select ((c, i) => (c, result[i])));
   }

   // Prints result to the console
   static void PrintResult () {
      Line ();
      if (sFound) {
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
   static bool sGameOver, sFound;
   static string sSeed = string.Empty, sWord = string.Empty;
   static string[] sValidWords = [];
   #endregion

   #region Constants ------------------------------------------------
   const int LEN = 5;
   #endregion
}
#endregion

#region enum EState -------------------------------------------------------------------------------
enum EState { NONE, EXACT, MISPLACED, ABSENT, HOLD }
#endregion