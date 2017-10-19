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
        private string _originPhotoPath;
        private string _targetPhotoName;

        public ConfigurePerson()
        {
            InitializeComponent();
            PutPhoto("none.png");
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
            _targetPhotoName = _person.Photo;
            PutPhoto(_person.Photo);
            string sex;
            switch (_person.Sex)
            {
                case 0:
                    sex = Properties.GUI.Male;
                    break;
                case 1:
                    sex = Properties.GUI.Female;
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
            MessageBoxResult result = MessageBox.Show(Properties.GUI.confScene_SureSave,
                Properties.GUI.Confirmation, 
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                int? sex = null;
                switch (SexComboBox.SelectedIndex)
                {
                    case 1:
                        sex = 0;
                        break;
                    case 2:
                        sex = 1;
                        break;
                    default:
                        sex = null;
                        break;
                }
                if (ReferenceEquals(null, _person))
                {
                    var person = DataAccessFacade.Instance.GetPersonAccess().Add(NameTextBox.Text, _targetPhotoName, BirthdayPicker.SelectedDate, sex);
                    DataAccessFacade.Instance.GetPersonAccess().AddToScene(person, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());
                    if(!ReferenceEquals(null, _originPhotoPath) && !ReferenceEquals(null, _targetPhotoName))
                        File.Copy(_originPhotoPath, DataAccessFacade.Instance.GetGeneralSettings().GetDataPersonsDirectory() + _targetPhotoName);
                }
                else
                {
                    string oldPath = _person.Photo;
                    _person.Name = NameTextBox.Text;
                    _person.Sex = sex;
                    _person.Photo = _targetPhotoName;
                    _person.Birthday = BirthdayPicker.SelectedDate;
                    DataAccessFacade.Instance.GetPersonAccess().Update(_person);
                    if(!String.IsNullOrEmpty(_originPhotoPath) && !String.IsNullOrEmpty(_targetPhotoName))
                    {
                        try
                        {
                            var oldFullPath = DataAccessFacade.Instance.GetGeneralSettings().GetDataPersonsDirectory() + oldPath;
                            if (File.Exists(oldFullPath))
                                File.Delete(oldFullPath);
                        }
                        catch(Exception)
                        {

                        }
                        File.Copy(_originPhotoPath, DataAccessFacade.Instance.GetGeneralSettings().GetDataPersonsDirectory() + _targetPhotoName);
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
                if (File.Exists(DataAccessFacade.Instance.GetGeneralSettings().GetDataPersonsDirectory() + dlg.SafeFileName))
                {
                    do
                    {
                        fileName = Util.StringUtil.RandomString(7) + System.IO.Path.GetExtension(dlg.SafeFileName);
                    } while (File.Exists(DataAccessFacade.Instance.GetGeneralSettings().GetDataPersonsDirectory() + fileName));
                }
                else
                    fileName = dlg.SafeFileName;
                _targetPhotoName = fileName;
                _originPhotoPath = dlg.FileName;
                PhotoPathTextBox.Text = _originPhotoPath;
                
                PutPhoto(_originPhotoPath);
            }
        }

        private void PutPhoto(string personPhoto)
        {
            if (!String.IsNullOrEmpty(personPhoto) &&
                File.Exists(System.IO.Path.Combine(DataAccessFacade.Instance.GetGeneralSettings().GetDataPersonsDirectory(), personPhoto)))
            {
                personPhotoImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(System.IO.Path.Combine(DataAccessFacade.Instance.GetGeneralSettings().GetDataPersonsDirectory(), personPhoto))));
            }
            else
            {
                personPhotoImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(System.IO.Path.Combine(DataAccessFacade.Instance.GetGeneralSettings().GetDataPersonsDirectory(), "none.png"))));
            }
        }
    }
}
