// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to print unique solutions for the N-Queens problem.
// ------------------------------------------------------------------------------------------------
using static System.Console;
using System.Text;

internal class Program {
   private static void Main () {
      Solve (0);
      PrintBoard ();
   }

   static void Solve (int row) {
      if (row == size) {
         if (!IsDuplicate (board))
            solutions.Add ([.. board]);
         return;
      }
      for (int col = 0; col < size; col++) {
         if (IsSafe (row, col)) {
            board[row] = col;
            Solve (row + 1);
         }
      }
   }

   static bool IsSafe (int row, int col) {
      for (int i = 0; i < row; i++) {
         if (board[i] == col ||
             board[i] - i == col - row ||
             board[i] + i == col + row) {
            return false;
         }
      }
      return true;
   }

   static bool IsDuplicate (int[] soln) {
      for (int i = 0; i < 4; i++) {
         soln = Rotate (soln);
         if (Exists (soln)) return true;
         if (Exists ([.. soln.Select (x => size - 1 - x)])) return true;
         if (Exists ([.. soln.Reverse ()])) return true;
      }
      return false;

      bool Exists (int[] a) {
         for (int i = 0; i < solutions.Count; i++)
            if (solutions[i].SequenceEqual (a))
               return true;
         return false;
      }

      int[] Rotate (int[] r) {
         int[] temp = new int[size];
         for (int i = 0; i < size; i++)
            temp[r[i]] = size - 1 - i;
         return temp;
      }
   }

   static void PrintBoard () {
      OutputEncoding = Encoding.UTF8;
      int i = 0;
      foreach (var soln in solutions) {
         Clear ();
         WriteLine ($"Solution {++i}\n┏━━━┳━━━┳━━━┳━━━┳━━━┳━━━┳━━━┳━━━┓");
         for (int row = 0; row < size; row++) {
            Write ("┃");
            for (int col = 0; col < size; col++)
               Write ($" {(col == soln[row] ? "\u2655" : " ")} ┃");
            if (row < size - 1)
               Write ("\n┣━━━╋━━━╋━━━╋━━━╋━━━╋━━━╋━━━╋━━━┫\n");
         }
         Write ("\n┗━━━┻━━━┻━━━┻━━━┻━━━┻━━━┻━━━┻━━━┛\n");
         ReadKey ();
      }
   }

   const int size = 8; // size of the board

   static int[] board = new int[size]; // board representation

   static List<int[]> solutions = []; // list to store unique solutions
}