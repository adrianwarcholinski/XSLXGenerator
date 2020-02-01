﻿using FileManagement.Material;
using Model.Material;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using XLSXManagement.Material;

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
        public ICommand GenerateCommand { get; private set; }

        #endregion Commands

        #region  Properties
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

        #endregion Properties


        public MyViewModel()
        {
            SelectFileCommand = new Command(SelectFile);
            GenerateCommand = new Command(Generate);
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

            _selectedFileFullPath = Window.SelectReadableFile(ref fileName);

            if (_selectedFileFullPath != null)
            {
                SelectedFileName = fileName.Replace("_", "__");
            }
        }

        public void Generate()
        {
            _selectedTargetPath = Window.SelectWritableFile();

            Task.Run(() =>
            {
                MaterialList list = MaterialListReader.ReadMaterialList(_selectedFileFullPath);
                MaterialXLSXWriter.WriteMaterialList(list, _selectedTargetPath);
            });
        }

        #endregion Commands methods

        #region Private stuff

        private string _selectedSourceFileName;
        private string _selectedTargetPath;
        private bool _isActiveGenerateButton;
        private string _selectedFileFullPath;

        #endregion Private stuff
    }
}