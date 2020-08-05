using System;
using System.Collections.Generic;
using System.Linq;

namespace Console.Core.Commands.ConverterSystem
{




    public static class CustomConvertManager
    {
        private static List<AConverter> Converters = new List<AConverter>();

        public static void AddConverter(AConverter converter)
        {
            Converters.Add(converter);
        }

        public static bool CanConvert(object parameter, Type target) =>
            Converters.Any(x => x.CanConvert(parameter, target));

        public static object Convert(object parameter, Type target)
        {
            AConverter conv = Converters.FirstOrDefault(x => x.CanConvert(parameter, target));
            if (conv != null)
            {
                return conv.Convert(parameter, target);
            }
            return parameter;
        }
    }
}