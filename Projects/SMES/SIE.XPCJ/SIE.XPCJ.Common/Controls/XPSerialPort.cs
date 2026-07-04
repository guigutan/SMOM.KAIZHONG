using Microsoft.Win32;
using SIE.XPCJ.Models.ConfigsSetting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPSerialPort : BaseUserControl
    {
        /// <summary>
        /// 外部删除委托
        /// </summary>
        public Action DeleteAction { get; set; }
        public XPSerialPort()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        private void watermarkTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void xpButton1_Click(object sender, EventArgs e)
        {
            DeleteAction?.Invoke();
        }


        /// <summary>
        /// 获取串口
        /// </summary>
        /// <returns></returns>
        public SerialPort GetSerialPort()
        {
            if (this.comboBox2.SelectedItem ==null)
            {
                MessageBox.Show("请选择波特率");
                this.comboBox2.Focus();
                return null;
            }

            if (this.comboBox1.SelectedItem == null)
            {
                MessageBox.Show("请选择COM口");
                this.comboBox1.Focus();
                return null;
            }
            return new SerialPort()
            {
                PortName = this.comboBox1.SelectedItem.ToString(),
                BaudRate = int.Parse(this.comboBox2.SelectedItem.ToString())

            };
        }
        //private List<string> GetPortDeviceName()
        //{
        //    var comlist = new List<string>();
        //    RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
        //    if (keyCom != null)
        //    {
        //        string[] sSubKeys = keyCom.GetValueNames();
        //        foreach (string sName in sSubKeys)
        //        {
        //            string sValue = (string)keyCom.GetValue(sName);
        //            comlist.Add(sValue);
        //        }
        //    }
        //    return comlist;
        //}


        private void SerialPortCtr_Load(object sender, EventArgs e)
        {
            var portDeviceNames = System.IO.Ports.SerialPort.GetPortNames();
            this.comboBox1.DataSource = portDeviceNames;
        }

        /// <summary>
        /// 设置串口数据
        /// </summary>
        /// <param name="serialPort"></param>
        public void SetSerialPort(SerialPort serialPort)
        {
            var baudRateIndex = this.comboBox2.Items.IndexOf(serialPort.BaudRate.ToString());
            this.comboBox2.SelectedIndex = baudRateIndex;
            var index = this.comboBox1.Items.IndexOf(serialPort.PortName);
            this.comboBox1.SelectedIndex = index;
        }

    }
}
