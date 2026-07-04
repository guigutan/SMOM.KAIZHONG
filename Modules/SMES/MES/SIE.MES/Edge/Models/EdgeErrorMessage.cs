using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 错误消息
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("错误消息")]
    public class EdgeErrorMessage : DataEntity
    {
        #region 消息Id MsgId
        /// <summary>
        /// 消息Id
        /// </summary>
        [Label("消息Id")]
        public static readonly Property<string> MsgIdProperty = P<EdgeErrorMessage>.Register(e => e.MsgId);

        /// <summary>
        /// 消息Id
        /// </summary>
        public string MsgId
        {
            get { return this.GetProperty(MsgIdProperty); }
            set { this.SetProperty(MsgIdProperty, value); }
        }
        #endregion

        #region 消息名称 Name
        /// <summary>
        /// 消息名称
        /// </summary>
        [Label("消息名称")]
        public static readonly Property<string> NameProperty = P<EdgeErrorMessage>.Register(e => e.Name);

        /// <summary>
        /// 消息名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 消息内容 Bodys
        /// <summary>
        /// 消息内容
        /// </summary>
        [Label("消息内容")]
        [MaxLength(3000)]
        public static readonly Property<string> BodysProperty = P<EdgeErrorMessage>.Register(e => e.Bodys);

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Bodys
        {
            get { return this.GetProperty(BodysProperty); }
            set { this.SetProperty(BodysProperty, value); }
        }
        #endregion

        #region 消息库存组织 MsgInvOrg
        /// <summary>
        /// 消息库存组织
        /// </summary>
        [Label("库存消息库存组织组织")]
        public static readonly Property<string> MsgInvOrgProperty = P<EdgeErrorMessage>.Register(e => e.MsgInvOrg);

        /// <summary>
        /// 消息库存组织
        /// </summary>
        public string MsgInvOrg
        {
            get { return this.GetProperty(MsgInvOrgProperty); }
            set { this.SetProperty(MsgInvOrgProperty, value); }
        }
        #endregion

        #region 错误内容 ErrorContent
        /// <summary>
        /// 错误内容
        /// </summary>
        [Label("错误内容")]
        [MaxLength(1000)]
        public static readonly Property<string> ErrorContentProperty = P<EdgeErrorMessage>.Register(e => e.ErrorContent);

        /// <summary>
        /// 错误内容
        /// </summary>
        public string ErrorContent
        {
            get { return this.GetProperty(ErrorContentProperty); }
            set { this.SetProperty(ErrorContentProperty, value); }
        }
        #endregion

        #region 失败次数 ErrorTimes
        /// <summary>
        /// 失败次数
        /// </summary>
        [Label("失败次数")]
        public static readonly Property<int> ErrorTimesProperty = P<EdgeErrorMessage>.Register(e => e.ErrorTimes);

        /// <summary>
        /// 失败次数
        /// </summary>
        public int ErrorTimes
        {
            get { return this.GetProperty(ErrorTimesProperty); }
            set { this.SetProperty(ErrorTimesProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<EdgeErrorMessage>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<EdgeErrorMessage>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<EdgeErrorMessage>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<EdgeErrorMessage>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<EdgeErrorMessage>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region Guid Guid
        /// <summary>
        /// Guid
        /// </summary>
        [Label("Guid")]
        public static readonly Property<string> GuidProperty = P<EdgeErrorMessage>.Register(e => e.Guid);

        /// <summary>
        /// Guid
        /// </summary>
        public string Guid
        {
            get { return this.GetProperty(GuidProperty); }
            set { this.SetProperty(GuidProperty, value); }
        }
        #endregion

        #region 是否异常 IsError
        /// <summary>
        /// 是否异常
        /// </summary>
        [Label("是否异常")]
        public static readonly Property<YesNo> IsErrorProperty = P<EdgeErrorMessage>.Register(e => e.IsError);

        /// <summary>
        /// 是否异常
        /// </summary>
        public YesNo IsError
        {
            get { return this.GetProperty(IsErrorProperty); }
            set { this.SetProperty(IsErrorProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 错误消息 实体配置
    /// </summary>
    internal class EdgeErrorMessageConfig : EntityConfig<EdgeErrorMessage>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("EDGE_ERROR_MESSAGE").MapAllProperties();
            Meta.Property(EdgeErrorMessage.ErrorContentProperty).ColumnMeta.HasLength(1000);
            Meta.Property(EdgeErrorMessage.BodysProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(EdgeErrorMessage.MsgIdProperty).ColumnMeta.HasIndex();
            Meta.Property(EdgeErrorMessage.NameProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}
