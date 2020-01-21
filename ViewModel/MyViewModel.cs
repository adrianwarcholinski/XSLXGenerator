using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace ViewModel
{
    public class MyViewModel : INotifyPropertyChanged
    {
        #region Public stuff

        public event PropertyChangedEventHandler PropertyChanged;
        public IWindow Window;

        #endregion Public stuff

        #region Commands

        public ICommand SelectFileCommand { get; private set; }

        #endregion Commands

        public string SelectedFileName
        {
            get => _selectedFileName;
            set
            {
                _selectedFileName = value;
                RaisePropertyChanged();
            }
        }

        public MyViewModel()
        {
            SelectFileCommand = new Command(SelectFile);
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SelectFile()
        {
            string fileName = "";
            _selectedFile = Window.SelectFile(ref fileName);
            SelectedFileName = fileName.Replace("_", "__");
        }

        #region Private stuff

        private string _selectedFileName;
        private Stream _selectedFile;

        #endregion Private stuff
    }
}