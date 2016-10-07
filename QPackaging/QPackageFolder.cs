using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPackaging
{
    public class QPackageFolder
    {

      public string FullPathName { get; set; }
      public SortedList<string, QPackageFolder> Folders { get; set; }
      public SortedList<string, QPackageFile> Files { get; set; }

      public QPackageFolder()
      {
         Folders = new SortedList<string, QPackageFolder>();
         Files = new SortedList<string, QPackageFile>();
         FullPathName = "";
      }


      public override string ToString()
      {
         return FullPathName;
      }
   }
}
