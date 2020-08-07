﻿using System;
using Console.Core;
using Console.Core.Attributes.CommandSystem.Helper;

namespace Console.UtilExtension
{
    public class UtilExtensionInitializer : AExtensionInitializer
    {
        public override void Initialize()
        {
            CommandAttributeUtils.AddCommands<ConsoleUtilCommands>();
        }
    }
}