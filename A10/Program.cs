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
   /// <summary>Parses valid input string and returns the output in a tuple<summary>
   public static (string, string, string, string) Parse (string input) {
      var (drive, folder, file, ext) = ("", "", "", "");
      EState s = A;
      foreach (var ch in input.Trim ().ToUpper () + '~') {
         s = (s, ch) switch {
            (A, >= 'A' and <= 'Z') => B,
            (B, ':') => C,
            (C or E, '\\') => D,
            (D or E, >= 'A' and <= 'Z') => E,
            (E, '.') => F,
            (F or G, >= 'A' and <= 'Z') => G,
            (G, '~') => H,
            _ => Z,
         };
         if (s is B) drive = ch + "";
         if (s is E or D) folder += ch;
         if (s is F) {
            file = folder.Split ('\\').Last ();
            folder = folder[..^file.Length];
         }
         if (s is F or G) ext += ch;
      }
      if (s is Z) throw new ArgumentException ("Not a valid file path!!");
      return (drive, folder, file, ext);
   }
   #endregion
}
// Enums holding various states of FSM
enum EState { A, B, C, D, E, F, G, H, Z }
#endregion