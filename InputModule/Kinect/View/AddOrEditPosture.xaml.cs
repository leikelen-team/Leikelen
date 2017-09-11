using cl.uv.leikelen.Data.Model;
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
using System.Windows.Shapes;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;

namespace cl.uv.leikelen.InputModule.Kinect.View
{
    /// <summary>
    /// Lógica de interacción para AddOrEditPosture.xaml
    /// </summary>
    public partial class AddOrEditPosture : Window
    {
        private Posture _posture;
        private string path;

        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public AddOrEditPosture()
        {
            InitializeComponent();
        }

        public AddOrEditPosture(Posture posture)
        {
            InitializeComponent();

            _posture = posture;
            nameTextBox.Text = _posture.Name;
            fileNameTextBox.Text = _posture.File;
            TypeCombobox.SelectedItem = _posture.GestureType;
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".gbd"; // Default file extension
            dlg.Filter = "Gesture solution build " + "(*.gbd)|*.gbd|Gesture project build (*.gba)|*.gba"; // Filter files by extension
            dlg.Title = "Importar postura";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                this.fileNameTextBox.Text = dlg.SafeFileName;
                this.path = dlg.FileName;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.path == null)
            {
                MessageBox.Show("Debes ingresar un archivo");
            }
            string name = nameTextBox.Text;
            if (_dataAccessFacade.GetSubModalAccess().Exists((string)this.TypeCombobox.SelectedItem, name))
            {
                if (_posture == null || _posture.Name != name)
                {
                    MessageBox.Show("La postura '" + name + "' ya existe. Utilice otro nombre.");
                }
                else if (_posture.Name == name && _posture.File == path)
                {
                    this.Close();
                }
            }
            else if (!String.IsNullOrEmpty(this.path) && !String.IsNullOrEmpty(name))
            {
                Console.WriteLine("Saving posture: name: {0} --- path: {1}", name, this.path);


                string internalFilePath = _dataAccessFacade.GetGeneralSettings().GetDataDirectory()+"modal/"+_posture.GestureType+"/"+ fileNameTextBox.Text;
                if (!File.Exists(internalFilePath))
                    File.Copy(path, internalFilePath);
                if (_posture != null)
                {
                    var subModal = _dataAccessFacade.GetSubModalAccess().Get((string)this.TypeCombobox.SelectedItem, name);
                    string oldPath = subModal.File;

                    subModal.SubModalTypeId = name;
                    subModal.File = path;
                    subModal.Description = DescriptionTextBox.Text;
                    _dataAccessFacade.GetSubModalAccess().Update(subModal);

                    if (oldPath != internalFilePath)
                    {
                        File.Delete(oldPath);
                    }
                }
                else
                {
                    _dataAccessFacade.GetSubModalAccess().Add((string)this.TypeCombobox.SelectedItem, name, DescriptionTextBox.Text, this.path);
                }
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
