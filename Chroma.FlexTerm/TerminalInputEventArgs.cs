using System;
using Chroma.Input;

namespace Chroma.FlexTerm
{
    public class TerminalInputEventArgs : EventArgs
    {
        public InputType Type { get; }
        
        public KeyCode? Key { get; }
        public string Text { get; }

        internal TerminalInputEventArgs(string text)
        {
            Text = text;
            Type = InputType.Text;
        }

        internal TerminalInputEventArgs(KeyCode key)
        {
            Key = key;
            Type = InputType.Key;
        }
    }
}