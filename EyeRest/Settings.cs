using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace EyeRest
{
    public partial class Settings : Form
    {

        bool isActive = false;
        Form MainForm = new Form();
        Form another = new Form();

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsmodifiers, int vlc);

        public Settings()
        {
            InitializeComponent();
            //hide form to taskbar
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            //set hotkeys
            RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 0x0004 | 0x0002, 0x48);//Set hotkey as 'ctrl + shift + h'
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                if(isActive)
                {
                    //disable
                    MainForm.Hide();
                    another.Hide();
                    Cursor.Show();
                    isActive = false;
                }
                else
                {
                    //get all monitors
                    var allScreens = Screen.AllScreens;
                    for (int i = 0; i < allScreens.Count(); i++ )
                    {
                        another.FormBorderStyle = FormBorderStyle.None;
                        another.BackColor = Color.Black;
                        another.Width = allScreens[i].Bounds.Width;
                        another.Height = allScreens[i].Bounds.Height;
                        another.Show();
                        this.Location = allScreens[i].WorkingArea.Location;
                        Cursor.Hide();
                        isActive = true;
                    }
                        //foreach monitor show a new form
                        //ShowEyeRest();
                }
            }
            base.WndProc(ref m);
        }

        private void Settings_Load(object sender, EventArgs e)
        {   
            //GetNotificationTitle really should be pulling from AppConfig
            notifyIcon1.BalloonTipTitle = txtBoxNotificationTitle.Text;

            //GetHotkey Combination, really should be pulling from AppConfig
            notifyIcon1.BalloonTipText = "Your Global Hot Key is: Ctrl + Shift + H";
            notifyIcon1.ShowBalloonTip(5000);
        }

        private void ShowEyeRest()
        {
            MainForm.FormBorderStyle = FormBorderStyle.None;
            MainForm.BackColor = Color.Black;
            MainForm.Width = Screen.PrimaryScreen.Bounds.Width;
            MainForm.Height = Screen.PrimaryScreen.Bounds.Height;
            MainForm.Show();
            Cursor.Hide();
            isActive = true;
        }

        private void LoadComboboxes()
        {
            comboBox1.Items.Add("Alt");
            comboBox1.Items.Add("Ctrl");
            comboBox1.Items.Add("Shift");

            comboBox2.Items.Add("Alt");
            comboBox2.Items.Add("Ctrl");
            comboBox2.Items.Add("Shift");
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
    }
}