using cl.uv.multimodalvisualizer.src.dbcontext;
using cl.uv.multimodalvisualizer.src.model;
using cl.uv.multimodalvisualizer.src.kinectmedia;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace cl.uv.multimodalvisualizer.src.helpers
{
    public class IO
    {

        private static string MvsExtension = ".mvs";
        private static string MvsFilter = "Multimodal Visualizer Scene (*.mvs)|*.mvs";

        public static void EnsureDirectoriesHasBeenCreated()
        {
            if (!Directory.Exists(Properties.Paths.tmpDirectory)) Directory.CreateDirectory(Properties.Paths.tmpDirectory);
            if (!Directory.Exists(Properties.Paths.CurrentSceneDirectory)) Directory.CreateDirectory(Properties.Paths.CurrentSceneDirectory);
            if (!Directory.Exists(Properties.Paths.ImportedSceneDirectory)) Directory.CreateDirectory(Properties.Paths.ImportedSceneDirectory);
            if (!Directory.Exists(Properties.Paths.RecordedSceneDirectory)) Directory.CreateDirectory(Properties.Paths.RecordedSceneDirectory);
        }

        public static void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                DefaultExt = MvsExtension,
                Filter = MvsFilter
            };


            if (dlg.ShowDialog().GetValueOrDefault())
            {
                KinectMediaFacade.Instance.Player.Close();
                if (File.Exists(Properties.Paths.CurrentKdvrFile)) File.Delete(Properties.Paths.CurrentKdvrFile);
                if (File.Exists(Properties.Paths.CurrentDataFile)) File.Delete(Properties.Paths.CurrentDataFile);
                ZipFile.ExtractToDirectory(dlg.FileName, Properties.Paths.CurrentSceneDirectory);
                KinectMediaFacade.Instance.Player.OpenFile(Properties.Paths.CurrentKdvrFile);
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
                if (KinectMediaFacade.Instance.Player.IsOpen)
                {
                    KinectMediaFacade.Instance.Player.Close();
                    wasOpened = true;
                }

                BackupDataContext.SaveScene(Properties.Paths.CurrentDataFile);
                ZipFile.CreateFromDirectory(Properties.Paths.CurrentSceneDirectory, dlg.FileName);

                if (wasOpened)
                {
                    KinectMediaFacade.Instance.Player.OpenFile(Properties.Paths.CurrentKdvrFile);
                }
                MessageBox.Show("Scene exported succefully");
            }
        }

        
    }
}
