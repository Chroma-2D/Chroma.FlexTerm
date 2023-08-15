using System;
using Chroma.Input;

namespace Chroma.FlexTerm
{
    public class TerminalInputEventArgs : EventArgs
    {
        public InputType Type { get; }
        
        public KeyCode? Key { get; }
        public string? Text { get; }
        public string[]? Tokens { get; }

        internal TerminalInputEventArgs(string text, string[] tokens)
        {
            Text = text;
            Type = InputType.Text;
            Tokens = tokens;
        }

        internal TerminalInputEventArgs(KeyCode key)
        {
            Key = key;
            Type = InputType.Key;
        }
    }
}