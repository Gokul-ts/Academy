namespace Eval;

#region class Token -------------------------------------------------------------------------------
/// <summary>Base class for all tokens</summary>
abstract class Token {
}
#endregion

#region class TNumber -----------------------------------------------------------------------------
/// <summary>Base class for all number tokens</summary>
abstract class TNumber : Token {
   #region Properties -----------------------------------------------
   public abstract double Value { get; }
   #endregion
}
#endregion

#region class TLiteral ----------------------------------------------------------------------------
/// <summary>Base class for all literal values derived from class TNumber</summary>
class TLiteral : TNumber {
   #region Constructor ----------------------------------------------
   public TLiteral (double f) => mValue = f;
   #endregion

   #region Properties -----------------------------------------------
   public override double Value => mValue;
   public override string ToString () => $"literal:{Value}";
   #endregion

   #region Private data ---------------------------------------------
   readonly double mValue;
   #endregion
}
#endregion

#region class TVariable ---------------------------------------------------------------------------
/// <summary>Base class for all variables derived from class TNumber</summary>
class TVariable : TNumber {
   #region Constructor ----------------------------------------------
   public TVariable (Evaluator eval, string name) => (Name, mEval) = (name, eval);
   #endregion

   #region Properties -----------------------------------------------
   public string Name { get; private set; }
   public override double Value => mEval.GetVariable (Name);
   public override string ToString () => $"var:{Name}";
   #endregion

   #region Private data ---------------------------------------------
   readonly Evaluator mEval;
   #endregion
}
#endregion

#region class TOperator ---------------------------------------------------------------------------
/// <summary>Base class for all operators derived from class Token</summary>
abstract class TOperator : Token {
   #region Constructor ----------------------------------------------
   protected TOperator (Evaluator eval) => mEval = eval;
   #endregion

   #region Properties -----------------------------------------------
   public abstract int Priority { get; }
   #endregion

   #region Private data ---------------------------------------------
   readonly protected Evaluator mEval;
   #endregion
}
#endregion

#region class TOpArithmetic -----------------------------------------------------------------------
/// <summary>Base class for all arithmetic operators derived from class TOperator</summary>
class TOpArithmetic : TOperator {
   #region Constructor ----------------------------------------------
   public TOpArithmetic (Evaluator eval, char ch) : base (eval) => Op = ch;
   #endregion

   #region Properties -----------------------------------------------
   public char Op { get; private set; }
   public override string ToString () => $"op:{Op}:{Priority}";
   public override int Priority => sPriority[Op] + mEval.BasePriority;
   #endregion

   #region Private data ---------------------------------------------
   static Dictionary<char, int> sPriority = new () {
      ['+'] = 1, ['-'] = 1, ['*'] = 2, ['/'] = 2, ['^'] = 3, ['='] = 4,
   };
   #endregion

   #region Methods --------------------------------------------------
   public double Evaluate (double a, double b) {
      return Op switch {
         '+' => a + b,
         '-' => a - b,
         '*' => a * b,
         '/' => a / b,
         '^' => Math.Pow (a, b),
         _ => throw new EvalException ($"Unknown operator: {Op}"),
      };
   }

   public double Evaluate (double a) {
      return Op switch {
         '-' => -a,
         _ => throw new EvalException ($"Unknown operator: {Op}"),
      };
   }
   #endregion
}
#endregion

#region class TOpFunction -------------------------------------------------------------------------
/// <summary>Base class for all function operators derived from class TOperator</summary>
class TOpFunction : TOperator {
   #region Constructor ----------------------------------------------
   public TOpFunction (Evaluator eval, string name) : base (eval) => Func = name;
   #endregion

   #region Properties -----------------------------------------------
   public string Func { get; private set; }
   public override string ToString () => $"func:{Func}:{Priority}";
   public override int Priority => 4 + mEval.BasePriority;
   #endregion

   #region Methods --------------------------------------------------
   public double Evaluate (double f) {
      return Func switch {
         "sin" => Math.Sin (D2R (f)),
         "cos" => Math.Cos (D2R (f)),
         "tan" => Math.Tan (D2R (f)),
         "sqrt" => Math.Sqrt (f),
         "log" => Math.Log (f),
         "exp" => Math.Exp (f),
         "asin" => R2D (Math.Asin (f)),
         "acos" => R2D (Math.Acos (f)),
         "atan" => R2D (Math.Atan (f)),
         _ => throw new EvalException ($"Unknown function: {Func}")
      };

      double D2R (double f) => f * Math.PI / 180;
      double R2D (double f) => f * 180 / Math.PI;
   }
   #endregion
}
#endregion

#region class TOpUnary ----------------------------------------------------------------------------
/// <summary>Base class for unary operators derived from class TOperator</summary>
class TOpUnary : TOperator {
   #region Constructor ----------------------------------------------
   public TOpUnary (Evaluator eval, char ch) : base (eval) => Op = ch;
   #endregion

   #region Properties -----------------------------------------------
   public override string ToString () => $"op:{Op}:{Priority}";
   public char Op { get; private set; }
   public override int Priority => 4 + mEval.BasePriority;
   #endregion

   #region Methods --------------------------------------------------
   public double Evaluate (double a) {
      return Op switch {
         '+' => a,
         '-' => -a,
         _ => throw new EvalException ($"Unknown operator: {Op}"),
      };
   }
   #endregion
}
#endregion

#region class TPunctuation ------------------------------------------------------------------------
/// <summary>Base class for punctuation values derived from class Token</summary>
class TPunctuation : Token {
   #region Constructor ----------------------------------------------
   public TPunctuation (char ch) => Punct = ch;
   #endregion

   #region Properties -----------------------------------------------
   public char Punct { get; private set; }
   public override string ToString () => $"punct:{Punct}";
   #endregion
}
#endregion

#region class TEnd --------------------------------------------------------------------------------
class TEnd : Token {
   #region Properties -----------------------------------------------
   public override string ToString () => "end";
   #endregion
}
#endregion

#region class TEnd --------------------------------------------------------------------------------
class TError : Token {
   #region Constructor ----------------------------------------------
   public TError (string message) => Message = message;
   #endregion

   #region Properties -----------------------------------------------
   public string Message { get; private set; }
   public override string ToString () => $"error:{Message}";
   #endregion
}
#endregion