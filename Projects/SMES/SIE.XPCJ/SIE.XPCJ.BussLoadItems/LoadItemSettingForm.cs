using Newtonsoft.Json;
using SIE.XPCJ.BussLoadItems.Properties;
using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Models.ConfigsSetting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SIE.XPCJ.BussLoadItems
{
    public partial class LoadItemSettingForm : SIE.XPCJ.Common.Forms.XPFormBaseDialog
    {
        private LoadItemSetting _loadItemSetting;
        public LoadItemSettingForm()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            _loadItemSetting = new LoadItemSetting();

            _loadItemSetting.DevPort = !this.swichBtnPort.AChecked;
            _loadItemSetting.ReflashTime = this.watermarkTextBox10.Text;

            bool isGetSerialPortok = true;
            foreach (var ctr in this.SerialPortPanel.Controls)
            {
                var serialPort = (ctr as XPSerialPort).GetSerialPort();
                if (serialPort == null)//只要有一个获取失败则停止保存
                {
                    isGetSerialPortok = false;
                    break;
                }
                if (_loadItemSetting.SerialPorts.Find(p => p.PortName == serialPort.PortName && p.BaudRate == serialPort.BaudRate) != null)
                {
                    MessageBox.Show("不能添加相同的串口 {0}:{1}".L10nFormat(serialPort.PortName, serialPort.BaudRate));
                    return;
                }
                _loadItemSetting.SerialPorts.Add(serialPort);
            }
            if (!isGetSerialPortok)
            {
                return;
            }

            Settings.Default.LoadSettiing = JsonConvert.SerializeObject(_loadItemSetting);
            Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void MoveSettingForm_Load(object sender, EventArgs e)
        {

            GetMoveSetting();
        }

        /// <summary>
        /// 获取过站采集配置项
        /// </summary>
        /// <param name="key"></param>
        public void GetMoveSetting()
        {
            var setting = Settings.Default.LoadSettiing;
            if (!string.IsNullOrEmpty(setting))
            {
                _loadItemSetting = JsonConvert.DeserializeObject<LoadItemSetting>(setting);
            }
            if (_loadItemSetting != null)
            {
                this.watermarkTextBox10.Text = _loadItemSetting.ReflashTime;
                _loadItemSetting.DevPort = !this.swichBtnPort.AChecked;

                foreach (var serialPort in _loadItemSetting.SerialPorts)
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

        private void watermarkTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void InputValue(XPWatermarkTextBox watermarkTextBox, string title = "请输入波特率")
        {
            var com1 = string.IsNullOrEmpty(watermarkTextBox.Text) ? 0 : Convert.ToDecimal(watermarkTextBox.Text);
            if (XPFormNumberInput.ShowInput(title.L10N(), true, com1, out decimal newQty) == DialogResult.OK)
            {
                watermarkTextBox.Text = newQty > 0 ? newQty.ToString() : "";
            }
        }

        private void watermarkTextBox10_Click(object sender, EventArgs e)
        {
            InputValue(watermarkTextBox10, "上料自动刷新-刷新时间".L10N());
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
