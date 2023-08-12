using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Markov {
    public class MarkovAlgorithm {
        private readonly List<KeyValuePair<string, string>> Alg = new();

        public MarkovAlgorithm(string path) {
            var file = File.ReadAllText(path);
            foreach (var step in Regex.Matches(file, @".+->.+", RegexOptions.Multiline).ToArray()) {
                var splitted = Regex.Split(step.ToString().Trim(), "->");
                Alg.Add(KeyValuePair.Create(splitted[0], splitted[1]));
            }
        }
    
        public override string ToString() {
            return Alg.Aggregate("Your Algorithm:\n",
                    (current, step) => current + (step.Key + "->" + step.Value + "\n"));
        }

        public string Do(string word) {
            Start:
            Console.WriteLine(word);
            foreach (var step in Alg.Where(step => word.Contains(step.Key))) {
                var i = word.IndexOf(step.Key, StringComparison.Ordinal);
                if (step.Value is "" or " ") {
                    word = word.Remove(i, step.Key.Length);
                    goto Start;
                }

                if (step.Key == " ") {
                    word = word.Insert(0, step.Value);
                    goto Start;
                }

                if (step.Value[0] == '.')
                    return word.Remove(i, step.Key.Length).Insert(i, step.Value.Remove(0, 1));


                word = word.Remove(i, step.Key.Length).Insert(i, step.Value);
                goto Start;
            }

            return word;
        }
    }

    internal static class Program {
        private static void Main() {
            var alg = new MarkovAlgorithm(@"C:\Users\hpceen\RiderProjects\Markov\test.txt");
            Console.WriteLine(alg.ToString());
            Console.WriteLine(alg.Do("1111*111111"));
        }
    }
}