using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
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

        public string SelectFile(ref string fileName)
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
    }
}