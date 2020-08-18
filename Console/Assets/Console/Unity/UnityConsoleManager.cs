using Console.Core;
using Console.Unity.Components;


/// <summary>
/// Namespace of the Unity Integration
/// </summary>
namespace Console.Unity
{
    /// <summary>
    /// AConsoleManager Implementation
    /// </summary>
    public class UnityConsoleManager : AConsoleManager
    {
        /// <summary>
        /// The Console Manager Component
        /// </summary>
        private ConsoleManagerComponent managerComponent;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="managerComponent"></param>
        public UnityConsoleManager(ConsoleInitOptions options, ConsoleManagerComponent managerComponent) : base(options)
        {
            this.managerComponent = managerComponent;
        }

        /// <summary>
        /// Wrapper Function that allows external Clases to Invoke the OnFinishInitialize Event in the AConsoleManager Class
        /// </summary>
        public void InvokeOnInitialize() => InvokeOnFinishInitialize();

        /// <summary>
        /// Wrapper Function that allows external Clases to Invoke the OnLog Event in the AConsoleManager Class
        /// </summary>
        public void InvokeOnLog(string log) => InvokeLogEvent(log);

        /// <summary>
        /// Writes a Log to the Console Output
        /// </summary>
        /// <param name="object">The Log to Write</param>
        public override void Log(object @object) => ConsoleManagerComponent.Log(@object);

        /// <summary>
        /// Writes a Log Warning to the Console Output
        /// </summary>
        /// <param name="object">The Log Warning to Write</param>
        public override void LogWarning(object @object) => ConsoleManagerComponent.LogWarning(@object);

        /// <summary>
        /// Writes a Log Error to the Console Output
        /// </summary>
        /// <param name="object">The Log Error to Write</param>
        public override void LogError(object @object) => ConsoleManagerComponent.LogError(@object);
        /// <summary>
        /// Clears the Console Output
        /// </summary>
        public override void Clear() => ConsoleManagerComponent.Clear();

        /// <summary>
        /// Writes Plain Text to the Console Output
        /// </summary>
        /// <param name="text">The Text to Write</param>
        public override void LogPlainText(string text) => ConsoleManagerComponent.LogPlainText(text);
        /// <summary>
        /// Gets Invoked when a Command was Submitted.
        /// </summary>
        /// <param name="command">The Submitted Command Line</param>
        protected override void OnSubmitCommand(string command) => managerComponent.SubmitCommand(command);
        /// <summary>
        /// Object Selector Instance.
        /// </summary>
        public override AObjectSelector ObjectSelector => managerComponent.ObjectSelectorComponent.Selector;
    }
}