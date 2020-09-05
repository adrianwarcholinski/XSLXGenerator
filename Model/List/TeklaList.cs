using System;
using Model.DataModel;
using System.Collections.Generic;
using System.Linq;

namespace Model.List
{
    public class TeklaList : AbstractList
    {
        public TeklaList(string content)
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

                    case ContentType.Summary:
                    {
                        AppendSummary(chunk);
                        break;
                    }
                }
            }
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
                    Columns.ElementAt(i).GetLastChunk().AddEntry(entry);
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
                case ContentType.Summary:
                    return ContentType.Data;

                case ContentType.Data:
                {
                    return IsHeader(content) ? ContentType.Header : ContentType.Summary;
                }

                default:
                {
                    return IsHeader(content) ? ContentType.Header : ContentType.None;
                }
            }
        }

        public override string ToString()
        {
            return $"Header: {Header}";
        }
    }
}