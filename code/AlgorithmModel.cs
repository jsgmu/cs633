using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis
{
    public class AlgorithmModel
    {
        public int AlgorithmIndex { get; set; }
        public int AlgorithmId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string DegenerateCaseText { get; set; }
        public List<string> PseudoCodeText { get; set; }
        public string PseudoCodeTextText { get; set; }
        public List<string> LiveCodeText { get; set; }
        public string LiveCodeTextText { get; set; }
        public Dictionary<string, List<int>> LiveCodeSections { get; set; }
        public Dictionary<string, List<int>> PseudoCodeSections { get; set; }

        public void ProcessLiveCode()
        {
            int line = 0;
            var sb = new StringBuilder();

            var activeSections = new List<string>();
            LiveCodeSections = new Dictionary<string, List<int>>();

            for (int i = 0; i < LiveCodeText.Count; i++)
            {
                var t = LiveCodeText[i].Trim();

                if(t.StartsWith("#start"))
                {
                    var sectionList = t.Substring(7);

                    var bits = sectionList.Split(',');
                    foreach(var b in bits)
                    {
                        var tb = b.Trim();
                        activeSections.Add(tb);
                        if(!LiveCodeSections.ContainsKey(tb))
                        {
                            LiveCodeSections.Add(tb, new List<int>());
                        }
                    }
                } else if(t.StartsWith("#end"))
                {
                    activeSections.Clear();
                }
                else
                {
                    foreach(var s in activeSections)
                    {
                        LiveCodeSections[s].Add(line);
                    }

                    sb.Append(LiveCodeText[i].TrimEnd()).Append("\r\n");
                    line++;
                }
            }

            LiveCodeTextText = sb.ToString();
        }

        public void ProcessPseudoCode()
        {
            int line = 0;
            var sb = new StringBuilder();

            var activeSections = new List<string>();
            PseudoCodeSections = new Dictionary<string, List<int>>();

            for (int i = 0; i < PseudoCodeText.Count; i++)
            {
                var t = PseudoCodeText[i].Trim();

                if (t.StartsWith("#start"))
                {
                    var sectionList = t.Substring(7);

                    var bits = sectionList.Split(',');
                    foreach (var b in bits)
                    {
                        var tb = b.Trim();
                        activeSections.Add(tb);
                        if (!PseudoCodeSections.ContainsKey(tb))
                        {
                            PseudoCodeSections.Add(tb, new List<int>());
                        }
                    }
                }
                else if (t.StartsWith("#end"))
                {
                    activeSections.Clear();
                }
                else
                {
                    foreach (var s in activeSections)
                    {
                        PseudoCodeSections[s].Add(line);
                    }

                    sb.Append(PseudoCodeText[i].TrimEnd()).Append("\r\n");
                    line++;
                }
            }

            PseudoCodeTextText = sb.ToString();
        }
        private static string NextChunk(String[] lines, ref int index)
        {
            var sb = new StringBuilder();

            while (index < lines.Length
                && !lines[index].Trim().Equals("!") 
                && !lines[index].Trim().StartsWith("===")
                && !lines[index].Trim().StartsWith("~"))
            {
                sb.Append(lines[index]);
                sb.Append("\n");
                index++;
            }
            index++;

            return sb.ToString();
        }

        private static List<string> NextChunkAsList(String[] lines, ref int index)
        {
            var list = new List<string>();

            while (index < lines.Length && !lines[index].Trim().Equals("!")
                && !lines[index].Trim().StartsWith("===")
                && !lines[index].Trim().StartsWith("~"))
            {
                list.Add(lines[index]);
                index++;
            }
            index++;

            return list;
        }

        public static AlgorithmModel Parse(String[] lines, ref int index)
        {
            var a = new AlgorithmModel();

            a.AlgorithmIndex = Int32.Parse(lines[index++].Trim());
            a.AlgorithmId = Int32.Parse(lines[index++].Trim());
            a.Name = lines[index++];
            a.ShortDescription = lines[index++];
            a.Description = NextChunk(lines, ref index);
            a.DegenerateCaseText = NextChunk(lines, ref index);
            a.PseudoCodeText = NextChunkAsList(lines, ref index);
            a.LiveCodeText = NextChunkAsList(lines, ref index);

            return a;
        }
    }
}
