namespace Eval;

#region class EvalException -----------------------------------------------------------------------
/// <summary>Class inherited from exception class with customized message</summary>
class EvalException : Exception {
   public EvalException (string message) : base (message) { }
}
#endregion

#region class Evaluator ---------------------------------------------------------------------------
/// <summary>Class to evaluate and process the collected tokens</summary>
class Evaluator {
   #region Methods --------------------------------------------------
   /// <summary>Evaluates input tokens and returns the result</summary>
   public double Evaluate (string text) {
      if (string.IsNullOrWhiteSpace (text)) throw new EvalException ("Enter a string!");
      List<Token> tokens = new ();
      var tokenizer = new Tokenizer (this, text);
      for (; ; ) {
         var token = tokenizer.Next ();
         tokenizer.PrevToken = token;
         if (token is TEnd) break;
         if (token is TError err) throw new EvalException (err.Message);
         tokens.Add (token);
      }
      // Check if this is a variable assignment
      TVariable? tVariable = null;
      if (tokens.Count > 2 && tokens[0] is TVariable tvar && tokens[1] is TOpArithmetic { Op: '=' }) {
         tVariable = tvar;
         tokens.RemoveRange (0, 2);
      }
      foreach (var t in tokens) Process (t);
      while (mOperators.Count > 0) ApplyOperator ();
      double f = mOperands.Pop ();
      if (tVariable != null) mVars[tVariable.Name] = f;
      return f;
   }

   /// <summary>Returns the value of the variable</summary>
   public double GetVariable (string name) {
      if (mVars.TryGetValue (name, out double f)) return f;
      throw new EvalException ($"Unknown variable: {name}");
   }
   #endregion

   #region Implementation -------------------------------------------
   // Process each tokens based on their signature
   void Process (Token token) {
      switch (token) {
         case TNumber num:
            mOperands.Push (num.Value);
            break;
         case TOperator op:
            while (mOperators.Count > 0 && mOperators.Peek ().Priority > op.Priority)
               ApplyOperator ();
            mOperators.Push (op);
            break;
         case TPunctuation p:
            BasePriority += p.Punct == '(' ? 10 : -10;
            break;
         default:
            throw new EvalException ($"Unknown token: {token}");
      }
   }

   // Performs operations with the operands and operators
   void ApplyOperator () {
      var op = mOperators.Pop ();
      var f1 = mOperands.Pop ();
      if (op is TOpFunction func) mOperands.Push (func.Evaluate (f1));
      else if (op is TOpArithmetic arith) {
         var f2 = mOperands.Pop ();
         if (mOperators.Count > 0 && mOperators.Peek () is TOpArithmetic a && a.Op is '-') {
            f2 = a.Evaluate (f2);
            mOperators.Pop ();
            mOperators.Push (new TOpArithmetic (this, '+'));
         }
         mOperands.Push (arith.Evaluate (f2, f1));
      } else if (op is TOpUnary unary)
         mOperands.Push (unary.Evaluate (f1));
   }
   #endregion

   #region Properties -----------------------------------------------
   public int BasePriority { get; private set; }
   #endregion

   #region Private data ---------------------------------------------
   readonly Stack<double> mOperands = [];
   readonly Stack<TOperator> mOperators = [];
   readonly Dictionary<string, double> mVars = [];
   #endregion
}
#endregion