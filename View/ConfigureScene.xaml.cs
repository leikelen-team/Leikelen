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
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.Properties;

namespace cl.uv.leikelen.View
{
    /// <summary>
    /// Interaction logic for ConfigureScene.xaml
    /// </summary>
    /// <summary xml:lang="es">
    /// Lógica de interacción para ConfigureScene.xaml
    /// </summary>
    public partial class ConfigureScene : Window
    {
        private Scene _scene;

        public ConfigureScene()
        {
            InitializeComponent();
            AcceptBtn.Click += AcceptBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
        }

        public ConfigureScene(Scene scene)
        {
            InitializeComponent();
            _scene = scene;
            NameTextBox.Text = scene.Name;
            TypeTextBox.Text = scene.Type;
            DescriptionTextBox.Text = scene.Description;
            ParticipantsTextBox.Text = scene.NumberOfParticipants.ToString();
            PlaceTextBox.Text = scene.Place;
            RealDatePicker.Text = scene.RecordRealDateTime.ToString("dd-MM-yyyy");
            RealTimePicker.Text = scene.RecordRealDateTime.ToString("hh:mm");
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

                if (_scene == null)
                {
                    _scene = new Scene();
                }
                _scene.Name = NameTextBox?.Text;
                _scene.Type = TypeTextBox?.Text;
                _scene.Description = DescriptionTextBox?.Text;
                _scene.NumberOfParticipants = Int32.Parse(ParticipantsTextBox.Text);
                _scene.Place = PlaceTextBox?.Text;
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

                SceneInUse.Instance.Set(_scene);
                Close();
            }
        }
    }
}
