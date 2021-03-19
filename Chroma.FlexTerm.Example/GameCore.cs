using System.IO;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Input;

namespace Chroma.FlexTerm.Example
{
    public class GameCore : Game
    {
        private Terminal _terminal;

        public GameCore() : base(new(false, false))
        {
            Window.GoWindowed(new(640,480));
        }

        protected override void LoadContent()
        {
            _terminal = new Terminal(
                Vector2.Zero,
                Window.Size,
                TerminalFont.Copam_8x8
            );

            _terminal.InputReceived += (sender, args) =>
            {
                if (args.Text == "yeet")
                {
                    _terminal.Write("I like.\n");
                }
            };

            _terminal.Write(
                "A quick brown fox jumps over the lazy dog.\n" +
                "0123456789 ¿?¡!`'\"., <>()[]{} &@%*^#$\\/\n" +
                "\n" +
                "* Wieniläinen sioux'ta puhuva ökyzombie diggaa Åsan roquefort-tacoja.\n" +
                "* Ça me fait peur de fêter noël là, sur cette île bizarroïde où une mère et sa môme essaient de me tuer avec un gâteau à la cigüe brûlé.\n" +
                "* Zwölf Boxkämpfer jagten Eva quer über den Sylter Deich.\n" +
                "* El pingüino Wenceslao hizo kilómetros bajo exhaustiva lluvia y frío, añoraba a su querido cachorro.\n" +
                "┌─┬─┐ ╔═╦═╗ ╒═╤═╕ ╓─╥─╖\n" +
                "│ │ │ ║ ║ ║ │ │ │ ║ ║ ║\n" +
                "├─┼─┤ ╠═╬═╣ ╞═╪═╡ ╟─╫─╢\n" +
                "└─┴─┘ ╚═╩═╝ ╘═╧═╛ ╙─╨─╜\n" +
                "\n" +
                "░░░░░ ▐▀█▀▌ .·∙•○°○•∙·.\n" +
                "▒▒▒▒▒ ▐ █ ▌ ☺☻ ♥♦♣♠ ♪♫☼\n" +
                "▓▓▓▓▓ ▐▀█▀▌  $ ¢ £ ¥ ₧\n" +
                "█████ ▐▄█▄▌ ◄►▲▼ ←→↑↓↕↨\n" +
                "\n" +
                "⌠\n" +
                "│dx ≡ Σ √x²ⁿ·δx\n" +
                "⌡\n\n"
            );
        }

        protected override void Update(float delta)
        {
            _terminal.Update(delta);

            if (!_terminal.IsReadingInput)
            {
                _terminal.Write("C:\\>");
                _terminal.ReadLine();
            }
        }

        protected override void FixedUpdate(float delta)
        {
            _terminal.FixedUpdate(delta);
        }

        protected override void Draw(RenderContext context)
        {
            _terminal.Draw(context);
        }

        protected override void TextInput(TextInputEventArgs e)
        {
            _terminal.TextInput(e);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            _terminal.KeyPressed(e);
        }
    }
}