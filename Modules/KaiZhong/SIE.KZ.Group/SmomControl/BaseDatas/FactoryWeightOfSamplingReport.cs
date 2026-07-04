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
    /// 工厂取样净重详情表
    /// </summary>
    [RootEntity, Serializable]
    [Label("工厂取样净重详情表")]
    public class FactoryWeightOfSamplingReport : FactoryBase
    {
        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<FactoryWeightOfSamplingReport>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
            set { this.SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 工单完工数 FinishQty
        /// <summary>
        /// 工单完工数
        /// </summary>
        [Label("工单完工数")]
        public static readonly Property<decimal> FinishQtyProperty = P<FactoryWeightOfSamplingReport>.Register(e => e.FinishQty);

        /// <summary>
        /// 工单完工数
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion

        #region 任务单号 TaskNo
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> TaskNoProperty = P<FactoryWeightOfSamplingReport>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
            set { this.SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 取样净重 Weight
        /// <summary>
        /// 取样净重
        /// </summary>
        [Label("取样净重")]
        public static readonly Property<decimal> WeightProperty = P<FactoryWeightOfSamplingReport>.Register(e => e.Weight);

        /// <summary>
        /// 取样净重
        /// </summary>
        public decimal Weight
        {
            get { return this.GetProperty(WeightProperty); }
            set { this.SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<FactoryWeightOfSamplingReport>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<FactoryWeightOfSamplingReport>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion


    }

    internal class FactoryWeightOfSamplingReportConfig : EntityConfig<FactoryWeightOfSamplingReport>
    {
        protected override void ConfigMeta()
        {
            Meta.MapView("FAC_WEIGHT_OF_SAMP_REPORT_V").MapAllProperties();
            Meta.DisableInvOrg();
            Meta.DisablePhantoms();
        }
    }
}
