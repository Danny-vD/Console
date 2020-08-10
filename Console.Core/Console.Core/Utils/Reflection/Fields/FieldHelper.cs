﻿using System;
using System.Collections;
using System.Reflection;

namespace Console.Core.Utils.Reflection.Fields
{
    public class FieldHelper : ReflectionHelper
    {
        public override bool CanWrite => !Info.IsInitOnly;
        internal FieldInfo Info { get; }
        internal object Instance { get; }

        internal FieldHelper(object instance, FieldInfo info)
        {
            Instance = instance;
            Info = info;
        }

        public override object GetValue()
        {
            return Info.GetValue(Instance);
        }

        public override void SetValue(object value)
        {
            object convertedValue = null;
            if (value is Array ar && !typeof(IEnumerable).IsAssignableFrom(Info.FieldType))
            {
                convertedValue = ar.GetValue(0);
            }
            else
            {
                convertedValue = CommandAttributeUtils.ConvertToNonGeneric(value, Info.FieldType);
            }

            Info.SetValue(Instance, convertedValue);
        }
    }
}