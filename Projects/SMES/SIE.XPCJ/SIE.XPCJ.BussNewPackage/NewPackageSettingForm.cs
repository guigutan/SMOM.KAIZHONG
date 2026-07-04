using Newtonsoft.Json;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.BussNewPackage.Properties;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SIE.XPCJ.BussNewPackage
{
    public partial class NewPackageSettingForm : SIE.XPCJ.Common.Forms.XPFormBaseDialog
    {
        private NewPackageSetting _Setting;
        public NewPackageSettingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            _Setting = new NewPackageSetting();
            _Setting.Printer = this.comboBox1.SelectedItem==null?"": this.comboBox1.SelectedItem.ToString();
            _Setting.DevPort = !this.swichBtnPort.AChecked;
            _Setting.PrintMode = this.xpSwitchPrintMode.AChecked ? SIE.XPCJ.Models.WIP.Packing.PrintMode.InAdvance : Models.WIP.Packing.PrintMode.Online;
            bool isGetSerialPortok = true;
            foreach (var ctr in this.SerialPortPanel.Controls)
            {
                var serialPort = (ctr as XPSerialPort).GetSerialPort();
                if (serialPort == null)//只要有一个获取失败则停止保存
                {
                    isGetSerialPortok = false;
                    break;
                }
                if (_Setting.SerialPorts.Find(p => p.PortName == serialPort.PortName && p.BaudRate == serialPort.BaudRate) != null)
                {
                    MessageBox.Show("不能添加相同的串口 {0}:{1}".L10nFormat(serialPort.PortName, serialPort.BaudRate));
                    return;
                }
                _Setting.SerialPorts.Add(serialPort);
            }
            if (!isGetSerialPortok)
            {
                return;
            }

            Settings.Default.NewPackageSetting = JsonConvert.SerializeObject(_Setting);
            Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void NewPackageSettingForm_Load(object sender, EventArgs e)
        {
            PrinterSettings.StringCollection printNames = PrinterSettings.InstalledPrinters;
            List<string> items = new List<string>();
            foreach (var name in printNames)
            {
                items.Add(name.ToString());
            }
            // 将打印机名称转换为字符串数组
            comboBox1.DataSource = items;

            GetSetting();
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="key"></param>
        public void GetSetting()
        {
            var setting = Settings.Default.NewPackageSetting;
            if (!string.IsNullOrEmpty(setting))
            {
                _Setting = JsonConvert.DeserializeObject<NewPackageSetting>(setting);
            }
            if (_Setting != null)
            {

                this.swichBtnPort.AChecked = !_Setting.DevPort;
                this.comboBox1.SelectedItem = _Setting.Printer;
                this.xpSwitchPrintMode.AChecked = _Setting.PrintMode == Models.WIP.Packing.PrintMode.InAdvance;

                foreach (var serialPort in _Setting.SerialPorts)
                {
                    XPSerialPort serialPortCtr = new XPSerialPort();
                    serialPortCtr.DeleteAction = () =>
                    {
                        SerialPortPanel.Controls.Remove(serialPortCtr);
                    };
                    serialPortCtr.Dock = DockStyle.Top;
                    SerialPortPanel.Controls.Add(serialPortCtr);
                    serialPortCtr.SetSerialPort(serialPort);
                }
            }

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpButton1_Click(object sender, EventArgs e)
        {
            if (SerialPortPanel.Controls.Count >= System.IO.Ports.SerialPort.GetPortNames().Length)
            {
                MessageBox.Show("已达到设备最大COM数".L10N());
                return;
            }
            XPSerialPort serialPortCtr = new XPSerialPort();
            serialPortCtr.DeleteAction = () =>
            {
                SerialPortPanel.Controls.Remove(serialPortCtr);
            };
            serialPortCtr.Dock = DockStyle.Top;
            SerialPortPanel.Controls.Add(serialPortCtr);
        }
    }
}
