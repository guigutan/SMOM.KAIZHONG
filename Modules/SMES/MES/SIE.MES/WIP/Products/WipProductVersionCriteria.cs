using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品版本查询实体
    /// </summary>
    [Label("产品版本查询实体")]
    [QueryEntity, Serializable]
    public class WipProductVersionCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipProductVersionCriteria()
        {
            StartDate = new DateRange();
        }

        #region 组合板工单号 PanelWorkOrderNo
        /// <summary>
        /// 组合板工单号
        /// </summary>
        [Label("组合板工单号")]
        public static readonly Property<string> PanelWorkOrderNoProperty = P<WipProductVersionCriteria>.Register(e => e.PanelWorkOrderNo);

        /// <summary>
        /// 组合板工单号
        /// </summary>
        public string PanelWorkOrderNo
        {
            get { return this.GetProperty(PanelWorkOrderNoProperty); }
            set { this.SetProperty(PanelWorkOrderNoProperty, value); }
        }
        #endregion

        #region 工单号 No
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> NoProperty = P<WipProductVersionCriteria>.Register(e => e.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 条码 Sn
        /// <summary>
        /// 条码
        /// </summary>
        [Label("产品条码")]
        public static readonly Property<string> SnProperty = P<WipProductVersionCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 关键件来源条码 ItemSN
        /// <summary>
        /// 关键件来源条码
        /// </summary>
        [Label("关键件来源条码")]
        public static readonly Property<string> ItemSNProperty = P<WipProductVersionCriteria>.Register(e => e.ItemSN);
        /// <summary>
        /// 装配条码
        /// </summary>
        public string ItemSN
        {
            get { return this.GetProperty(ItemSNProperty); }
            set { this.SetProperty(ItemSNProperty, value); }
        }
        #endregion

        #region 当前工序 Process
        /// <summary>
        /// 当前工序Id
        /// </summary>
        [Label("当前工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WipProductVersionCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);
        /// <summary>
        /// 当前工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<WipProductVersionCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);
        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 上线日期 StartDate
        /// <summary>
        /// 上线日期
        /// </summary>
        [Label("上线日期")]
        public static readonly Property<DateRange> StartDateProperty = P<WipProductVersionCriteria>.Register(e => e.StartDate, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.DateTime, });
        /// <summary>
        /// 上线日期
        /// </summary>
        public DateRange StartDate
        {
            get { return GetProperty(StartDateProperty); }
            set { SetProperty(StartDateProperty, value); }
        }
        #endregion

        #region 下一工序 NextProcess
        /// <summary>
        /// 下一工序Id
        /// </summary>
        [Label("下一工序")]
        public static readonly IRefIdProperty NextProcessIdProperty = P<WipProductVersionCriteria>.RegisterRefId(e => e.NextProcessId, ReferenceType.Normal);
        /// <summary>
        /// 下一工序Id
        /// </summary>
        public double? NextProcessId
        {
            get { return (double?)GetRefNullableId(NextProcessIdProperty); }
            set { SetRefNullableId(NextProcessIdProperty, value); }
        }

        /// <summary>
        /// 下一工序
        /// </summary>
        public static readonly RefEntityProperty<Process> NextProcessProperty = P<WipProductVersionCriteria>.RegisterRef(e => e.NextProcess, NextProcessIdProperty);
        /// <summary>
        /// 下一工序
        /// </summary>
        public Process NextProcess
        {
            get { return GetRefEntity(NextProcessProperty); }
            set { SetRefEntity(NextProcessProperty, value); }
        }
        #endregion

        #region 拼板码 PanelCode
        /// <summary>
        /// 拼板码
        /// </summary>
        [Label("拼板码")]
        public static readonly Property<string> PanelCodeProperty = P<WipProductVersionCriteria>.Register(e => e.PanelCode);

        /// <summary>
        /// 拼板码
        /// </summary>
        public string PanelCode
        {
            get { return this.GetProperty(PanelCodeProperty); }
            set { this.SetProperty(PanelCodeProperty, value); }
        }
        #endregion

        #region 组件条码 KeyLabel
        /// <summary>
        /// 组件条码
        /// </summary>
        [Label("组件条码")]
        public static readonly Property<string> KeyLabelProperty = P<WipProductVersionCriteria>.Register(e => e.KeyLabel);

        /// <summary>
        /// 组件条码
        /// </summary>
        public string KeyLabel
        {
            get { return this.GetProperty(KeyLabelProperty); }
            set { this.SetProperty(KeyLabelProperty, value); }
        }
        #endregion

        #region 是否完工 IsFinish
        /// <summary>
        /// 是否完工
        /// </summary>
        [Label("是否完工")]
        public static readonly Property<YesNo?> IsFinishProperty = P<WipProductVersionCriteria>.Register(e => e.IsFinish);

        /// <summary>
        /// 是否完工
        /// </summary>
        public YesNo? IsFinish
        {
            get { return this.GetProperty(IsFinishProperty); }
            set { this.SetProperty(IsFinishProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WipProductVersionCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WipProductVersionCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion


        /// <summary>
        /// 获取产品版本列表
        /// </summary>
        /// <returns>产品版本列表</returns>
        protected override EntityList Fetch()
        {
            using (Diagnostics.DebugTrace.Start("生产通用报表查询：".L10N()))
            {
                return RT.Service.Resolve<WipProductVersionController>().GetWipProductVersions(this);
            }
        }
    }
}