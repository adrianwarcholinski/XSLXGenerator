using Model;
using Model.List;
using NPOI.SS.UserModel;

namespace XLSXManagement.WriteHeaderStrategy
{
    public interface IXLSXWriteHeaderStrategy
    {
        public void FormatHeader(AbstractList list, ISheet sheet, ListType listType);
    }
}