﻿using Microsoft.Samples.Kinect.VisualizadorMultimodal.db;
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
        public AddOrEditPostureType()
        {
            InitializeComponent();
        }
        private string path = null;
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
            string name = nameTextBox.Text;
            if (this.path != null && name != "")
            {
                Console.WriteLine("Saving posture: name: {0} --- path: {1}", name, this.path);
                string internalFilePath = @"Database\" + fileNameTextBox.Text;
                if ( !File.Exists(internalFilePath) )
                    File.Copy(path, internalFilePath);
                SqliteAppContext.db.PostureType.Add(new PostureType(name, internalFilePath));
                SqliteAppContext.db.SaveChanges();
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