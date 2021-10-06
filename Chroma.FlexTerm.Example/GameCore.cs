using System.Numerics;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.SabreVGA;

namespace Chroma.FlexTerm.Example
{
    public class GameCore : Game
    {
        private Terminal _terminal;

        public GameCore() : base(new(false, false))
        {
            Window.Mode.SetWindowed(1024, 640, true);
        }

        protected override void LoadContent()
        {
            _terminal = new Terminal(
                Vector2.Zero,
                Window.Size,
                TerminalFont.ToshibaSat_8x14
            );

            ClearToColor(
                _terminal.VgaScreen, 
                new Color(0, 0, 0), 
                new Color(192, 192, 192)
            );

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

            Window.Title = PerformanceCounter.FPS.ToString();
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

        private void ClearToColor(VgaScreen screen, Color bg, Color fg)
        {
            screen.ActiveForegroundColor = fg;
            screen.ActiveBackgroundColor = bg;
            
            for (var i = 0; i < screen.TotalColumns * screen.TotalRows; i++)
            {
                screen[i].Background = bg;
                screen[i].Foreground = fg;
            }
        }
    }
}