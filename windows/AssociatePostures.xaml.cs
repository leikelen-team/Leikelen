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
    /// Lógica de interacción para AssociatePostures.xaml
    /// </summary>
    public partial class AssociatePostures : Window
    {
        private myGesture editingGesture = null;
        public AssociatePostures()
        {
            InitializeComponent();
            refreshCombos();
        }

        public AssociatePostures(myGesture gesture)
        {
            InitializeComponent();
            //this.refreshCombos();
            this.editingGesture = gesture;
            this.nameTextBox.Text = gesture.name;
            this.startCombo.SelectedItem = gesture.startPosture;
            this.continuosCombo.SelectedItem = gesture.continuousPosture;
            this.endCombo.SelectedItem = gesture.endPosture;
        }

        public void refreshCombos()
        {
            List<PostureType> postures = PostureTypeContext.db.PostureType.ToList();
            startCombo.ItemsSource = postures;
            continuosCombo.ItemsSource = postures;
            endCombo.ItemsSource = postures;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text;
            List<PostureType> postures = PostureTypeContext.db.PostureType.ToList();
            if (PostureTypeContext.db.myGesture.Any(p => p.name == name))
            {
                if (editingGesture == null || editingGesture.name != name)
                {
                    MessageBox.Show("La postura '" + name + "' ya existe. Utilize otro nombre.");
                }
            }
            else 
            {
                Console.WriteLine("Saving posture: name: {0}", name);
                if (editingGesture != null)
                {
                    myGesture gestureToUpdate = PostureTypeContext.db.myGesture
                        .FirstOrDefault(g => g.myGestureId == editingGesture.myGestureId);
                    gestureToUpdate.name = name;
                    gestureToUpdate.startPosture = postures.Find(p => p.Name == startCombo.Text);
                    gestureToUpdate.endPosture = postures.Find(p => p.Name == endCombo.Text);
                    gestureToUpdate.continuousPosture = postures.Find(p => p.Name == continuosCombo.Text);
                    //SqliteAppContext.db.Entry(postureToUpdate).CurrentValues.SetValues(product);
                    PostureTypeContext.db.SaveChanges();
                }
                else
                {
                    PostureType start = postures.Find(p => p.Name == startCombo.Text);
                    PostureType cont = postures.Find(p => p.Name == continuosCombo.Text);
                    PostureType end = postures.Find(p => p.Name == endCombo.Text);
                    PostureIntervalGroup pigGesture = new PostureIntervalGroup(new PostureType(name, ""));
                    int id = 0;
                    try
                    {
                        id = PostureTypeContext.db.myGesture.Last().myGestureId + 1;
                    }
                    catch (Exception excp)
                    {
                        
                        id = 0;
                    }
                    myGesture gest = new myGesture(id, name, start, cont, end, pigGesture);
                    PostureTypeContext.db.myGesture.Add(gest);
                    PostureTypeContext.db.SaveChanges();
                }
            

                
                //MainWindow.gestureCRUD.refreshList();
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
