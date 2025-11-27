// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement a clone of double.TryParse method.
// ------------------------------------------------------------------------------------------------
using static System.Console;

#region class Program -----------------------------------------------------------------------------
/// <summary>Program class to test DoubleParser class implementation</summary>
internal class Program {
   static void Main () {
      for (; ; ) {
         WriteLine ("Enter [T] to run test case or any other key to continue: ");
         switch (ReadKey (true).Key) {
            case ConsoleKey.T: {
                  var data = new Dictionary<string, double>
                  { { "123", 123 }, { "-123", -123 }, { "123.45", 123.45 }, { "-123.45", -123.45 },
                  { "+123.45e45", 1.2345e+47 }, { "-123.45e-45", -1.2345e-43 }, { "123e-45", 1.23e-43 },
                  { ".45e3", 450 }, { "123.", 123 }, { "4.e45", 4e+45 }, { "34.4E3", 34400 }, { "", 0 },
                  { "-12-3e3", 0 }, { "e24", 0 }, { "nan", 0 }, { "123+", 0 }, { ".e-", 0 }, { "-e+", 0 },
                  { "-+98", 0 }, { "-123.-1", 0 }, { "1..1", 0 }, { "8-e", 0 }, { "1e-1", 0.1 } };
                  bool pass = true;
                  foreach (var (inp, op) in data) {
                     DoubleParser.TryParse (inp, out double result);
                     if (result != op) { pass = false; break; }
                  }
                  WriteLine ($"Test cases {(pass ? "passed" : "failed")}");
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
#endregion

#region class ParseException -----------------------------------------------------------------------
/// <summary>Class inherited from exception class with customized message</summary>
class ParseException (string message) : Exception (message) {
}
#endregion

#region class DoubleParser ------------------------------------------------------------------------
/// <summary>Parser class to implement double parsing methods given a string</summary>
static class DoubleParser {
   #region Methods --------------------------------------------------
   /// <summary>Tries to parse string value into double</summary>
   public static bool TryParse (string input, out double result) {
      result = 0.0;
      if (string.IsNullOrWhiteSpace (input)) return false;
      // Remove unwanted spaces
      input = input.Trim ();
      // Check for multiple signs
      int count = 0;
      while (count < input.Length && (input[count] == '+' || input[count] == '-')) {
         count++;
         if (count > 1) return false;
      }
      // Determine overall sign
      bool isNegative = input.StartsWith ('-');
      if (input.StartsWith ('+') || isNegative) input = input[1..];
      // Split into base and exponent
      int eIndex = input.IndexOfAny (['e', 'E']);
      bool hasExp = eIndex >= 0;
      string basePart = hasExp ? input[..eIndex] : input;
      string expPart = hasExp ? input[(eIndex + 1)..] : "0";
      try {
         result = TryParseBase (basePart) * Math.Pow (10, TryParseExp (expPart)) * (isNegative ? -1 : 1);
      } catch {
         return false;
      }
      return true;
   }
   #endregion

   #region Implementation -------------------------------------------
   // Tries to parse string base value into double
   static double TryParseBase (string str) {
      if (EvaluateBase (str)) return sBase;
      Error ();
      return 0;
   }

   // Tries to parse string exponent value into integer
   static int TryParseExp (string str) {
      if (EvaluateExp (str)) return sExp;
      Error ();
      return 0;
   }

   // Throws not valid input exception 
   static void Error () => throw new ParseException ("Not a valid input!!");

   // Evaluates the input string base value
   static bool EvaluateBase (string basePart) {
      if (string.IsNullOrEmpty (basePart)) return false;
      bool hasDecimal = false;
      double result = 0;
      double decimalFactor = 0.1;
      int i = 0;
      if (basePart.StartsWith ('.')) { hasDecimal = true; i++; }
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
         else { result += digit * decimalFactor; decimalFactor /= 10; }
         i++;
      }
      sBase = result;
      return true;
   }

   // Evaluates the input string exponent value
   static bool EvaluateExp (string exp) {
      if (string.IsNullOrEmpty (exp)) return false;
      int i = 0;
      bool isNegative = exp[i] == '-';
      if (exp[i] == '+' || isNegative) i++;
      if (i >= exp.Length) return false;
      int result = 0;
      while (i < exp.Length) {
         char c = exp[i++];
         if (c < '0' || c > '9') return false;
         result = result * 10 + (c - '0');
      }
      sExp = isNegative ? -result : result;
      return true;
   }
   #endregion

   #region Private data ---------------------------------------------
   static double sBase; // stores base value
   static int sExp; // stores exponent value
   #endregion
}
#endregion