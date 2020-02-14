using Model.List;
using NPOI.SS.UserModel;

namespace XLSXManagement.WriteDataStrategy
{
    internal interface IXLSXWriteDataStrategy
    {
        public void WriteData(AbstractList list, ISheet sheet);
    }
}