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
        private readonly Scene _scene;

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
            RealDatePicker.DisplayDate = scene.RecordRealDateTime;
            AcceptBtn.Click += AcceptBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: está seguro dialog
            if (_scene == null)
            {
                var newScene = new Scene()
                {
                    Name = NameTextBox.Text,
                    Type = TypeTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    NumberOfParticipants = Int32.Parse(ParticipantsTextBox.Text),
                    Place = PlaceTextBox.Text,
                    RecordRealDateTime = RealDatePicker.DisplayDate,
                    PersonInScenes = new List<PersonInScene>()
                };
                SceneInUse.Instance.Set(newScene);
            }
            else
            {
                _scene.Name = NameTextBox.Text;
                _scene.Type = TypeTextBox.Text;
                _scene.Description = DescriptionTextBox.Text;
                _scene.NumberOfParticipants = Int32.Parse(ParticipantsTextBox.Text);
                _scene.Place = PlaceTextBox.Text;
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
                SceneInUse.Instance.Set(_scene);
            }
            Close();
        }
    }
}
