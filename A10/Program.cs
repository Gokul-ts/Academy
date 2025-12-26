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
   /// <summary>Parses valid input string and returns the output in a tuple or throws an exception if any</summary>
   public static (string, string, string, string) Parse (string input) {
      var (drive, folder, file, ext) = ("", "", "", "");
      EState s = A; int idx = 0;
      input = input.Trim ().ToUpper () + '~';
      while (idx < input.Length) {
         char ch = input[idx++];
         bool isAlphabet = ch is >= 'A' and <= 'Z';
         Action step = (s, ch) switch {
            (A, _) when isAlphabet => () => { s = B; drive += ch; },
            (B, ':') => () => { s = C; },
            (C, '\\') => () => { s = D; },
            (D or F, _) when isAlphabet => () => { s = s is D ? E : G; folder += Extract (); },
            (E or G, '\\') => () => { s = F; },
            (G, '.') => () => { s = H; },
            (H, _) when isAlphabet => () => {
               s = I; file = folder.Split ('\\').Last ();
               folder = folder[..^(file.Length + 1)];
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
         int start = idx - (folder is "" ? 1 : 2);
         while (idx < input.Length) {
            if (input[idx++] is >= 'A' and <= 'Z') continue;
            idx--; break;
         }
         return input[start..idx];
      }
   }
   #endregion
}
#endregion

#region enum Estate -------------------------------------------------
// Enums holding various states of FileParser
enum EState { A, B, C, D, E, F, G, H, I, J, Z }
#endregion