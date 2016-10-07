using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPackaging
{
   public class QExtensionUpdate
   {
      public string SourceExtension { get; set; }
      public string DestinationExtension { get; set; }

      public QExtensionUpdate(string sourceExtension, string destinationExtension)
      {
         SourceExtension = sourceExtension;
         DestinationExtension = destinationExtension;

      }
   }
}
