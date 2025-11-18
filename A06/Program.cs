// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to print unique solutions for the N-Queens problem.
// ------------------------------------------------------------------------------------------------
using static System.Console;
using System.Text;

#region class Program  ----------------------------------------------------------------------------
internal class Program {
   #region Methods --------------------------------------------------
   static void Main () {
      GenSolutions (0);
      PrintBoard ();
   }

   /// <summary>Generates multiple solutions with unique queen positions</summary>
   static void GenSolutions (int row) {
      if (row == Size) {
         if (!IsDuplicate (sBoard)) sSolutions.Add ([.. sBoard]);
         return;
      }
      for (int col = 0; col < Size; col++) {
         if (IsSafe (row, col)) {
            sBoard[row] = col;
            GenSolutions (row + 1);
         }
      }
   }

   /// <summary>Checks whether the queen is safe to place in
   /// linear and diagonal directions</summary>
   static bool IsSafe (int row, int col) {
      for (int i = 0; i < row; i++) {
         if (sBoard[i] == col ||
             sBoard[i] - i == col - row ||
             sBoard[i] + i == col + row) {
            return false;
         }
      }
      return true;
   }

   /// <summary>Returns whether a duplicate solution exists or not</summary>
   static bool IsDuplicate (int[] soln) {
      for (int i = 0; i < 4; i++) {
         soln = Rotate (soln);
         // Checks if the solution already exists
         if (Exists (soln)) return true;
         // Checks for horizontal mirror duplicate solutions
         if (Exists ([.. soln.Select (x => Size - 1 - x)])) return true;
         // Checks for vertical mirror duplicate solutions
         if (Exists ([.. soln.Reverse ()])) return true;
      }
      return false;

      bool Exists (int[] arr) {
         for (int i = 0; i < sSolutions.Count; i++)
            if (sSolutions[i].SequenceEqual (arr)) return true;
         return false;
      }

      int[] Rotate (int[] arr) {
         int[] temp = new int[Size];
         for (int i = 0; i < Size; i++)
            temp[arr[i]] = Size - 1 - i;
         return temp;
      }
   }

   /// <summary>Prints the solution to the console</summary>
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
            if (row < Size - 1) Write ("\n┣━━━╋━━━╋━━━╋━━━╋━━━╋━━━╋━━━╋━━━┫\n");
         }
         Write ("\n┗━━━┻━━━┻━━━┻━━━┻━━━┻━━━┻━━━┻━━━┛\n");
         ReadKey (true);
      }
   }
   #endregion

   #region Private variables ----------------------------------------
   const int Size = 8; // size of the board

   static int[] sBoard = new int[Size]; // board representation

   static List<int[]> sSolutions = []; // list to store unique solutions
   #endregion
}
#endregion