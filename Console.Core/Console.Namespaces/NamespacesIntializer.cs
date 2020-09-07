using System.Collections.Generic;
using System.Text;

using Console.Core.CommandSystem;
using Console.Core.CommandSystem.Attributes;
using Console.Core.CommandSystem.Commands;
using Console.Core.ExtensionSystem;
using Console.Core.ILOptimizations;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;

namespace Console.Namespaces
{
    public class NamespacesIntializer : AExtensionInitializer, ICommandFilter
    {

        private static readonly ALogger Logger = TypedLogger.CreateTypedWithPrefix("Namespace");

        public static readonly List<string> ImportedNamespaces = new List<string>();

        [Property("namespaces.enable")]
        public static bool EnableNamespaces { get; set; } = true;

        [Property("namespaces.disabled.require.qualifiedname")]
        public static bool RequireQualifiedNameWhenDisabled { get; set; }

        public bool Allow(string filterQuery, bool startsWith, CommandIdentity command)
        {
            if (!EnableNamespaces)
            {
                return command.UnfilteredContainsName(filterQuery, startsWith, RequireQualifiedNameWhenDisabled);
            }

            bool needsQualification = !ImportedNamespaces.Contains(command.Namespace);
            bool allowed = needsQualification
                               ? command.UnfilteredContainsName(filterQuery, startsWith, needsQualification)
                               : command.UnfilteredContainsName(filterQuery, startsWith, true) ||
                                 command.UnfilteredContainsName(filterQuery, startsWith, false);
            return allowed;
        }


        protected override void Initialize()
        {
            CommandAttributeUtils.AddCommands<NamespacesIntializer>();
            PropertyAttributeUtils.AddProperties<NamespacesIntializer>();
            ImportedNamespaces.Add(""); //Import the Default Namespace
            CommandFilter.Add(this);
        }

        public int Compare(AbstractCommand cmd1, AbstractCommand cmd2)
        {
            return ImportedNamespaces.IndexOf(cmd1.Identity.Namespace)
                                     .CompareTo(ImportedNamespaces.IndexOf(cmd2.Identity.Namespace));
        }

        [Command(
            "load-namespace",
            HelpMessage = "Loads a namespace and its Commands",
            Namespace = "namespace",
            Aliases = new[] { "import" }
        )]
        [OptimizeIL]
        public static void LoadNamespace(string nameSpace)
        {
            if (!ImportedNamespaces.Contains(nameSpace))
            {
                ImportedNamespaces.Add(nameSpace);
            }
        }

        [Command(
            "unload-namespace",
            HelpMessage = "Unloads a namespace and its Commands",
            Namespace = "namespace",
            Aliases = new[] { "unload" }
        )]
        [OptimizeIL]
        public static void UnloadNamespace(string nameSpace)
        {
            ImportedNamespaces.Remove(nameSpace);
        }

        [Command(
            "list-namespaces",
            HelpMessage = "Lists the loaded Namespaces",
            Aliases = new[] { "namespaces" },
            Namespace = "namespace"
        )]
        [OptimizeIL]
        public static void ShowLoadedNamespaces()
        {
            StringBuilder sb = new StringBuilder("Loaded:\n");
            sb.AppendLine(Unpack(ImportedNamespaces));
            Logger.Log(sb.ToString());
        }

        private static string Unpack(List<string> list)
        {
            if (list.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder(list[0]);

            for (int i = 1; i < list.Count; i++)
            {
                stringBuilder.Append($", {list[i]}");
            }

            return stringBuilder.ToString();
        }

    }
}