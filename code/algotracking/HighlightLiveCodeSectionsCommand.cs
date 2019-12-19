using CompGeomVis.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class HighlightLiveCodeSectionsCommand : AlgorithmStateCommand
    {
        public int AlgorithmId { get; set; }
        public List<string> Sections { get; set; }

        public HighlightLiveCodeSectionsCommand()
        {

        }

        public HighlightLiveCodeSectionsCommand(int algId, params string[] sections)
        {
            AlgorithmId = algId;
            Sections = new List<string>(sections);
        }

        public override void apply()
        {
            EventBus.Publish<HighlightLiveCode>(new HighlightLiveCode {
                AlgorithmId = AlgorithmId,
                Sections = Sections
            });
        }
    }
}
