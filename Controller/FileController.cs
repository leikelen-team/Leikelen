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
    public class FileController
    {
        public void Import(string fileName)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(fileName, GeneralSettings.Instance.GetTmpSceneDirectory());
            var sqliteProvider = new SqliteProvider();
            sqliteProvider.CreateConnection("Filename=" + GeneralSettings.Instance.GetTmpSceneDirectory()+ "scene.sqlite3");
            var scene = sqliteProvider.LoadScenes()[0];
            Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveNew(scene);

            CopyContents(GeneralSettings.Instance.GetTmpSceneDirectory() + "scene/" + SceneInUse.Instance.Scene.SceneId,
                GeneralSettings.Instance.GetDataDirectory() + "scene/" + SceneInUse.Instance.Scene.SceneId);
            foreach (var person in SceneInUse.Instance.Scene.PersonsInScene)
            {
                CopyContents(GeneralSettings.Instance.GetTmpSceneDirectory() + "person/" + person.PersonId,
                GeneralSettings.Instance.GetDataDirectory() + "person/" + person.PersonId);

                foreach (var smt_pis in person.SubModalType_PersonInScenes)
                {
                    CopyContents(GeneralSettings.Instance.GetTmpSceneDirectory() + "modal/" + smt_pis.SubModalTypeId,
                        GeneralSettings.Instance.GetDataDirectory() + "modal/" + smt_pis.SubModalTypeId);
                }
            }
        }

        public void Export(bool isOnlyBd, string fileName)
        {
            if (isOnlyBd)
            {
                CreateSqlFile(fileName);
            }
            else
            {
                var sqlFileName = GeneralSettings.Instance.GetTmpSceneDirectory() + "scene.sqlite3";
                CreateSqlFile(sqlFileName);

                CopyContents(GeneralSettings.Instance.GetDataDirectory() + "scene/" + SceneInUse.Instance.Scene.SceneId, 
                    GeneralSettings.Instance.GetTmpSceneDirectory() + "scene/" + SceneInUse.Instance.Scene.SceneId);
                foreach (var person in SceneInUse.Instance.Scene.PersonsInScene)
                {
                    CopyContents(GeneralSettings.Instance.GetDataDirectory() + "person/" + person.PersonId,
                    GeneralSettings.Instance.GetTmpSceneDirectory() + "person/" + person.PersonId);

                    foreach(var smt_pis in person.SubModalType_PersonInScenes)
                    {
                        if (!System.IO.Directory.Exists(GeneralSettings.Instance.GetDataDirectory() + "modal/" + smt_pis.SubModalTypeId))
                        {
                            CopyContents(GeneralSettings.Instance.GetDataDirectory() + "modal/" + smt_pis.SubModalTypeId,
                            GeneralSettings.Instance.GetTmpSceneDirectory() + "modal/" + smt_pis.SubModalTypeId);
                        }
                            
                    }
                }
                System.IO.Compression.ZipFile.CreateFromDirectory(GeneralSettings.Instance.GetTmpSceneDirectory(),
                    fileName);
                //TODO: guardar sqlite en tmp, y luego comprimir y mandar a fileName
            }
            
        }

        private void CreateSqlFile(string fileName)
        {
            var sqliteProvider = new SqliteProvider();
            sqliteProvider.CreateConnection("Filename=" + fileName);
            sqliteProvider.Save(SceneInUse.Instance.Scene);
            sqliteProvider.CloseConnection();
        }

        private void CopyContents(string sourcePath, string targetPath)
        {
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            if (!System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    var fileName = System.IO.Path.GetFileName(s);
                    var destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }
        }
    }
}
