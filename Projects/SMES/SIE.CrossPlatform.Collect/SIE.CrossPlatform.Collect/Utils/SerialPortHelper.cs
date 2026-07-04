using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.CrossPlatform.Collect.Utils
{
    public static class SerialPortHelper
    {
        public static void GetAllSerialPorts(ComboBox cbSerialPort)
        {
            string[] portNames = SerialPort.GetPortNames();
            if (portNames.Length > 0)
            {
                foreach (var item in portNames)
                {
                    cbSerialPort.Items.Add(item);
                }
                cbSerialPort.SelectedItem = portNames[0];
            }
        }

    }
}
