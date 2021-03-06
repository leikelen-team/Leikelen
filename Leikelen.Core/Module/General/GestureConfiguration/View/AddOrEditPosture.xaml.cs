﻿using cl.uv.leikelen.Data.Model;
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

            nameTextBox.IsEnabled = false;
            TypeCombobox.ItemsSource = new string[] { Properties.GestureConfiguration.DiscretePosture,
                Properties.GestureConfiguration.ContinuousPosture };
            TypeCombobox.SelectedItem = _posture.GestureType;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".gbd",
                Filter = "All gesture files (*.gbd, *.gba)|*.gbd; *.gba|Gesture solution build (*.gbd)|*.gbd|Gesture project analisys (*.gba)|*.gba",
                Title = Properties.GestureConfiguration.ImportPosture
            };
            if (dlg.ShowDialog() == true)
            {
                FileNameTextBox.Text = dlg.FileName;
                _safeFileName = dlg.SafeFileName;
                nameTextBox.Text = System.IO.Path.GetFileNameWithoutExtension(dlg.SafeFileName);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string error = "";
            string name = nameTextBox.Text;
            if (String.IsNullOrEmpty(_safeFileName) && ReferenceEquals(null, _posture))
            {
                error += "\n"+Properties.GestureConfiguration.Error_NoFile;
            }
            if (String.IsNullOrEmpty(name))
            {
                error += "\n"+Properties.GestureConfiguration.Error_NoName;
            }
            if(_dataAccessFacade.GetSubModalAccess().Exists(PostureCRUD.PostureTypes[TypeCombobox.SelectedIndex], name)
                && ReferenceEquals(null, _posture))
            {
                error += $"\n{Properties.GestureConfiguration.Error_ThePosture} {name} {Properties.GestureConfiguration.Error_PostureExists}";
            }
            if (!String.IsNullOrEmpty(error))
            {
                MessageBox.Show($"{Properties.GestureConfiguration.Error_ThereAreErrors}{error}", Properties.GestureConfiguration.Error);
            }
            else if(ReferenceEquals(null, _posture)) //is adding
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
                

                var subModal = _dataAccessFacade.GetSubModalAccess().Get(PostureCRUD.PostureTypes[TypeCombobox.SelectedIndex], name);
                string oldPath = subModal.File;

                subModal.SubModalTypeId = name;
                subModal.File = _safeFileName?? oldPath;
                subModal.Description = DescriptionTextBox.Text;
                _dataAccessFacade.GetSubModalAccess().Update(subModal);

                if (!String.IsNullOrEmpty(_safeFileName))
                {
                    string internalFilePath = $"{_dataAccessFacade.GetGeneralSettings().GetModalDirectory(PostureCRUD.PostureTypes[TypeCombobox.SelectedIndex])}/{_safeFileName}";
                    if (oldPath.Equals(internalFilePath))
                    {
                        if (File.Exists(oldPath))
                            File.Delete(oldPath);
                        if (!File.Exists(internalFilePath))
                            File.Copy(FileNameTextBox.Text, internalFilePath);
                    }
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
