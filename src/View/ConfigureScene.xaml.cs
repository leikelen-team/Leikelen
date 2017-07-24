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
using cl.uv.leikelen.src.Data.Access.Internal;
using cl.uv.leikelen.src.Data.Model;

namespace cl.uv.leikelen.src.View
{
    /// <summary>
    /// Lógica de interacción para ConfigureScene.xaml
    /// </summary>
    public partial class ConfigureScene : Window
    {
        private Scene scene;

        public ConfigureScene()
        {
            InitializeComponent();
            AcceptBtn.Click += AcceptBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
        }

        public ConfigureScene(Scene scene)
        {
            InitializeComponent();
            this.scene = scene;
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
            this.Close();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: está seguro dialog
            if (scene == null)
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
                SceneInUse.Instance.Set(scene);
            }
            this.Close();
        }
    }
}
