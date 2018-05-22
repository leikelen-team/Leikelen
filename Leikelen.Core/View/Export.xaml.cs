using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using cl.uv.leikelen.Controller;

namespace cl.uv.leikelen.View
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class Export : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Export"/> class.
        /// </summary>
        public Export()
        {
            InitializeComponent();
        }

        private void FileSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog()
            {
                Filter = GeneralSettings.Instance.ExtensionFilter,
                DefaultExt = GeneralSettings.Instance.Extension
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                if (File.Exists(dlg.FileName))
                {
                    var result = MessageBox.Show(Properties.GUI.FileExists, 
                        Properties.GUI.FileExistsTitle, 
                        MessageBoxButton.YesNo, 
                        MessageBoxImage.Exclamation);
                    if(result == MessageBoxResult.Yes)
                    {
                        File.Delete(dlg.FileName);
                        FilePathTextBox.Text = dlg.FileName;
                    }
                }
                else
                {
                    FilePathTextBox.Text = dlg.FileName;
                }
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(FilePathTextBox.Text))
            {
                try
                {
                    bool onlyBd = (!ReferenceEquals(null, OnlyBdCheckbox?.IsChecked) && OnlyBdCheckbox?.IsChecked == true);
                    FileController.Export(onlyBd, FilePathTextBox.Text);
                    MessageBox.Show(Properties.GUI.SceneExportedSuccesfully, 
                        Properties.GUI.SceneExportedSuccesfullyTitle, 
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Properties.GUI.SceneExportedError + "\n" + ex.Message, 
                        Properties.GUI.SceneExportedErrorTitle, 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                }
                Close();
            }
            else
            {
                MessageBox.Show(Properties.GUI.SceneExportedNoFile, 
                    Properties.GUI.SceneExportedNoFileTitle, 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
            
        }
    }
}
