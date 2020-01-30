using System.IO;

namespace ViewModel
{
    public interface IWindow
    {
        string SelectReadableFile(ref string fileName);
        string SelectWritableFile();
    }
}