using NCrawler.Demo;
using NCrawler.Interfaces;
using NCrawler.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using iCrawler.Demo;
using Microsoft.Win32;
using System.Threading;

namespace iCrawler
{
    public partial class frmMain : Form
    {
        bool autoStart = false;
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
       
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            //// Remove limits from Service Point Manager
            //ServicePointManager.MaxServicePoints = 999999;
            //ServicePointManager.DefaultConnectionLimit = 999999;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            //ServicePointManager.CheckCertificateRevocationList = true;
            //ServicePointManager.EnableDnsRoundRobin = true;

            //// Run demo 8
            //AdvancedCrawlDemo.Run();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Remove limits from Service Point Manager
            ServicePointManager.MaxServicePoints = 999999;
            ServicePointManager.DefaultConnectionLimit = 999999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.EnableDnsRoundRobin = true;

            List<string> urls = new List<string>();
            iCrawlerEntities _db = new iCrawlerEntities();
            foreach (var item in _db.Urls)
            {
                if (item != null && new TimerHelper().IsInTimes(item.Times, true))
                {
                    this.lblICrawler.Text = DateTime.Now.ToString();
                    this.lblUrlRunning.Text = item.Url1;
                    AdvancedCrawlDemo.Run(item.Url1);                    
                }
            }

            
            //// Run demo 8
            //AdvancedCrawlDemo.Run();
        }

        private void btnAutoStart_Click(object sender, EventArgs e)
        {
            autoStart = !autoStart;
            if (autoStart)
            {
                //Create a registry key
                RegistryKey regKey;
                //Open the run registry key
                regKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                //Give a name and value to the key recently created :regKey
                regKey.SetValue("myKey", Application.ExecutablePath);
                //Close the key
                regKey.Close();
                //Show a message
                this.btnAutoStart.Text = "Disable AutoStart";
                //MessageBox.Show("Auto-start activated successfully !");
            }
            else
            {
                //Create a registry key
                RegistryKey regKey;
                //Open the run registry key
                regKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                //Delete The key wich named: myKey
                regKey.DeleteValue("myKey", true);
                //Close the key
                regKey.Close();
                //Show a message
                this.btnAutoStart.Text = "AutoStart";
                //MessageBox.Show("Auto-start activated successfully !");
            }
        }
    }
}
