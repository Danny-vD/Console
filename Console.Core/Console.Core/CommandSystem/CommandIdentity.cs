using System.Collections.Generic;
using System.Linq;

namespace Console.Core.CommandSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandIdentity
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="names"></param>
        public CommandIdentity(string nameSpace, string[] names)
        {
            this.names = names.ToList();
            Namespace = nameSpace;
        }

        /// <summary>
        /// 
        /// </summary>
        public string QualifiedName => names.Count == 0 ? "" : GetQualifiedName(Namespace, names.First());

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<string> QualifiedNames =>
            names.Select(name => GetQualifiedName(Namespace, name)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<string> Names => names.AsReadOnly();

        private List<string> names { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetQualifiedName(string nameSpace, string name)
        {
            return string.IsNullOrEmpty(nameSpace) ? name : nameSpace + "::" + name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="startsWith"></param>
        /// <returns></returns>
        public bool FilteredContainsName(string commandName, bool startsWith = false)
        {
            return CommandFilter.AllowCommand(commandName, startsWith, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="startsWith"></param>
        /// <param name="qualified"></param>
        /// <returns></returns>
        public bool UnfilteredContainsName(string commandName, bool startsWith, bool qualified)
        {
            IEnumerable<string> ns = qualified ? QualifiedNames : Names;

            if (startsWith)
            {
                return ns.Any(x => x.StartsWith(commandName));
            }

            return ns.Contains(commandName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newNames"></param>
        public void AddNames(IEnumerable<string> newNames)
        {
            foreach (string newName in newNames)
            {
                AddName(newName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void AddName(string name)
        {
            if (!UnfilteredContainsName(name, false, false))
            {
                names.Add(name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        public void SetNamespace(string nameSpace)
        {
            Namespace = nameSpace;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void RemoveName(string name)
        {
            names.Remove(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public void Replace(string oldName, string newName)
        {
            int index = names.IndexOf(oldName);
            if (index == -1)
            {
                return;
            }

            names.RemoveAt(index);
            names.Insert(index, newName);
        }

    }
}