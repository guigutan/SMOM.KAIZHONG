using SIE.Core.Common;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.ProcessProperty
{
    /// <summary>
    /// 工序参数数采查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工序参数数采查询实体")]
    public class ProcessParamCollectCriteria : Criteria
    {
        #region SN
        /// <summary>
        /// SN
        /// </summary>
        [Label("SN")]
        public static readonly Property<string> SNProperty = P<ProcessParamCollectCriteria>.Register(e => e.SN);

        /// <summary>
        /// SN
        /// </summary>
        public string SN
        {
            get { return this.GetProperty(SNProperty); }
            set { this.SetProperty(SNProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ProcessParamCollectCriteria>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 设备名称 EquipmentName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipmentNameProperty = P<ProcessParamCollectCriteria>.Register(e => e.EquipmentName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return this.GetProperty(EquipmentNameProperty); }
            set { this.SetProperty(EquipmentNameProperty, value); }
        }
        #endregion

        #region 质量状态 QualityStatus
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<InspResult?> QualityStatusProperty = P<ProcessParamCollectCriteria>.Register(e => e.QualityStatus);

        /// <summary>
        /// 质量状态
        /// </summary>
        public InspResult? QualityStatus
        {
            get { return this.GetProperty(QualityStatusProperty); }
            set { this.SetProperty(QualityStatusProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<ProcessParamCollectCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProcessParamCollectController>().CriterialProcessParamCollect(this);
        }
    }
}
