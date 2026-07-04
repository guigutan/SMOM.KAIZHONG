using Newtonsoft.Json;
using SIE.XPCJ.BussRepairs.Properties;
using SIE.XPCJ.Common.Controls;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Models.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SIE.XPCJ.BussRepairs
{
    /// <summary>
    /// 维修配置项
    /// </summary>
    public partial class RepairsSettingForm : XPFormBaseDialog
    {
        private RepairsSetting _repairsSetting;

        /// <summary>
        /// 置换后下料选项
        /// </summary>
        private List<KeyValuePair<int, string>> DicChangeItemHandleMethod = new List<KeyValuePair<int, string>>()
        {
           new KeyValuePair<int,string>(10,"置换后作废".L10N() ),
            new KeyValuePair<int,string>(20,"置换后正常下料".L10N() ),
             new KeyValuePair<int,string>(30,"置换后不良下料".L10N() ),
        };
        public RepairsSettingForm()
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
            _repairsSetting = new RepairsSetting();

            _repairsSetting.DevPort = !this.swichBtnPort.AChecked;
            _repairsSetting.ChangeItemHandleMethod = (ChangeItemHandleMethod)(int)xpComboBox1.SelectedValue;
            bool isGetSerialPortok = true;
            foreach (var ctr in this.SerialPortPanel.Controls)
            {
                var serialPort = (ctr as XPSerialPort).GetSerialPort();
                if (serialPort == null)//只要有一个获取失败则停止保存
                {
                    isGetSerialPortok = false;
                    break;
                }
                if (_repairsSetting.SerialPorts.Find(p => p.PortName == serialPort.PortName && p.BaudRate == serialPort.BaudRate) != null)
                {
                    MessageBox.Show("不能添加相同的串口 {0}:{1}".L10nFormat(serialPort.PortName, serialPort.BaudRate));
                    return;
                }
                _repairsSetting.SerialPorts.Add(serialPort);
            }
            if (!isGetSerialPortok)
            {
                return;
            }

            Settings.Default.RepairsSettiing = JsonConvert.SerializeObject(_repairsSetting);
            Settings.Default.Save();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void RepairsSettingForm_Load(object sender, EventArgs e)
        {
            this.xpComboBox1.DisplayMember = "Value";
            this.xpComboBox1.ValueMember = "Key";
            this.xpComboBox1.DataSource = DicChangeItemHandleMethod;
            GetRepairsSetting();
        }

        /// <summary>
        /// 获取过站采集配置项
        /// </summary>
        /// <param name="key"></param>
        public void GetRepairsSetting()
        {
            var setting = Settings.Default.RepairsSettiing;
            if (!string.IsNullOrEmpty(setting))
            {
                _repairsSetting = JsonConvert.DeserializeObject<RepairsSetting>(setting);
            }
            if (_repairsSetting != null)
            {
                var changeItemHandleMethodValue = (int)_repairsSetting.ChangeItemHandleMethod;

                if (changeItemHandleMethodValue > 0)
                {
                    this.xpComboBox1.SelectedIndex = this.DicChangeItemHandleMethod.FindIndex(m => m.Key == changeItemHandleMethodValue);
                }
                _repairsSetting.DevPort = !this.swichBtnPort.AChecked;
                foreach (var serialPort in _repairsSetting.SerialPorts)
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
