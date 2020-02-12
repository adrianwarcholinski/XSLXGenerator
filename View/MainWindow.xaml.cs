using Microsoft.Win32;
using System;
using System.Windows;
using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            MyViewModel vm = (MyViewModel) DataContext;
            vm.Window = this;
        }

        public string SelectReadableFile(ref string fileName)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".xsr";
            openFileDialog.Filter = "Pliki XSR (*.xsr)|*.xsr";

            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.SafeFileName;
                return openFileDialog.FileName;
            }

            return null;
        }

        public string SelectWritableFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".xlsx";
            saveFileDialog.Filter = "Arkusze XLSX (*.xlsx)|*.xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }
    }
}