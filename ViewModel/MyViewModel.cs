using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
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

        #region  Properties
        public string SelectedFileName
        {
            get => _selectedFileName;
            set
            {
                _selectedFileName = value;
                IsActiveGenerateButton = !string.IsNullOrEmpty(value);
                RaisePropertyChanged();
            }
        }

        public bool IsActiveGenerateButton
        {
            get => _isActiveGenerateButton;
            set
            {
                _isActiveGenerateButton = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties


        public MyViewModel()
        {
            SelectFileCommand = new Command(SelectFile);
        }

        #region Raise PropertyChanged
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Commands methods
        public void SelectFile()
        {
            string fileName = "";

            Stream file = Window.SelectFile(ref fileName);

            if (file != null)
            {
                _selectedFile = file;
                SelectedFileName = fileName.Replace("_", "__");
            }
        }

        #endregion Commands methods

        #region Private stuff

        private string _selectedFileName;
        private Stream _selectedFile;
        private bool _isActiveGenerateButton;

        #endregion Private stuff
    }
}