using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Persistence.Provider;
using cl.uv.leikelen.Data.Access.Internal;
using System.IO;
using System.IO.Compression;

namespace cl.uv.leikelen.Controller
{
    public static class FileController
    {
        public static void Import(string fileName)
        {
            Directory.Delete(GeneralSettings.Instance.GetTmpSceneDirectory(), true);
            Directory.CreateDirectory(GeneralSettings.Instance.GetTmpSceneDirectory());
            Directory.CreateDirectory(GeneralSettings.Instance.GetTmpSceneDirectory() + "modal/");
            Directory.CreateDirectory(GeneralSettings.Instance.GetTmpSceneDirectory() + "person/");
            Directory.CreateDirectory(GeneralSettings.Instance.GetTmpSceneDirectory() + "scene/");

            System.IO.Compression.ZipFile.ExtractToDirectory(fileName, GeneralSettings.Instance.GetTmpSceneDirectory());
            var sqliteProvider = new SqliteProvider();
            sqliteProvider.CreateConnection("Filename=" + GeneralSettings.Instance.GetTmpSceneDirectory()+ "scene.sqlite3");
            var scene = sqliteProvider.LoadScene(sqliteProvider.LoadScenes()[0].SceneId);
            int sceneIdInFile = scene.SceneId;
            SceneInUse.Instance.Set(Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveNew(scene));

            Console.WriteLine($"in file: {sceneIdInFile}, in real: {SceneInUse.Instance.Scene.SceneId}");
            Console.WriteLine($"{GeneralSettings.Instance.GetTmpSceneDirectory() + "scene/" + sceneIdInFile}");


            if(Directory.Exists(GeneralSettings.Instance.GetTmpSceneDirectory() + "scene/" + 1))
                CopyContents(GeneralSettings.Instance.GetTmpSceneDirectory() + "scene/" + 1,
                GeneralSettings.Instance.GetDataDirectory() + "scene/" + SceneInUse.Instance.Scene.SceneId);
            foreach (var person in SceneInUse.Instance.Scene.PersonsInScene)
            {
                if(!String.IsNullOrEmpty(person.Person.Photo) &&
                    File.Exists(GeneralSettings.Instance.GetTmpSceneDirectory() + "person/" + person.Person.Photo))
                    File.Copy(GeneralSettings.Instance.GetTmpSceneDirectory() + "person/" + person.Person.Photo, 
                    GeneralSettings.Instance.GetDataDirectory() + "person/" + person.Person.Photo);

                foreach (var smt_pis in person.SubModalType_PersonInScenes)
                {
                    if(Directory.Exists(GeneralSettings.Instance.GetTmpSceneDirectory() + "modal/" + smt_pis.SubModalType.ModalTypeId))
                        CopyContents(GeneralSettings.Instance.GetTmpSceneDirectory() + "modal/" + smt_pis.SubModalType.ModalTypeId,
                        GeneralSettings.Instance.GetDataDirectory() + "modal/" + smt_pis.SubModalType.ModalTypeId);
                }
            }
        }

        public static void Export(bool isOnlyBd, string fileName)
        {
            if (isOnlyBd)
            {
                CreateSqlFile(fileName);
            }
            else
            {
                Directory.Delete(GeneralSettings.Instance.GetTmpSceneDirectory(), true);
                Directory.CreateDirectory(GeneralSettings.Instance.GetTmpSceneDirectory());
                Directory.CreateDirectory(GeneralSettings.Instance.GetTmpSceneDirectory() + "modal/");
                Directory.CreateDirectory(GeneralSettings.Instance.GetTmpSceneDirectory() + "person/");
                Directory.CreateDirectory(GeneralSettings.Instance.GetTmpSceneDirectory() + "scene/");

                var _playerC = new PlayerController();

                var sqlFileName = GeneralSettings.Instance.GetTmpSceneDirectory() + "scene.sqlite3";
                CreateSqlFile(sqlFileName);

                if(Directory.Exists(GeneralSettings.Instance.GetDataDirectory() + "scene/" + SceneInUse.Instance.Scene.SceneId))
                    CopyContents(GeneralSettings.Instance.GetDataDirectory() + "scene/" + SceneInUse.Instance.Scene.SceneId, 
                    GeneralSettings.Instance.GetTmpSceneDirectory() + "scene/" + 1);
                foreach (var person in SceneInUse.Instance.Scene.PersonsInScene)
                {
                    if (!String.IsNullOrEmpty(person.Person.Photo))
                        File.Copy(GeneralSettings.Instance.GetDataDirectory() + "person/" + person.Person.Photo,
                        GeneralSettings.Instance.GetTmpSceneDirectory() + "person/" + person.Person.Photo);

                    foreach(var smt_pis in person.SubModalType_PersonInScenes)
                    {
                        if (Directory.Exists(GeneralSettings.Instance.GetDataDirectory() + "modal/" + smt_pis.SubModalType.ModalTypeId))
                        {
                            CopyContents(GeneralSettings.Instance.GetDataDirectory() + "modal/" + smt_pis.SubModalType.ModalTypeId,
                            GeneralSettings.Instance.GetTmpSceneDirectory() + "modal/" + smt_pis.SubModalType.ModalTypeId);
                        }
                            
                    }
                }
                System.IO.Compression.ZipFile.CreateFromDirectory(GeneralSettings.Instance.GetTmpSceneDirectory(),
                    fileName);
                
                //TODO: guardar sqlite en tmp, y luego comprimir y mandar a fileName
            }
            
        }

        private static void CreateSqlFile(string fileName)
        {
            var sqliteProvider = new SqliteProvider();
            sqliteProvider.CreateConnection("Filename=" + fileName);
            sqliteProvider.Save(SceneInUse.Instance.Scene);
            sqliteProvider.CloseConnection();
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
                Console.WriteLine($"archivos: {files}");

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
