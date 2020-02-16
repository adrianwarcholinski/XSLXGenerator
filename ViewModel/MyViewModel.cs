using FileManagement;
using Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Model.List;
using XLSXManagement;

namespace ViewModel
{
    public class MyViewModel : INotifyPropertyChanged
    {
        #region Public stuff

        public event PropertyChangedEventHandler PropertyChanged;
        public IWindow Window;

        #endregion Public stuff

        #region Commands

        public ICommand SelectFileCommand { get; }
        public ICommand GenerateCommand { get; }
        public ICommand ShowListTypesCommand { get; }
        public ICommand HideListTypesCommand { get; }

        #endregion Commands

        #region Properties

        public string SelectedFileName
        {
            get => _selectedSourceFileName;
            set
            {
                _selectedSourceFileName = value;
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

        public ListType SelectedListType
        {
            get => _selectedListType;
            set
            {
                _selectedListType = value;
                RaisePropertyChanged();
            }
        }

        public FileContentType SelectedFileContentType
        {
            get => _selectedFileContentType;
            set
            {
                _selectedFileContentType = value;
                RaisePropertyChanged();
            }
        }

        public bool IsListTypesAvailable
        {
            get => _isListTypesAvailable;
            set
            {
                _isListTypesAvailable = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties


        public MyViewModel()
        {
            SelectFileCommand = new Command(SelectFile);
            GenerateCommand = new Command(Generate);
            SelectedListType = ListType.Delivery;
            SelectedFileContentType = FileContentType.MainConstruction;
            IsListTypesAvailable = true;

            ShowListTypesCommand = new Command(ShowListTypes);
            HideListTypesCommand = new Command(HideListTypes);
        }

        #region Raise PropertyChanged

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Commands methods

        public void ShowListTypes()
        {
            IsListTypesAvailable = true;
        }

        public void HideListTypes()
        {
            IsListTypesAvailable = false;
        }

        public void SelectFile()
        {
            string fileName = "";

            _selectedFileFullPath = Window.SelectReadableFile(ref fileName);

            if (_selectedFileFullPath != null)
            {
                SelectedFileName = fileName.Replace("_", "__");
            }
        }

        public void Generate()
        {
            _selectedTargetPath = Window.SelectWritableFile();

            if (string.IsNullOrEmpty(_selectedTargetPath))
            {
                return;
            }

            Task.Run(() =>
            {
                ListType type = SelectedFileContentType == FileContentType.MainConstruction
                    ? SelectedListType
                    : ListType.BoltsDelivery;
                
                AbstractList list = ListReader.ReadList(type, _selectedFileFullPath);
                XLSXWriterSwitcher.WriteList(type, list, _selectedTargetPath);
            });
        }

        #endregion Commands methods

        #region Private stuff

        private string _selectedSourceFileName;
        private string _selectedTargetPath;
        private bool _isActiveGenerateButton;
        private string _selectedFileFullPath;
        private ListType _selectedListType;
        private FileContentType _selectedFileContentType;

        private bool _isListTypesAvailable;

        #endregion Private stuff
    }
}