using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public abstract class AbstractList
    {
        public ContentType _currentContentType;

        public Header Header { get; protected set; }
        public List<StringColumn> Columns { get; protected set; }

        protected void InitColumns(string content)
        {
            Columns = new List<StringColumn>();
            string[] columns = content.Split("  ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string column in columns)
            {
                string trimedColumn = column.Trim();
                if (!string.IsNullOrEmpty(trimedColumn))
                {
                    Columns.Add(new StringColumn(TranslateUtils.Translate(trimedColumn)));
                }
            }
        }

        protected IEnumerable<string> SplitData(string data)
        {
            string[] chunksByHyphen = data.Split("-", StringSplitOptions.RemoveEmptyEntries);

            ICollection<string> chunksByEqualSign = new List<string>();

            for (int i = 0; i < chunksByHyphen.Length; i++)
            {
                if (chunksByHyphen[i].Contains("="))
                {
                    string[] twoChunks = chunksByHyphen[i].Split("=", StringSplitOptions.RemoveEmptyEntries);

                    chunksByEqualSign.Add(twoChunks.First());
                    chunksByEqualSign.Add(twoChunks.Last());
                }
                else
                {
                    chunksByEqualSign.Add(chunksByHyphen[i]);
                }
            }

            return chunksByEqualSign.ToArray();
        }

        protected void AppendData(string content)
        {
            string[] lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine == "")
                {
                    continue;
                }

                string[] entries = line.Split("  ", StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < entries.Length; i++)
                {
                    string entry = entries[i];
                    Columns.ElementAt(i).GetLastChunk().AddEntry(entry);
                }
            }
        }

        protected void AppendSummary(string content)
        {
            string[] lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine == "")
                {
                    continue;
                }

                string[] entries = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < Columns.Count; i++)
                {
                    if (i < Columns.Count - entries.Length)
                    {
                        Columns.ElementAt(i).FinishChunk("");
                    }
                    else
                    {
                        string entry = entries[i - Columns.Count + entries.Length];
                        Columns.ElementAt(i).FinishChunk(entry);
                    }
                }
            }
        }

        protected static bool IsHeader(string content)
        {
            return content.ToUpper().Contains("TEKLA");
        }
    }
}