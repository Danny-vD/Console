namespace Console.ScriptSystem.Deblocker.Implementations
{

    /// <summary>
    /// ADeblocker Implementation with key "ifelseif"
    /// Implements if Syntax with SequenceSystem / Evaluator Extension as backend.
    /// </summary>
    public class IfElseIfDeblocker : IfDeblocker
    {
        /// <summary>
        /// The Key of the Deblocker that has to match the block command to be activated.
        /// </summary>
        public override string Key => "ifelseif";
    }
}