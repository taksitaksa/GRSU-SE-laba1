
namespace ConsoleApp1
{
    class Program
    {
        static List<GeneticData> LoadSequences(string filePath)
            {
                List<GeneticData> list = new List<GeneticData>();
                string[] lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split('\t');
                    if (parts.Length >= 3)
                    {
                        GeneticData gen = new GeneticData(parts[0],
                                                        parts[1],
                                                        GeneticData.RLDecoding(parts[2]));
                        list.Add(gen);
                    }
                }
                return list;
            }
        static void search(string substring, List<GeneticData> proteins, StreamWriter writer)
        {
            writer.WriteLine("organism\t\t\t\t\tprotein");
            for (int i = 0; i < proteins.Count; i++)
            {
                GeneticData gen = proteins[i];
                if (gen.amino_acids.Contains(substring))
                {
                    writer.WriteLine(gen.organism + "\t\t" + gen.protein);
                    writer.WriteLine("--------------------------------------------------------------------------");
                    return;
                }
            }
            writer.WriteLine("NOT FOUND");
            writer.WriteLine("--------------------------------------------------------------------------");
        }
        static void diff(string protein1, string protein2, List<GeneticData> proteins, StreamWriter writer)
        {

            bool isProtein1 = proteins.Any(gen => gen.protein == protein1);
            bool isProtein2 = proteins.Any(gen => gen.protein == protein2);

            writer.WriteLine("amino-acids difference: ");

            if (!isProtein1 && !isProtein2)
                writer.WriteLine($"MISSING:\t{protein1}\t{protein2}");

            else if (!isProtein1 || !isProtein2)
                writer.WriteLine(!isProtein1 ? $"MISSING:\t{protein1}" : $"MISSING:\t{protein2}");

            else
            {
                GeneticData gen1 = proteins.FirstOrDefault(gen => gen.protein == protein1);
                GeneticData gen2 = proteins.FirstOrDefault(gen => gen.protein == protein2);

                int longer = Math.Max(gen1.amino_acids.Length, gen2.amino_acids.Length);
                int shorter = Math.Min(gen1.amino_acids.Length, gen2.amino_acids.Length);
                int lenghtDiff = longer - shorter;
                for (int i = 0; i < shorter; i++)
                {
                    if (gen1.amino_acids[i] != gen2.amino_acids[i])
                        lenghtDiff++;
                }
                writer.WriteLine(lenghtDiff);
                writer.WriteLine("--------------------------------------------------------------------------");

            }
        }
        static void mode(string protein, List<GeneticData> proteins, StreamWriter writer)
        {

            bool isProtein = proteins.Any(gen => gen.protein == protein);

            writer.WriteLine("amino-acid occurs: ");
            if (!isProtein)
                writer.WriteLine("MISSING: " + protein);
            else
            {

            }

            GeneticData gen = proteins.FirstOrDefault(gen => gen.protein == protein);

            Dictionary<char, int> letters = new Dictionary<char, int>();
            foreach (char letter in gen.amino_acids)
            {
                if (letters.ContainsKey(letter))
                    letters[letter]++;
                else letters[letter] = 1;
            }
            char frequent = letters.OrderByDescending(pair => pair.Value).First().Key;
            writer.WriteLine($"{frequent}\t\t\t{letters[frequent]}");
            writer.WriteLine("--------------------------------------------------------------------------");

        }

        static void Main(string[] args)
        {
            for (int i = 0; i < 3; i++)
            {
                string[] files = {
                    $"sequences.{i}.txt",
                    $"commands.{i}.txt",
                    $"genedata.{i}.txt"
                };

                List<GeneticData> proteins = LoadSequences(files[0]);
                StreamWriter writer = new StreamWriter(files[2], false);

                writer.WriteLine("Uladzislau Sniahurski");
                writer.WriteLine("Genetic Searching");
                writer.WriteLine("--------------------------------------------------------------------------");

                int count = 1;
                string[] commands = File.ReadAllLines(files[1]);

                for (int k = 0; k < commands.Length; k++)           // k=i
                {
                    string line = commands[k];                      // i
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split('\t');
                    string command = parts[0];

                    switch (command)
                    {
                        case "search":
                            string target = GeneticData.RLDecoding(parts[1]);
                            writer.WriteLine(count.ToString("D3") + $"\tsearch\t{target}");
                            search(target, proteins, writer);
                            break;
                        case "diff":
                            string protein1 = parts[1];
                            string protein2 = parts[2];
                            writer.WriteLine(count.ToString("D3") + $"\tdiff\t{protein1}\t{protein2}");
                            diff(protein1, protein2, proteins, writer);
                            break;
                        case "mode":
                            string protein = parts[1];
                            writer.WriteLine(count.ToString("D3") + $"\tmode\t{protein}");
                            mode(protein, proteins, writer);
                            break;
                    }
                    count++;

                }
                writer.Close();
            }
        }
    } 
}
