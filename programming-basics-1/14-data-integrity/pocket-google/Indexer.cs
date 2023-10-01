using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        public readonly char[] Separators;
        public readonly Dictionary<string, Dictionary<int, List<int>>> SearchData;

        public Indexer()
        {
            Separators = new char[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
            SearchData = new Dictionary<string, Dictionary<int, List<int>>>();
        }

        public void Add(int id, string text)
        {
            var start = 0;
            var end = 0;
            while (end < text.Length && start < text.Length)
            {
                if (Separators.Contains(text[start]))
                {
                    start++;
                    continue;
                }
                end = start + 1;
                while (end != text.Length && !Separators.Contains(text[end]))
                    end++;
                var word = text.Substring(start, end - start);
                if (!SearchData.ContainsKey(word)) SearchData[word] = new Dictionary<int, List<int>>();
                if (!SearchData[word].ContainsKey(id)) SearchData[word][id] = new List<int>();
                SearchData[word][id].Add(start);
                start = end + 1;
            }
        }

        public List<int> GetIds(string word)
        {
            var ids = new List<int>();
            if (SearchData.ContainsKey(word))
                ids.AddRange(SearchData[word].Keys);
            return ids;
        }

        public List<int> GetPositions(int id, string word)
        {
            if (!SearchData.ContainsKey(word)) return new List<int>();
            return SearchData[word][id].ToList();
        }

        public void Remove(int id)
        {
            foreach (var wordData in SearchData.ToList())
            {
                if (!wordData.Value.Remove(id)) continue;
                if (!wordData.Value.Any()) SearchData.Remove(wordData.Key);
            }
        }
    }
}
