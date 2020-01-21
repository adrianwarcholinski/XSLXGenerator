using System.IO;

namespace ViewModel
{
    public interface IWindow
    {
        Stream SelectFile(ref string fileName);
    }
}