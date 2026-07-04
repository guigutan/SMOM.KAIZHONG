using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Group.SmomControl.BaseDatas
{
    /// <summary>
    /// 工厂报工记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("工厂报工记录")]
    public class FactoryReportRecord : FactoryBase
    {
        #region 任务单号 No
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> NoProperty = P<FactoryReportRecord>.Register(e => e.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 关联工单 Wo
        /// <summary>
        /// 关联工单
        /// </summary>
        [Label("关联工单")]
        public static readonly Property<string> WoProperty = P<FactoryReportRecord>.Register(e => e.Wo);

        /// <summary>
        /// 关联工单
        /// </summary>
        public string Wo
        {
            get { return this.GetProperty(WoProperty); }
            set { this.SetProperty(WoProperty, value); }
        }
        #endregion

        #region 任务数量 DispatchQty
        /// <summary>
        /// 任务数量
        /// </summary>
        [Label("任务数量")]
        public static readonly Property<decimal> DispatchQtyProperty = P<FactoryReportRecord>.Register(e => e.DispatchQty);

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal DispatchQty
        {
            get { return this.GetProperty(DispatchQtyProperty); }
            set { this.SetProperty(DispatchQtyProperty, value); }
        }
        #endregion

        #region 报工时间 ReportTime
        /// <summary>
        /// 报工时间
        /// </summary>
        [Label("报工时间")]
        public static readonly Property<DateTime?> ReportTimeProperty = P<FactoryReportRecord>.Register(e => e.ReportTime);

        /// <summary>
        /// 报工时间
        /// </summary>
        public DateTime? ReportTime
        {
            get { return this.GetProperty(ReportTimeProperty); }
            set { this.SetProperty(ReportTimeProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<FactoryReportRecord>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<FactoryReportRecord>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 车间 WorkShopCode
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopCodeProperty = P<FactoryReportRecord>.Register(e => e.WorkShopCode);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
            set { this.SetProperty(WorkShopCodeProperty, value); }
        }
        #endregion

        #region 完工数量 FinishQty
        /// <summary>
        /// 完工数量
        /// </summary>
        [Label("完工数量")]
        public static readonly Property<decimal> FinishQtyProperty = P<FactoryReportRecord>.Register(e => e.FinishQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion

    }

    internal class FactoryReportRecordConfig : EntityConfig<FactoryReportRecord>
    {
        protected override void ConfigMeta()
        {
            Meta.MapView("FACTORY_REPORT_RECORD_V").MapAllProperties();
            Meta.DisableInvOrg();
            Meta.DisablePhantoms();
        }
    }
}
