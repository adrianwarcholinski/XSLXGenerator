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
        public List<string> columnsNames { get; }


        public MaterialList(string content)
        {
            string[] dataChunks = SplitData(content);

            foreach (string chunk in dataChunks)
            {
                _currentContentType = GetContentType(chunk);
                switch (_currentContentType)
                {
                    case MaterialContentType.Header:
                        Header = new MaterialHeader(content);
                        break;
                    
                }
            }

            Header = new MaterialHeader(content);
        }

        private string[] SplitData(string data)
        {
            string[] chunksByHyphen = data.Split("-", StringSplitOptions.RemoveEmptyEntries);

            ICollection<string> chunksByEqualSign = new List<string>();

            for (int i = 0; i < chunksByHyphen.Length; i++)
            {
                if (chunksByHyphen[i].Contains("="))
                {
                    string[] twoChunks = chunksByHyphen[i].Split("=");

                    // string formattedFirstChunk = twoChunks[0] + '-';
                    // formattedFirstChunk = formattedFirstChunk.Trim();

                    // string formattedSecondChunk = twoChunks.Last() + '=';
                    // formattedSecondChunk = formattedSecondChunk.Trim();

                    chunksByEqualSign.Add(twoChunks.First());
                    chunksByEqualSign.Add(twoChunks.Last());
                }
                else
                {
                    // string formattedChunk = chunksByHyphen[i] + '-';
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
                    if (isHeader(content))
                    {
                        return MaterialContentType.Header;
                    }

                    return MaterialContentType.Summary;
                }

                default:
                {
                    if (isHeader(content))
                    {
                        return MaterialContentType.Header;
                    }

                    return MaterialContentType.None;
                }
            }
        }

        private bool isHeader(string content)
        {
            return content.ToUpper().Contains("TEKLA");
        }

        public override string ToString()
        {
            return $"Header: {Header}";
        }
    }
}