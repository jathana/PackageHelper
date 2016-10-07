using DevExpress.XtraEditors;
using QPackaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageHelper
{
   public partial class Form1 : Form
   {
      QPackageCreator pcreator = new QPackageCreator();

      public Form1()
      {
         InitializeComponent();
      }


      private void InitUI()
      {
         listBoxControl1.Items.Clear();
         foreach (var item in pcreator.BuildsFolders)
         {
            listBoxControl1.Items.Add(item);
         }
      }

      private void txtDestFolder_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
          if(folderBrowserDialog1.ShowDialog()==DialogResult.OK)
         {
            txtDestFolder.Text = folderBrowserDialog1.SelectedPath;
         }
      }

      private void Form1_Load(object sender, EventArgs e)
      {
         InitUI();
      }

      private void btnRun_Click(object sender, EventArgs e)
      {

         try
         {
            btnRun.Enabled = false;
            
            pcreator.PackageFolder.FullPathName = txtDestFolder.Text;
            pcreator.Copying += Pcreator_Copying;
            pcreator.CreatePackage();
         }
         catch(Exception ex)
         {
            XtraMessageBox.Show(ex.Message);
         }
         finally
         {
            btnRun.Enabled = true;
         }

      }

      private void Pcreator_Copying(object Sender, QPackaging.ProcessingEventArgs e)
      {
         lblCopy.Text = e.CurrentItem;
         prgCopying.Properties.Maximum = e.TotalItems;
         prgCopying.PerformStep();
         Application.DoEvents();
         //System.Diagnostics.Debug.WriteLine(e.CurrentItem);
      }
   }
}
