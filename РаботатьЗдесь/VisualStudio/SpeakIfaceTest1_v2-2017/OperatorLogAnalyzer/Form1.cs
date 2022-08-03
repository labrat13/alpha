using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OperatorLogAnalyzer
{
    public partial class Form1 : Form
    {
        private IContainer components = (IContainer)null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openLogFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ListView listView1;
        private ColumnHeader columnHeader_Count;
        private ColumnHeader columnHeader_Query;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem sortByQueryToolStripMenuItem;
        private ToolStripMenuItem sortByCountToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;

        public Form1()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.openLogFileToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.editToolStripMenuItem = new ToolStripMenuItem();
            this.sortByQueryToolStripMenuItem = new ToolStripMenuItem();
            this.sortByCountToolStripMenuItem = new ToolStripMenuItem();
            this.statusStrip1 = new StatusStrip();
            this.listView1 = new ListView();
            this.columnHeader_Count = new ColumnHeader();
            this.columnHeader_Query = new ColumnHeader();
            this.helpToolStripMenuItem = new ToolStripMenuItem();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            this.menuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.fileToolStripMenuItem,
        (ToolStripItem) this.editToolStripMenuItem,
        (ToolStripItem) this.helpToolStripMenuItem
      });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(292, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.openLogFileToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.exitToolStripMenuItem
      });
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.openLogFileToolStripMenuItem.Name = "openLogFileToolStripMenuItem";
            this.openLogFileToolStripMenuItem.Size = new Size(157, 22);
            this.openLogFileToolStripMenuItem.Text = "Open log file...";
            this.openLogFileToolStripMenuItem.Click += new EventHandler(this.openLogFileToolStripMenuItem_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(154, 6);
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new Size(157, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.sortByQueryToolStripMenuItem,
        (ToolStripItem) this.sortByCountToolStripMenuItem
      });
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new Size(37, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.sortByQueryToolStripMenuItem.Name = "sortByQueryToolStripMenuItem";
            this.sortByQueryToolStripMenuItem.Size = new Size(153, 22);
            this.sortByQueryToolStripMenuItem.Text = "Sort by Query";
            this.sortByQueryToolStripMenuItem.Click += new EventHandler(this.sortByQueryToolStripMenuItem_Click);
            this.sortByCountToolStripMenuItem.Name = "sortByCountToolStripMenuItem";
            this.sortByCountToolStripMenuItem.Size = new Size(153, 22);
            this.sortByCountToolStripMenuItem.Text = "Sort by Count";
            this.sortByCountToolStripMenuItem.Click += new EventHandler(this.sortByCountToolStripMenuItem_Click);
            this.statusStrip1.Location = new Point(0, 244);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(292, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            this.listView1.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeader_Count,
        this.columnHeader_Query
      });
            this.listView1.Dock = DockStyle.Fill;
            this.listView1.Location = new Point(0, 24);
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(292, 220);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Details;
            this.columnHeader_Count.Text = "Count";
            this.columnHeader_Query.Text = "Query text";
            this.columnHeader_Query.Width = 200;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.aboutToolStripMenuItem
      });
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new EventHandler(this.aboutToolStripMenuItem_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(292, 266);
            this.Controls.Add((Control)this.listView1);
            this.Controls.Add((Control)this.statusStrip1);
            this.Controls.Add((Control)this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Operator log analyzer";
            this.Load += new EventHandler(this.Form1_Load);
            this.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void openLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.ExecutablePath;
            openFileDialog.Title = "Open Operator log file...";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            Dictionary<string, int> dictionary = LogFile.CommandCount(openFileDialog.FileName);
            string text = "Queryes not found!";
            if (dictionary == null)
                text = "Error reading file: " + openFileDialog.SafeFileName;
            this.listView1.Items.Clear();
            if (dictionary == null || dictionary.Count < 1)
            {
                this.listView1.Items.Add(this.makeListView(string.Empty, text));
            }
            else
            {
                List<ListViewItem> list = new List<ListViewItem>(dictionary.Count);
                foreach (KeyValuePair<string, int> keyValuePair in dictionary)
                    list.Add(this.makeListView(keyValuePair.Value.ToString(), keyValuePair.Key));
                list.Sort(new Comparison<ListViewItem>(Form1.SortByQuery));
                this.listView1.Items.AddRange(list.ToArray());
            }
        }

        private ListViewItem makeListView(string count, string text)
        {
            return new ListViewItem(count)
            {
                SubItems = { text }
            };
        }

        internal static int SortByQuery(ListViewItem x, ListViewItem y)
        {
            if (x == null)
                return y == null ? 0 : -1;
            if (y == null)
                return 1;
            return x.SubItems[1].Text.CompareTo(y.SubItems[1].Text);
        }

        internal static int SortByCount(ListViewItem x, ListViewItem y)
        {
            if (x == null)
                return y == null ? 0 : -1;
            if (y == null)
                return 1;
            return int.Parse(x.SubItems[0].Text).CompareTo(int.Parse(y.SubItems[0].Text));
        }

        private void sortByQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (ListViewItem listViewItem in this.listView1.Items)
                list.Add(listViewItem);
            this.listView1.BeginUpdate();
            this.listView1.Items.Clear();
            list.Sort(new Comparison<ListViewItem>(Form1.SortByQuery));
            this.listView1.Items.AddRange(list.ToArray());
            this.listView1.EndUpdate();
        }

        private void sortByCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (ListViewItem listViewItem in this.listView1.Items)
                list.Add(listViewItem);
            this.listView1.BeginUpdate();
            this.listView1.Items.Clear();
            list.Sort(new Comparison<ListViewItem>(Form1.SortByCount));
            this.listView1.Items.AddRange(list.ToArray());
            this.listView1.EndUpdate();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int num = (int)new AboutBox1().ShowDialog();
        }
    }
}