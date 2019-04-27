using SkypeIOT.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace SkypeIOT
{
    public partial class Form1 : Form
    {
        public string userEmail;
        public bool isThreadStarted = false;
        public string currentStatus = "";
        bool mqttEnableb = true;

        public Form1()
        {
            InitializeComponent();
            label5.Text = "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Click on the link below to continue learning how to build a desktop app using WinForms!
            System.Diagnostics.Process.Start("http://aka.ms/dotnet-get-started-desktop");

        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            userEmail = textBox1.Text;
            if(ValidateEmail()){
                label3.ForeColor = Color.Black;
                label3.Text = "Current User : " + userEmail;
                if (!isThreadStarted) { checkStatus(); }
            }
            else{
                label3.Text = "Invalid Email";
                label3.ForeColor = Color.Red;
            }

        }

        private void checkStatus()
        {
            
            new Thread(() =>
            {
                try
                {

                
                isThreadStarted = true;
                for (;;)
                {
                    Thread.CurrentThread.IsBackground = true;
                    LyncManager lyncManager = new LyncManager(userEmail);
                    Thread.Sleep(1000);
                    
                    if(lyncManager.timeElapsed!="N/A")
                    {
                        //label5.Text = lyncManager.timeElapsed;
                        SetText(lyncManager.timeElapsed);
                    }
                    else
                    {
                        //label5.Text = "";
                        SetText("");
                    }
                        if (lyncManager.userStatus.ToLower() == "offline" && lyncManager.isOutOfOfficeCheck == "True")
                        {
                            pictureBox2.Image = Resources.OutOfOffice;
                            currentStatus = "offline";
                            if (mqttEnableb)
                            { RequestGenerator.postRequest("offline"); }
                        }
                        else if (lyncManager.userStatus.ToLower() == "offline" && lyncManager.isOutOfOfficeCheck == "False")
                        {
                            pictureBox2.Image = Resources.Offline;
                            currentStatus = "offline";
                            if (mqttEnableb)
                            { RequestGenerator.postRequest("offline"); }
                        }
                        else if (lyncManager.userStatus.ToLower() == "free")
                        {
                            pictureBox2.Image = Resources.Available;
                            currentStatus = "free";
                            if (mqttEnableb) { RequestGenerator.postRequest("available"); }
                        }
                        else if (lyncManager.userStatus.ToLower() == "busy")
                        {
                            pictureBox2.Image = Resources.Busy;
                            currentStatus = "busy";
                            if (mqttEnableb) { RequestGenerator.postRequest("busy"); }
                        }
                        else if (lyncManager.userStatus.ToLower() == "donotdisturb")
                        {
                            pictureBox2.Image = Resources.dnd;
                            currentStatus = "donotdisturb";
                            if (mqttEnableb) { RequestGenerator.postRequest("busy"); }
                        }
                        else if (lyncManager.userStatus.ToLower() == "temporarilyaway")
                        {
                            pictureBox2.Image = Resources.Idle;
                            currentStatus = "temporarilyaway";
                            if (mqttEnableb)
                            { RequestGenerator.postRequest("idle"); }
                        }
                        else if (lyncManager.userStatus.ToLower() == "away")
                        {
                            pictureBox2.Image = Resources.Idle;
                            currentStatus = "away";
                            if (mqttEnableb)
                            { RequestGenerator.postRequest("idle"); }
                        }
                    }
                }
                catch(Exception e)
                {
                    StreamWriter w = File.AppendText("log.txt");
                    w.Write("\r\nLog Entry : ");
                    w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                    w.WriteLine("  :");
                    w.WriteLine($"  :"+e.Message);
                    w.WriteLine("-------------------------------");
                }


            }).Start(); 
            

        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label5.InvokeRequired){
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else{
                this.label5.Text = text;
            }
        }

        private bool ValidateEmail()
        {
            string email = textBox1.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            return match.Success;
        }

       

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

      
    }
}
