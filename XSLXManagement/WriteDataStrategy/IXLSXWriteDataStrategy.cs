using Model;
using NPOI.SS.UserModel;

namespace XLSXManagement
{
    internal interface IXLSXWriteDataStrategy
    {
        public void WriteData(AbstractList list, ISheet sheet);
    }
}