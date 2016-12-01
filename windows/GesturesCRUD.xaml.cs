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
using cl.uv.multimodalvisualizer.db;
using cl.uv.multimodalvisualizer.models;

namespace cl.uv.multimodalvisualizer.windows
{
    /// <summary>
    /// Lógica de interacción para GesturesCRUD.xaml
    /// </summary>
    public partial class GesturesCRUD : Window
    {
        public GesturesCRUD()
        {
            InitializeComponent();
            this.refreshList();
        }


        public void refreshList()
        {
            gestureCrudDataGrid.ItemsSource = PostureTypeContext.db.myGesture.ToList();
        }

        private void addGestureButton_Click(object sender, RoutedEventArgs e)
        {
            AssociatePostures associatePsView = new AssociatePostures();
            associatePsView.Show();
        }

        private void editGestureButton_Click(object sender, RoutedEventArgs e)
        {
            myGesture selectedPosture = (myGesture)gestureCrudDataGrid.SelectedItem;
            selectedPosture = PostureTypeContext.db.myGesture.FirstOrDefault(p => p.myGestureId == selectedPosture.myGestureId);
            AssociatePostures addPostureView = new AssociatePostures(selectedPosture);
            addPostureView.Show();
        }

        private void removeGestureButton_Click(object sender, RoutedEventArgs e)
        {
            myGesture selectedItem = (myGesture)gestureCrudDataGrid.SelectedItem;
            PostureTypeContext.db.myGesture.Remove(PostureTypeContext.db.myGesture.FirstOrDefault(p => p.myGestureId == selectedItem.myGestureId));
            PostureTypeContext.db.SaveChanges();
            this.refreshList();
            
        }
    }
}
