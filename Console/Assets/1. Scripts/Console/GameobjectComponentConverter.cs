using System;
using Console.Core.Commands.ConverterSystem;
using UnityEngine;

public class GameObjectComponentConverter : AConverter
{
    public override bool CanConvert(object parameter, Type target)
    {
        return typeof(Component).IsAssignableFrom(target) && parameter is GameObject;
    }

    public override object Convert(object parameter, Type target)
    {
        GameObject obj = parameter as GameObject;

        return obj.GetComponent(target);
    }
}
