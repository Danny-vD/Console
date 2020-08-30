﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core;
using Console.Core.CommandSystem;
using Console.Core.PropertySystem;

namespace Console.EnvironmentVariables
{
    /// <summary>
    /// Static EnvironmentVariable API
    /// </summary>
    public static class EnvironmentVariableManager
    {
        /// <summary>
        /// The Character that is used to Activate the environment variable expander
        /// </summary>
        public static char ActivationPrefix
        {
            get => _activationPrefix;
            set
            {
                ConsoleCoreConfig.ReplaceChar(_activationPrefix, value);
                _activationPrefix = value;
            }
        }
        private static char _activationPrefix = '$';

        /// <summary>
        /// Character used to Start the Content section of the Environment Expander
        /// </summary>
        [Property("environmentvariables.syntax.open")]
        public static char OpenBracket = '(';

        /// <summary>
        /// Character used to End the Content section of the Environment Expander
        /// </summary>
        [Property("environmentvariables.syntax.close")]
        public static char CloseBracket = ')';

        /// <summary>
        /// All Providers
        /// </summary>
        public static readonly Dictionary<string, VariableProvider> Providers =
            new Dictionary<string, VariableProvider>();

        /// <summary>
        /// Returns all Environment Providers
        /// </summary>
        /// <returns>; Seperated List of Providers</returns>
        public static string GetEnvironmentList()
        {
            string s = "";
            List<string> keys = Providers.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                string instanceProvider = keys[i];
                s += instanceProvider;
                if (i != keys.Count - 1)
                {
                    s += "; ";
                }
            }
            return s;
        }

        /// <summary>
        /// Adds all Static Methods of a Type that have one string input and string return as VariableProviders.
        /// </summary>
        /// <param name="funcPrefix">The Desired prefix</param>
        /// <param name="qualifiedType">The Qualified Assembly Name of the Type</param>
        [Command("add-env-api", "Adds all public static methods with valid return and one string parameter.")]
        public static void AddStringTransformMethods(string funcPrefix, string qualifiedType)
        {
            Type t = Type.GetType(qualifiedType);

            if (t == null)
            {
                EnvInitializer.Logger.LogWarning("Can not find Type with name: " + qualifiedType);
                return;
            }
            AddStringTransformMethods(funcPrefix, t);
        }

        /// <summary>
        /// Adds all Static Methods of a Type that have one string input and string return as VariableProviders.
        /// </summary>
        /// <param name="funcPrefix">The Desired prefix</param>
        /// <param name="type">The Type</param>
        public static void AddStringTransformMethods(string funcPrefix, Type type)
        {
            MethodInfo[] mi = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            foreach (MethodInfo methodInfo in mi)
            {
                if (ValidType(methodInfo.ReturnType))
                {
                    if (methodInfo.GetParameters().Length == 1 &&
                        methodInfo.GetParameters()[0].ParameterType == typeof(string))
                    {
                        AddProvider(GetProvider(funcPrefix, methodInfo));
                    }
                }
            }
        }

        /// <summary>
        /// Returns True if the Type is a string or a Primitive
        /// </summary>
        /// <param name="t">Type to Check</param>
        /// <returns>True if the Type is valid as VariableProvider return/input</returns>
        private static bool ValidType(Type t)
        {
            return t.IsPrimitive || t == typeof(string);
        }

        /// <summary>
        /// Returns the Variable provider from a MethodInfo Class
        /// </summary>
        /// <param name="funcPrefix">Desired FuncName</param>
        /// <param name="info">The Method that will provide the values.</param>
        /// <returns></returns>
        public static VariableProvider GetProvider(string funcPrefix, MethodInfo info)
        {
            return new DelegateVariableProvider(funcPrefix + info.Name, s => info.Invoke(null, new[] {s})?.ToString());
        }

        /// <summary>
        /// Expands the Input String with the VariableProvider Implementations
        /// </summary>
        /// <param name="cmd">Input String</param>
        /// <returns>Expanded Output String</returns>
        public static string Expand(string cmd)
        {
            string ret = cmd;
            int idx;
            int start = 0;
            while ((idx = ret.IndexOf(ActivationPrefix, start)) != -1)
            {
                if (CommandParser.IsEscaped(ret, idx))
                {
                    start = idx + 1;
                    continue;
                }
                else
                {
                    start = 0;
                }
                int bracketOpen = ret.IndexOf(OpenBracket, idx);
                if (bracketOpen == -1)
                {
                    return cmd;
                }
                int funcLen = bracketOpen - idx - 1;
                if (funcLen < 0)
                {
                    return cmd;
                }
                string funcName = ret.Substring(idx + 1, funcLen);
                int bracketClose = ConsoleCoreConfig.FindClosing(ret, OpenBracket, CloseBracket, bracketOpen);
                if (bracketClose == -1)
                {
                    return cmd;
                }
                string content = ret.Substring(bracketOpen + 1, bracketClose - bracketOpen - 1);
                string expandedContent = Expand(content);
                string exp;

                string rep = $"${funcName}{OpenBracket}{content}{CloseBracket}";
                if (Providers.TryGetValue(funcName, out VariableProvider prov))
                {
                    exp = prov.GetValue(expandedContent);
                }
                else
                {
                    exp = "PARSE_ERROR";
                }
                ret = ret.Replace(rep, exp);
            }

            return ret;
        }


        /// <summary>
        /// Adds a Provider to the EnvironmentVariable Manager.
        /// </summary>
        /// <param name="provider">Provider to Add</param>
        public static void AddProvider(VariableProvider provider)
        {
            Providers[provider.FunctionName] = provider;
        }

        /// <summary>
        /// Removes a Provider from the EnvironmentVariable Manager.
        /// </summary>
        /// <param name="funcName">Name of the Provider to Remove</param>
        public static void RemoveProvider(string funcName)
        {
            Providers.Remove(funcName);
        }
    }
}