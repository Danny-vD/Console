using System;
using System.IO;
using Console.Core.ConverterSystem;

namespace Console.DefaultConverters
{
    public class DirInfoConverter : AConverter
    {
        public override bool CanConvert(object parameter, Type target)
        {
            return parameter is string && target == typeof(DirectoryInfo);
        }

        public override object Convert(object parameter, Type target)
        {
            return new DirectoryInfo(parameter.ToString());
        }
    }
}