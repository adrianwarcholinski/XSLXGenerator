using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class StructuralList : AbstractList
    {
        public StructuralList(string content)
        {
            IEnumerable<string> dataChunks = SplitData(content);

            foreach (string chunk in dataChunks)
            {
                _currentContentType = GetContentType(chunk);
                switch (_currentContentType)
                {
                    case ContentType.Header:
                        {
                            Header = new Header(content);
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

            Header = new Header(content);
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
                    Columns.Add(new StringColumn(TranslateUtils.Translate(trimedColumn)));
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