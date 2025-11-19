// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement a clone of double.TryParse method.
// ------------------------------------------------------------------------------------------------
using static System.Console;

internal class Program {
   static void Main () {
      for (; ; ) {
         WriteLine ("Enter [T] to run test case or any other key to continue: ");
         switch (ReadKey (true).Key) {
            case ConsoleKey.T: {
                  var data = new Dictionary<string, double> { { "0.1", 0.1 }, { "2.5e-4", 0.00025 },
                  { "-15.035e-1", -1.5035 }, { "-+1.5", 0 }, { "0.9e-+1",0}, { "0..1", 0 } };
                  int i = 0;
                  foreach (var (inp, op) in data) {
                     DoubleParser.TryParse (inp, out double result);
                     WriteLine ($"Test case {++i} {(result == op ? "passed" : "failed")}");
                  }
                  continue;
               }
            default:
               for (; ; ) {
                  Write ("Enter a string to parse: ");
                  if (!DoubleParser.TryParse (ReadLine ()!, out double value)) {
                     WriteLine ("Invalid input. Please try again.");
                     continue;
                  }
                  WriteLine ("Parsed value: " + value);
               }
         }
      }
   }
}

/// <summary>Exception class with customized message</summary>
class EvalException (string message) : Exception (message) {
}

#region class DoubleParser ------------------------------------------------------------------------
/// <summary>Parses string into double with base and exponent</summary>
static class DoubleParser {
   #region Methods --------------------------------------------------
   public static bool TryParse (string input, out double result) {
      result = 0.0;
      if (string.IsNullOrWhiteSpace (input)) return false;
      // Remove unwanted spaces
      input = input.Trim ();
      // Check for multiple signs
      int sCount = 0;
      while (sCount < input.Length && (input[sCount] == '+' || input[sCount] == '-')) {
         sCount++;
         if (sCount > 1) return false;
      }
      // Determine overall sign
      bool isNegative = input.StartsWith ('-');
      if (input.StartsWith ('+') || isNegative) input = input[1..];
      // Split into base and exponent
      int eIndex = input.IndexOfAny (['e', 'E']);
      string basePart = eIndex >= 0 ? input[..eIndex] : input;
      string expPart = eIndex >= 0 ? input[(eIndex + 1)..] : "0";
      try {
         result = TryParseBase (basePart) * Math.Pow (10, TryParseExp (expPart)) * (isNegative ? -1 : 1);
      } catch {
         return false;
      }
      return true;
   }

   /// <summary>Tries to parse string base value into double</summary>
   static double TryParseBase (string str) {
      if (EvaluateBase (str)) return sBase;
      throw new EvalException ("Not a valid input!!");
   }

   /// <summary>Tries to parse string exponent value into integer</summary>
   static int TryParseExp (string str) {
      if (EvaluateExp (str)) return sExp;
      throw new EvalException ("Not a valid input!!");
   }

   /// <summary>Evaluates the input string base value</summary>
   static bool EvaluateBase (string basePart) {
      if (string.IsNullOrEmpty (basePart)) return false;
      bool hasDecimal = false;
      double result = 0;
      double decimalFactor = 0.1;
      int i = 0;
      if (basePart.StartsWith ('.')) {
         hasDecimal = true;
         i++;
      }
      while (i < basePart.Length) {
         char c = basePart[i];
         if (c == '.') {
            if (hasDecimal) return false;
            hasDecimal = true;
            i++;
            continue;
         }
         if (c < '0' || c > '9') return false;
         int digit = c - '0';
         if (!hasDecimal) result = result * 10 + digit;
         else {
            result += digit * decimalFactor;
            decimalFactor /= 10;
         }
         i++;
      }
      sBase = result;
      return true;
   }

   /// <summary>Evaluates the input string exponent value</summary>
   static bool EvaluateExp (string exp) {
      if (string.IsNullOrEmpty (exp)) return false;
      int i = 0;
      bool isNegative = exp[i] == '-';
      if (exp[i] == '+' || isNegative) i++;
      if (i >= exp.Length) return false;
      int result = 0;
      while (i < exp.Length) {
         char c = exp[i];
         if (c < '0' || c > '9') return false;
         result = result * 10 + (c - '0');
         i++;
      }
      sExp = isNegative ? -result : result;
      return true;
   }
   #endregion

   #region Private variables ----------------------------------------
   static double sBase; // stores base value
   static int sExp; // stores exponent value
   #endregion
}
#endregion