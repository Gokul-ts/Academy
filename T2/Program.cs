internal class Program {
   static void Main () {
      int start = 50;
      int mZero = 0;
      var input = File.ReadAllLines (@"data\dial.txt");
      foreach (var line in input) {
         string s = string.Empty;
         for (int i = 1; i < line.Length; i++) {
            s += line[i];
         }
         int n = int.Parse (s);
         for (int j = 0; j < n; j++) {
            if (line[0] == 'L') {
               if (start == 0) { start = 99; mZero++; continue; }
               start--;
            } else {
               if (start == 100) { start = 1; mZero++; continue; }
               start++;
            }
         }
      }
      Console.WriteLine ($"Password: {mZero}");
   }
}