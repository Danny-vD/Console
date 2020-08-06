using System;
using System.IO;
using Console.Core.Commands.ConverterSystem;

namespace Console.DefaultConverters
{
    public class FileInfoConverter : AConverter
    {
        public override bool CanConvert(object parameter, Type target)
        {
            return parameter is string && target == typeof(FileInfo);
        }

        public override object Convert(object parameter, Type target)
        {
            return new FileInfo(parameter.ToString());
        }
    }
}