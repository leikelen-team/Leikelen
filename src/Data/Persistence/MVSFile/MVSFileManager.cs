using cl.uv.leikelen.src.Data.Model.AccessLogic;
using cl.uv.leikelen.src.Data.Persistence.Interface;
using cl.uv.leikelen.src.Data.Persistence.Sqlite;
using cl.uv.leikelen.src.Input.Kinect;
using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace cl.uv.leikelen.src.Data.Persistence.MVSFile
{
    public class MVSFileManager
    {
        private static readonly string MvsExtension = ".mvs";
        private static readonly string MvsFilter = "Multimodal Visualizer Scene (*.mvs)|*.mvs";
        private static IBackupDataContext dataContext = new SqliteDataContext();

        public static void Import()
        {
            var dlg = new OpenFileDialog()
            {
                DefaultExt = MvsExtension,
                Filter = MvsFilter
            };


            if (dlg.ShowDialog().GetValueOrDefault())
            {
                KinectMediaFacade.Instance.Player.Close();
                if (File.Exists(CoreSettings.Instance.currentKdvrFile)) File.Delete(CoreSettings.Instance.currentKdvrFile);
                if (File.Exists(CoreSettings.Instance.currentDataFile)) File.Delete(CoreSettings.Instance.currentDataFile);
                ZipFile.ExtractToDirectory(dlg.FileName, CoreSettings.Instance.currentSceneDirectory);
                KinectMediaFacade.Instance.Player.OpenFile(CoreSettings.Instance.currentKdvrFile);
                StaticScene.Instance.CreateFromDbContext(dataContext, "Filename=" + CoreSettings.Instance.currentDataFile);
            }
        }

        public static void Export()
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

                dataContext.SaveScene(StaticScene.Instance);
                ZipFile.CreateFromDirectory(CoreSettings.Instance.currentSceneDirectory, dlg.FileName);

                if (wasOpened)
                {
                    KinectMediaFacade.Instance.Player.OpenFile(CoreSettings.Instance.currentKdvrFile);
                }
                MessageBox.Show("Scene exported succefully");
            }
        }
    }
}
