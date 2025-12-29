using System.Text;
using static System.Console;

#region class Program --------------------------------------------------------------------------
internal class Program {
   private static void Main () {
      var editor = new TextEditor ();
      for (; ; ) {
         var input = ReadLine () ?? "";
         var (data, cmd, value) = (input.Split (' '), string.Empty, string.Empty);
         cmd = data[0];
         for (int i = 1; i < data.Length; i++) value += data[i];
         switch (cmd.ToUpper ()) {
            case "ADD": {
                  editor.Add (value);
                  break;
               }
            case "DELETE": {
                  if (int.TryParse (value, out int num)) {
                     editor.Delete (num);
                  } else WriteLine ("Enter a valid number");
                  break;
               }
            case "SHOW": {
                  editor.Show ();
                  break;
               }
            case "UNDO": {
                  if (value != string.Empty) Error ();
                  editor.Undo ();
                  break;
               }
            case "REDO": {
                  if (value != string.Empty) Error ();
                  editor.Redo ();
                  break;
               }
            case "EXIT": {
                  if (value != string.Empty) Error ();
                  editor.Exit ();
                  break;
               }
            default: {
                  Error ();
                  break;
               }
         }
      }

      void Error () => WriteLine ("Invalid cmd..Please try again!");
   }
}
#endregion

#region class TextEditor --------------------------------------------------------------------------
class TextEditor () {
   #region Methods --------------------------------------------------
   public void Add (string value) {
      editor.Append (value);
      int n = value.Length;
      undo.Push (() => { Delete (n); });
   }

   public void Delete (int count) {
      if (count > editor.Length) { return; }
      string s = editor.ToString ().Substring (editor.Length - count, count);
      undo.Push (() => { Add (s); });
      editor.Remove (editor.Length - count, count);
     
   }

   public void Undo () {
      if (undo.Count > 0) {
         undo.Peek ().Invoke ();
         redo.Push (undo.Pop ());
      }
   }

   public void Redo () {
      if (redo.Count > 0) {
         redo.Peek ().Invoke ();
         undo.Push (redo.Pop ());
      }
   }

   public void Show () {
      if (editor.Length > 0) WriteLine (editor);
   }

   public void Exit () => Environment.Exit (0);
   #endregion

   #region Private data ---------------------------------------------
   StringBuilder editor = new StringBuilder ();
   Stack<Action> redo = [];
   Stack<Action> undo = [];
   #endregion
}
#endregion