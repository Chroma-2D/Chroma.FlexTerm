﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Input;
using Chroma.SabreVGA;

namespace Chroma.FlexTerm
{
    public class Terminal
    {
        public VgaScreen VgaScreen { get; }
        
        private Queue<char> _inputQueue = new();
        private List<char> _inputBuffer = new();
        private int _inputBufferIndex;

        private bool _pendingVisualUpdate;
        private int _inputStartX;
        private int _inputStartY;

        private bool _isReadingKey;
        private bool _isReadingChar;
        private bool _isReadingString;

        private Dictionary<char, Action<Terminal>> _controlCodes = new();

        public bool IsReadingInput => _isReadingChar || _isReadingString;
        public bool EchoInput { get; set; } = true;

        public ConsoleFont Font => VgaScreen.Font;

        public Vector2 Position
        {
            get => VgaScreen.Position;
            set => VgaScreen.Position = value;
        }

        public Size Size
        {
            get => VgaScreen.Size;
            set => VgaScreen.Size = value;
        }

        public event EventHandler<TerminalInputEventArgs> InputReceived;

        public Terminal(VgaScreen vgaScreen)
        {
            VgaScreen = vgaScreen;
        }

        public Terminal(VgaScreen vgaScreen, TerminalFont font)
        {
            VgaScreen = vgaScreen;
            VgaScreen.Font = new ConsoleFont(LoadEmbeddedFont(font));

            var cellSize = DetermineCellSizeForFont(font);
            VgaScreen.SetCellSizes(cellSize.Width, cellSize.Height);
        }

        public Terminal(Vector2 position, Size size, TerminalFont font)
        {
            var cellSize = DetermineCellSizeForFont(font);

            VgaScreen = new VgaScreen(
                position,
                size,
                new ConsoleFont(LoadEmbeddedFont(font)),
                cellSize.Width,
                cellSize.Height
            );

            VgaScreen.Cursor.Shape = CursorShape.Underscore;
            SetDefaultControlCodes();
        }

        public void SetControlCode(char c, Action<Terminal> handler)
        {
            if (!_controlCodes.TryAdd(c, handler))
                _controlCodes[c] = handler;
        }

        public void UnSetControlCode(char c)
        {
            if (_controlCodes.ContainsKey(c))
                _controlCodes.Remove(c);
        }

        public void ReadKey()
        {
            if (IsReadingInput)
                return;

            _inputStartX = VgaScreen.Cursor.X;
            _inputStartY = VgaScreen.Cursor.Y;

            _isReadingKey = true;
        }

        public void Read()
        {
            if (IsReadingInput)
                return;

            _inputStartX = VgaScreen.Cursor.X;
            _inputStartY = VgaScreen.Cursor.Y;

            _isReadingChar = true;
        }

        public void ReadLine()
        {
            if (IsReadingInput)
                return;

            _inputStartX = VgaScreen.Cursor.X;
            _inputStartY = VgaScreen.Cursor.Y;

            _isReadingString = true;
        }

        public void Write(char c)
        {
            if (_controlCodes.ContainsKey(c))
            {
                _controlCodes[c](this);
            }
            else
            {
                Printable(c);
            }
        }

        public void Write(string s)
        {
            foreach (var c in s)
                Write(c);
        }

        public void Draw(RenderContext context)
        {
            VgaScreen.Draw(context);
        }

        public void Update(float delta)
        {
            VgaScreen.Cursor.ForceHidden = !IsReadingInput;
            VgaScreen.Update(delta);

            if (_pendingVisualUpdate)
            {
                UpdateInputVisual(false);
                _pendingVisualUpdate = false;
            }
        }

        public void FixedUpdate(float delta)
        {
            while (_inputQueue.Any())
            {
                var c = _inputQueue.Dequeue();

                if (_isReadingChar)
                {
                    InputReceived?.Invoke(this, new TerminalInputEventArgs(c.ToString()));
                    _isReadingChar = false;
                }
                else if (_isReadingString)
                {
                    _inputBuffer.Insert(_inputBufferIndex, c);
                    _inputBufferIndex++;
                }

                if (EchoInput)
                {
                    Write(c);
                    _pendingVisualUpdate = true;
                }
            }
        }

        public void TextInput(TextInputEventArgs e)
        {
            if (IsReadingInput && !_isReadingKey)
            {
                foreach (var c in e.Text)
                {
                    _inputQueue.Enqueue(c);
                }
            }
        }

        public void KeyPressed(KeyEventArgs e)
        {
            if (_isReadingKey)
            {
                _isReadingKey = false;
                
                InputReceived?.Invoke(this, new TerminalInputEventArgs(e.KeyCode));
                return;
            }
            
            switch (e.KeyCode)
            {
                case KeyCode.Return:
                    Write('\n');
                    FlushInputBuffer();
                    break;

                case KeyCode.Backspace:
                    Write('\b');
                    break;

                case KeyCode.Delete:
                    Write((char)0x7F);
                    break;

                case KeyCode.Home:
                    Write((char)0x02);
                    break;

                case KeyCode.End:
                    Write((char)0x03);
                    break;

                case KeyCode.Left:
                    if (_inputBufferIndex - 1 < 0)
                        break;

                    _inputBufferIndex--;
                    RetractCursor();

                    break;

                case KeyCode.Right:
                    if (_inputBufferIndex + 1 > _inputBuffer.Count)
                        break;

                    _inputBufferIndex++;
                    AdvanceCursor();

                    break;
            }
        }

        protected void AdvanceCursor()
        {
            if (VgaScreen.Cursor.X >= VgaScreen.WindowColumns)
            {
                NextLine();
            }
            else
            {
                VgaScreen.Cursor.X++;
            }
        }

        protected void RetractCursor()
        {
            if (VgaScreen.Cursor.X - 1 >= VgaScreen.Margins.Left)
            {
                VgaScreen.Cursor.X--;
            }
            else
            {
                if (VgaScreen.Cursor.Y - 1 < VgaScreen.Margins.Top)
                    return;

                VgaScreen.Cursor.Y--;
                VgaScreen.Cursor.X = VgaScreen.WindowColumns;
            }
        }

        protected void NextLine()
        {
            VgaScreen.Cursor.X = 0;

            if (VgaScreen.Cursor.Y >= VgaScreen.WindowRows)
            {
                VgaScreen.Scroll();

                if (IsReadingInput)
                {
                    if (_inputStartY - 1 >= 0)
                        _inputStartY--;
                }
            }
            else
            {
                VgaScreen.Cursor.Y++;
            }
        }

        private void UpdateInputVisual(bool clear)
        {
            if (!EchoInput)
                return;

            var realX = VgaScreen.Cursor.X;
            var realY = VgaScreen.Cursor.Y;

            VgaScreen.Cursor.X = _inputStartX;
            VgaScreen.Cursor.Y = _inputStartY;

            if (clear)
            {
                for (var i = 0; i < _inputBuffer.Count; i++)
                    Printable(' ');
            }
            else
            {
                foreach (var c in _inputBuffer)
                    Printable(c);
            }
            VgaScreen.Cursor.X = realX;
            VgaScreen.Cursor.Y = realY;
        }

        private void Printable(char c)
        {
            bool hasGlyph;
            if (Font.IsBitmapFont)
            {
                hasGlyph = Font.BitmapFont.HasGlyph(c);
            }
            else
            {
                hasGlyph = Font.TrueTypeFont.HasGlyph(c);
            }

            if (!hasGlyph)
            {
                // Assumes font has ^?[] and digit glyphs at least.
                Write("^?[" + ((int)c).ToString("X2") + "]");
            }
            else
            {
                VgaScreen.PutCharAt(
                    c,
                    VgaScreen.Cursor.X,
                    VgaScreen.Cursor.Y
                );
                    
                AdvanceCursor();
            }
        }

        private void FlushInputBuffer()
        {
            if (_isReadingString)
            {
                _isReadingString = false;

                var str = new string(_inputBuffer.ToArray());
                
                _inputBufferIndex = 0;
                _inputBuffer.Clear();

                InputReceived?.Invoke(this, new TerminalInputEventArgs(str));
            }
        }

        private void Backspace()
        {
            if (!_isReadingString)
                return;

            if (_inputBufferIndex - 1 >= 0)
            {
                UpdateInputVisual(true);

                _inputBuffer.RemoveAt(--_inputBufferIndex);
                RetractCursor();

                _pendingVisualUpdate = true;
            }
        }

        private void Delete()
        {
            if (!_isReadingString)
                return;

            if (_inputBufferIndex < _inputBuffer.Count)
            {
                UpdateInputVisual(true);
                _inputBuffer.RemoveAt(_inputBufferIndex);

                _pendingVisualUpdate = true;
            }
        }

        private void StartOfInput()
        {
            if (!_isReadingString)
                return;

            while (_inputBufferIndex != 0)
            {
                _inputBufferIndex--;
                RetractCursor();
            }
        }

        private void EndOfInput()
        {
            if (!_isReadingString)
                return;

            while (_inputBufferIndex < _inputBuffer.Count)
            {
                _inputBufferIndex++;
                AdvanceCursor();
            }
        }

        private TrueTypeFont LoadEmbeddedFont(TerminalFont font)
        {
            var embeddedResourceString = $"Chroma.FlexTerm.Resources.Fonts.{font.ToString()}.ttf";

            using var assemblyResourceStream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream(embeddedResourceString);

            var ttf = new TrueTypeFont(
                assemblyResourceStream,
                DetermineFontSize(font),
                new string(CodePage.BuildCodePage437())
            )
            {
                ForceAutoHinting = false,
            };

            ttf.Atlas.FilteringMode = TextureFilteringMode.NearestNeighbor;
            return ttf;
        }

        private Size DetermineCellSizeForFont(TerminalFont font)
        {
            return font switch
            {
                TerminalFont.Acer_9x14 => new(9, 14),
                TerminalFont.ATI_9x16 => new(9, 16),
                TerminalFont.IGS_8x16 => new(8, 16),
                TerminalFont.MBytePC_8x16 => new(8, 16),
                TerminalFont.Tandy1K_II_9x14 => new(9, 14),
                TerminalFont.ToshibaSat_8x14 => new(8, 14),
                TerminalFont.Trident_8x16 => new(8, 16),
                TerminalFont.Copam_8x8 => new(8, 8),
                _ => new(8, 16)
            };
        }

        private int DetermineFontSize(TerminalFont font)
        {
            return font switch
            {
                TerminalFont.Copam_8x8 => 8,
                _ => 16
            };
        }

        private void SetDefaultControlCodes()
        {
            SetControlCode('\n', (t) => { t.NextLine(); });
            SetControlCode('\r', (t) => { t.VgaScreen.Cursor.X = t.VgaScreen.Margins.Left; });
            SetControlCode('\b', (t) => { t.Backspace(); });
            SetControlCode((char)0x02, (t) => { t.StartOfInput(); });
            SetControlCode((char)0x03, (t) => { t.EndOfInput(); });
            SetControlCode((char)0x7F, (t) => { t.Delete(); });
        }
    }
}