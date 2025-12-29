using static System.Console;

internal class Program {
   private static void Main () {
      var editor = new TextEditor ();
      for (; ; ) {
         var input = ReadLine () ?? "";
         var data = input.Split (' ');
         var cmd = string.Empty;
         string value = string.Empty;
         cmd = data[0];
         for (int i = 1; i < data.Length; i++) {
            value += data[i];
         }
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
                  editor.Undo ();
                  break;
               }
            case "REDO": {
                  editor.Redo ();
                  break;
               }
            case "EXIT": {
                  editor.Exit ();
                  break;
               }
            default: {
                  WriteLine ("Invalid cmd..Please try again!");
                  break;
               }
         }
      }
   }
}

class TextEditor () {
   public void Add (string value) {
      editor.Add (value);
      int n = value.Length;
      undo.Push (() => { Delete (n); });

   }
   public void Delete (int count) {
      if (count > editor[^1].Length) return;
      string s = editor[^1];
      undo.Push (() => { Add (s); });
      var temp = editor[^1][..count];
      editor[^1] = editor[^1][..^count];
      editor.Remove ("");
   }
   public void Undo () {
      undo.Peek ().Invoke ();
      redo.Push (undo.Pop ());

   }
   public void Redo () {
      redo.Peek ().Invoke ();
      undo.Push (redo.Pop ());
   }

   public void Show () {
      foreach (var item in editor) {
         Write (item);
      }
      if (editor.Count > 0) WriteLine ();
   }
   public void Exit () {

      Environment.Exit (0);
   }

   List<string> editor = [];
   Stack<Action> redo = [];
   Stack<Action> undo = [];
}