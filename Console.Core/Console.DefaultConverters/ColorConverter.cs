using System;
using System.Drawing;
using System.Globalization;
using Console.Core.ConverterSystem;

namespace Console.DefaultConverters
{
    public class ColorConverter : AConverter
    {

        public override bool CanConvert(object parameter, Type target)
        {
            return parameter is string s && target == typeof(Color);
        }

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

        public byte Convert(string hexCode)
        {
            int val = int.Parse(hexCode, NumberStyles.HexNumber);
            if (val >= 0 && val <= 255) return (byte)val;
            throw new FormatException("Hex code is not in the right format");
        }
    }
}