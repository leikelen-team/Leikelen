using cl.uv.leikelen.src.dbcontext;
using cl.uv.leikelen.src.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cl.uv.leikelen.src.view.window
{
    /// <summary>
    /// Interaction logic for AddOrEditPostureType.xaml
    /// </summary>
    public partial class AddOrEditPostureType : Window
    {
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

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".gbd"; // Default file extension
            dlg.Filter = "Gesture build data" + " " + "(*.gbd)|*.gbd"; // Filter files by extension
            dlg.Title = "Importar postura";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                this.fileNameTextBox.Text = dlg.SafeFileName;
                this.path = dlg.FileName;
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
                    PostureTypeContext.db.SaveChanges();

                    if ( !PostureTypeContext.db.PostureType.Any(p => p.Path == oldPath)
                        && oldPath != internalFilePath )
                    {
                        File.Delete(oldPath);
                    }
                    

                }
                else
                {
                    int id = PostureTypeContext.db.PostureType.Last().PostureTypeId+1;
                    PostureTypeContext.db.PostureType.Add(new PostureType(name, internalFilePath) { PostureTypeId = id});
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
