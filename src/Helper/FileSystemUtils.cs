using System.IO;

namespace cl.uv.leikelen.src.Helper
{
    public static class FileSystemUtils
    {
        public static void EnsureDirectoriesHasBeenCreated()
        {
            if (!Directory.Exists(Properties.Paths.tmpDirectory)) Directory.CreateDirectory(Properties.Paths.tmpDirectory);
            if (!Directory.Exists(Properties.Paths.CurrentSceneDirectory)) Directory.CreateDirectory(Properties.Paths.CurrentSceneDirectory);
            if (!Directory.Exists(Properties.Paths.ImportedSceneDirectory)) Directory.CreateDirectory(Properties.Paths.ImportedSceneDirectory);
            if (!Directory.Exists(Properties.Paths.RecordedSceneDirectory)) Directory.CreateDirectory(Properties.Paths.RecordedSceneDirectory);
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
