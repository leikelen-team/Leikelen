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
using System.IO;
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.Properties;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Util;

namespace cl.uv.leikelen.View
{
    /// <summary>
    /// Interaction logic for ConfigureScene.xaml
    /// </summary>
    public partial class ConfigureScene : Window
    {
        private Scene _scene;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureScene"/> class
        /// to create a new scene.
        /// </summary>
        public ConfigureScene()
        {
            InitializeComponent();
            
            AcceptBtn.Click += AcceptBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureScene"/> class
        /// with a scene to edit.
        /// </summary>
        /// <param name="scene">The scene.</param>
        public ConfigureScene(Scene scene)
        {
            InitializeComponent();
            _scene = scene;
            NameTextBox.Text = scene.Name;
            TypeTextBox.Text = scene.Type;
            DescriptionTextBox.Text = scene.Description;
            ParticipantsTextBox.Text = scene.NumberOfParticipants?.ToString();
            PlaceTextBox.Text = scene.Place;
            RealDatePicker.Text = scene.RecordRealDateTime?.ToString("dd-MM-yyyy");
            RealTimePicker.Text = scene.RecordRealDateTime?.ToString("hh:mm");
            AcceptBtn.Click += AcceptBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(GUI.confScene_SureSave, GUI.confScene_SureSaveTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                int numberOfParticipants = 0;
                

                if (ReferenceEquals(null, _scene))
                {
                    _scene = new Scene();
                }
                _scene.Name = NameTextBox?.Text;
                _scene.Type = TypeTextBox?.Text;
                _scene.Place = PlaceTextBox?.Text;
                _scene.Description = DescriptionTextBox?.Text;
                if (int.TryParse(ParticipantsTextBox.Text, out numberOfParticipants))
                {
                    _scene.NumberOfParticipants = numberOfParticipants;
                }
                if (!String.IsNullOrEmpty(RealDatePicker.Text))
                {
                    if (RealTimePicker.SelectedTime.HasValue)
                    {
                        _scene.RecordRealDateTime = new DateTime(RealDatePicker.DisplayDate.Year,
                            RealDatePicker.DisplayDate.Month, RealDatePicker.DisplayDate.Day,
                            RealTimePicker.SelectedTime.Value.Hour, RealTimePicker.SelectedTime.Value.Minute,
                            RealTimePicker.SelectedTime.Value.Second);
                    }
                    else
                    {
                        _scene.RecordRealDateTime = new DateTime(RealDatePicker.DisplayDate.Year,
                            RealDatePicker.DisplayDate.Month, RealDatePicker.DisplayDate.Day, 0, 0, 0);
                    }
                }
                else
                {
                    if (RealTimePicker.SelectedTime.HasValue)
                    {
                        _scene.RecordRealDateTime = new DateTime(0, 0, 0,
                            RealTimePicker.SelectedTime.Value.Hour, RealTimePicker.SelectedTime.Value.Minute,
                            RealTimePicker.SelectedTime.Value.Second);
                    }
                }
                var createdScene = DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(_scene);
                Directory.CreateDirectory(DataAccessFacade.Instance.GetGeneralSettings().GetDataScenesDirectory()+ createdScene.SceneId);
                SceneInUse.Instance.Set(createdScene);
                Close();
            }
        }

        private void ParticipantsTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            IntegerInput.PastingHandler(sender, e);
        }

        private void ParticipantsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IntegerInput.PreviewTextInputHandler(sender, e);
        }
    }
}
