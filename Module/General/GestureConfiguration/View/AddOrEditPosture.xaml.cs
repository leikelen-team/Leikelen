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

namespace cl.uv.leikelen.Module.General.GestureConfiguration.View
{
    /// <summary>
    /// Lógica de interacción para AddOrEditPosture.xaml
    /// </summary>
    public partial class AddOrEditPosture : Window
    {
        private Posture _posture;
        private string _safeFileName;
        

        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public AddOrEditPosture()
        {
            InitializeComponent();
            Title = Properties.GestureConfiguration.AddOrEditTitle;

            TypeCombobox.ItemsSource = new string[] { Properties.GestureConfiguration.DiscretePosture,
                Properties.GestureConfiguration.ContinuousPosture };
            TypeCombobox.SelectedIndex = 0;
        }

        public AddOrEditPosture(Posture posture)
        {
            InitializeComponent();
            Title = Properties.GestureConfiguration.AddOrEditTitle;

            _posture = posture;
            nameTextBox.Text = _posture.Name;
            FileNameTextBox.Text = _posture.File;
            DescriptionTextBox.Text = _posture.Description;

            TypeCombobox.ItemsSource = new string[] { Properties.GestureConfiguration.DiscretePosture,
                Properties.GestureConfiguration.ContinuousPosture };
            TypeCombobox.SelectedItem = _posture.GestureType;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".gbd",
                Filter = "Gesture solution build " + "(*.gbd)|*.gbd|Gesture project build (*.gba)|*.gba",
                Title = "Importar postura"
            };
            if (dlg.ShowDialog() == true)
            {
                FileNameTextBox.Text = dlg.FileName;
                _safeFileName = dlg.SafeFileName;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string error = "";
            string name = nameTextBox.Text;
            if (String.IsNullOrEmpty(_safeFileName))
            {
                error += "\nNo ingresó un archivo";
            }
            if (String.IsNullOrEmpty(name))
            {
                error += "\nNo ingresó un nombre";
            }
            if(_dataAccessFacade.GetSubModalAccess().Exists(PostureCRUD.PostureTypes[TypeCombobox.SelectedIndex], name))
            {
                if(_posture == null)
                {
                    error += $"\nLa postura {name} ya existe. Utilice otro nombre";
                }
            }
            if (!String.IsNullOrEmpty(error))
            {
                MessageBox.Show("Hay errores:"+error, "Error");
            }
            else if(_posture == null) //is adding
            {
                string internalFilePath = $"{_dataAccessFacade.GetGeneralSettings().GetDataDirectory()}" +
                    $"modal/{PostureCRUD.PostureTypes[TypeCombobox.SelectedIndex]}/{_safeFileName}";

                if (!File.Exists(internalFilePath))
                    File.Copy(FileNameTextBox.Text, internalFilePath);

                _dataAccessFacade.GetSubModalAccess().Add(PostureCRUD.PostureTypes[TypeCombobox.SelectedIndex],
                    name, DescriptionTextBox.Text, _safeFileName);
                Close();
            }
            else //editing
            {
                string internalFilePath = $"{_dataAccessFacade.GetGeneralSettings().GetDataDirectory()}" +
                    $"modal/{PostureCRUD.PostureTypes[TypeCombobox.SelectedIndex]}/{_safeFileName}";

                var subModal = _dataAccessFacade.GetSubModalAccess().Get(PostureCRUD.PostureTypes[TypeCombobox.SelectedIndex], name);
                string oldPath = subModal.File;

                subModal.SubModalTypeId = name;
                subModal.File = _safeFileName;
                subModal.Description = DescriptionTextBox.Text;
                _dataAccessFacade.GetSubModalAccess().Update(subModal);

                if (oldPath != internalFilePath)
                {
                    File.Delete(oldPath);
                    if (!File.Exists(internalFilePath))
                        File.Copy(FileNameTextBox.Text, internalFilePath);
                }
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
