using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Input;
using Chroma.MemoryManagement;
using Chroma.SabreVGA;

namespace Chroma.FlexTerm
{
    public class Terminal : DisposableResource
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

        public IFontProvider Font => VgaScreen.Font;
        public event EventHandler<TerminalInputEventArgs> InputReceived;

        public Terminal(VgaScreen vgaScreen)
        {
            VgaScreen = vgaScreen;
        }

        public Terminal(VgaScreen vgaScreen, TerminalFont font)
            : this(vgaScreen)
        {
            VgaScreen.Font = LoadEmbeddedFont(font);
            var cellSize = DetermineCellSizeForFont(font);
            
            VgaScreen.SetCellSizes(cellSize.Width, cellSize.Height);
            VgaScreen.RecalculateDimensions();
        }

        public Terminal(Vector2 position, Size size, TerminalFont font)
        {
            var cellSize = DetermineCellSizeForFont(font);

            VgaScreen = new VgaScreen(
                position,
                size,
                LoadEmbeddedFont(font),
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


        public void Backspace()
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

        public void Delete()
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

        public void StartOfInput()
        {
            if (!_isReadingString)
                return;

            while (_inputBufferIndex != 0)
            {
                _inputBufferIndex--;
                RetractCursor();
            }
        }

        public void EndOfInput()
        {
            if (!_isReadingString)
                return;

            while (_inputBufferIndex < _inputBuffer.Count)
            {
                _inputBufferIndex++;
                AdvanceCursor();
            }
        }

        public void AdvanceCursor()
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

        public void RetractCursor()
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

        public void NextLine()
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

        protected virtual void UpdateInputVisual(bool clear)
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

        protected virtual void Printable(char c)
        {
            if (!Font.HasGlyph(c))
            {
                if (Font.HasGlyph('?'))
                    Write("?");
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

        protected virtual void FlushInputBuffer()
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

        protected TrueTypeFont LoadEmbeddedFont(TerminalFont font)
        {
            var embeddedResourceString = $"Chroma.FlexTerm.Resources.Fonts.{font.ToString()}.ttf";

            using var assemblyResourceStream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream(embeddedResourceString);

            return new TrueTypeFont(
                assemblyResourceStream,
                DetermineFontSize(font),
                new string(BuildCodePage(font))
            );
        }

        protected Size DetermineCellSizeForFont(TerminalFont font)
        {
            return font switch
            {
                TerminalFont.Copam_8x8 => new(8, 8),
                TerminalFont.IBM_CGA_8x8 => new(8, 8),
                TerminalFont.NEC_MultiSpeed_8x8 => new(8, 8),
                TerminalFont.ToshibaSat_8x14 => new(8, 14),
                TerminalFont.PhoenixVGA_8x14 => new(8, 14),
                TerminalFont.Tandy1K_II_9x14 => new(9, 14),
                TerminalFont.EuroPC_9x14 => new(9, 14),
                TerminalFont.Acer_9x14 => new(9, 14),
                TerminalFont.AST_8x19 => new(8, 19),
                _ => new(8, 16)
            };
        }

        protected int DetermineFontSize(TerminalFont font)
        {
            return font switch
            {
                TerminalFont.Copam_8x8 => 8,
                TerminalFont.IBM_CGA_8x8 => 8,
                TerminalFont.NEC_MultiSpeed_8x8 => 8,
                TerminalFont.AST_8x19 => 20,
                _ => 16
            };
        }

        protected virtual char[] BuildCodePage(TerminalFont font)
        {
            return font switch
            {
                TerminalFont.ToshibaSat_8x14 => CodePage.BuildCodePage437Plus(),
                TerminalFont.IBM_VGA_8x16 => CodePage.BuildCodePage437Plus(),
                TerminalFont.AST_8x19 => CodePage.BuildCodePage437Plus(),
                TerminalFont.IBM_CGA_8x8 => CodePage.BuildCodePage437Plus(),
                _ => CodePage.BuildCodePage437()
            };
        }

        protected void SetDefaultControlCodes()
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