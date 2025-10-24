
using System.Text;

namespace ConsoleApp1
{
    public struct GeneticData
    {
        public string protein;
        public string organism;
        public string amino_acids;

        public GeneticData(string protein, string organism, string amino_acids)
        {
            this.protein = protein;
            this.organism = organism;
            this.amino_acids = amino_acids;
        }

        public static string RLEncoding(string amino_acids)
        {
            string result = "";
            int count = 1;
            char prev_char = amino_acids[0];
            for (int i = 1; i < amino_acids.Length; i++)
            {
                if (amino_acids[i] == prev_char)
                    count++;
                else
                {
                    result += count >= 3 ? $"{count}{prev_char}" : new string(prev_char, count);
                    prev_char = amino_acids[i];
                    count = 1;
                }
            }
            result += count >= 3 ? $"{count}{prev_char}" : new string(prev_char, count);
            return result;
        }
        public static string RLDecoding(string amino_acids)
        {
            StringBuilder result = new StringBuilder();
            int count;
            for (int i = 0; i < amino_acids.Length; i++)
            {
                char current_char = amino_acids[i];
                if (int.TryParse(current_char.ToString(), out count))
                {
                    if (i + 1 < amino_acids.Length)
                    {
                        char next_char = amino_acids[i + 1];
                        result.Append(next_char, count);
                        i++;
                    }
                }
                else result.Append(current_char);
            }
            return result.ToString();
        }
    }
}
