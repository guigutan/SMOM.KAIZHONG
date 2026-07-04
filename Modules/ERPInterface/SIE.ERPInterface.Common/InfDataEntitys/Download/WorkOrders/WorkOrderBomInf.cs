using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 工单BOM中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("工单BOM中间表")]
    public partial class WorkOrderBomInf : DownloadBaseEntity
    {
        #region 需求量 RequireQty
        /// <summary>
        /// 需求量
        /// </summary>
        [Label("需求量")]
        public static readonly Property<decimal> RequireQtyProperty = P<WorkOrderBomInf>.Register(e => e.RequireQty);

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal RequireQty
        {
            get { return GetProperty(RequireQtyProperty); }
            set { SetProperty(RequireQtyProperty, value); }
        }
        #endregion

        #region 单机定额 SingleQty
        /// <summary>
        /// 单机定额
        /// </summary>
        [Label("单机定额")]
        public static readonly Property<decimal> SingleQtyProperty = P<WorkOrderBomInf>.Register(e => e.SingleQty);

        /// <summary>
        /// 单机定额
        /// </summary>
        public decimal SingleQty
        {
            get { return GetProperty(SingleQtyProperty); }
            set { SetProperty(SingleQtyProperty, value); }
        }
        #endregion

        #region 是否反冲物料 IsRecoilItem
        /// <summary>
        /// 是否反冲物料
        /// </summary>
        [Label("是否反冲物料")]
        public static readonly Property<bool> IsRecoilItemProperty = P<WorkOrderBomInf>.Register(e => e.IsRecoilItem);

        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public bool IsRecoilItem
        {
            get { return GetProperty(IsRecoilItemProperty); }
            set { SetProperty(IsRecoilItemProperty, value); }
        }
        #endregion

        #region 是否虚拟物料 IsVritualItem
        /// <summary>
        /// 是否虚拟物料
        /// </summary>
        [Label("是否虚拟物料")]
        public static readonly Property<bool> IsVritualItemProperty = P<WorkOrderBomInf>.Register(e => e.IsVritualItem);

        /// <summary>
        /// 是否虚拟物料
        /// </summary>
        public bool IsVritualItem
        {
            get { return GetProperty(IsVritualItemProperty); }
            set { SetProperty(IsVritualItemProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<WorkOrderBomInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<WorkOrderBomInf>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<WorkOrderBomInf>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return GetProperty(WoNoProperty); }
            set { SetProperty(WoNoProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单BOM中间表 实体配置
    /// </summary>
    internal class WorkOrderBomInfConfig : EntityConfig<WorkOrderBomInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_WO_BOM").MapAllProperties();
            Meta.Property(WorkOrderBomInf.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}