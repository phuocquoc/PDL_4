using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        string pathfile;
        string pathsave = "C:/Users/ADMIN/Desktop/";
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
               
            if(Server2.path.Length >0)
            {
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Not Receive File");
            }
            
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            Server2 server = new Server2();
            Thread.Sleep(1000);
            List<string> fname = new List<string>();
            int dem = 0;
            while (true)
            {
                int temp = 0;
                Server2.path = "C:/Users/ADMIN/Desktop/";
                Server2.path += Server2.name; //mặc định nhận file tại desktop  
                pathfile = Server2.path + "/";
                var ext = System.IO.Path.GetExtension(pathfile);
                if (ext == String.Empty)
                {
                    Directory.CreateDirectory("" + pathfile);
                }
                if (Server2.name != null && dem == 0)
                {
                    dem = 1;
                    msg();
                    fname.Add(Server2.name);
                }
                if(fname.Count!=0)
                {
                    for (int i = 0; i < fname.Count; i++)
                    {
                        if (fname[i] == Server2.name)
                        {
                            temp = 1;
                            break;
                        }
                    }
                    if (temp != 1)
                    {
                        msg();
                        fname.Add(Server2.name);
                    }
                }
            }
        }

        private void buttonfile_Click(object sender, EventArgs e)
        {
            
            Button btn = (Button)sender;
            string[] arrListStr = btn.Text.Split('_');
            StreamReader read = new StreamReader(pathsave+"/"+ arrListStr[0] + "/"+ btn.Text);
            textBox1.Text = read.ReadToEnd();
            read.Close();
        }
        
        private void buttoncomputer_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            foreach (Control control in this.flowLayoutPanel1.Controls)
            {
                this.flowLayoutPanel1.Controls.Clear();
                control.Dispose();
            }
            DirectoryInfo d = new DirectoryInfo("" + pathsave+"/"+ btn.Text.Trim());//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files                                            
            foreach (FileInfo file in Files)
            {
                Button button = new Button();
                button.Size = new Size(140, 140);
                button.Image = Server.Properties.Resources.document_90px;
                button.Text = file.Name;
                button.ImageAlign=ContentAlignment.TopCenter;
                button.TextAlign = ContentAlignment.BottomCenter;
                this.flowLayoutPanel1.Controls.Add(button);
                button.Click += buttonfile_Click;
            }
        }
        private void Createbutton()
        {
            Button button1 = new Button();
            button1.Location = new System.Drawing.Point(12, 12);
            button1.Size = new System.Drawing.Size(295, 92);
            button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button1.Image = Server.Properties.Resources.computer_100px;
            button1.Text = "       " + Server2.name;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.TextAlign = ContentAlignment.MiddleCenter;
            button1.Click += buttoncomputer_Click;
            flowLayoutPanel2.Controls.Add(button1);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void msg()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
                Createbutton();
        }

    }
}
