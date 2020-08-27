using System;
using System.Linq;
using System.Reflection;
using Console.EnvironmentVariables;

namespace Console.ClassQueries
{
    /// <summary>
    /// VariableProvider Implementation that returns the Assembly Qualified Name of the Specified Type Search Term
    /// </summary>
    public class ClassQueryProvider : VariableProvider
    {
        /// <summary>
        /// The Function name that is used to get this Variable.
        /// </summary>
        public override string FunctionName => "class";

        /// <summary>
        /// Returns the Assembly Qualified Name of the Searched Type
        /// </summary>
        /// <param name="parameter">Search term for the Type</param>
        /// <returns>Assembly Qualified Name of the Type or "NOT_FOUND" when no type is found.</returns>
        public override string GetValue(string parameter)
        {

            Type t = Find(parameter);
            if (t != null) return t.AssemblyQualifiedName;
            return "NOT_FOUND";
        }

        /// <summary>
        /// Finds a Type by name in all assemblies
        /// </summary>
        /// <param name="name">Search Term</param>
        /// <returns>Search Result. Null if not found.</returns>
        private Type Find(string name)
        {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.GetTypes().Any(y => IsMatch(y, name)))
                .ToArray();
            return asms.FirstOrDefault()?.GetTypes().FirstOrDefault(x => IsMatch(x, name));
        }

        /// <summary>
        /// Returns true if the Name is either the TypeName the FullName or the AssemblyQualifiedName
        /// </summary>
        /// <param name="t">Type to Test</param>
        /// <param name="name">Name of the Target Type</param>
        /// <returns>True if the Name is matching the type</returns>
        private bool IsMatch(Type t, string name)
        {
            return t.Name == name || t.FullName == name || t.AssemblyQualifiedName == name;
        }
    }
}