using System;
using System.Collections.Generic;
using System.Text;
/*
 * 
Num files to execute : 1
ready to execute png: zu2dna35.3xh..graph.png 
CONSOLE: %c 12:39:35 (424) :%cDEBUG:  color:grey; color: green; Initializing mermaid (from line # in "")
CONSOLE: %c 12:39:35 (425) :%cDEBUG:  color:grey; color: green; Initializing mermaidAPI (from line # in "")
CONSOLE: %c 12:39:35 (425) :%cDEBUG:  color:grey; color: green; Setting conf  sequenceDiagram - useMaxWidth (from line # in "")
CONSOLE: %c 12:39:35 (425) :%cDEBUG:  color:grey; color: green; Setting config: sequenceDiagram useMaxWidth to false (from line # in "")
CONSOLE: %c 12:39:35 (425) :%cDEBUG:  color:grey; color: green; Setting conf  flowchart - useMaxWidth (from line # in "")
CONSOLE: %c 12:39:35 (425) :%cDEBUG:  color:grey; color: green; Setting config: flowchart useMaxWidth to false (from line # in "")
CONSOLE: %c 12:39:35 (424) :%cDEBUG:  color:grey; color: green; Starting rendering diagrams (from line # in "")
CONSOLE: %c 12:39:35 (424) :%cDEBUG:  color:grey; color: green; Start On Load before: undefined (from line # in "")
CONSOLE: %c 12:39:35 (425) :%cDEBUG:  color:grey; color: green; Initializing mermaidAPI (from line # in "")
CONSOLE: %c 12:39:35 (502) :%cDEBUG:  color:grey; color: green; Drawing flowchart (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... A B (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... B C (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... C D1 (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... C E2 (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... D1 E1 (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... E1 F1 (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... E2 F2 (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... F1 Z (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... F2 Z (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... A B (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... B C (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... C D1 (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... C E2 (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... D1 E1 (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... E1 F1 (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... E2 F2 (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... F1 Z (from line # in "")
CONSOLE: %c 12:39:35 (426) :%cINFO:  color:grey; color: blue; Got edge... F2 Z (from line # in "")
saved png: zu2dna35.3xh..graph.png

*/
namespace Mermaider.Core
{
    public static class RawResultParser
    {
        internal static Tuple<List<string>, List<string>> ParseStdOut(string consoleStdOut)
        {
            var infos = new List<string>();
            var errors = new List<string>();
            var splits = consoleStdOut.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var split in splits)
            {
                if (split.StartsWith("CONSOLE", StringComparison.OrdinalIgnoreCase) == false)
                {
                    infos.Add(TrimLeadingNoise(split));
                }
                else
                {
                    if (split.IndexOf("error", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        errors.Add(TrimLeadingNoise(split));
                    }
                    else
                    {
                        infos.Add(TrimLeadingNoise(split));
                    }
                }
            }



            return new Tuple<List<string>, List<string>>(infos, errors);
        }

        private static string TrimLeadingNoise(string noisyString)
        {
            var indexOfFirstColor = noisyString.IndexOf("color", StringComparison.OrdinalIgnoreCase);
            if (indexOfFirstColor < 0) { return noisyString;}

            var indexOfSecondColor = noisyString.IndexOf("color", indexOfFirstColor +1, StringComparison.OrdinalIgnoreCase);
            if (indexOfSecondColor< 0) { return noisyString; }

            var indexOfSemicolonAfterSecondColor = noisyString.IndexOf(";", indexOfSecondColor, StringComparison.OrdinalIgnoreCase);
            if (indexOfSemicolonAfterSecondColor < 0) { return noisyString; }

            var importantPart = noisyString.Substring(indexOfSemicolonAfterSecondColor + 1);
            return importantPart.Trim();
        }


    }
}
