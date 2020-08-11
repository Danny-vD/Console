using System.Linq.Expressions;
using System.Reflection;
using System;
namespace Console.Core.Utils.Reflection.Methods
{
    public static class SymbolExtensions
    {
        public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
        {
            if (expression.Body is MethodCallExpression member)
                return member.Method;

            throw new ArgumentException("Expression is not a method", "expression");
        }

        public static MethodInfo GetMethodInfo<T>(string name)
        {
            return typeof(T).GetMethod(name, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        }
    }
}