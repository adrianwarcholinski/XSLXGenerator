using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Model.Material
{
    public class MaterialList
    {
        private readonly MaterialContentType _currentContentType;

        public MaterialHeader Header { get; }
        public List<string> ColumnsNames { get; private set; }



        public MaterialList(string content)
        {
            IEnumerable<string> dataChunks = SplitData(content);

            foreach (string chunk in dataChunks)
            {
                _currentContentType = GetContentType(chunk);
                switch (_currentContentType)
                {
                    case MaterialContentType.Header:
                    {
                        Header = new MaterialHeader(content);
                        break;
                    }

                    case MaterialContentType.Columns:
                    {
                        if (ColumnsNames == null)
                        {
                            InitColumnNames(chunk);
                        }
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

        private MaterialContentType GetContentType(string content)
        {
            switch (_currentContentType)
            {
                case MaterialContentType.Header:
                    return MaterialContentType.Columns;

                case MaterialContentType.Columns:
                case MaterialContentType.Summary:
                    return MaterialContentType.Data;

                case MaterialContentType.Data:
                {
                    if (IsHeader(content))
                    {
                        return MaterialContentType.Header;
                    }

                    return MaterialContentType.Summary;
                }

                default:
                {
                    if (IsHeader(content))
                    {
                        return MaterialContentType.Header;
                    }

                    return MaterialContentType.None;
                }
            }
        }

        private static bool IsHeader(string content)
        {
            return content.ToUpper().Contains("TEKLA");
        }

        private void InitColumnNames(string content)
        {
            ColumnsNames = new List<string>();
            string[] columns = content.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string column in columns)
            {
                string trimedColumn = column.Trim();
                if (!string.IsNullOrEmpty(trimedColumn))
                {
                    ColumnsNames.Add(trimedColumn);
                }
            }
        }

        public override string ToString()
        {
            return $"Header: {Header}";
        }
    }
}