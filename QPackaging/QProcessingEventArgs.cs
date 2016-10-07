using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPackaging
{
   public class ProcessingEventArgs:EventArgs
   {
      public string CurrentItem { get; set; }
      public int CurrentIndex { get; set; }
      public int TotalItems { get; set; }

      public ProcessingEventArgs(string currentItem, int currentIndex, int totalItems)
      {
         CurrentItem = currentItem;
         CurrentIndex = currentIndex;
         TotalItems = totalItems;
      }
   }
}
