// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement the wordle game.
// ------------------------------------------------------------------------------------------------
using System.Text;
using static System.Console;
using static System.ConsoleColor;
using static System.ConsoleKey;

#region Class program -----------------------------------------------------------------------------
internal class Program {
   static void Main () => new Wordle ().Run ();
}
#endregion

#region Class Wordle ------------------------------------------------------------------------------
/// <summary>Class to implement the wordle game</summary>
class Wordle {
   #region Constructor ----------------------------------------------
   public Wordle () => mWord = mSeed = string.Empty;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Runs the wordle game</summary>
   public void Run () {
      OutputEncoding = Encoding.UTF8;
      CursorVisible = false;
      Initialize ();
      Display ();
      while (!mGameOver) {
         ConsoleKeyInfo key = ReadKey (true);
         UpdateGame (key);
         Display ();
      }
      PrintResult ();
   }
   #endregion

   #region Implementation -------------------------------------------
   // Initializes the words for seed and valid words
   void Initialize () {
      try {
         var (seedWords, valWords) = (File.ReadLines (@"data\puzzle.txt"), File.ReadLines (@"data\dict.txt"));
         int max = seedWords.Count () - 1;
         if (seedWords != null && valWords != null)
            (mSeed, mValidWords) = (seedWords.ToArray ()[new Random ().Next (0, max)], [.. valWords]);
      } catch (Exception ex) {
         Write (ex.Message); ReadKey (true);
      }
   }

   // Displays the interface to the console
   void Display () {
      Clear ();
      int total = 6 * LEN, alpha = 26, rowSize = 7;
      for (int i = 0; i < total; i++) {
         if (i % LEN == 0) Write ("\n\t");
         if (i < mInputs.Count) {
            var (ch, type) = mInputs[i];
            if (i < mColored) ForegroundColor = type switch {
               1 => Green,
               2 => Blue,
               _ => DarkGray,
            };
            Write ($"{ch} ");
            ResetColor ();
         } else Write ($"{(i == mPos ? '\u25cc' : '.')} ");
      }
      Line ();
      var temp = mInputs.Take (mColored);
      for (int j = 1; j <= alpha; j++) {
         char c = (char)(j + 64);
         ForegroundColor = temp switch {
            _ when temp.Contains ((c, 1)) => Green,
            _ when temp.Contains ((c, 2)) => Blue,
            _ when temp.Contains ((c, 3)) => DarkGray,
            _ => White
         };
         Write ($"{c}   ");
         if (j % rowSize == 0) WriteLine ();
         ResetColor ();
      }
   }

   // Draws a separation line
   void Line () => WriteLine ($"\n{new string ('\u2500', 25)}\n");

   // Updates the game state
   void UpdateGame (ConsoleKeyInfo info) {
      char ch = char.ToUpper (info.KeyChar);
      if (ch is >= 'A' and <= 'Z' && mWord.Length != LEN) {
         mInputs.Add ((ch, 4));
         mWord += ch;
         mPos++;
         return;
      }
      if (info.Key is Enter && mWord.Length == LEN) {
         if (mValidWords.Contains (mWord)) {
            Restructure ();
            mColored += LEN;
            mFound = mWord == mSeed;
            mGameOver = mFound || mColored / LEN == 6;
            if (mGameOver) return;
         } else {
            PrintMsg (mWord);
            mInputs.RemoveRange (mPos - LEN, LEN);
            mPos -= LEN;
         }
         mWord = string.Empty;
         return;
      }
      if (info.Key is Backspace or Delete && mWord.Length > 0) {
         mInputs.RemoveAt (--mPos);
         mWord = mWord[..(mPos % LEN)];
         return;
      }
   }

   // Returns the type of character
   void Restructure () {
      var rem = new Dictionary<char, int> ();
      var result = new int[LEN];
      for (int i = 0; i < LEN; i++)
         if (mWord[i] == mSeed[i]) result[i] = 1;
         else rem[mSeed[i]] = rem.GetValueOrDefault (mSeed[i]) + 1;
      for (int i = 0; i < LEN; i++)
         if (result[i] == default)
            if (rem.GetValueOrDefault (mWord[i]) > 0) {
               result[i] = 2;
               rem[mWord[i]]--;
            } else result[i] = 3;
      mInputs.RemoveRange (mPos - LEN, LEN);
      mInputs.AddRange (mWord.Select ((c, i) => (c, result[i])));
   }

   // Prints result to the console
   void PrintResult () {
      Line ();
      if (mFound) {
         ForegroundColor = Green;
         WriteLine ($"You found the word in {mColored / LEN} tries");
         ResetColor ();
      } else WriteLine ($"Sorry - the word was {mSeed}");
   }

   // Prints a message to the console
   void PrintMsg (string s) {
      Line ();
      ForegroundColor = Yellow;
      WriteLine ($"   {s} is not a word");
      ResetColor ();
      WriteLine ("Enter any key to continue");
      ReadKey (true);
   }
   #endregion

   #region Private data ---------------------------------------------
   List<(char, int)> mInputs = [];
   int mPos, mColored;
   bool mGameOver, mFound;
   string mSeed, mWord;
   string[] mValidWords = [];
   #endregion

   #region Constants ------------------------------------------------
   const int LEN = 5;
   #endregion
}
#endregion