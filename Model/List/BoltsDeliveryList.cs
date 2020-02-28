using System;
using System.Collections.Generic;
using System.Linq;
using Model.DataModel;

namespace Model.List
{
    public class BoltsDeliveryList : AbstractList
    {
        public BoltsDeliveryList(string content)
        {
            IEnumerable<string> dataChunks = SplitData(content);

            foreach (string chunk in dataChunks)
            {
                _currentContentType = GetContentType(chunk);
                switch (_currentContentType)
                {
                    case ContentType.Header:
                    {
                        Header = new Header(chunk);
                        break;
                    }

                    case ContentType.Columns:
                    {
                        if (Columns == null)
                        {
                            InitColumns(chunk);
                        }

                        break;
                    }

                    case ContentType.Data:
                    {
                        AppendData(chunk);
                        break;
                    }
                }
            }
        }

        protected override void InitColumns(string content)
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

        protected virtual IEnumerable<string> SplitData(string data)
        {
            string[] chunksByHyphen = data.Split("--", StringSplitOptions.RemoveEmptyEntries);

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
                    if (!string.IsNullOrEmpty(chunksByHyphen[i].Trim().Replace("-", "")))
                    {
                        chunksByEqualSign.Add(chunksByHyphen[i]);
                    }
                }
            }

            return chunksByEqualSign.ToArray();
        }

        protected virtual void AppendData(string content)
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
                    if (!string.IsNullOrEmpty(entry.Trim().Replace("-", "")))
                    {
                        Columns.ElementAt(i).GetLastChunk().AddEntry(entry.Trim());
                    }
                }
            }
        }

        private ContentType GetContentType(string content)
        {
            switch (_currentContentType)
            {
                case ContentType.Header:
                    return ContentType.Columns;

                case ContentType.Columns:
                    return ContentType.Data;

                case ContentType.Data:
                {
                    return IsHeader(content) ? ContentType.Header : ContentType.Data;
                }

                default:
                {
                    return IsHeader(content) ? ContentType.Header : ContentType.None;
                }
            }
        }
    }
}