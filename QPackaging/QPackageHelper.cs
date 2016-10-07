using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPackaging
{
   public static class QPackageHelper
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="targetVersion">Major, minor</param>
      /// <param name="sourceFolder"></param>
      /// <param name="destFolder"></param>
      public static void AddSubFolders(string targetVersion, string sourceFolder, QPackageFolder destFolder, List<QExtensionUpdate> extensions, List<string> includedeExtensions)
      {
         try
         {
            if (Directory.Exists(sourceFolder))
            {
                            

               // add each sub folder
               string[] dirs = Directory.GetDirectories(sourceFolder);
               foreach (string dir in dirs)
               {

                  // check version
                  string version = Path.GetFileName(dir);
                  if (version.StartsWith(targetVersion))
                  {
                     QPackageFolder newFolder = new QPackageFolder()
                     {
                        FullPathName = Path.Combine(destFolder.FullPathName, Path.GetFileName(dir))
                     };
                     destFolder.Folders.Add(Path.GetFileName(dir), newFolder);

                     // add files
                     string[] files = Directory.GetFiles(dir);
                     foreach (string file in files)
                     {
                        if (includedeExtensions.Contains(Path.GetExtension(file)) || includedeExtensions.Contains("*.*"))
                        {
                           QPackageFile newFile = new QPackageFile()
                           {
                              OriginalPath = file,
                              DestPath = Path.Combine(newFolder.FullPathName, Utils.EnsureExtension(Path.Combine(Path.Combine(newFolder.FullPathName, Path.GetFileName(file))), extensions))
                           };
                           newFolder.Files.Add(Path.GetFileName(newFile.DestPath), newFile);
                        }
                     }

                     //AddSubFolders(dir, newFolder);
                  }
               }
            }
         }
         catch(Exception ex)
         {
            throw new Exception(string.Format("Error while adding subfolders of folder {0}", sourceFolder),ex);
         }

      }

      public static void DoFoldersNumberingByVersion(QPackageFolder objFolder)
      {
         SortedList<Version, string> index = new SortedList<Version, string>();         
         if(objFolder!=null)
         {
            // load versions
            foreach(var subdir in objFolder.Folders)
            {
               index.Add(new Version(subdir.Key), subdir.Key);
            }
            // change full path names
            foreach (var pair in index)
            {
               // get sub folder
               QPackageFolder subdir = objFolder.Folders[pair.Value];
               //new subdir name
               string lpadZeros = string.Format("D{0}", index.Count.ToString().Length);
               string destName = string.Format("{0}. {1}", (index.IndexOfKey(pair.Key) + 1).ToString(lpadZeros), Path.GetFileName(subdir.FullPathName));
               subdir.FullPathName = Path.Combine(Path.GetDirectoryName(subdir.FullPathName), destName);
               foreach(var file in subdir.Files)
               {
                  // fix dest file path
                  file.Value.DestPath = Path.Combine(subdir.FullPathName, Path.GetFileName(file.Value.DestPath));
               }
               //DoFoldersNumberingByVersion(subdir);
            }

         }
      }

      public static void CreateStructure(List<QPackageFolder> BuildsFolders, QPackageFolder destFolder, List<QExtensionUpdate> extUpdates, List<string> includedExtensions)
      {
         destFolder.Files.Clear();
         destFolder.Folders.Clear();
         foreach (var folder in BuildsFolders)
         {
            string folderver = Path.GetFullPath(folder.FullPathName);
            folderver = Path.GetDirectoryName(folderver);
            folderver = Path.GetFileName(folderver);
            Version targetVersion = new Version(folderver);
            QPackageHelper.AddSubFolders(folderver, folder.FullPathName, destFolder, extUpdates, includedExtensions);
         }

         QPackageHelper.DoFoldersNumberingByVersion(destFolder);
      }
   }
}
