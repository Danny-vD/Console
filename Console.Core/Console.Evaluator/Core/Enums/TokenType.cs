namespace Console.Evaluator.Core.Enums
{
    /// <summary>
    /// Defines All Token Types that the Evaluator Understands
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// No Token Type
        /// </summary>
        None,
        /// <summary>
        /// End Of Formula Token
        /// </summary>
        EndOfFormula,
        /// <summary>
        /// + Operator Token
        /// </summary>
        OperatorPlus,
        /// <summary>
        /// -/– Operator Token
        /// </summary>
        OperatorMinus,
        /// <summary>
        /// * Operator Token
        /// </summary>
        OperatorMul,
        /// <summary>
        /// / Operator Token
        /// </summary>
        OperatorDiv,
        /// <summary>
        /// % Operator Token
        /// </summary>
        OperatorPercent,
        /// <summary>
        /// [ Operator Token
        /// </summary>
        OpenParenthesis,
        /// <summary>
        /// , Operator Token
        /// </summary>
        Comma,
        /// <summary>
        /// . Operator Token
        /// </summary>
        Dot,
        /// <summary>
        /// ] Operator Token
        /// </summary>
        CloseParenthesis,
        /// <summary>
        /// != Operator Token
        /// </summary>
        OperatorNe,
        /// <summary>
        /// &gt; Operator Token
        /// </summary>
        OperatorGt,
        /// <summary>
        /// >= Operator Token
        /// </summary>
        OperatorGe,
        /// <summary>
        /// = Operator Token
        /// </summary>
        OperatorEq,
        /// <summary>
        /// &lt;= Operator Token
        /// </summary>
        OperatorLe,
        /// <summary>
        /// &lt; Operator Token
        /// </summary>
        OperatorLt,
        /// <summary>
        /// and Operator Token
        /// </summary>
        OperatorAnd,
        /// <summary>
        /// or Operator Token
        /// </summary>
        OperatorOr,
        /// <summary>
        /// not Operator Token
        /// </summary>
        OperatorNot,
        /// <summary>
        /// (and) Operator Token
        /// </summary>
        OperatorConcat,
        /// <summary>
        /// if Operator Token
        /// </summary>
        OperatorIf,
        /// <summary>
        /// Default Operator Token
        /// </summary>
        ValueIdentifier,
        /// <summary>
        /// true/yes Operator Token
        /// </summary>
        ValueTrue,
        /// <summary>
        /// false/no Operator Token
        /// </summary>
        ValueFalse,
        /// <summary>
        /// Number Operator token
        /// </summary>
        ValueNumber,
        /// <summary>
        /// "/' Operator Token
        /// </summary>
        ValueString,
        /// <summary>
        /// # Operator Token
        /// </summary>
        ValueDate,
        /// <summary>
        /// ( Operator Token
        /// </summary>
        OpenBracket,
        /// <summary>
        /// ) Operator Token
        /// </summary>
        CloseBracket
    }
}