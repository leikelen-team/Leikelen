using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;
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

namespace cl.uv.leikelen.Module.General.GestureConfiguration.View
{
    /// <summary>
    /// Lógica de interacción para PostureCRUD.xaml
    /// </summary>
    public partial class PostureCRUD : Window, ICloneable
    {

        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public static readonly string[] PostureTypes = new string[] { "Discrete Posture", "Continuous Posture" };

        public PostureCRUD()
        {
            InitializeComponent();
            
            Title = Properties.GestureConfiguration.PostureCRUDTitle;
            RefreshList();
        }

        private void RefreshList()
        {
            //get all discrete and continuous postures
            var discretePostures = _dataAccessFacade.GetSubModalAccess().GetAll("Discrete Posture");
            var continuousPostures = _dataAccessFacade.GetSubModalAccess().GetAll("Continuous Posture");
            var postureList = new List<Posture>();

            foreach (var discrete in discretePostures)
            {
                postureList.Add(new Posture(discrete.SubModalTypeId, discrete.File, discrete.Description, 0));
            }
            foreach (var continuous in continuousPostures)
            {
                postureList.Add(new Posture(continuous.SubModalTypeId, continuous.File, continuous.Description, 1));
            }

            postureCrudDataGrid.ItemsSource = postureList;
            postureCrudDataGrid.Items.Refresh();
        }

        private void AddPostureButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditPosture addPostureView = new AddOrEditPosture();
            addPostureView.Show();
            addPostureView.Closed += (closedSender, closedE) =>
            {
                RefreshList();
            };
        }

        private void RemovePostureButton_Click(object sender, RoutedEventArgs e)
        {
            if(postureCrudDataGrid.SelectedItem is Posture selectedPosture)
            {
                _dataAccessFacade.GetSubModalAccess().Delete(PostureTypes[selectedPosture.Type], selectedPosture.Name);
                RefreshList();
            }
        }

        private void EditPostureButton_Click(object sender, RoutedEventArgs e)
        {
            if (postureCrudDataGrid.SelectedItem is Posture selectedPosture)
            {
                SubModalType subModalType = _dataAccessFacade.GetSubModalAccess().Get(PostureTypes[selectedPosture.Type], selectedPosture.Name);
                AddOrEditPosture addPostureView = new AddOrEditPosture(selectedPosture);
                addPostureView.Show();
                addPostureView.Closed += (closedSender, closedE) =>
                {
                    RefreshList();
                };
            }
        }

        public object Clone()
        {
            return new PostureCRUD();
        }
    }

    public class Posture
    {
        public string Name { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public string GestureType { get; set; }
        public int Type { get; set; }

        public Posture(string name, string fileName, string description, int type)
        {
            Name = name;
            File = fileName;
            Description = description;
            Type = type;
            switch (Type)
            {
                case 0:
                    GestureType = Properties.GestureConfiguration.DiscretePosture;
                    break;
                case 1:
                    GestureType = Properties.GestureConfiguration.ContinuousPosture;
                    break;
            }
        }
    }
}
