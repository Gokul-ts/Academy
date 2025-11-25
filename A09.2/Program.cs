// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement an expression evaluator.
// ------------------------------------------------------------------------------------------------
using static System.Console;
namespace Eval;

#region class Program -----------------------------------------------------------------------------
/// <summary>Class to get input from the user using a REPL and print the result</summary>
class Program {
   static void Main () {
      var eval = new Evaluator ();
      for (; ; ) {
         Write ("> ");
         string text = ReadLine () ?? "";
         if (text == "exit") break;
         try {
            double result = eval.Evaluate (text);
            ForegroundColor = ConsoleColor.Green;
            WriteLine (result);
         } catch (Exception e) {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine (e.Message);
         }
         ResetColor ();
      }
   }
}
#endregion