using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Settings;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPBottomBar : UserControl
    {
        private bool IsNeedLoadVersionName = false;

        public XPBottomBar()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.labelDate.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            this.RefreshInfo();
            this.IsNeedLoadVersionName = string.IsNullOrEmpty(Global.VersionName);
        }

        public void RefreshInfo()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
                labelNetStatus.Text = "连接正常".L10N();
            else
                labelNetStatus.Text = "网络连接不可用".L10N();

            string hostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);

            foreach (IPAddress ipAddress in ipEntry.AddressList)
            {
                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    labelLocalIP.Text = "IP：" + ipAddress.ToString();
                    break;
                }
            }

            this.labelCompanyInfo.Text = $"V {Global.VersionName} Copyright © 2024 "+"广州赛意信息科技股份有限公司".L10N();

            this.labelDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.labelTime.Text = DateTime.Now.ToString("hh:mm:ss");
            this.timer1.Start();
        }

        public string GetExecutableFileVersion()
        {
            // 获取当前执行文件的路径
            string filePath = Process.GetCurrentProcess().MainModule.FileName;

            // 获取文件版本信息
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);

            // 返回版本号字符串
            return versionInfo.FileVersion;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.labelTime.Text = DateTime.Now.ToString("hh:mm:ss");
            if (this.labelTime.Text == "00:00:01")
            {
                this.labelDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (this.IsNeedLoadVersionName)
            {
                this.labelCompanyInfo.Text = $"V {Global.VersionName} Copyright © 2024 " + "广州赛意信息科技股份有限公司".L10N();
                this.IsNeedLoadVersionName = string.IsNullOrEmpty(Global.VersionName);
            }
        }

        //private string GetWeek()
        //{
        //    DayOfWeek today = DateTime.Today.DayOfWeek;

        //    switch (today)
        //    {
        //        case DayOfWeek.Sunday:
        //            return "星期日".L10N();
        //        case DayOfWeek.Monday:
        //            return "星期一";
        //        case DayOfWeek.Tuesday:
        //            return "星期二";
        //        case DayOfWeek.Wednesday:
        //            return "星期三";
        //        case DayOfWeek.Thursday:
        //            return "星期四";
        //        case DayOfWeek.Friday:
        //            return "星期五";
        //        case DayOfWeek.Saturday:
        //            return "星期六";
        //        default:
        //            return "星期几";
        //    }
        //}
    } 
}
