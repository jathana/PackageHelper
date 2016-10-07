using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPackaging
{
   public class QPackageCreator
   {
      public string BeforeDBChangesFolderName { get; set; }
      public string BuildsFolderName { get; set; }
      /// <summary>
      /// List of builds folders e.g.E:\QCS Projects\6.4\Builds (local tfs checkouts)
      /// </summary>
      public List<QPackageFolder> BuildsFolders { get; set; }

      public QPackageFolder PackageFolder { get; set; }

      public QPackageCreator()
      {
         PackageFolder = new QPackageFolder();
         BuildsFolders = new List<QPackageFolder>();
         PackageFolder = new QPackageFolder();
         
         BeforeDBChangesFolderName = "0004d.Before DB Changes";
         BuildsFolderName = "0100. Builds";
         ReadSettings();

      }


# region create package folders structure

      public void CreatePackage()
      {
         PackageFolder.Folders.Clear();
         if (!Directory.Exists(PackageFolder.FullPathName))
         {
            QPackageFolder bdFolder = new QPackageFolder()
            {
               FullPathName = Path.Combine(PackageFolder.FullPathName, BeforeDBChangesFolderName)
            };

            QPackageFolder buildsFolder = new QPackageFolder()
            {
               FullPathName = Path.Combine(PackageFolder.FullPathName, BuildsFolderName)
            };
            PackageFolder.Folders.Add(bdFolder.FullPathName, bdFolder);
            PackageFolder.Folders.Add(buildsFolder.FullPathName, buildsFolder);

            QPackageHelper.CreateStructure(BuildsFolders, bdFolder, new List<QExtensionUpdate> { new QExtensionUpdate(".bd",".sql") }, new List<String> { ".bd"});
            QPackageHelper.CreateStructure(BuildsFolders, buildsFolder, new List<QExtensionUpdate> { }, new List<String> { ".sql", ".txt" });
            Utils.EnsureFolder(PackageFolder.FullPathName);
            Copy(bdFolder);
            Copy(buildsFolder);
         }
         else
         {
            throw new Exception(string.Format("Folder {0} exists", PackageFolder.FullPathName));
         }
      }


      private void Copy(QPackageFolder folder)
      {
         int totalFiles = 0;
         foreach (var item in folder.Folders)
         {
            totalFiles += item.Value.Files.Count;
         }

         int index = 0;

         Utils.EnsureFolder(folder.FullPathName);
         foreach(var subfolder in folder.Folders)
         {
            try
            {
               Utils.EnsureFolder(subfolder.Value.FullPathName);
               foreach (var file in subfolder.Value.Files)
               {
                  try
                  {
                     index++;
                     File.Copy(file.Value.OriginalPath, file.Value.DestPath);
                     OnCopying(new ProcessingEventArgs(string.Format("Copy {0} to {1}", file.Value.OriginalPath, file.Value.DestPath), index, totalFiles));
                  }
                  catch (Exception ex)
                  {
                     throw new Exception(string.Format("Cannot copy file {0}", file.Value.DestPath), ex);
                  }
               }
            }
            catch (Exception ex)
            {
               throw new Exception(string.Format("Cannot create folder {0}", subfolder.Value.FullPathName), ex);
            }
         }
      }

      private void ReadSettings()
      {
         BuildsFolders = new List<QPackageFolder>();
         List<string> CheckoutFolders = ConfigurationManager.AppSettings.AllKeys.ToList<string>();
         foreach (string key in CheckoutFolders)
         {
            QPackageFolder newItem = new QPackageFolder()
            {
               FullPathName = ConfigurationManager.AppSettings[key].ToString()
            };
            BuildsFolders.Add(newItem);
         }
      }
      #endregion


      #region events
      public delegate void FilesProcessingEventHandler(object Sender, ProcessingEventArgs e);
      public event FilesProcessingEventHandler Counting;
      public event FilesProcessingEventHandler Copying;

      protected virtual void OnCounting(ProcessingEventArgs e)
      {
         FilesProcessingEventHandler handler = Counting;
         if (handler != null)
         {
            handler(this, e);
         }
      }

      protected virtual void OnCopying(ProcessingEventArgs e)
      {
         FilesProcessingEventHandler handler = Copying;
         if (handler != null)
         {
            handler(this, e);
         }
      }

      #endregion
   }
}
