using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTaskManager
{
    public partial class MyTaskManager : Form
    {
        public MyTaskManager()
        {
            InitializeComponent();
            Init();
            timer1.Start();

        }
        private void Init()
        {
            foreach (var p in Process.GetProcesses())
            {
                listView1.Items.Add(GetListViewItem(p));
            }
        }
        private ListViewItem GetListViewItem(Process process)
        {

            ListViewItem lv = new ListViewItem();
            lv.Tag = process;
            lv.Text = process.Id.ToString();

            lv.SubItems.Add(new ListViewItem.ListViewSubItem(lv, process.ProcessName));
            lv.SubItems.Add(new ListViewItem.ListViewSubItem(lv, process.BasePriority.ToString()));
            lv.SubItems.Add(new ListViewItem.ListViewSubItem(lv, process.WorkingSet64.ToString()));
            lv.SubItems.Add(new ListViewItem.ListViewSubItem(lv, process.Threads.Count.ToString()));



            return lv;
        }
        private void RefreshListView()
        {
            //Add new 
            var writtenProceses = listView1.Items.Cast<ListViewItem>().Select(i => (Process)i.Tag).ToList();
            var currentProceses = Process.GetProcesses().ToList();
            foreach (var p in currentProceses)
            {
                //Add new
                if (!writtenProceses.Exists(pr => pr.Id == p.Id))
                {
                    listView1.Items.Add(GetListViewItem(p));
                }


            }



            foreach (var item in listView1.Items.Cast<ListViewItem>())
            {
                if (!currentProceses.Exists(p => p.Id == ((Process)item.Tag).Id))
                {
                    listView1.Items.Remove(item);
                }
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ToTakeDownProcess = listView1.SelectedItems.Cast<ListViewItem>().Select(x => (Process)x.Tag);
            foreach (var proc in ToTakeDownProcess)
            {
                try
                {
                    proc.Kill();
                }
                catch (Exception)
                {
                    MessageBox.Show("You cannot kill this process");
                }
            }
            RefreshListView();
        }
    }
}
