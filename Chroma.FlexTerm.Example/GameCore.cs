using System.Numerics;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Input;

namespace Chroma.FlexTerm.Example
{
    public class GameCore : Game
    {
        private Terminal _terminal;

        public GameCore() : base(new(false, false))
        {
            Window.GoWindowed(new(1024, 660));
        }

        protected override void LoadContent()
        {
            _terminal = new Terminal(
                Vector2.Zero,
                Window.Size,
                TerminalFont.IBM_VGA_8x16
            );

            _terminal.InputReceived += (_, args) =>
            {
                if (args.Text == "æ")
                {
                    _terminal.Write("<insert random kid having their fingers amputated by car door here>\n");
                }
            };

            var cp437 = "A quick brown fox jumps over the lazy dog.\n" +
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
                        "⌡\n\n";

            var plus = "* Quizdeltagerne spiste jordbær med fløde, mens cirkusklovnen Wolther spillede på xylofon.\n" +
                       "* Ταχίστη αλώπηξ βαφής ψημένη γη, δρασκελίζει υπέρ νωθρού κυνός.\n" +
                       "* Le cœur déçu mais l'âme plutôt naïve, Louÿs rêva de crapaüter en canoë au delà des îles, près du mälström où brûlent les novæ.\n" +
                       "* D'fhuascail Íosa, Úrmhac na hÓighe Beannaithe, pór Éava agus Ádhaimh.\n" +
                       "* Árvíztűrő tükörfúrógép.\n" +
                       "* Kæmi ný öxi hér ykist þjófum nú bæði víl og ádrepa.\n" +
                       "* Pchnąć w tę łódź jeża lub ośm skrzyń fig\n" +
                       "* В чащах юга жил бы цитрус? Да, но фальшивый экземпляр!\n" +
                       "* Pijamalı hasta, yağız şoföre çabucak güvendi.\n" +
                       "* »TECHNICIÄNS ÖF SPÅCE SHIP EÅRTH  THIS IS YÖÜR CÄPTÅIN SPEÄKING  YÖÜR ØÅPTÅIN IS DEĂD«\n";

            _terminal.Write(cp437);
            _terminal.Write(plus);
        }

        protected override void Update(float delta)
        {
            _terminal.Update(delta);
            if (!_terminal.IsReadingInput)
            {
                _terminal.Write("C:\\>");
                _terminal.ReadLine();
            }

            for (var y = _terminal.VgaScreen.Margins.Top; y <= _terminal.VgaScreen.WindowRows; y++)
            {
                for (var x = _terminal.VgaScreen.Margins.Left; x <= _terminal.VgaScreen.WindowColumns; x++)
                {
                    _terminal.VgaScreen[x, y].Foreground = new(
                        (byte)((2 * y + (int)PerformanceCounter.LifetimeFrames) % 256),
                        (byte)((4 * y + (int)PerformanceCounter.LifetimeFrames) % 256),
                        (byte)((6 * y + (int)PerformanceCounter.LifetimeFrames) % 256)
                    );
                }
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