using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Model.Material
{
    public class MaterialList
    {
        private readonly MaterialContentTypes _currentContentType;

        public MaterialHeader Header { get; }
        public ICollection<StringColumn> Columns { get; private set; }


        public MaterialList(string content)
        {
            IEnumerable<string> dataChunks = SplitData(content);

            foreach (string chunk in dataChunks)
            {
                _currentContentType = GetContentType(chunk);
                switch (_currentContentType)
                {
                    case MaterialContentTypes.Header:
                    {
                        Header = new MaterialHeader(content);
                        break;
                    }

                    case MaterialContentTypes.Columns:
                    {
                        if (Columns == null)
                        {
                            InitColumns(chunk);
                        }

                        break;
                    }

                    case MaterialContentTypes.Data:
                    {
                        AppendData(chunk);
                        break;
                    }

                    case MaterialContentTypes.Summary:
                    {
                        AppendSummary(chunk);
                        break;
                    }
                }
            }

            Header = new MaterialHeader(content);
        }

        private IEnumerable<string> SplitData(string data)
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

        private MaterialContentTypes GetContentType(string content)
        {
            switch (_currentContentType)
            {
                case MaterialContentTypes.Header:
                    return MaterialContentTypes.Columns;

                case MaterialContentTypes.Columns:
                case MaterialContentTypes.Summary:
                    return MaterialContentTypes.Data;

                case MaterialContentTypes.Data:
                {
                    if (IsHeader(content))
                    {
                        return MaterialContentTypes.Header;
                    }

                    return MaterialContentTypes.Summary;
                }

                default:
                {
                    if (IsHeader(content))
                    {
                        return MaterialContentTypes.Header;
                    }

                    return MaterialContentTypes.None;
                }
            }
        }

        private static bool IsHeader(string content)
        {
            return content.ToUpper().Contains("TEKLA");
        }

        private void InitColumns(string content)
        {
            Columns = new List<StringColumn>();
            string[] columns = content.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string column in columns)
            {
                string trimedColumn = column.Trim();
                if (!string.IsNullOrEmpty(trimedColumn))
                {
                    Columns.Add(new StringColumn(trimedColumn));
                }
            }
        }

        private void AppendData(string content)
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

        private void AppendSummary(string content)
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

        public override string ToString()
        {
            return $"Header: {Header}";
        }
    }
}