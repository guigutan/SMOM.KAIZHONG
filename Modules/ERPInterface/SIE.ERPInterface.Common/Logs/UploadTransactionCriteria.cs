using SIE.Core.Enums;
using SIE.Domain;
using SIE.ERPInterface.Common.Enums;
using SIE.Inventory.Transactions;
using SIE.MES.TaskManagement.Reports;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 事物上传ERP记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("事物上传查询实体")]
    public class UploadTransactionCriteria : Criteria
    {
        #region 单据大类 OrderType
        /// <summary>
        /// 单据大类
        /// </summary>
        [Label("单据大类")]
        public static readonly Property<OrderType?> OrderTypeProperty = P<UploadTransactionCriteria>.Register(e => e.OrderType);

        /// <summary>
        /// 单据大类
        /// </summary>
        public OrderType? OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 单据小类编码 TransactionCode
        /// <summary>
        /// 单据小类编码
        /// </summary>
        [Label("单据小类编码")]
        public static readonly Property<string> TransactionCodeProperty = P<UploadTransactionCriteria>.Register(e => e.TransactionCode);

        /// <summary>
        /// 单据小类编码
        /// </summary>
        public string TransactionCode
        {
            get { return this.GetProperty(TransactionCodeProperty); }
        }
        #endregion

        #region 交易类型 TransactionType
        /// <summary>
        /// 交易类型
        /// </summary>
        [Label("交易类型")]
        public static readonly Property<TransactionType?> TransactionTypeProperty = P<UploadTransactionCriteria>.Register(e => e.TransactionType);

        /// <summary>
        /// 交易类型
        /// </summary>
        public TransactionType? TransactionType
        {
            get { return GetProperty(TransactionTypeProperty); }
            set { SetProperty(TransactionTypeProperty, value); }
        }
        #endregion

        #region 处理状态 State
        /// <summary>
        /// 处理状态
        /// </summary>
        [Label("处理状态")]
        public static readonly Property<string> StateProperty = P<UploadTransactionCriteria>.Register(e => e.State);

        /// <summary>
        /// 处理状态
        /// </summary>
        public string State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<UploadTransactionCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<UploadTransactionCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<UploadTransactionCriteria>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return GetProperty(WoNoProperty); }
            set { SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 批次 LotCode
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotCodeProperty = P<UploadTransactionCriteria>.Register(e => e.LotCode);

        /// <summary>
        /// 批次
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
            set { this.SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<UploadTransactionCriteria>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 返回物料凭证 Mblnr
        /// <summary>
        /// 返回物料凭证
        /// </summary>
        [Label("返回物料凭证")]
        public static readonly Property<string> MblnrProperty = P<UploadTransactionCriteria>.Register(e => e.Mblnr);

        /// <summary>
        /// 返回物料凭证
        /// </summary>
        public string Mblnr
        {
            get { return this.GetProperty(MblnrProperty); }
            set { this.SetProperty(MblnrProperty, value); }
        }
        #endregion

        #region 返回物料凭证年度 Mjahr
        /// <summary>
        /// 返回物料凭证年度
        /// </summary>
        [Label("返回物料凭证年度")]
        public static readonly Property<string> MjahrProperty = P<UploadTransactionCriteria>.Register(e => e.Mjahr);

        /// <summary>
        /// 返回物料凭证年度
        /// </summary>
        public string Mjahr
        {
            get { return this.GetProperty(MjahrProperty); }
            set { this.SetProperty(MjahrProperty, value); }
        }
        #endregion

        #region 处理信息 ProcessMessage
        /// <summary>
        /// 处理信息
        /// </summary>
        [Label("处理信息")]
        public static readonly Property<string> ProcessMessageProperty = P<UploadTransactionCriteria>.Register(e => e.ProcessMessage);

        /// <summary>
        /// 处理信息
        /// </summary>
        public string ProcessMessage
        {
            get { return this.GetProperty(ProcessMessageProperty); }
            set { this.SetProperty(ProcessMessageProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange?> CreateDateProperty = P<UploadTransactionCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange? CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region SAP事务ID Zuid
        /// <summary>
        /// SAP事务ID
        /// </summary>
        [Label("SAP事务ID")]
        public static readonly Property<string> ZuidProperty = P<UploadTransactionCriteria>.Register(e => e.Zuid);

        /// <summary>
        /// SAP事务ID
        /// </summary>
        public string Zuid
        {
            get { return this.GetProperty(ZuidProperty); }
            set { this.SetProperty(ZuidProperty, value); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<UploadTransactionCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 父级旧料号 Bismt
        /// <summary>
        /// 父级旧料号
        /// </summary>
        [Label("父级旧料号")]
        public static readonly Property<string> BismtProperty = P<UploadTransactionCriteria>.Register(e => e.Bismt);

        /// <summary>
        /// 父级旧料号
        /// </summary>
        public string Bismt
        {
            get { return this.GetProperty(BismtProperty); }
            set { this.SetProperty(BismtProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间ID
        /// </summary>
        [Label("车间")]
        public static new readonly IRefIdProperty WorkShopIdProperty =
            P<UploadTransactionCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间ID
        /// </summary>
        public new double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<UploadTransactionCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<UploadTransactionCriteria>.Register(e => e.WorkShopCode);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
            set { this.SetProperty(WorkShopCodeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<UploadLogControllercs>().GetUploadTransactions(this);
        }
    }
}
