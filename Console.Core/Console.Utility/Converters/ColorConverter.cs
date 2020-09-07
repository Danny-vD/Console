using System;
using System.Drawing;
using System.Globalization;

using Console.Core.ConverterSystem;

namespace Console.Utility.Converters
{
    /// <summary>
    /// AConverter Implementation that Converts a RRGGBB or AARRGGBB hex string into a C# Color Struct if Prepended with a Hashtag
    /// If a Hashtag is not present the color gets interpreted by name(e.g. Red == FFFF0000 OR FF0000)
    /// </summary>
    public class ColorConverter : AConverter
    {

        /// <summary>
        /// Returns true when the Converter is Able to Convert the parameter into the target type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>True if the conversion can be done</returns>
        public override bool CanConvert(object parameter, Type target)
        {
            return parameter is string s && target == typeof(Color);
        }


        /// <summary>
        /// Converts the Parameter into the Target Type
        /// </summary>
        /// <param name="parameter">Parameter Value</param>
        /// <param name="target">Target Type</param>
        /// <returns>Converted Value</returns>
        public override object Convert(object parameter, Type target)
        {
            string p = parameter as string;
            if (p.StartsWith("#"))
            {
                byte[] color = new byte[4];
                int off = 0;
                if (p.Length == 7) //#RRGGBB
                {
                    color[0] = 255;
                }
                else if (p.Length == 9) //#AARRGGBB
                {
                    color[0] = Convert(p.Substring(1, 2));
                    off = 2;
                }
                else
                {
                    throw new FormatException("Color Hex String is not in the correct format.");
                }


                color[1] = Convert(p.Substring(1 + off, 2));
                color[1] = Convert(p.Substring(3 + off, 2));
                color[1] = Convert(p.Substring(5 + off, 2));

                return Color.FromArgb(color[0], color[1], color[2], color[3]);
            }

            return Color.FromName(p);
        }

        /// <summary>
        /// Converts the Hex Code into a byte
        /// </summary>
        /// <param name="hexCode"></param>
        /// <returns></returns>
        public byte Convert(string hexCode)
        {
            int val = int.Parse(hexCode, NumberStyles.HexNumber);
            if (val >= 0 && val <= 255)
            {
                return (byte) val;
            }

            throw new FormatException("Hex code is not in the right format");
        }

    }
}