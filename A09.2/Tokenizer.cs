namespace Eval;

#region class Tokenizer ---------------------------------------------------------------------------
/// <summary>Class to classify and create tokens based on the type of input</summary>
class Tokenizer {
   #region Constructor ----------------------------------------------
   public Tokenizer (Evaluator eval, string text) {
      mText = text; mN = 0; mEval = eval;
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Returns the next token from input</summary>
   public Token Next () {
      while (mN < mText.Length) {
         char ch = char.ToLower (mText[mN++]);
         switch (ch) {
            case ' ' or '\t': continue;
            case (>= '0' and <= '9') or '.':
               return (PrevToken is TLiteral)
                  ? new TError ("Invalid operation") : GetNumber ();
            case '(' or ')': return new TPunctuation (ch);
            case '+' or '-':
               return (PrevToken is null or TOperator or TPunctuation { Punct: '(' })
                  ? new TOpUnary (mEval, ch)
                  : new TOpArithmetic (mEval, ch);
            case '*' or '/' or '^' or '=': return new TOpArithmetic (mEval, ch);
            case >= 'a' and <= 'z': return GetIdentifier ();
            default: return new TError ($"Unknown symbol: {ch}");
         }
      }
      return PrevToken is TNumber or TPunctuation { Punct: ')' } ? new TEnd () : new TError ("Invalid operation");
   }
   #endregion

   #region Implementation -------------------------------------------
   // Identifies the type of TOp token and returns the value
   Token GetIdentifier () {
      int start = mN - 1;
      while (mN < mText.Length) {
         char ch = char.ToLower (mText[mN++]);
         if (ch is >= 'a' and <= 'z') continue;
         mN--; break;
      }
      string sub = mText[start..mN];
      if (mFuncs.Contains (sub)) return new TOpFunction (mEval, sub);
      else return new TVariable (mEval, sub);
   }

   // Returns the TLiteral token with value or TError if invalid
   Token GetNumber () {
      int start = mN - 1;
      while (mN < mText.Length) {
         char ch = mText[mN++];
         if (ch is (>= '0' and <= '9') or '.') continue;
         mN--; break;
      }
      // Now, mN points to the first character of mText that is not part of the number
      string sub = mText[start..mN];
      if (double.TryParse (sub, out double f)) return new TLiteral (f);
      return new TError ($"Invalid number: {sub}");
   }
   #endregion

   #region Properties -----------------------------------------------
   public Token? PrevToken { get { return mPrevToken; } set { mPrevToken = value; } }
   Token? mPrevToken;
   #endregion

   #region Private data ---------------------------------------------
   readonly string[] mFuncs = { "sin", "cos", "tan", "sqrt", "log", "exp", "asin", "acos", "atan" };
   readonly Evaluator mEval;  // The evaluator that owns this 
   readonly string mText;     // The input text we're parsing through
   int mN;                    // Position within the text
   #endregion
}
#endregion