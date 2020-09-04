using System;
using System.Windows.Forms;
using Console.Core.CommandSystem.Builder;

namespace Console.Form
{
    public class FormsCommandBuilderInput : ICommandBuilderInput
    {
        public bool Abort { get; set; }
        private readonly TextBox Text;
        private bool pressed;
        private ConsoleKeyInfo key;

        public FormsCommandBuilderInput(TextBox text)
        {
            Text = text;
            Text.KeyDown += Text_KeyDown;
            Text.Multiline = true;
            Text.AcceptsTab = true;
        }

        private void Text_KeyDown(object sender, KeyEventArgs e)
        {
            pressed = true;
            char orig = (char)e.KeyValue;
            char v;
            if (orig >= 'A' && orig <= 'Z')
                v = (char)(e.Shift ? orig : orig + 32);
            else
                v = orig;
            key = new ConsoleKeyInfo(v, (ConsoleKey)((int)e.KeyCode), e.Shift, e.Alt, e.Control);
            e.SuppressKeyPress = true;
        }

        public void ResetLine()
        {
            Text.Text = "";
        }

        public void Write(string str)
        {
            Text.Text += str;
        }

        public void SetCursorPosition(int pos)
        {
            Text.Select(pos, 0);
        }

        public void EndLine()
        {
            Text.Clear();
        }

        public ConsoleKeyInfo ReadKey()
        {
            while (!pressed && !Abort)
            {
                Application.DoEvents();
            }

            pressed = false;
            return key;
        }
    }
}