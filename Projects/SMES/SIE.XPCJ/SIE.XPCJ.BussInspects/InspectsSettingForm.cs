using Newtonsoft.Json;
using SIE.XPCJ.BussInspects.Properties;
using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Models.ConfigsSetting;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SIE.XPCJ.BussInspects
{
    public partial class InspectsSettingForm : SIE.XPCJ.Common.Forms.XPFormBaseDialog
    {
        private ConfigSettingBase _inspectsSetting;
        public InspectsSettingForm()
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
            _inspectsSetting = new ConfigSettingBase();
            _inspectsSetting.Printer = this.comboBox1.SelectedItem==null?"": this.comboBox1.SelectedItem.ToString();
            _inspectsSetting.DevPort = !this.swichBtnPort.AChecked;
            bool isGetSerialPortok = true;
            foreach (var ctr in this.SerialPortPanel.Controls)
            {
                var serialPort = (ctr as XPSerialPort).GetSerialPort();
                if (serialPort == null)//只要有一个获取失败则停止保存
                {
                    isGetSerialPortok = false;
                    break;
                }
                if (_inspectsSetting.SerialPorts.Find(p => p.PortName == serialPort.PortName && p.BaudRate == serialPort.BaudRate) != null)
                {
                    MessageBox.Show("不能添加相同的串口 {0}:{1}".L10nFormat(serialPort.PortName, serialPort.BaudRate));
                    return;
                }
                _inspectsSetting.SerialPorts.Add(serialPort);
            }
            if (!isGetSerialPortok)
            {
                return;
            }

            Settings.Default.InspectsSetting = JsonConvert.SerializeObject(_inspectsSetting);
            Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void InspectsSettingForm_Load(object sender, EventArgs e)
        {
            PrinterSettings.StringCollection printNames = PrinterSettings.InstalledPrinters;
            List<string> items = new List<string>();
            foreach (var name in printNames)
            {
                items.Add(name.ToString());
            }
            // 将打印机名称转换为字符串数组
            comboBox1.DataSource = items;

            GetMoveSetting();
        }

        /// <summary>
        /// 获取过站采集配置项
        /// </summary>
        /// <param name="key"></param>
        public void GetMoveSetting()
        {
            var setting = Settings.Default.InspectsSetting;
            if (!string.IsNullOrEmpty(setting))
            {
                _inspectsSetting = JsonConvert.DeserializeObject<ConfigSettingBase>(setting);
            }
            if (_inspectsSetting != null)
            {

                this.swichBtnPort.AChecked = !_inspectsSetting.DevPort;
                this.comboBox1.SelectedItem = _inspectsSetting.Printer;

                foreach (var serialPort in _inspectsSetting.SerialPorts)
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
