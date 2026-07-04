using SIE.Common.CollectBarcodeConverters;
using SIE.Common.Configs;
using SIE.Common.Prints;
using SIE.Defects;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.Reports;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Tech.Stations;
using SIE.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.Wpf.MES.TaskManagement.Reports
{
    /// <summary>
    /// 任务单报工 视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("任务单报工")]
    public class TaskReportViewModel : ViewModel, IFocusTrigger 
    {
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

        #region 任务单 DispatchTask
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<TaskReportViewModel>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double? DispatchTaskId
        {
            get { return (double?)this.GetRefNullableId(DispatchTaskIdProperty); }
            set { this.SetRefNullableId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 任务单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<TaskReportViewModel>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 任务单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 报工人员 ReportEmployee
        /// <summary>
        /// 报工人员Id
        /// </summary>
        [Label("报工人员")]
        public static readonly IRefIdProperty ReportEmployeeIdProperty =
            P<TaskReportViewModel>.RegisterRefId(e => e.ReportEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 报工人员Id
        /// </summary>
        public double? ReportEmployeeId
        {
            get { return (double?)this.GetRefNullableId(ReportEmployeeIdProperty); }
            set { this.SetRefNullableId(ReportEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 报工人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReportEmployeeProperty =
            P<TaskReportViewModel>.RegisterRef(e => e.ReportEmployee, ReportEmployeeIdProperty);

        /// <summary>
        /// 报工人员
        /// </summary>
        public Employee ReportEmployee
        {
            get { return this.GetRefEntity(ReportEmployeeProperty); }
            set { this.SetRefEntity(ReportEmployeeProperty, value); }
        }
        #endregion

        #region 累计合格数 TotalOkQty
        /// <summary>
        /// 累计合格数
        /// </summary>
        [Label("累计合格数")]
        public static readonly Property<decimal> TotalOkQtyProperty = P<TaskReportViewModel>.Register(e => e.TotalOkQty);

        /// <summary>
        /// 累计合格数
        /// </summary>
        public decimal TotalOkQty
        {
            get { return this.GetProperty(TotalOkQtyProperty); }
            set { this.SetProperty(TotalOkQtyProperty, value); }
        }
        #endregion

        #region 累计不合格数 TotalNgQty
        /// <summary>
        /// 累计不合格数
        /// </summary>
        [Label("累计不合格数")]
        public static readonly Property<decimal> TotalNgQtyProperty = P<TaskReportViewModel>.Register(e => e.TotalNgQty);

        /// <summary>
        /// 累计不合格数
        /// </summary>
        public decimal TotalNgQty
        {
            get { return this.GetProperty(TotalNgQtyProperty); }
            set { this.SetProperty(TotalNgQtyProperty, value); }
        }
        #endregion

        #region 合格数 OkQty
        /// <summary>
        /// 合格数
        /// </summary>
        [Label("合格数")]
        public static readonly Property<decimal> OkQtyProperty = P<TaskReportViewModel>.Register(e => e.OkQty);

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal OkQty
        {
            get { return GetProperty(OkQtyProperty); }
            set { SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不合格数 NgQty
        /// <summary>
        /// 不合格数
        /// </summary>
        [Label("不合格数")]
        public static readonly Property<decimal> NgQtyProperty = P<TaskReportViewModel>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<TaskReportViewModel>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double? StationId
        {
            get { return (double?)this.GetRefNullableId(StationIdProperty); }
            set { this.SetRefNullableId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty =
            P<TaskReportViewModel>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 统计工时（小时） Hour
        /// <summary>
        /// 统计工时（小时）
        /// </summary>
        [Label("统计工时（小时）")]
        public static readonly Property<decimal> HourProperty = P<TaskReportViewModel>.Register(e => e.Hour);

        /// <summary>
        /// 统计工时（小时）
        /// </summary>
        public decimal Hour
        {
            get { return GetProperty(HourProperty); }
            set { SetProperty(HourProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<TaskReportViewModel>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 缺陷列表 DefectList
        /// <summary>
        /// 缺陷列表
        /// </summary>
        public static readonly ListProperty<EntityList<Defect>> DefectListProperty = P<TaskReportViewModel>.RegisterList(e => e.DefectList, new ListPropertyMeta()
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = (DispatchTask) => { return new EntityList<Defect>(); }
        });

        /// <summary>
        /// 缺陷列表
        /// </summary>
        public EntityList<Defect> DefectList
        {
            get { return this.GetLazyList(DefectListProperty); }
        }
        #endregion

        #region 缺陷值 DefectNames
        /// <summary>
        /// 缺陷值
        /// </summary>
        [Label("缺陷值")]
        public static readonly Property<string> DefectNamesProperty = P<TaskReportViewModel>.Register(e => e.DefectNames);

        /// <summary>
        /// 缺陷值
        /// </summary>
        public string DefectNames
        {
            get { return this.GetProperty(DefectNamesProperty); }
            set { this.SetProperty(DefectNamesProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<TaskReportViewModel>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 报工记录列表 ReportRecordList
        /// <summary>
        /// 报工记录列表
        /// </summary>
        public static readonly ListProperty<EntityList<ReportRecord>> ReportRecordListProperty = P<TaskReportViewModel>.RegisterList(e => e.ReportRecordList, new ListPropertyMeta()
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = (DispatchTask) => { return new EntityList<ReportRecord>(); }
        });

        /// <summary>
        /// 报工记录列表
        /// </summary>
        public EntityList<ReportRecord> ReportRecordList
        {
            get { return this.GetLazyList(ReportRecordListProperty); }
        }
        #endregion

        #region 报检数量 InspQty
        /// <summary>
        /// 报检数量
        /// </summary>
        [Label("报检数量")]
        public static readonly Property<int> InspQtyProperty = P<TaskReportViewModel>.Register(e => e.InspQty);

        /// <summary>
        /// 报检数量
        /// </summary>
        public int InspQty
        {
            get { return this.GetProperty(InspQtyProperty); }
            set { this.SetProperty(InspQtyProperty, value); }
        }
        #endregion

        #region Printer 打印机
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        public static readonly Property<string> PrinterProperty = P<TaskReportViewModel>.Register(e => e.Printer);

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }
        #endregion

        #region Template 模板
        /// <summary>
        /// 模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty = P<TaskReportViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 模板Id
        /// </summary>
        public double? TemplateId
        {
            get { return (double)this.GetRefNullableId(TemplateIdProperty); }
            set { this.SetRefNullableId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty = P<TaskReportViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region 提示信息

        #region Tips 提示信息
        /// <summary>
        /// 提示信息
        /// </summary>
        [Label("提示信息")]
        public static readonly Property<string> TipsProperty = P<TaskReportViewModel>.Register(e => e.Tips);

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Tips
        {
            get { return this.GetProperty(TipsProperty); }
            set { this.SetProperty(TipsProperty, value); }
        }
        #endregion

        #region Error 错误信息
        /// <summary>
        /// 错误信息
        /// </summary>
        [Label("错误信息")]
        public static readonly Property<string> ErrorProperty = P<TaskReportViewModel>.Register(e => e.Error);

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
            if (error == null)
            {
                return;
            }
            string errMsg = error;
            if (errMsg.Contains("执行失败:"))
            {
                var errMsgArr = errMsg.Split(new String[] { "执行失败:" }, StringSplitOptions.RemoveEmptyEntries);
                if (errMsgArr.Length > 1)
                    errMsg = errMsgArr[1];
            }
            ClearInfos();
            Error = errMsg.Replace("\r\n", string.Empty);
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
            ClearInfos();
            Tips = tips.Replace("\r\n", string.Empty);
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

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("扫描条码")]
        public static readonly Property<string> BarcodeProperty = P<TaskReportViewModel>.Register(e => e.Barcode, new PropertyMetadata<string>
        {
            PropertyChangedCallBack = (s, e) => (s as TaskReportViewModel).OnBarcodeChanged(e),
            CoerceGetValueCallBack = (s, v) => (s as TaskReportViewModel).CoerceGetBarcode(v)
        });

        /// <summary>
        /// 转换条码内容
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>转换后的条码内容</returns>
        protected virtual string CoerceGetBarcode(string barcode)
        {
            if (barcode.IsNullOrEmpty()) return barcode;
            switch (BarcodeFormatConfig.Format)
            {
                case BarcodeFormat.None: return barcode;
                case BarcodeFormat.Upper: return barcode.ToUpper();
                case BarcodeFormat.Lower: return barcode.ToLower();
                default:
                    {
                        var regex = new Regex(BarcodeFormatConfig.Regex);
                        var matchs = regex.Matches(barcode);
                        if (matchs.Count > 0)
                            return matchs[0].Groups["code"].Value;
                        return barcode;
                    }
            }
        }

        /// <summary>
        /// 条码
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
            if (Barcode.IsNullOrEmpty())
            {
                return;
            }
            try
            {
                ClearInfos();
                ScanBarcodeHandle(Barcode);
            }
            catch (Exception exc)
            {
                ShowError(exc.Message);
            }
            finally
            {
                Barcode = null;
            }
        }

        /// <summary>
        /// 条码格式化配置值
        /// </summary>
        CollectBarcodeConverterConfigValue _barcodeFormatConfig;

        /// <summary>
        /// 条码格式化配置值
        /// </summary>
        CollectBarcodeConverterConfigValue BarcodeFormatConfig
        {
            get { return _barcodeFormatConfig ?? (_barcodeFormatConfig = ConfigService.GetConfig(new CollectBarcodeConverterConfig())); }
        }
        #endregion

        /// <summary>
        /// 加载
        /// </summary>
        public virtual void Onload()
        {
            Reset(true);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void OnClose()
        {

        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="isClearEmployee"></param>
        public virtual void Reset(bool isClearEmployee)
        {
            Error = null;
            Tips = isClearEmployee ? "请扫描员工编码".L10N() : "请扫描任务单号".L10N();
            FocuseBarcode();

            //重置界面数据
            this.ResetInfo(isClearEmployee);
        }

        /// <summary>
        /// 重置界面信息
        /// </summary>
        /// <param name="isClearEmployee"></param>
        public virtual void ResetInfo(bool isClearEmployee) 
        {
            ReportEmployeeId = isClearEmployee ? null: ReportEmployeeId;
            ReportEmployee = isClearEmployee ? null : ReportEmployee;
            DispatchTaskId = null;
            DispatchTask = null;
            TotalOkQty = 0;
            TotalNgQty = 0;
            OkQty = 0;
            NgQty = 0;
            StationId = null;
            Station = null;
            Hour = 0;
            BatchNo = null;
            DefectNames = null;
            Remark = null;
            this.DefectList.Clear();
        }

        void ScanBarcodeHandle(string barcode) 
        {
            if (ReportEmployee == null || DispatchTask != null)
            {
                ReportEmployee = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(barcode);
                if (ReportEmployee == null)
                    ShowError("员工编码【{0}】不存在".L10nFormat(barcode));
                else 
                {
                    ShowTips("请扫描任务单号".L10N());
                    Reset(false);
                }
            }
            else 
            {
                DispatchTask = RT.Service.Resolve<DispatchController>().GetDispatchTaskByBarcode(barcode, ReportEmployee);//加载任务单信息
                LoadTaskReportInfo();//加载任务单报工信息
            }
        }

        /// <summary>
        /// 开工
        /// </summary>
        public virtual void StartWork() 
        {
            try
            {
                var employee = RT.Service.Resolve<EmployeeController>().GetEmployeeById(RT.IdentityId);
                RT.Service.Resolve<ReportController>().StartWork(employee, DispatchTask);
                ShowTips("任务单【{0}】开工成功!".L10nFormat(DispatchTask.No));
                DispatchTask.TaskStatus = DispatchTaskStatus.Executing;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 首件报检
        /// </summary>
        public virtual void ReportFirstInsp() 
        {
            try
            {
                RT.Service.Resolve<ReportController>().ReportFirstInsp(DispatchTask);
                ShowTips("任务单【{0}】首件报检成功!".L10nFormat(DispatchTask.No));
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 报工
        /// </summary>
        public virtual ReportRecord TaskReport()
        {
            try
            {
                ReportTaskInfo info = new ReportTaskInfo()
                {
                    BatchNo = BatchNo,
                    OkQty = OkQty,
                    NgQty = NgQty,
                    ReportNgQty = NgQty,
                    Remark = Remark,
                    TaskId = DispatchTaskId.Value,
                    Hour = Hour,
                    ProcessId = DispatchTask.ProcessId,
                    StationId = StationId,
                    WorkOrderId = DispatchTask.WorkOrderId ?? 0,
                    DefectIds = DefectList.Select(p => p.Id).ToList(),
                    IsTaskFinish = true,
                    IsValidatePrepare = true
                };
                var reportRecord = RT.Service.Resolve<ReportController>().TaskReport(info, true, true);
                this.ReportRecordList.Add(reportRecord);
                LoadTaskReportInfo();//刷新任务单报工信息
                ShowTips("任务单【{0}】报工成功!".L10nFormat(DispatchTask.No));
                return reportRecord;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 加载任务单报工信息
        /// </summary>
        void LoadTaskReportInfo() 
        {
            var reportRecord = RT.Service.Resolve<ReportController>().GetOrCreateMainReportRecord(DispatchTask.Id);
            TotalOkQty = reportRecord.TotalOkQty;
            TotalNgQty = reportRecord.TotalNgQty;
            OkQty = reportRecord.OkQty;
            NgQty = reportRecord.NgQty;
        }
    }
}
