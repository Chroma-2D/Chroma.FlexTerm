using System;
using System.Collections.Generic;

namespace Chroma.FlexTerm
{
    public class Tokenizer
    {
        private int _pos;
        private string _input;

        public Tokenizer(string input)
        {
            _input = input;
        }
        
        public string[] GetTokens()
        {
            var tokens = new List<string>();
            
            var token = string.Empty;

            for (; _pos < _input.Length; _pos++)
            {
                var c = _input[_pos];
                switch (c)
                {
                    case '"':
                    case '\'':
                        token = FetchString();
                        tokens.Add(token);
                        token = string.Empty;
                        break;

                    case '\\':
                    {
                        if (_pos + 1 >= _input.Length)
                            token += c;
                        else
                        {
                            _pos++;
                            token += _input[_pos];
                        }
                        break;
                    }
                    
                    case ' ':
                        if (!string.IsNullOrEmpty(token))
                        {
                            tokens.Add(token);
                            token = string.Empty;
                        }
                        break;
                    
                    default:
                        token += c;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(token))
                tokens.Add(token);

            return tokens.ToArray();
        }

        private string FetchString()
        {
            var ret = string.Empty;
            var startChar = _input[_pos++];
    
            while (true)
            {
                if (_pos >= _input.Length)
                    throw new IndexOutOfRangeException("Unterminated string.");
                
                if (_input[_pos] == startChar)
                {
                    break;
                }
                else if (_input[_pos] == '\\')
                {
                    if (_pos < _input.Length)
                    {
                        _pos++;
                        ret += _input[_pos++];
                    }
                }
                else
                {
                    ret += _input[_pos++];
                }
            }

            return ret;
        }
    }
}