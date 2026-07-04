using Newtonsoft.Json;
using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.BussMove.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.BussMove
{
    public partial class MoveSettingForm : SIE.XPCJ.Common.Forms.XPFormBaseDialog
    {
        private MoveSetting _moveSetting;
        public MoveSettingForm()
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
            _moveSetting = new MoveSetting();
            _moveSetting.Printer =this.comboBox1.SelectedItem == null ? "" : this.comboBox1.SelectedItem.ToString();
            _moveSetting.DevPort = !this.swichBtnPort.AChecked;
            _moveSetting.IsPrintOutCode = !this.switchBtnIsPrintOutCode.AChecked;
            bool isGetSerialPortok = true;
            foreach (var ctr in this.SerialPortPanel.Controls)
            {
                var serialPort = (ctr as XPSerialPort).GetSerialPort();
                if (serialPort == null)//只要有一个获取失败则停止保存
                {
                    isGetSerialPortok = false;
                    break;
                }
                if (_moveSetting.SerialPorts.Find(p => p.PortName == serialPort.PortName && p.BaudRate == serialPort.BaudRate) != null)
                {
                    MessageBox.Show("不能添加相同的串口 {0}:{1}".L10nFormat(serialPort.PortName, serialPort.BaudRate));
                    return;
                }
                _moveSetting.SerialPorts.Add(serialPort);
            }
            if (!isGetSerialPortok)
            {
                return;
            }

            Settings.Default.MoveSetting = JsonConvert.SerializeObject(_moveSetting);
            Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void MoveSettingForm_Load(object sender, EventArgs e)
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
            var setting = Settings.Default.MoveSetting;
            if (!string.IsNullOrEmpty(setting))
            {
                _moveSetting = JsonConvert.DeserializeObject<MoveSetting>(setting);
            }
            if (_moveSetting != null)
            {

                this.swichBtnPort.AChecked = !_moveSetting.DevPort;
                this.switchBtnIsPrintOutCode.AChecked = !_moveSetting.IsPrintOutCode;
                this.comboBox1.SelectedItem = _moveSetting.Printer;

                foreach (var serialPort in _moveSetting.SerialPorts)
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
