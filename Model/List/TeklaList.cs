using Model.DataModel;
using System.Collections.Generic;

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