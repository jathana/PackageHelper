using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPackaging
{
   public static class Utils
   {
      public static void EnsureFolder(string path)
      {
         try
         {
            if (!Directory.Exists(path))
            {
               Directory.CreateDirectory(path);
            }
         }
         catch(Exception ex)
         {
            throw new Exception(string.Format("Cannot create directory {0}", path), ex);
         }
      }


      public static string EnsureExtension(string file, List<QExtensionUpdate> extensions)
      {
         string retval = file;
         // find extension
         QExtensionUpdate ext=extensions.FirstOrDefault(e => e.SourceExtension.ToLower() == Path.GetExtension(file).ToLower());
         if (ext != null )
         {
            // check if file already has the right extension
            if (!Path.GetExtension(retval).ToLower().Equals(ext.DestinationExtension.ToLower()))
            {
               retval = Path.GetFileNameWithoutExtension(file);
               // check if file contains the correct extension afer moving the inital extension (.sql.bd) 
               if (!Path.GetExtension(retval).ToLower().Equals(ext.DestinationExtension.ToLower()))
               {
                  retval = string.Format("{0}{1}", retval, ext.DestinationExtension.ToLower());
               }
            }
         }
         return retval;
      }
   }
}
