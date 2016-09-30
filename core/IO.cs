//cl.uv.multimodalvisualizer
using cl.uv.multimodalvisualizer.db;
using cl.uv.multimodalvisualizer.models;
using cl.uv.multimodalvisualizer.views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cl.uv.multimodalvisualizer.core
{
    public class IO
    {

        private static string MvsExtension = ".mvs";
        private static string MvsFilter = "Multimodal Visualizer Scene (*.mvs)|*.mvs";

        public static void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                DefaultExt = MvsExtension,
                Filter = MvsFilter
            };


            if (dlg.ShowDialog().GetValueOrDefault())
            {
                Kinect.Instance.Player.Close();
                if (File.Exists(Properties.Paths.CurrentKdvrFile)) File.Delete(Properties.Paths.CurrentKdvrFile);
                if (File.Exists(Properties.Paths.CurrentDataFile)) File.Delete(Properties.Paths.CurrentDataFile);

                ZipFile.ExtractToDirectory(dlg.FileName, Properties.Paths.CurrentSceneDirectory);
                Kinect.Instance.Player.OpenFile(Properties.Paths.CurrentKdvrFile);
                Scene.CreateFromDbContext(Properties.Paths.CurrentDataFile);
                MainWindow.Instance().SourceComboBox.SelectedIndex = 1;
            }
        }

        public static void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog()
            {
                FileName = DateTime.Now.ToString("yyyy-MM-dd _ hh-mm-ss"),
                DefaultExt = MvsExtension,
                Filter = MvsFilter
            };

            if (dlg.ShowDialog().GetValueOrDefault())
            {
                //string tmpScenePath = Properties.Paths.tmpDirectory + @"\exporting_scene";
                //Utils.DirectoryCopy(Properties.Paths.RecordedSceneDirectory, tmpScenePath);
                //if (File.Exists(dlg.FileName)) File.Delete(dlg.FileName);

                if (File.Exists(dlg.FileName))
                {
                    System.Windows.Forms.DialogResult dialogResult =
                    System.Windows.Forms.MessageBox.Show(
                        "This file already exists. Replace file?",
                        "Think about it!",
                        System.Windows.Forms.MessageBoxButtons.YesNo);
                    if (dialogResult == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }

                bool wasOpened = false;
                if (Kinect.Instance.Player.IsOpen)
                {
                    Kinect.Instance.Player.Close();
                    wasOpened = true;
                }

                BackupDataContext.SaveScene(Properties.Paths.CurrentDataFile);
                ZipFile.CreateFromDirectory(Properties.Paths.CurrentSceneDirectory, dlg.FileName);

                if (wasOpened)
                {
                    Kinect.Instance.Player.OpenFile(Properties.Paths.CurrentKdvrFile);
                }
                MessageBox.Show("Scene exported succefully");
            }
        }

        
    }
}
