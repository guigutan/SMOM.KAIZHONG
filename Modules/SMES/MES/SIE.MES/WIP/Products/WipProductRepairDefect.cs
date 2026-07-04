using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品维修记录缺陷列表
    /// </summary>
    [ChildEntity, Serializable]	
    [Label("产品维修记录缺陷列表")]
    public partial class WipProductRepairDefect : DataEntity
    {
        #region 产品缺陷记录 WipProductDefect
        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        public static readonly IRefIdProperty WipProductDefectIdProperty = P<WipProductRepairDefect>.RegisterRefId(e => e.WipProductDefectId, ReferenceType.Normal);

        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        public double WipProductDefectId
        {
            get { return (double)GetRefId(WipProductDefectIdProperty); }
            set { SetRefId(WipProductDefectIdProperty, value); }
        }

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductDefect> WipProductDefectProperty = P<WipProductRepairDefect>.RegisterRef(e => e.WipProductDefect, WipProductDefectIdProperty);

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public WipProductDefect WipProductDefect
        {
            get { return GetRefEntity(WipProductDefectProperty); }
            set { SetRefEntity(WipProductDefectProperty, value); }
        }
        #endregion

        #region 产品维修记录 WipProductRepair
        /// <summary>
        /// 产品维修记录Id
        /// </summary>
        public static readonly IRefIdProperty WipProductRepairIdProperty = P<WipProductRepairDefect>.RegisterRefId(e => e.WipProductRepairId, ReferenceType.Parent);

        /// <summary>
        /// 产品维修记录Id
        /// </summary>
        public double WipProductRepairId
        {
            get { return (double)GetRefId(WipProductRepairIdProperty); }
            set { SetRefId(WipProductRepairIdProperty, value); }
        }

        /// <summary>
        /// 产品维修记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductRepair> WipProductRepairProperty = P<WipProductRepairDefect>.RegisterRef(e => e.WipProductRepair, WipProductRepairIdProperty);

        /// <summary>
        /// 产品维修记录
        /// </summary>
        public WipProductRepair WipProductRepair
        {
            get { return GetRefEntity(WipProductRepairProperty); }
            set { SetRefEntity(WipProductRepairProperty, value); }
        }
        #endregion

        #region 视图属性         
        #region 缺陷代码 DefectCode
        /// <summary>
        /// 缺陷代码
        /// </summary>
        [Label("缺陷代码")]
        public static readonly Property<string> DefectCodeProperty = P<WipProductRepairDefect>.RegisterView(e => e.DefectCode, p => p.WipProductDefect.Defect.Code);

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public string DefectCode
        {
            get { return this.GetProperty(DefectCodeProperty); }
        }
        #endregion

        #region 缺陷描述 DefectDesc
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescProperty = P<WipProductRepairDefect>.RegisterView(e => e.DefectDesc, p => p.WipProductDefect.Defect.Description);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc
        {
            get { return this.GetProperty(DefectDescProperty); }
        }
        #endregion

        #region 缺陷位置 DefectLocation
        /// <summary>
        /// 缺陷位置
        /// </summary>
        [Label("缺陷位置")]
        public static readonly Property<string> DefectLocationProperty = P<WipProductRepairDefect>.RegisterView(e => e.DefectLocation, p => p.WipProductDefect.Location);

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string DefectLocation
        {
            get { return this.GetProperty(DefectLocationProperty); }
        }
        #endregion

        #region 产品缺陷备注 DefectRemark
        /// <summary>
        /// 产品缺陷备注
        /// </summary>
        [Label("产品缺陷备注")]
        public static readonly Property<string> DefectRemarkProperty = P<WipProductRepairDefect>.RegisterView(e => e.DefectRemark, p => p.WipProductDefect.Remark);

        /// <summary>
        /// 产品缺陷备注
        /// </summary>
        public string DefectRemark
        {
            get { return this.GetProperty(DefectRemarkProperty); }
        }
        #endregion


        #region 返修人 RepaireByName
        /// <summary>
        /// 返修人
        /// </summary>
        [Label("返修人")]
        public static readonly Property<string> RepaireByNameProperty = P<WipProductRepairDefect>.RegisterView(e => e.RepaireByName, p => p.WipProductRepair.ReparieBy.Name);

        /// <summary>
        /// 返修人
        /// </summary>
        public string RepaireByName
        {
            get { return this.GetProperty(RepaireByNameProperty); }
        }
        #endregion

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<WipProductRepairDefect>.RegisterView(e => e.StationName, p => p.WipProductRepair.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<WipProductRepairDefect>.RegisterView(e => e.ProcessName, p => p.WipProductRepair.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<WipProductRepairDefect>.RegisterView(e => e.ResourceName, p => p.WipProductRepair.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 班次名称 ShiftName
        /// <summary>
        /// 班次名称
        /// </summary>
        [Label("班次")]
        public static readonly Property<string> ShiftNameProperty = P<WipProductRepairDefect>.RegisterView(e => e.ShiftName, p => p.WipProductRepair.Shift.Name);

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName
        {
            get { return this.GetProperty(ShiftNameProperty); }
        }
        #endregion

        #region 维修开始时间 RepairStart
        /// <summary>
        /// 维修开始时间
        /// </summary>
        [Label("维修开始时间")]
        public static readonly Property<DateTime> RepairStartProperty = P<WipProductRepairDefect>.RegisterView(e => e.RepairStart, p => p.WipProductRepair.RepairStart);

        /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime RepairStart
        {
            get { return this.GetProperty(RepairStartProperty); }
        }
        #endregion


        #region 维修完成时间 RepaireTime
        /// <summary>
        /// 维修完成时间
        /// </summary>
        [Label("维修完成时间")]
        public static readonly Property<DateTime?> RepaireTimeProperty = P<WipProductRepairDefect>.RegisterView(e => e.RepaireTime, p => p.WipProductRepair.RepaireTime);

        /// <summary>
        /// 维修完成时间
        /// </summary>
        public DateTime? RepaireTime
        {
            get { return this.GetProperty(RepaireTimeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 产品维修记录缺陷列表 实体配置
    /// </summary>
    internal class WipProductRepairDefectConfig : EntityConfig<WipProductRepairDefect>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_REP_DEFECT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
