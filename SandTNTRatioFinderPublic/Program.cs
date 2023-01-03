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

        private static Dictionary<string, double> bottomAligners = new Dictionary<string, double>
        {
            {"bottom_carpet", 0.0625 },
            {"bottom_repeater", 0.125 },
            {"bottom_trapdoor", 0.1875 },
            {"bottom_daylight_sensor", 0.375 },
            {"bottom_campfire", 0.4375 },
            {"bottom_slab", 0.5 },
            {"bottom_stonecutter", 0.5625 },
            {"bottom_chain", 0.59375 },
            {"bottom_lightning_rod", 0.625 },
            {"bottom_conduit", 0.6875 },
            {"bottom_enchanting_table", 0.75 },
            {"bottom_chest", 0.875 },
            {"bottom_path", 0.9375 },
            {"bottom_full_block", 1 }
        };
            
        //subsititutes for some blocks(currently not used)
        static Dictionary<string, string[]> aliases = new Dictionary<string, string[]>
        {
            {"top_trapdoor", new [] { "top_small_amethyst" } },
            {"sideways_lightning_rod", new [] { "sideways_end_rod" } },
            {"conduit", new [] { "cocobean_stage_1" } },
            {"sideways_skull", new [] { "sideways_bell", "sideways_hopper", "small_amethyst_sideways" } },
            { "sideways_amethyst_cluster", new [] { "cocobean_stage_2" } },
        };

        static bool verbose;
        static bool savetofile;

        static int maxgtdiff;
        static int maxgtdrop;
        static double differencethreshhold;
        static int showratios;
        static int guiderdiff;
        static bool bottom_align;
        static bool belgianmode;

        static readonly double tntexpp = 0;
        static readonly double sandexpp = 0.833;

        static List<SandTNTSyncRatio> doSyncSim()
        {
            if (belgianmode)
            {
                topAligners.Remove("top_medium_amethyst");
                topAligners.Remove("top_large_amethyst");
                topAligners.Remove("amethyst_cluster");
                topAligners.Remove("cocobean_stage_0");
                topAligners.Remove("sideways_amethyst_cluster");
            }
            Console.WriteLine("Finding Ratios!");
            int combinations = topAligners.Count * (topAligners.Count + (bottom_align ? bottomAligners.Count : 0));
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
                                    ratios.Add(new SandTNTSyncRatio(0, g, sta, 0, tta, i, t, difference, ConvertPosition(sandpos + sandexpp), ConvertPosition(tntpos + tntexpp), ConvertPosition(sandpos), ConvertPosition(tntpos), sandvel, tntvel));
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
                if (bottom_align) foreach (string tta in bottomAligners.Keys)
                {
                    if (verbose) Console.WriteLine($"--TNT Bottom Aligner: {tta}");
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
                            double tntpos = i + (1 - bottomAligners[tta]);
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
                                    ratios.Add(new SandTNTSyncRatio(0, g, sta, 0, tta, i, t, difference, ConvertPosition(sandpos + sandexpp), ConvertPosition(tntpos + tntexpp), ConvertPosition(sandpos), ConvertPosition(tntpos), sandvel, tntvel));
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
            bottom_align = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("bottom_align"));
            belgianmode = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("belgian_mode"));
            showratios = Convert.ToInt32(ConfigurationManager.AppSettings.Get("showratios"));
            guiderdiff = Convert.ToInt32(ConfigurationManager.AppSettings.Get("maxguiderydifference"));
        }

        public static double ConvertPosition(double pos)
        {
            return (-guiderdiff - (pos - 1))*-1;
        }
        static void Main(string[] args)
        {
            Console.Title = "Sand TNT Ratio Finder - by jesus_is_hot";

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
