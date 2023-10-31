using System.Collections.Generic;
using System.Linq;

namespace PocketGoogle {
    public class Indexer : IIndexer {
        private static readonly HashSet<char> separators
            = new[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' }.ToHashSet();

        private readonly Dictionary<string, Dictionary<int, List<int>>> data
            = new Dictionary<string, Dictionary<int, List<int>>>();

        public void Add(int id, string text) {
            var wordBegin = 0;
            int wordEnd;
            while (wordBegin < text.Length) {
                while (wordBegin != text.Length - 1 && separators.Contains(text[wordBegin]))
                    wordBegin++;
                wordEnd = wordBegin + 1;
                while (wordEnd != text.Length && !separators.Contains(text[wordEnd]))
                    wordEnd++;

                var word = text.Substring(wordBegin, wordEnd - wordBegin);
                if (!data.ContainsKey(word)) data[word] = new Dictionary<int, List<int>>();
                if (!data[word].ContainsKey(id)) data[word][id] = new List<int>();
                data[word][id].Add(wordBegin);
                wordBegin = wordEnd + 1;
            }
        }

        public List<int> GetIds(string word)
            => data.ContainsKey(word) ? data[word].Keys.ToList() : new List<int>();

        public List<int> GetPositions(int id, string word)
            => data.ContainsKey(word) ? data[word][id].ToList() : new List<int>();

        public void Remove(int id) {
            var toRemove = data
                .Where(d => d.Value.ContainsKey(id))
                .ToList();
            foreach (var data in toRemove) {
                data.Value.Remove(id);
                if (data.Value.Count == 0) this.data.Remove(data.Key);
            }
        }
    }
}
