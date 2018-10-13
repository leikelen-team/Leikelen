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
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;
using System.IO;
using Microsoft.Win32;

namespace cl.uv.leikelen.Module.General.AddMicrophoneScene.View
{
    /// <summary>
    /// Interaction logic for AddMicrophoneData.xaml
    /// </summary>
    public partial class AddMicrophoneData : Window, ICloneable
    {
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        /// <summary>
        /// Initializes a new instance of the <see cref="AddMicrophoneData"/> class.
        /// </summary>
        public AddMicrophoneData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new object copied of this instance.
        /// </summary>
        /// <returns>
        /// New object copied of this instance.
        /// </returns>
        public object Clone()
        {
            return new AddMicrophoneData();
        }

        private void CSVBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                DefaultExt = ".csv",
                Filter = "CSV files (*.csv)|*.csv"
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                CSVfileNameTextBox.Text = dlg.FileName;
            }
        }

        private void WAVBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                DefaultExt = ".wav",
                Filter = "WAV files (*.wav)|*.wav"
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                WAVfileNameTextBox.Text = dlg.FileName;
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(""))
            {

            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
