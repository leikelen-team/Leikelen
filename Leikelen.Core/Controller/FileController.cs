using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Persistence.Provider;
using cl.uv.leikelen.Data.Access.Internal;
using System.IO;
using System.IO.Compression;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.Data.Access;

namespace cl.uv.leikelen.Controller
{
    /// <summary>
    /// The controller to import and export from files
    /// </summary>
    public static class FileController
    {
        /// <summary>
        /// Imports the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The scene imported</returns>
        public static Scene Import(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName).ToLower();
            var settings = DataAccessFacade.Instance.GetGeneralSettings();
            if (fileExtension.Equals(".sqlite3") || fileExtension.Equals(".sqlite") || fileExtension.Equals(".db"))
            {
                return ImportSqliteFile(settings.GetIndexOfSceneInFile(), fileName);
            }
            else
            {
                InitializeDirectory();

                
                ZipFile.ExtractToDirectory(fileName, settings.GetTmpSceneDirectory());
                var insertedScene = ImportSqliteFile(settings.GetIndexOfSceneInFile(), Path.Combine(settings.GetTmpSceneDirectory(), "scene.sqlite3"));

                string tmpSceneDir = Path.Combine(settings.GetTmpSceneDirectory(), "scene/") + 1;
                if (Directory.Exists(tmpSceneDir))
                    CopyContents(tmpSceneDir, settings.GetSceneInUseDirectory());
                foreach (var person in SceneInUse.Instance.Scene.PersonsInScene)
                {
                    if (!String.IsNullOrEmpty(person.Person.Photo))
                    {
                        string tmpPhotoDirectory = Path.Combine(Path.Combine(settings.GetTmpSceneDirectory(), "person/"), person.Person.Photo);
                        if (File.Exists(tmpPhotoDirectory))
                        {
                            File.Copy(tmpPhotoDirectory,
                                Path.Combine(settings.GetDataPersonsDirectory(), person.Person.Photo));
                        }
                    }

                    foreach (var smt_pis in person.SubModalType_PersonInScenes)
                    {
                        string tmpModalDir = Path.Combine(Path.Combine(settings.GetTmpSceneDirectory(), "modal/"), smt_pis.SubModalType.ModalTypeId);
                        string dataModalDir = Path.Combine(settings.GetDataModalsDirectory(), smt_pis.SubModalType.ModalTypeId);
                        if (Directory.Exists(tmpModalDir))
                            CopyContents(tmpModalDir, dataModalDir);
                    }
                }
                return insertedScene;
            }
        }

        /// <summary>
        /// Imports from a sqlite file.
        /// </summary>
        /// <param name="index">The index of the scene to import.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The scene imported</returns>
        private static Scene ImportSqliteFile(int index, string fileName)
        {
            var sqliteProvider = new SqliteProvider();
            sqliteProvider.CreateConnection("Filename=" + fileName);
            var scene = sqliteProvider.LoadScene(sqliteProvider.LoadScenes()[index].SceneId);
            sqliteProvider.CloseConnection();
            int sceneIdInFile = scene.SceneId;
            var insertedScene = DataAccessFacade.Instance.GetSceneAccess().SaveNew(scene);
            SceneInUse.Instance.Set(insertedScene);
            return insertedScene;
        }

        /// <summary>
        /// Exports the scene in use.
        /// </summary>
        /// <param name="isOnlyBd">if set to <c>true</c> [is only database].</param>
        /// <param name="fileName">Name of the file.</param>
        public static void Export(bool isOnlyBd, string fileName)
        {
            if (isOnlyBd)
            {
                CreateSqlFile(fileName);
            }
            else
            {
                InitializeDirectory();
                var settings = DataAccessFacade.Instance.GetGeneralSettings();

                var _playerC = new PlayerController();

                var sqlFileName = Path.Combine(settings.GetTmpSceneDirectory(), "scene.sqlite3");
                CreateSqlFile(sqlFileName);

                if(Directory.Exists(settings.GetSceneInUseDirectory()))
                    CopyContents(settings.GetSceneInUseDirectory(), Path.Combine(settings.GetTmpSceneDirectory(), "scene/") + 1);
                foreach (var person in SceneInUse.Instance.Scene.PersonsInScene)
                {
                    if (!String.IsNullOrEmpty(person.Person.Photo))
                    {
                        string tmpPhotoDirectory = Path.Combine(Path.Combine(settings.GetTmpSceneDirectory(), "person/"), person.Person.Photo);
                        if (File.Exists(tmpPhotoDirectory))
                            File.Copy(Path.Combine(settings.GetDataPersonsDirectory(), person.Person.Photo), tmpPhotoDirectory);
                    }

                    foreach(var smt_pis in person.SubModalType_PersonInScenes)
                    {
                        string dataModalDir = Path.Combine(settings.GetDataModalsDirectory(), smt_pis.SubModalType.ModalTypeId);
                        string tmpModalDir = Path.Combine(Path.Combine(settings.GetTmpSceneDirectory(), "modal/"), smt_pis.SubModalType.ModalTypeId);
                        if (Directory.Exists(dataModalDir))
                        {
                            CopyContents(dataModalDir, tmpModalDir);
                        }
                            
                    }
                }
                ZipFile.CreateFromDirectory(settings.GetTmpSceneDirectory(), fileName);
            }
            
        }

        private static void CreateSqlFile(string fileName)
        {
            File.Delete(fileName);
            var sqliteProvider = new SqliteProvider();
            sqliteProvider.CreateConnection("Filename=" + fileName);
            sqliteProvider.Save(SceneInUse.Instance.Scene);
            sqliteProvider.CloseConnection();
        }

        private static void InitializeDirectory()
        {
            var settings = DataAccessFacade.Instance.GetGeneralSettings();

            Directory.Delete(DataAccessFacade.Instance.GetGeneralSettings().GetTmpSceneDirectory(), true);
            Directory.CreateDirectory(DataAccessFacade.Instance.GetGeneralSettings().GetTmpSceneDirectory());
            Directory.CreateDirectory(DataAccessFacade.Instance.GetGeneralSettings().GetDataModalsDirectory());
            Directory.CreateDirectory(DataAccessFacade.Instance.GetGeneralSettings().GetDataPersonsDirectory());
            Directory.CreateDirectory(DataAccessFacade.Instance.GetGeneralSettings().GetDataScenesDirectory());
        }

        private static void CopyContents(string sourcePath, string targetPath)
        {
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                for (int ifile = 0;ifile<files.Count();ifile++)
                {
                    var s = files[ifile];
                    // Use static Path methods to extract only the file name from the path.
                    var fileName = System.IO.Path.GetFileName(s);
                    var destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }
        }
    }
}
