using Microsoft.Samples.Kinect.VisualizadorMultimodal.db;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.utils;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.windows
{
    /// <summary>
    /// Interaction logic for AddOrEditPostureType.xaml
    /// </summary>
    public partial class AddOrEditPostureType : Window
    {
        //public enum Action { Add, Edit };
        private PostureType editingPosture = null;
        private string path = null;

        public AddOrEditPostureType()
        {
            InitializeComponent();
        }

        public AddOrEditPostureType(PostureType editingPosture)
        {
            InitializeComponent();
            this.editingPosture = editingPosture;
            this.fileNameTextBox.Text = System.IO.Path.GetFileName(editingPosture.Path);
            this.nameTextBox.Text = editingPosture.Name;
            this.path = editingPosture.Path;
        }

        
        //private string name = null;
       


        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            Win32.OpenFileDialog dlg = new Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".gbd"; // Default file extension
            dlg.Filter = "Gesture build data" + " " + "(*.gbd)|*.gbd"; // Filter files by extension
            dlg.Title = "Importar postura";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                //Console.WriteLine("filePath: " + dlg.FileName);
                this.fileNameTextBox.Text = dlg.SafeFileName;
                this.path = dlg.FileName;
                
                //File.Copy(this.playingFilePath, @"Database\"+dlg.N);

                //this.kstudio.ImportScene(dlg.FileName);
                //enableButtons();
                //this.Title = "Visualizador Multimodal - " + Scene.Instance.name;
                //sceneTitleLabel.Content = Scene.Instance.name;
                //sceneDurationLabel.Content = Scene.Instance.duration.ToString(@"hh\:mm\:ss");
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.path == null)
            {
                MessageBox.Show("Debes ingresar un archivo");
            }
            string name = nameTextBox.Text;
            if (PostureTypeContext.db.PostureType.Any(p => p.Name == name))
            {
                if(editingPosture == null)
                {
                    MessageBox.Show("La postura '"+name+"' ya existe. Utilize otro nombre.");
                }else if( editingPosture.Name != name)
                {
                    MessageBox.Show("La postura '" + name + "' ya existe. Utilize otro nombre.");
                }else if( editingPosture.Name == name && editingPosture.Path == path)
                {
                    this.Close();
                }
            }
            else if (this.path != null && name != "")
            {
                Console.WriteLine("Saving posture: name: {0} --- path: {1}", name, this.path);
                

                string internalFilePath = @"Database\" + fileNameTextBox.Text;
                if ( !File.Exists(internalFilePath) )
                    File.Copy(path, internalFilePath);
                if (editingPosture != null)
                {
                    PostureType postureToUpdate = PostureTypeContext.db.PostureType
                        .FirstOrDefault(p => p.PostureTypeId == editingPosture.PostureTypeId);
                    string oldPath = postureToUpdate.Path;
                    postureToUpdate.Name = name;
                    postureToUpdate.Path = internalFilePath;
                    //SqliteAppContext.db.Entry(postureToUpdate).CurrentValues.SetValues(product);
                    PostureTypeContext.db.SaveChanges();

                    if ( !PostureTypeContext.db.PostureType.Any(p => p.Path == oldPath)
                        && oldPath != internalFilePath )
                    {
                        File.Delete(oldPath);
                    }
                    

                }
                else
                {
                    PostureTypeContext.db.PostureType.Add(new PostureType(name, internalFilePath));
                }
                
                PostureTypeContext.db.SaveChanges();
                MainWindow.postureCrud.refreshList();
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
