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

namespace cl.uv.leikelen.InputModule.Kinect.View
{
    /// <summary>
    /// Lógica de interacción para PostureCRUD.xaml
    /// </summary>
    public partial class PostureCRUD : Window, ICloneable
    {

        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public PostureCRUD()
        {
            InitializeComponent();
            refreshList();
        }

        private void refreshList()
        {
            //get all discrete and continuous postures
            var discretePostures = _dataAccessFacade.GetSubModalAccess().GetAll("Discrete Posture");
            var continuousPostures = _dataAccessFacade.GetSubModalAccess().GetAll("Continuous Posture");
            var postureList = new List<Posture>();

            foreach (var discrete in discretePostures)
            {
                postureList.Add(new Posture(discrete.SubModalTypeId, discrete.File, "Discrete Posture"));
            }
            foreach (var continuous in continuousPostures)
            {
                postureList.Add(new Posture(continuous.SubModalTypeId, continuous.File, "Continuous Posture"));
            }

            postureCrudDataGrid.ItemsSource = postureList;
        }

        private void addPostureButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditPosture addPostureView = new AddOrEditPosture();
            addPostureView.Show();
        }

        private void removePostureButton_Click(object sender, RoutedEventArgs e)
        {
            Posture selectedPosture = postureCrudDataGrid.SelectedItem as Posture;
            if (selectedPosture == null)
                return;
            _dataAccessFacade.GetSubModalAccess().Delete(selectedPosture?.GestureType, selectedPosture?.Name);
            this.refreshList();

        }

        private void editPostureButton_Click(object sender, RoutedEventArgs e)
        {
            Posture selectedPosture = postureCrudDataGrid.SelectedItem as Posture;
            if (selectedPosture == null)
                return;
            SubModalType subModalType = _dataAccessFacade.GetSubModalAccess().Get(selectedPosture?.GestureType, selectedPosture?.Name);
            AddOrEditPosture addPostureView = new AddOrEditPosture(selectedPosture);
            addPostureView.Show();
        }

        public object Clone()
        {
            return new PostureCRUD();
        }
    }

    public class Posture
    {
        public string Name;
        public string File;
        public string GestureType;

        public Posture(string name, string fileName, string gestureType)
        {
            Name = name;
            File = fileName;
            GestureType = gestureType;
        }
    }
}
