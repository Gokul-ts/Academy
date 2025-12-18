// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2025.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------
// Program.cs
// Program to implement file name parser with state machine.
// ------------------------------------------------------------------------------------------------
using static System.Console;
using static EState;

#region class Program -----------------------------------------------------------------------------
/// <summary>Class to get input from the user using a REPL and print the result</summary>
internal class Program {
   static void Main () {
      for (; ; ) {
         Write ("Enter a file path (Drive:\\Folder\\File.extension): ");
         string path = ReadLine () ?? "";
         if (string.IsNullOrEmpty (path)) {
            WriteLine ("Path cannot be empty");
            continue;
         }
         try {
            var (drive, folder, file, ext) = FileParser.Parse (path);
            WriteLine ($"Drive : {drive}\nFolder: {folder}\nFile  : {file}\nExt   : {ext}");
         } catch (Exception e) {
            WriteLine (e.Message);
         }
      }
   }
}
#endregion

#region class FileParser --------------------------------------------------------------------------
/// <summary>Parser class to check whether the input string is a valid file path and return the
/// drive letter, folder name and filename with extension. Refer state_transition_diag.png</summary>
static class FileParser {
   #region Methods --------------------------------------------------
   /// <summary>Parses valid input string and returns the output in a tuple or throws an
   /// exception if any<summary>
   public static (string, string, string, string) Parse (string input) {
      var (drive, folder, file, ext) = ("", "", "", "");
      EState s = A; int mN = 0;
      input = input.Trim ().ToUpper () + '~';
      while (mN < input.Length) {
         Action step = (s, input[mN++]) switch {
            (A, var c) when c is >= 'A' and <= 'Z' => () => { s = B; drive += c; },
            (B, ':') => () => { s = C; },
            (C, '\\') => () => { s = D; },
            (D or F, var c) when c is >= 'A' and <= 'Z' => () => { s = s is D ? E : G; folder += Extract (); },
            (E or G, '\\') => () => { s = F; },
            (G, '.') => () => { s = H; },
            (H, var c) when c is >= 'A' and <= 'Z' => () => {
               s = I; file = folder.Split ('\\').Last ();
               folder = folder[..^file.Length].Trim ('\\');
               ext += Extract ();
            },
            (I, '~') => () => { s = J; },
            _ => () => { s = Z; }
         };
         step ();
         if (s is Z) throw new ArgumentException ("Not a valid file path!!");
      }
      return (drive, folder, file, ext);

      // Helper function to extract letters
      string Extract () {
         int start = mN - 2;
         while (mN < input.Length) {
            if (input[mN++] is >= 'A' and <= 'Z') continue;
            mN--; break;
         }
         return input[start..mN];
      }
   }
   #endregion
}
// Enums holding various states of FileParser
enum EState { A, B, C, D, E, F, G, H, I, J, Z }
#endregion