using System;
using System.Collections.Generic;
using System.Linq;

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

            Header = new Header(content);
        }
    }
}