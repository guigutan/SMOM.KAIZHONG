using SIE.Barcodes;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{

    /// <summary>
    /// 条码查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("条码查询实体")]
    public partial class WipProductBarcodeCriteria : Criteria
    {
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipProductBarcodeCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 条码号 Sn
        /// <summary>
        /// 条码号
        /// </summary>
        [Label("条码号")]
        public static readonly Property<string> SnProperty = P<WipProductBarcodeCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>条码列表</returns>
        protected override EntityList Fetch()
        {
            var ctl = RT.Service.Resolve<WipProductBarcodeController>();
            return ctl.GetBarcodes(this);
        }
        #endregion
    }
}