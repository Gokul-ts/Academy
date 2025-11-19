// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to print unique solutions for the 8-Queens problem.
// ------------------------------------------------------------------------------------------------
using static System.Console;
using System.Text;

#region class Program  ----------------------------------------------------------------------------
/// <summary>Program class to generate and print unique solutions for 8-queens problem</summary>
internal class Program {
   static void Main () {
      GenSolutions (0);
      PrintBoard ();
   }

   #region Implementation -------------------------------------------
   // Generates multiple solutions with unique queen positions
   static void GenSolutions (int row) {
      if (row == Size) {
         if (!IsDuplicate (sBoard)) sSolutions.Add ([.. sBoard]);
         return;
      }
      for (int col = 0; col < Size; col++)
         if (IsSafe (row, col)) {
            sBoard[row] = col;
            GenSolutions (row + 1);
         }
   }

   // Checks whether the queen is safe to place in linear and diagonal directions
   static bool IsSafe (int row, int col) {
      for (int i = 0; i < row; i++)
         if (sBoard[i] == col ||
             sBoard[i] - i == col - row ||
             sBoard[i] + i == col + row)
            return false;
      return true;
   }

   // Returns whether a duplicate solution exists already or not
   static bool IsDuplicate (int[] soln) {
      for (int i = 0; i < 4; i++) {
         soln = Rotate (soln);
         if (Exists (soln) ||                             // Checks if the solution already exists
             Exists ([.. soln.Select (x => Last - x)]) || // Checks for horizontal mirror duplicates
             Exists ([.. soln.Reverse ()]))               // Checks for vertical mirror duplicates
            return true;
      }
      return false;

      // Helper function to compare two sequences
      bool Exists (int[] arr) {
         for (int i = 0; i < sSolutions.Count; i++)
            if (sSolutions[i].SequenceEqual (arr)) return true;
         return false;
      }

      // Helper function to rotate the solution
      int[] Rotate (int[] arr) {
         int[] temp = new int[Size];
         for (int i = 0; i < Size; i++)
            temp[arr[i]] = Last - i;
         return temp;
      }
   }

   // Prints all solutions to the console
   static void PrintBoard () {
      OutputEncoding = Encoding.UTF8;
      int i = 0;
      foreach (var soln in sSolutions) {
         Clear ();
         WriteLine ($"Solution {++i}\n┏━━━┳━━━┳━━━┳━━━┳━━━┳━━━┳━━━┳━━━┓");
         for (int row = 0; row < Size; row++) {
            Write ("┃");
            for (int col = 0; col < Size; col++)
               Write ($" {(col == soln[row] ? "\u2655" : " ")} ┃");
            if (row < Last) Write ("\n┣━━━╋━━━╋━━━╋━━━╋━━━╋━━━╋━━━╋━━━┫\n");
         }
         Write ("\n┗━━━┻━━━┻━━━┻━━━┻━━━┻━━━┻━━━┻━━━┛\n");
         ReadKey (true);
      }
   }
   #endregion

   #region Private data ---------------------------------------------
   const int Size = 8;                  // size of the board
   const int Last = Size - 1;           // index of last element
   static int[] sBoard = new int[Size]; // board representation
   static List<int[]> sSolutions = [];  // list to store unique solutions
   #endregion
}
#endregion