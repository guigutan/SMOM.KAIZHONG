using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WIP.Pressure
{
    /// <summary>
    /// 耐压测试批次数据查询实体
    /// </summary>
    [QueryEntity,Serializable]
    [Label("耐压测试批次数据查询实体")]
    public class WipPressureCriteria : Criteria
    {
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipPressureCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region SN Sn
        /// <summary>
        /// SN
        /// </summary>
        [Label("SN")]
        public static readonly Property<string> SnProperty = P<WipPressureCriteria>.Register(e => e.Sn);

        /// <summary>
        /// SN
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<WipPressureCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WipPressureCriteria>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<WipPressureCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<WipPressureCriteria>.Register(e => e.ResourceCode);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<WipPressureCriteria>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 测试开始时间 BeginTime
        /// <summary>
        /// 测试开始时间
        /// </summary>
        [Label("测试开始时间")]
        public static readonly Property<DateRange> BeginTimeProperty = P<WipPressureCriteria>.Register(e => e.BeginTime);

        /// <summary>
        /// 测试开始时间
        /// </summary>
        public DateRange BeginTime
        {
            get { return this.GetProperty(BeginTimeProperty); }
            set { this.SetProperty(BeginTimeProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WipPressureController>().CriteriaWipPressure(this);
        }
    }
}
