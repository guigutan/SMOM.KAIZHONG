using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.OutDepotHandovers
{
    /// <summary>
    /// 备件出库单查询器
    /// </summary>
    [QueryEntity, Serializable]
    public class OutDepotHandoverCriteria : Criteria
    {
        #region 交接单号 HandoverNo
        /// <summary>
        /// 交接单号
        /// </summary>
        [Label("交接单号")]
        public static readonly Property<string> HandoverNoProperty = P<OutDepotHandoverCriteria>.Register(e => e.HandoverNo);

        /// <summary>
        /// 交接单号
        /// </summary>
        public string HandoverNo
        {
            get { return this.GetProperty(HandoverNoProperty); }
            set { this.SetProperty(HandoverNoProperty, value); }
        }
        #endregion

        #region 出库单号 OutDepotNo
        /// <summary>
        /// 出库单号
        /// </summary>
        [Label("出库单号")]
        public static readonly Property<string> OutDepotNoProperty = P<OutDepotHandoverCriteria>.Register(e => e.OutDepotNo);

        /// <summary>
        /// 出库单号
        /// </summary>
        public string OutDepotNo
        {
            get { return this.GetProperty(OutDepotNoProperty); }
            set { this.SetProperty(OutDepotNoProperty, value); }
        }
        #endregion

        #region 状态 HandOverStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<HandOverStatus?> HandOverStatusProperty = P<OutDepotHandoverCriteria>.Register(e => e.HandOverStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public HandOverStatus? HandOverStatus
        {
            get { return this.GetProperty(HandOverStatusProperty); }
            set { this.SetProperty(HandOverStatusProperty, value); }
        }
        #endregion

        #region 备件编码 SparePart
        /// <summary>
        /// 备件编码Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<OutDepotHandoverCriteria>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件编码Id
        /// </summary>
        public double? SparePartId
        {
            get { return (double?)this.GetRefNullableId(SparePartIdProperty); }
            set { this.SetRefNullableId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件编码
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<OutDepotHandoverCriteria>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件编码
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<OutDepotHandoverCriteria>.Register(e => e.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 序列号 SeriaNo
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SeriaNoProperty = P<OutDepotHandoverCriteria>.Register(e => e.SeriaNo);

        /// <summary>
        /// 序列号
        /// </summary>
        public string SeriaNo
        {
            get { return this.GetProperty(SeriaNoProperty); }
            set { this.SetProperty(SeriaNoProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<OutDepotHandoverCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<OutDepotHandoverController>().GetOutDepotHandoverList(this);
        }
    }
}
