// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement the wordle game.
// ------------------------------------------------------------------------------------------------
using System.Text;
using static System.Console;
using static System.ConsoleKey;
using static System.ConsoleColor;

#region Class program -----------------------------------------------------------------------------
internal class Program {
   static void Main () => new Wordle ().Run ();
}
#endregion

#region Class Wordle ------------------------------------------------------------------------------
/// <summary>Class to implement the wordle game</summary>
class Wordle {
   #region Constructor ----------------------------------------------
   public Wordle () => this.mWord = this.mSeed = string.Empty;
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
         if (seedWords != null && valWords != null)
            (mSeed, mValidWords) = (seedWords.ToArray ()[new Random ().Next (0, 597)], [.. valWords]);
      } catch (Exception ex) {
         Write (ex.Message); ReadKey (true);
      }
   }

   // Displays the interface to the console
   void Display () {
      Clear ();
      int i = 0;
      foreach (var (ch, type) in inputs) {
         if (i % 5 == 0) Write ("\n\t");
         if (i < mColorCode) ForegroundColor = type switch {
            1 => Green,
            2 => Blue,
            _ => DarkGray,
         };
         Write ($"{ch} ");
         ResetColor ();
         i++;
      }
      while (i < 30) {
         if (i % 5 == 0) Write ("\n\t");
         Write ($"{(i == mPos ? '\u25cc' : '.')} ");
         i++;
      }
      WriteLine ($"\n{new string ('\u2500', 25)}\n");
      var temp = inputs.Take (mColorCode);
      for (int j = 1; j <= 26; j++) {
         char c = (char)(j + 64);
         ForegroundColor = temp switch {
            _ when temp.Contains ((c, 1)) => Green,
            _ when temp.Contains ((c, 2)) => Blue,
            _ when temp.Contains ((c, 3)) => DarkGray,
            _ => White
         };
         Write ($"{c}   ");
         if (j % 7 == 0) WriteLine ();
         ResetColor ();
      }
   }

   // Updates the game state
   void UpdateGame (ConsoleKeyInfo info) {
      char ch = char.ToUpper (info.KeyChar);
      if (ch is >= 'A' and <= 'Z' && mWord.Length != 5) {
         int status = 3;
         int idx = mSeed.IndexOf (ch);
         if (idx >= 0) status = (mPos % 5 == idx) ? 1 : 2;
         inputs.Add ((ch, status));
         mWord += ch;
         mPos++;
      }
      if (info.Key is Enter && mWord.Length == 5) {
         if (mValidWords.Contains (mWord)) {
            mTries++;
            mColorCode += 5;
            if (mWord == mSeed) { mGameOver = mFound = true; return; }
            mWord = string.Empty;
            if (mTries == 6) { mGameOver = true; return; }
         } else {
            PrintMsg (mWord);
            mWord = string.Empty;
            inputs.RemoveRange (mPos - 5, 5);
            mPos -= 5;
         }
      }
      if (info.Key is Backspace or Delete && mWord != "") {
         mPos--;
         inputs.RemoveRange (mPos, 1);
         mWord = (mWord.Length == 1) ? string.Empty : mWord[..(mPos % 5)];
      }
   }

   // Prints result to the console
   void PrintResult () {
      Write ($"\n\n{new string ('\u2500', 25)}\n");
      if (mFound) {
         ForegroundColor = Green;
         Write ($"\nYou found the word in {mTries} tries\n");
         ResetColor ();
      } else Write ($"\nSorry - the word was {mSeed}\n");
   }

   // Prints a message to the console
   void PrintMsg (string s) {
      Write ($"\n\n{new string ('\u2500', 25)}\n");
      ForegroundColor = Yellow;
      WriteLine ($"\n   {s} is not a word\n");
      ResetColor ();
      WriteLine ("Enter any key to continue");
      ReadKey (true);
   }
   #endregion

   #region Private data ---------------------------------------------
   List<(char, int)> inputs = [];
   int mPos, mTries, mColorCode;
   bool mGameOver, mFound;
   string mSeed, mWord;
   string[] mValidWords = [];
   #endregion
}
#endregion