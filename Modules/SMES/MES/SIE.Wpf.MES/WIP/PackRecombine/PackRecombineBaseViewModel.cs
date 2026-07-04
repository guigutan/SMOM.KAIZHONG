using SIE.Common;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.PackRecombine;
using SIE.MES.WIP.PackRecombine.Configs;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages;
using SIE.Wpf.Common;
using SIE.Wpf.MES.Controls.Messager;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace SIE.Wpf.MES.WIP.PackRecombine
{
    /// <summary>
    /// 包装拆合视图模型基类
    /// </summary>
    [RootEntity, Serializable]
    [Label("包装拆合")]
    public class PackRecombineBaseViewModel : ViewModel, IFocusTrigger
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PackRecombineBaseViewModel()
        {
            InitDevicePort();

            Printer = SIE.Common.Properties.Settings.Default.PrinterName;
        }


        /// <summary>
        /// 历史消息控件（在界面模板 XXXUITemplate 中初始化的）
        /// </summary>
        public MessagerControl MessagerControl { get; set; }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        private void AddMessageToHistory(string message, MessageType messageType)
        {
            if (MessagerControl != null)
            {
                MessagerControl.AddMessage(message, messageType);
            }
        }

        /// <summary>
        /// 待加入包装关系
        /// </summary>
        public BatchPackingRelation OuterPackingRelation { get; set; }

        /// <summary>
        /// 包装拆合控制器
        /// </summary>
        public PackRecombineController Controller { get; } = RT.Service.Resolve<PackRecombineController>();

        #region IFocusTrigger
        /// <summary>
        /// 聚焦事件
        /// </summary>
        public event EventHandler Focused;

        /// <summary>
        /// 触发条码输入框获取焦点
        /// </summary>
        public void FocuseBarcode()
        {
            Focused?.Invoke(this, EventArgs.Empty);
        }
        #endregion  

        #region 提示信息 

        #region 提示信息 Tips
        /// <summary>
        /// 提示信息
        /// </summary>
        [Label("提示信息")]
        public static readonly Property<string> TipsProperty = P<PackRecombineBaseViewModel>.Register(e => e.Tips);

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Tips
        {
            get { return this.GetProperty(TipsProperty); }
            set { this.SetProperty(TipsProperty, value); }
        }
        #endregion

        #region 错误信息 Error
        /// <summary>
        /// 错误信息
        /// </summary>
        [Label("错误信息")]
        public static readonly Property<string> ErrorProperty = P<PackRecombineBaseViewModel>.Register(e => e.Error);

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get { return this.GetProperty(ErrorProperty); }
            set { this.SetProperty(ErrorProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        public void ShowError(string error)
        {
            if(error == null)
            {
                return;
            }
            Error = error.Replace("\r\n", string.Empty);
            AddMessageToHistory(Error, MessageType.Error);
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="exc">异常</param>
        public void ShowError(Exception exc)
        {
            var validationException = exc.GetBaseException() as ValidationException;
            if (validationException != null)
                ShowError(DisplayHelper.Display(validationException.Message));
            else
                Extenstion.Alert(exc);
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="tips">提示信息</param>
        public void ShowTips(string tips)
        {
            if (tips == null)
            {
                return;
            }
            Tips = tips.Replace("\r\n", string.Empty);
            AddMessageToHistory(Tips, MessageType.Normal);
        }

        /// <summary>
        /// 清空提示信息
        /// </summary>
        protected virtual void ClearInfos()
        {
            Error = null;
            Tips = null;
        }
        #endregion

        #region 包装号 Barcode
        /// <summary>
        /// 包装号
        /// </summary>
        [Label("包装号")]
        public static readonly Property<string> BarcodeProperty = P<PackRecombineBaseViewModel>.Register(e => e.Barcode, new PropertyMetadata<string>
        {
            PropertyChangedCallBack = (s, e) => (s as PackRecombineBaseViewModel).OnBarcodeChanged(e),
        });

        /// <summary>
        /// 包装号
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }

        /// <summary>
        /// 条码扫完后处理逻辑
        /// </summary>
        /// <param name="e">属性变更参数</param>
        protected virtual void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<PackRecombineBaseViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<PackRecombineBaseViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工单包装规则 
        /// <summary>
        /// 工单包装规则
        /// </summary>
        public static readonly ListProperty<EntityList<WorkOrderPackageRuleDetail>> RuleDetailListProperty = P<PackRecombineBaseViewModel>.RegisterList(e => e.RuleDetailList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<WorkOrderPackageRuleDetail>()
        });

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public EntityList<WorkOrderPackageRuleDetail> RuleDetailList
        {
            get { return this.GetLazyList(RuleDetailListProperty); }
        }

        /// <summary>
        /// 刷新工单包装关系列表
        /// </summary>
        void RefreshPackageRuleList()
        {
            RuleDetailList.Clear();
            if (WorkOrder.PackageRuleDetailList.Count > 0)
            {
                RuleDetailList.AddRange(WorkOrder.PackageRuleDetailList);
                RuleDetailList.MarkSaved();
            }

            FocuseBarcode();
        }
        #endregion

        #region 操作明细 
        /// <summary>
        /// 操作明细
        /// </summary>
        public static readonly ListProperty<EntityList<OperationDetail>> DetailListProperty = P<PackRecombineBaseViewModel>.RegisterList(e => e.DetailList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<OperationDetail>()
        });

        /// <summary>
        /// 操作明细
        /// </summary>
        public EntityList<OperationDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 产品条码 ItemLabelList 
        /// <summary>
        /// 产品条码列表
        /// </summary>
        [Label("产品条码")]
        public static readonly ListProperty<EntityList<SIE.Packages.ItemLabels.ItemLabel>> ItemLabelListProperty = P<PackRecombineBaseViewModel>.RegisterList(e => e.ItemLabelList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<SIE.Packages.ItemLabels.ItemLabel>()
        });

        /// <summary>
        /// 产品条码列表
        /// </summary>
        public EntityList<SIE.Packages.ItemLabels.ItemLabel> ItemLabelList
        {
            get { return this.GetLazyList(ItemLabelListProperty); }
        }
        #endregion

        #region 包装关系 PackingRelationList 
        /// <summary>
        /// 包装关系列表
        /// </summary>
        [Label("包装关系")]
        public static readonly ListProperty<EntityList<BatchPackingRelation>> PackingRelationListProperty = P<PackRecombineBaseViewModel>.RegisterList(e => e.PackingRelationList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<BatchPackingRelation>()
        });

        /// <summary>
        /// 包装关系列表
        /// </summary>
        public EntityList<BatchPackingRelation> PackingRelationList
        {
            get { return this.GetLazyList(PackingRelationListProperty); }
        }
        #endregion

        #region 扫描方式 IsSplit
        /// <summary>
        /// 扫描方式
        /// </summary>
        [Label("扫描方式")]
        public static readonly Property<ScanMode> PackScanModeProperty = P<PackRecombineBaseViewModel>.Register(e => e.PackScanMode);

        /// <summary>
        /// 扫描方式
        /// </summary>
        public ScanMode PackScanMode
        {
            get { return this.GetProperty(PackScanModeProperty); }
            set { this.SetProperty(PackScanModeProperty, value); }
        }
        #endregion

        #region 设备
        /// <summary>
        /// 初始化设备端口信息
        /// </summary>
        private void InitDevicePort()
        {
            CloseSerial();
            ////初始化端口类型
            var devicePort = ConfigService.GetConfig(new PackRecombineDevicePortConfig(), typeof(PackRecombineBaseViewModel));
            if (devicePort.DevicePort == DevicePort.Serial)
                OpenSerial();
        }

        /// <summary>
        /// 串口列表
        /// </summary>
        List<System.IO.Ports.SerialPort> serials = new List<System.IO.Ports.SerialPort>();

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
        /// 打开串口
        /// </summary>
        void OpenSerial()
        {
            //初始化串口信息可配置多个串口
            var serialPortsConfig = ConfigService.GetConfig(new PackRecombinePortConfig(), typeof(PackRecombineBaseViewModel));
            foreach (var s in serialPortsConfig.SerialPortList)
            {
                var serialPort = new System.IO.Ports.SerialPort();
                serials.Add(serialPort);
                serialPort.PortName = s.PortName.ToString();
                serialPort.BaudRate = s.BaudRate;
                serialPort.DataReceived += Serial_DataReceived;
                try
                {
                    serialPort.Open();
                }
                catch (Exception exc)
                {
                    CRT.MessageService.ShowMessage("打开串口[{0}]失败:".L10nFormat(s.PortName) + exc.Message);
                }
            }
        }

        /// <summary>
        /// 串口数据接收
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            CRT.MainThread.InvokeIfRequired(() =>
            {
                try
                {
                    ReadBarcode((sender as System.IO.Ports.SerialPort).ReadLine().TrimEnd('\n', '\r', '\0').TrimStart('\0'));
                }
                catch (Exception exc)
                {
                    ShowError(exc);
                }
            });
        }

        /// <summary>
        /// 串口读取数据
        /// </summary>
        /// <param name="read">条码</param>
        protected virtual void ReadBarcode(string read)
        {
            Barcode = read;
        }
        #endregion

        #region 打印机 Printer
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        [Required]
        public static readonly Property<string> PrinterProperty = P<PackRecombineBaseViewModel>.Register(e => e.Printer, new RegisterRefIdArgs<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as PackRecombineBaseViewModel).OnPrinterChanged()
        });

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }

        /// <summary>
        /// 打印机
        /// </summary>
        void OnPrinterChanged()
        {
            SIE.Common.Properties.Settings.Default.PrinterName = Printer;
            SIE.Common.Properties.Settings.Default.Save();
        }
        #endregion 

        /// <summary>
        /// 加载
        /// </summary>
        public virtual void Onload()
        {
            Reset();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void OnClose()
        {
            CloseSerial();
        }

        /// <summary>
        /// 重置界面数据
        /// </summary>
        public virtual void Reset()
        {
            ClearInfo();
            PackScanMode = ScanMode.Move;
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public virtual void ClearInfo()
        {
            Barcode = null;
            Error = null;
            OuterPackingRelation = null;
            PackingRelationList.Clear();
            ItemLabelList.Clear();
            FocuseBarcode();
        }

        /// <summary>
        /// 添加加入包装操作明细信息
        /// </summary>
        /// <param name="recombineInfo">包装拆合信息</param>
        /// <param name="outerPackingRelation">加入的包装</param>
        /// <param name="packNo">将加入的包装号（条码号）</param>
        private void AddJoinDetails(RecombineInfo recombineInfo, BatchPackingRelation outerPackingRelation, string packNo)
        {
            var date = RF.Find<WorkOrder>().GetDbTime();
            ////移除明细
            if (recombineInfo.IsRemove)
                AddDetail(packNo, recombineInfo.PackingUnit, OperationType.Remove, recombineInfo.OldPackingNo, date);
            ////加入明细
            AddDetail(packNo, recombineInfo.PackingUnit, OperationType.JoinIn, recombineInfo.NewPackingNo, date);
        }

        /// <summary>
        /// 添加操作明细
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="packingUnit">包装单位</param>
        /// <param name="type">操作类型</param>
        /// <param name="outBarcode">外包装条码</param>
        /// <param name="date">操作时间</param> 
        protected void AddDetail(string barcode, string packingUnit, OperationType type, string outBarcode, DateTime date)
        {
            DetailList.Insert(0, new OperationDetail()
            {
                Barcode = barcode,
                PackingUnit = packingUnit,
                Type = type,
                OpertationDate = date,
                OutBarcode = outBarcode
            });
        }

        /// <summary>
        /// 刷新包装关系明细
        /// </summary>
        /// <param name="relationId">包装关系信息</param>
        public virtual void RefreshRelationDetail(double relationId)
        {
            var item = RF.GetById<BatchPackingRelation>(relationId);
            EntityList<BatchPackingRelation> list = new EntityList<BatchPackingRelation>();
            list.Add(item);
            var relationList = RT.Service.Resolve<PackageController>().GetAllRelationByParents(list);
            var currentRelation = PackingRelationList.FirstOrDefault(p => p.Id == relationId);
            ////刷新当前关系
            if (currentRelation != null)
                PackingRelationList.Remove(currentRelation);
            ////增加判断加入排除已有的关系    
            if (PackingRelationList.Count > 0)
            {
                var hasIds = PackingRelationList.Select(p => p.Id).ToList();
                relationList = relationList.Where(p => !hasIds.Contains(p.Id)).AsEntityList();
            }

            PackingRelationList.AddRange(relationList);
            ////加入只有一个最外层包装，切换会刷新列表，不需要排序
            if (PackScanMode != ScanMode.Join)
                SortPackingRelationList(item);
        }

        /// <summary>
        /// 将最新的包装关系排到第一
        /// </summary>
        /// <param name="itemRelation">要排序的包装关系</param>      
        public virtual void SortPackingRelationList(BatchPackingRelation itemRelation)
        {
            var parRelation = PackingRelationList.FirstOrDefault(p => p.Id == itemRelation.TreePId);
            if (parRelation == null)
            {
                PackingRelationList.Remove(PackingRelationList.FirstOrDefault(p => p.Id == itemRelation.Id));
                PackingRelationList.Insert(0, itemRelation);
            }
            else
            {
                SortPackingRelationList(parRelation);
            }
        }

        /// <summary>
        /// 刷新物料标签
        /// </summary>
        /// <param name="relationId">包装关系Id</param>
        public virtual void RefreshItemLabelList(double relationId)
        {
            var labels = RT.Service.Resolve<PackRecombineController>().GetItemLabels(relationId);
            ItemLabelList.Clear();
            ItemLabelList.AddRange(labels);
            ItemLabelList.MarkSaved();
        }

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="e">托管属性变更事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == PackScanModeProperty)
            {
                ClearInfo();
                switch (PackScanMode)
                {
                    case ScanMode.Move:
                        ShowTips("请扫描需移除的包装条码".L10N());
                        break;
                    case ScanMode.Join:
                        ShowTips("请扫描待加入的外包装条码".L10N());
                        break;
                    case ScanMode.Search:
                        ShowTips("请扫描待查询的包装条码".L10N());
                        break;
                    default:
                        break;
                }

                Error = null;
            }
            else if (e.Property == WorkOrderProperty && WorkOrder != null)
            {
                RefreshPackageRuleList();
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 移除包装
        /// </summary>
        /// <param name="packNo">包装号</param>
        /// <param name="isBatch">是否批次包装</param>
        protected virtual void SplitPacking(string packNo, bool isBatch)
        {
            ClearInfos();
            var recombineInfo = Controller.SplitPackingRelation(packNo, isBatch);
            ChaneWorkOrder(recombineInfo.WorkOrderId);
            ShowTips("[{0}][{1}]已从外包装[{2}]中移除".L10nFormat(recombineInfo.PackingUnit, packNo, recombineInfo.OldPackingNo));
            AddDetail(packNo, recombineInfo.PackingUnit, OperationType.Remove, recombineInfo.OldPackingNo, RF.Find<WorkOrder>().GetDbTime());
            ////增加逻辑，左边显示移除包装的外包装及其子孙
            var currentRelation = PackingRelationList.FirstOrDefault(p => p.Id == recombineInfo.RelationId);
            if (currentRelation != null)
                PackingRelationList.Remove(currentRelation);
            ////加入原来的父包装显示在左边
            var oldRelation = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(recombineInfo.OldPackingNo, true);
            RefreshRelationDetail(oldRelation.Id);
            RefreshItemLabelList(oldRelation.Id);
        }

        /// <summary>
        /// 加入包装
        /// </summary>
        /// <param name="packNo">包装号</param>
        /// <param name="isBatch">是否批次包装</param>
        protected virtual void JoinPacking(string packNo, bool isBatch)
        {
            ClearInfos();
            if (OuterPackingRelation == null)
            {
                OuterPackingRelation = Controller.JoinPackingRelationScanParent(packNo, isBatch);
                ShowTips("外包装[{0}][{1}]扫描成功，请扫描加入的内包装条码".L10nFormat(OuterPackingRelation.PackageUnit?.Name, packNo));
                ChaneWorkOrder(OuterPackingRelation.WorkOrderId);
                RefreshRelationDetail(OuterPackingRelation.Id);
            }
            else
            {
                var recombineInfo = Controller.JoinPackingRelationScanSon(packNo, OuterPackingRelation, isBatch);
                AddJoinDetails(recombineInfo, OuterPackingRelation, packNo);
                ChaneWorkOrder(OuterPackingRelation.WorkOrderId);
                RefreshRelationDetail(OuterPackingRelation.Id);
                RefreshItemLabelList(OuterPackingRelation.Id);
                if (recombineInfo.IsFullPack)
                {
                    ShowTips("[{0}]已加入外包装[{1}]，请扫描外包装条码".L10nFormat(packNo, OuterPackingRelation.PackageNo));
                    OuterPackingRelation = null;
                }
                else
                {
                    ShowTips("[{0}]已加入外包装[{1}]，请继续扫描加入的包装条码".L10nFormat(packNo, OuterPackingRelation.PackageNo));
                    OuterPackingRelation = RF.GetById<BatchPackingRelation>(OuterPackingRelation.Id);
                }
            }
        }

        /// <summary>
        /// 扫描查询
        /// </summary>
        /// <param name="packNo">包装或条码号</param>
        /// <param name="isBatch">批次</param>
        protected virtual void SearchPacking(string packNo, bool isBatch)
        {
            RecombineInfo recombineInfo;
            var relation = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(packNo, false);
            if (relation == null)
                recombineInfo = Controller.SearchPackingRelationBySn(packNo);
            else
                recombineInfo = Controller.SearchPackingRelation(packNo, isBatch);
            ClearInfos();
            OuterPackingRelation = null;
            ChaneWorkOrder(recombineInfo.WorkOrderId);
            PackingRelationList.Clear();
            RefreshRelationDetail(recombineInfo.RelationId);
            RefreshItemLabelList(recombineInfo.RelationId);
            ShowTips("[{0}][{1}]查询成功".L10nFormat(recombineInfo.PackingUnit, packNo));
            AddDetail(packNo, recombineInfo.PackingUnit, OperationType.Search, recombineInfo.OldPackingNo, RF.Find<WorkOrder>().GetDbTime());
        }

        /// <summary>
        /// 工单切换
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        protected virtual void ChaneWorkOrder(double workOrderId)
        {
            if (workOrderId != WorkOrderId)
            {
                var wo = RF.GetById<WorkOrder>(workOrderId);
                if (WorkOrder != null)
                    ShowError("工单已切换,由[{0}]切换到[{1}]".L10nFormat(WorkOrder.No, wo.No));
                else
                    ShowError("工单已切换到[{0}]".L10nFormat(wo.No));
                WorkOrder = wo;
                PackingRelationList.Clear();
                ItemLabelList.Clear();
            }
        }
    }
}