namespace Model.Material
{
    public class MaterialList
    {
        public MaterialHeader Header { get; private set; }

        public MaterialList(string[] lines)
        {
            Header = new MaterialHeader(lines);
        }

        public override string ToString()
        {
            return $"Header: {Header}";
        }
    }
}