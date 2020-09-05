using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.DataModel;

namespace Model.List
{
    public class StructuralList : AbstractList
    {
        public string WeightSummary { get; }

        private int _partColumnNo;

        public StructuralList(string content)
        {
            IEnumerable<string> dataChunks = SplitData(content);

            foreach (string chunk in dataChunks)
            {
                _currentContentType = GetContentType(chunk);
                bool isFinalSummary = chunk.Contains("Total");
                switch (_currentContentType)
                {
                    case ContentType.Header:
                        {
                            Header = new Header(chunk);
                            break;
                        }

                    case ContentType.Columns:
                        {
                            string[] lines = chunk.Trim().Split("\r", StringSplitOptions.RemoveEmptyEntries);

                            string columnsLine = lines[0];

                            if (Columns == null)
                            {
                                InitColumns(columnsLine);
                            }

                            StringBuilder sb = new StringBuilder();
                            for (int lineIndex = 1; lineIndex < lines.Length; lineIndex++)
                            {
                                sb.Append(lines[lineIndex]);
                            }

                            string columnsContent = sb.ToString();
                            if (!string.IsNullOrEmpty(columnsContent))
                            {
                                AppendData(columnsContent);
                            }

                            break;
                        }

                    case ContentType.Data:
                        {
                            if (isFinalSummary)
                            {
                                WeightSummary = chunk.Trim().Split(":").Last();
                            }
                            else
                            {
                                AppendData(chunk);
                            }
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

                string[] entries = SplitLine(line);
                for (int i = 0; i < entries.Length; i++)
                {
                    string entry = entries[i];
                    Columns.ElementAt(i).GetLastChunk().AddEntry(entry);
                }
            }
        }

        private string[] SplitLine(string line)
        {
            string[] tmp = line.Split("  ", StringSplitOptions.RemoveEmptyEntries);
            List<string> entries = new List<string>();
            tmp.ToList().ForEach(e => entries.Add(e));

            int requiredColumnsCount = 6;
            if (entries.Count < requiredColumnsCount)
            {
                List<string> correctedEntries = new List<string>();
                for (int i = 0; i < entries.Count; i++)
                {
                    string[] tmp2 = entries.ElementAt(i).Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    if (tmp2.Length > 1)
                    {
                        tmp2[0] = " " + tmp2[0];
                    }
                    tmp2.ToList().ForEach(e => correctedEntries.Add(e));
                }

                entries = correctedEntries;
            }
            return entries.ToArray();
        }

        protected override void InitColumns(string content)
        {
            Columns = new List<StringColumn>();
            string[] columns = content.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < columns.Length; i++)
            {
                string trimmedColumn = columns[i].Trim();
                if (!string.IsNullOrEmpty(trimmedColumn))
                {
                    if (trimmedColumn == TranslateUtils.Translate("Part"))
                    {
                        _partColumnNo = i;
                    }
                    Columns.Add(new StringColumn(TranslateUtils.Translate(trimmedColumn)));
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