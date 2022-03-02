using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;

namespace SandTNTRatioFinderPublic
{
    class Program
    {
        //list of blocks and their heights from bottom
        static Dictionary<string, double> topAligners = new Dictionary<string, double> {
            {"full_block", 0 },
            {"top_trapdoor", 0.8125 },
            {"top_medium_amethyst", 0.75 },
            {"top_large_amethyst", 0.6875 },
            {"amethyst_cluster", 0.5625 },
            {"top_slab", 0.5 },
            {"cocobean_stage_0", 0.4375 },
            {"sideways_chain",  0.40625},
            {"sideways_lightning_rod", 0.375 },
            {"conduit", 0.3125 },
            {"sideways_skull",  0.25},
            {"sideways_amethyst_cluster",  0.1875},
            {"sideways_grindstone", 0.125 },
            {"lantern", 0.0625 },
        };

        //subsititutes for some blocks(currently not used)
        static Dictionary<string, string[]> aliases = new Dictionary<string, string[]>
        {
            {"top_trapdoor", new string[] { "top_small_amethyst" } },
            {"sideways_lightning_rod", new string[] { "sideways_end_rod" } },
            {"conduit", new string[] { "cocobean_stage_1" } },
            {"sideways_skull", new string[] { "sideways_bell", "sideways_hopper", "small_amethyst_sideways" } },
            { "sideways_amethyst_cluster", new string[] { "cocobean_stage_2" } },
        };

        static bool verbose;
        static bool savetofile;

        static int maxgtdiff;
        static int maxgtdrop;
        static double differencethreshhold;
        static int showratios;
        static int guiderdiff;

        static readonly double tntexpp = 0;
        static readonly double sandexpp = 0.833;

        static List<SandTNTSyncRatio> doSyncSim()
        {
            Console.WriteLine("Finding Ratios!");
            int combinations = topAligners.Count * topAligners.Count;
            Console.WriteLine($"Testing {combinations} Guider Combinations");
            int segment = combinations / 50;
            int iterations = 0;
            int ns = 0;
            if (!verbose) Console.WriteLine("Progress: [--------------------------------------------------]");
            Console.Write("           ");
            List<SandTNTSyncRatio> ratios = new List<SandTNTSyncRatio> { };
            foreach (string sta in topAligners.Keys)
            {
                if (verbose) Console.WriteLine($"Sand Top Aligner: {sta}");
                foreach (string tta in topAligners.Keys)
                {
                    if (verbose) Console.WriteLine($"--TNT Top Aligner: {tta}");
                    ns++;
                    if (!verbose && ns > segment)
                    {
                        Console.Write("^");
                        ns = 0;
                    }
                    for (int i = guiderdiff; i > -guiderdiff; i--)
                    {
                        for (int g = maxgtdiff; g >= 0; g--)
                        {
                            double sandpos = topAligners[sta] + 0.02;
                            double sandvel = 0;
                            double tntpos = i + topAligners[tta] + 0.02;
                            double tntvel = 0;
                            for (int tm = 0; tm < g; tm++)
                            {
                                sandvel += -0.04;
                                sandpos += sandvel;
                                sandvel *= 0.98;
                            }

                            for (int t = 1; t < maxgtdrop; t++)
                            {
                                iterations++;
                                double difference = Math.Abs((tntpos + tntexpp) - (sandpos + sandexpp));
                                if (difference <= differencethreshhold)
                                {
                                    if (verbose) Console.WriteLine($"Ration Found: Difference {difference}");
                                    ratios.Add(new SandTNTSyncRatio(0, g, sta, 0, tta, i, t, difference, sandpos + sandexpp, tntpos + tntexpp, sandpos, tntpos, sandvel, tntvel));
                                }
                                sandvel += -0.04;
                                sandpos += sandvel;
                                sandvel *= 0.98;
                                tntvel += -0.04;
                                tntpos += tntvel;
                                tntvel *= 0.98;
                            }
                        }
                    }
                }
            }
            if (!verbose) Console.Write("^");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nTotal Iterations: {iterations}");
            return ratios;
        }

        public static void LoadConfig()
        {
            //load config
            verbose = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("verbose"));
            savetofile = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("savetofile"));

            maxgtdiff = Convert.ToInt32(ConfigurationManager.AppSettings.Get("maxgametickdifference"));
            maxgtdrop = Convert.ToInt32(ConfigurationManager.AppSettings.Get("maxgametickdrop"));
            differencethreshhold = Convert.ToDouble(ConfigurationManager.AppSettings.Get("differencethreshhold"));
            showratios = Convert.ToInt32(ConfigurationManager.AppSettings.Get("showratios"));
            guiderdiff = Convert.ToInt32(ConfigurationManager.AppSettings.Get("maxguiderydifference"));
        }

        static void Main(string[] args)
        {
            LoadConfig();
            List<SandTNTSyncRatio> ratios = doSyncSim();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Ratios Found: {ratios.Count}");
            ratios = ratios.OrderBy(x => x.Difference).Take(showratios).ToList();
            Console.ForegroundColor = ConsoleColor.Green;
            string rstring = "";
            foreach (SandTNTSyncRatio r in ratios)
            {
                rstring+=("\n" + r.ToString().Replace("<x>", (ratios.IndexOf(r) + 1).ToString()));
            }
            Console.WriteLine(rstring);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nSubstitutes:");
            foreach (string s in aliases.Keys)
            {
                Console.WriteLine( s + " => " + string.Join(", ", aliases[s]));
            }
            Console.ForegroundColor = ConsoleColor.White;
            if (savetofile)
            {
                string path = $"{Environment.CurrentDirectory}\\ratios.txt";
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }
                File.WriteAllText(path, rstring);
                Console.WriteLine($"Ratios Saved ({path})");
            }
            Console.ReadKey();
        }
    }
}
