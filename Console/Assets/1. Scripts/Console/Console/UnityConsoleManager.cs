using Console.Core.Console;
using Console.Core.ObjectSelection;

namespace Console.Console
{
    public class UnityConsoleManager : AConsoleManager
    {
        private ConsoleManagerComponent managerComponent;

        public UnityConsoleManager(ConsoleInitOptions options, ConsoleManagerComponent managerComponent) : base(options)
        {
            this.managerComponent = managerComponent;
        }

        public void InvokeOnInitialize() => InvokeOnFinishInitialize();
        public void InvokeCommandHandler(string command) => Handler.OnSubmitCommand(command);
        public void InvokeOnLog(string log) => InvokeLogEvent(log);
        public override void Log(object @object) => ConsoleManagerComponent.Log(@object);
        public override void LogWarning(object @object) => ConsoleManagerComponent.LogWarning(@object);
        public override void LogError(object @object) => ConsoleManagerComponent.LogError(@object);
        public override void Clear() => ConsoleManagerComponent.Clear();
        public override void LogPlainText(string text) => ConsoleManagerComponent.LogPlainText(text);
        protected override void SubmitCommand(string command) => managerComponent.SubmitCommand(command);
        public override AObjectSelector ObjectSelector => managerComponent.ObjectSelectorComponent.Selector;
    }
}