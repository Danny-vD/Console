using System;
using System.Linq;
using System.Reflection;
using Console.EnvironmentVariables;

namespace Console.ClassQueries
{
    public class ClassQueryProvider : VariableProvider
    {
        public override string FunctionName => "class";

        public override string GetValue(string parameter)
        {

            Type t = Find(parameter);
            if (t != null) return t.AssemblyQualifiedName;
            return "NOT_FOUND";
        }

        private Type Find(string name)
        {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetTypes().Any(y => IsMatch(y, name)))
                .ToArray();
            return asms.FirstOrDefault()?.GetTypes().FirstOrDefault(x => IsMatch(x, name));
        }

        private bool IsMatch(Type t, string name)
        {
            return t.Name == name || t.FullName == name || t.AssemblyQualifiedName == name;
        }
    }
}