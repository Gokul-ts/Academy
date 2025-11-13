// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement a clone of double.TryParse method.
// ------------------------------------------------------------------------------------------------
using static System.Console;
using static DoubleParser;

internal class Program {
   static void Main () {
      for (; ; ) {
         Write ("Enter a string to parse: ");
         if (!TryParse (ReadLine ()!, out double value)) {
            WriteLine ("Invalid input. Please try again.");
            continue;
         }
         WriteLine ("Parsed value: " + value);
      }
   }
}

static class DoubleParser {
   public static bool TryParse (string input, out double result) {
      result = 0.0;
      if (string.IsNullOrWhiteSpace (input))
         return false;
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
      if (input.StartsWith ('+') || isNegative)
         input = input[1..];
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

   static public double TryParseBase (string str) {
      if (EvaluateBase (str)) return BaseValue;
      throw new EvalException ("Not a valid input!!");
   }

   static public int TryParseExp (string str) {
      if (EvaluateExp (str)) return ExpValue;
      throw new EvalException ("Not a valid input!!");
   }

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
      BaseValue = result;
      return true;
   }

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
      ExpValue = isNegative ? -result : result;
      return true;
   }

   public static double BaseValue {
      get { return mBaseValue; }
      set { mBaseValue = value; }
   }

   public static int ExpValue {
      get { return mExpValue; }
      set { mExpValue = value; }
   }

   static double mBaseValue;

   static int mExpValue;
}

class EvalException (string message) : Exception (message) {
}