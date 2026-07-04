using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Print;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Forms
{
    public partial class FormBase : XPFormBase
    {
        /// <summary>
        /// 保留主页面引用
        /// </summary>
        protected Form FormMain;

        /// <summary>
        /// 明细页面列表 避免关闭当前页 弹出页面未能关闭
        /// </summary>
        public List<Form> detailsNoBoderForm { get; set; }


        /// <summary>
        /// 过站记录状态
        /// </summary>
        public WipProductProcessState WipProductProcessState { get; set; }

        /// <summary>
        /// 采集步骤
        /// </summary>
        protected virtual CollectStep Step { get; set; }


        /// <summary>
        /// 拼板信息
        /// </summary>
        protected SubmitPanelInfo PanelInfo { get; set; }

        /// <summary>
        /// 拼板码绑定SN是否自动生成
        /// </summary>

        protected bool IsConfigAuto { get; set; }

        /// <summary>
        /// 存在失败工序
        /// </summary>
        public bool HaveFailParameter { get; set; }

        /// <summary>
        /// 合格
        /// </summary>
        public bool Qualified { get; set; }


        /// <summary>
        /// 当前采集工序
        /// </summary>
        protected Process CurrentProcess { get; set; }

        /// <summary>
        /// 是否生成任务单
        /// </summary>
        public bool IsGenerateTask
        {
            get
            {
                return WipService.IsGenerateTask();
            }
        }

        #region 串口数据处理

        /// <summary>
        /// 串口列表
        /// </summary>
        private List<System.IO.Ports.SerialPort> serials = new List<System.IO.Ports.SerialPort>();

        /// <summary>
        /// 关闭通信串口
        /// </summary>
        protected void CloseSerial()
        {
            foreach (var serial in serials)
                if (serial.IsOpen)
                    serial.Close();
        }

        /// <summary>
        /// 初始化设备端口
        /// </summary>
        /// <param name="configSettingBase"></param>
        public void InitDevicePort(ConfigSettingBase configSettingBase)
        {
            CloseSerial();
            new Task(() =>
            {
                var devicePort = !configSettingBase.DevPort;
                if (devicePort)
                {
                    OpenSerial(configSettingBase);
                }
            }).Start();
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="configSettingBase"></param>
        void OpenSerial(ConfigSettingBase configSettingBase)
        {
            //初始化串口信息可配置多个串口
            foreach (var s in configSettingBase.SerialPorts)
            {
                var serialPort = new System.IO.Ports.SerialPort();
                serials.Add(serialPort);
                serialPort.PortName = s.PortName;
                serialPort.BaudRate = s.BaudRate;
                serialPort.DataReceived += Serial_DataReceived;
                try
                {
                    serialPort.Open();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("打开串口[{0}]失败:".L10nFormat(s.PortName) + exc.Message);
                }
            }
        }

        /// <summary>
        /// 串口数据接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                ReadBarcode((sender as System.IO.Ports.SerialPort).ReadLine().TrimEnd('\n', '\r', '\0').TrimStart('\0'));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        /// <summary>
        /// 读取条码
        /// </summary>
        /// <param name="read"></param>
        public virtual void ReadBarcode(string read)
        {
            //todo
        }
        #endregion

        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="info"></param>
        protected virtual void MergeData(ProductInfo info)
        {
            if (info.BarcodeType == BarcodeType.CombinedCode)
            {
                PanelInfo.Clear();
                if (info.PanelInfo.IsBinding)
                    PanelInfo.BindingMode = IsConfigAuto ? BindingMode.Auto : BindingMode.Manual;
                PanelInfo.BarcodeType = info.BarcodeType;
                PanelInfo.PanelQty = info.PanelInfo.CanBindQty;
                PanelInfo.ForkPlateQty = info.PanelInfo.ForkPlateQty;
                PanelInfo.PanelCode = info.PanelInfo.PanelCode;
                PanelInfo.SnList.AddRange(info.PanelInfo.SnList);
            }

            WipProductProcessState = info.WipProductProcessState;
        }

        /// <summary>
        /// 初始化拼板码信息
        /// </summary>
        /// <param name="collectData">采集数据</param>
        protected virtual void InitCombinedCodeInfo(CollectData collectData)
        {
            var bindingSns = PanelInfo.SnList.Select(p => new BindingSn() { Sn = p.Sn, Qty = p.Qty }).ToList();
            collectData.CombinedCode.BindingSns.AddRange(bindingSns);
            collectData.CombinedCode.AutoCreateAndBinding = IsConfigAuto;
            collectData.CombinedCode.ToBindingQty = PanelInfo.PanelQty - PanelInfo.ForkPlateQty;
            collectData.CollectBarcode.Type = PanelInfo.BarcodeType.HasValue ? PanelInfo.BarcodeType.Value : collectData.CollectBarcode.Type;
        }


        public FormBase()
        {
            InitializeComponent();
            detailsNoBoderForm = new List<Form>();
        }

        
        private void FormBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = 0; i < detailsNoBoderForm.Count; i++)
            {
                if (detailsNoBoderForm[i] != null)
                {
                    detailsNoBoderForm[i].Close();
                    detailsNoBoderForm[i] = null;//释放所有弹出明细页面
                }
            }
        }

        /// <summary>
        /// 打印拼版码
        /// </summary>
        public void PrintBindingSn(ConfigSettingBase configSettingBase, double workOrderId)
        {
            try
            {
                var bacodes = WipService.GetAndDeleteToBePrintedSnList(workOrderId, PanelInfo.PanelCode);
                if (!bacodes.Any())
                    return;
                var info = WipService.GetWorkOrderBarcodePrintInfo(workOrderId);
                info.Printer = configSettingBase.Printer;
                this.PrintBarcode(info, bacodes);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.Write(exc.Message);
            }
        }
        private void PrintBarcode(BarcodePrintInfo info, List<Barcode> barcodes)
        {
            if (!barcodes.Any())
            {
                throw new ValidationException("条码不能为空".L10N());
            }
            if (info == null || string.IsNullOrEmpty(info.Printer))
            {
                throw new ValidationException("打印机不能为空".L10N());
            }
            if (string.IsNullOrEmpty(info.TemplateType))
            {
                throw new ValidationException("模板类型不能为空".L10N());
            }

            string spath = XpPrinter.Instance.DownloadTempalteWithFileName(info.PrintTemplateId, LoginInfo.Instance.InvOrgId);

            List<object> list = new List<object>();
            barcodes.ForEach(it =>
            {
                list.Add(it);
            });
            XpPrinter.Instance.Print(spath, list, info.Printer);
        }
    }
}
