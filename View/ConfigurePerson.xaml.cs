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
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.View
{
    /// <summary>
    /// Lógica de interacción para ConfigurePerson.xaml
    /// </summary>
    public partial class ConfigurePerson : Window
    {
        private Person _person;
        private string _path;

        public ConfigurePerson()
        {
            InitializeComponent();

            PhotoBtn.Click += PhotoBtn_Click;
            AcceptBtn.Click += AcceptBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
        }

        public ConfigurePerson(Person person)
        {
            InitializeComponent();

            _person = person;
            NameTextBox.Text = _person.Name;
            BirthdayPicker.SelectedDate = _person.Birthday;
            PhotoPathTextBox.Text = _person.Photo;
            string sex;
            switch (_person.Sex)
            {
                case 'F':
                    sex = Properties.GUI.Female;
                    break;
                case 'M':
                    sex = Properties.GUI.Male;
                    break;
                default:
                    sex = Properties.GUI.Unknown;
                    break;
            }
            SexComboBox.SelectedItem = sex;

            PhotoBtn.Click += PhotoBtn_Click;
            AcceptBtn.Click += AcceptBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(Properties.GUI.confScene_SureSave, Properties.GUI.Confirmation, MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                char? sex = null;
                switch (SexComboBox.SelectedIndex)
                {
                    case 1:
                        sex = 'M';
                        break;
                    case 2:
                        sex = 'F';
                        break;
                    default:
                        sex = null;
                        break;
                }
                if (_person == null)
                {
                    var person = DataAccessFacade.Instance.GetPersonAccess().Add(NameTextBox.Text, PhotoPathTextBox.Text, BirthdayPicker.SelectedDate, sex);
                    DataAccessFacade.Instance.GetPersonAccess().AddToScene(person, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());
                    File.Copy(_path, GeneralSettings.Instance.DataDirectory.Value + "person/" + PhotoPathTextBox.Text);
                }
                else
                {
                    string oldPath = _person.Photo;
                    _person.Name = NameTextBox.Text;
                    _person.Sex = sex;
                    _person.Photo = PhotoPathTextBox.Text;
                    _person.Birthday = BirthdayPicker.SelectedDate;
                    DataAccessFacade.Instance.GetPersonAccess().Update(_person);
                    if(_path != null)
                    {
                        File.Delete(GeneralSettings.Instance.DataDirectory.Value + "person/" +oldPath);
                        File.Copy(_path, GeneralSettings.Instance.DataDirectory.Value + "person/" + PhotoPathTextBox.Text);
                    }
                        
                }
                Close();
            }
            
        }

        private void PhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                
            };
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                string fileName;
                if (File.Exists(GeneralSettings.Instance.DataDirectory.Value+"person/" + dlg.SafeFileName))
                    fileName = NameTextBox.Text + dlg.SafeFileName;
                else
                    fileName = dlg.SafeFileName;
                PhotoPathTextBox.Text = fileName;
                _path = dlg.FileName;
            }
        }
    }
}
