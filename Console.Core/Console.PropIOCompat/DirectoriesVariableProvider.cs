﻿using System.IO;
using Console.EnvironmentVariables;

namespace Console.PropIOCompat
{
    public class DirectoriesVariableProvider : VariableProvider
    {
        public override string FunctionName => "dirs";
        public override string GetValue(string parameter)
        {
            return FilesVariableProvider.ToList(Directory.GetDirectories(parameter, "*", SearchOption.TopDirectoryOnly));
        }
    }
}