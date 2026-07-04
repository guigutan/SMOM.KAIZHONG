using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联条码视图 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("关联条码视图查询实体")]
    public partial class UnionBarcodeViewCriteria : Criteria
    {
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<UnionBarcodeViewCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<UnionBarcodeViewCriteria>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return GetProperty(BarcodeProperty); }
            set { SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 成品检验单号 InspetNo
        /// <summary>
        /// 成品检验单号
        /// </summary>
        [Label("成品检验单号")]
        public static readonly Property<string> InspetNoProperty = P<UnionBarcodeViewCriteria>.Register(e => e.InspetNo);

        /// <summary>
        /// 成品检验单号
        /// </summary>
        public string InspetNo
        {
            get { return GetProperty(InspetNoProperty); }
            set { SetProperty(InspetNoProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写查询方法
        /// </summary>
        /// <returns>自定义选择数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<IReworkBarcode>().GetUnionBarcodeViews(this);
        }
    }
}