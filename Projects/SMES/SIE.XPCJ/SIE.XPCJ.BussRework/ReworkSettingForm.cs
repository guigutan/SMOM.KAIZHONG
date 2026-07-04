using Newtonsoft.Json;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.BussRework.Properties;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;
using SIE.XPCJ.Models.Enums;

namespace SIE.XPCJ.BussRework
{
    public partial class ReworkSettingForm : SIE.XPCJ.Common.Forms.XPFormBaseDialog
    {
        private ReworkSetting _Setting;
        public ReworkSettingForm()
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
            _Setting = new ReworkSetting();
            _Setting.DevPort = !this.swichBtnPort.AChecked;
            _Setting.ReplaceItemHandleMethod = this.xpReplaceItemHandleMethod.AChecked ? ReplaceItemHandleMethod.Recycle : ReplaceItemHandleMethod.Scrap;
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

            Settings.Default.ReworkSetting = JsonConvert.SerializeObject(_Setting);
            Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void NewPackageSettingForm_Load(object sender, EventArgs e)
        {
            GetSetting();
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="key"></param>
        public void GetSetting()
        {
            var setting = Settings.Default.ReworkSetting;
            if (!string.IsNullOrEmpty(setting))
            {
                _Setting = JsonConvert.DeserializeObject<ReworkSetting>(setting);
            }
            if (_Setting != null)
            {

                this.swichBtnPort.AChecked = !_Setting.DevPort;
                this.xpReplaceItemHandleMethod.AChecked = _Setting.ReplaceItemHandleMethod == ReplaceItemHandleMethod.Recycle;

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
