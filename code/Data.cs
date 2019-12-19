using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using CompGeomVis.models;

namespace CompGeomVis
{
    public class Data
    {
        public static List<AlgorithmModel> Algorithms { get; set; }
        public static List<DemoModel> WorkSpace { get; set; }

        static Data()
        {
            WorkSpace = new List<DemoModel>();
        }

        public static AlgorithmModel FindAlgoById(int id)
        {
            foreach (var a in Algorithms)
                if (a.AlgorithmId == id)
                    return a;

            return null;
        }

        public static void LoadWorkspace()
        {
            string fn = @"comp_geom_workspace.json";

            if(File.Exists(fn))
            {
                WorkSpace = JsonConvert.DeserializeObject<List<DemoModel>>(File.ReadAllText(fn));
            }
        }

        public static void SaveWorkspace()
        {
            if (WorkSpace == null)
                return;

            string fn = @"comp_geom_workspace.json";

            File.WriteAllText(fn, JsonConvert.SerializeObject(WorkSpace, Formatting.Indented));
        }

        public static void Load()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "CompGeomVis.algorithm_text.txt";

            Algorithms = new List<AlgorithmModel>();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                var lines = result.Split('\n');

                int i = 1;
                int totalAlgs = Int32.Parse(lines[0].Trim());
                int a = 0;

                while (i < lines.Length && !lines[i].StartsWith("~"))
                {
                    var alg = AlgorithmModel.Parse(lines, ref i);

                    alg.ProcessLiveCode();
                    alg.ProcessPseudoCode();

                    Algorithms.Add(alg);

                    a++;
                    if (a == totalAlgs)
                        break;
                }
            }
        }
    }
}
