using System.IO;

namespace ViewModel
{
    public interface IWindow
    {
        string SelectFile(ref string fileName);
    }
}