using System;
using System.Collections.Generic;
using System.Reflection;
using Chroma.Graphics.TextRendering;
using Chroma.Graphics.TextRendering.TrueType;

namespace Chroma.FlexTerm
{
    public static class TerminalFontStore
    {
        private static Dictionary<TerminalFont, IFontProvider> _fontCache = new();

        public static IFontProvider FetchEmbeddedFont(TerminalFont terminalFont)
            => FetchEmbeddedFont(terminalFont, false);

        public static void DestroyCache()
        {
            lock (_fontCache)
            {
                foreach (var (_, font) in _fontCache)
                {
                    ((TrueTypeFont)font).Dispose();
                }

                _fontCache.Clear();
            }
        }

        private static IFontProvider FetchEmbeddedFont(TerminalFont terminalFont, bool retrying)
        {
            lock (_fontCache)
            {
                if (_fontCache.TryGetValue(terminalFont, out var fontObject))
                {
                    return fontObject;
                }

                var embeddedResourceString = $"Chroma.FlexTerm.Resources.Fonts.{terminalFont.ToString()}.ttf";

                using var assemblyResourceStream = Assembly
                    .GetExecutingAssembly()
                    .GetManifestResourceStream(embeddedResourceString);

                if (assemblyResourceStream == null)
                {
                    if (!retrying)
                        return FetchEmbeddedFont(TerminalFont.ToshibaSat_8x14, true);

                    throw new ArgumentOutOfRangeException(
                        nameof(terminalFont),
                        "Couldn't find the requested embedded font and the fallback has failed."
                    );
                }

                fontObject = new TrueTypeFont(
                    assemblyResourceStream,
                    DetermineFontHeight(terminalFont),
                    new string(BuildCodePage(terminalFont))
                );

                _fontCache.Add(terminalFont, fontObject);

                return fontObject;
            }
        }
        
        private static int DetermineFontHeight(TerminalFont font)
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
        
        private static char[] BuildCodePage(TerminalFont font)
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
    }
}