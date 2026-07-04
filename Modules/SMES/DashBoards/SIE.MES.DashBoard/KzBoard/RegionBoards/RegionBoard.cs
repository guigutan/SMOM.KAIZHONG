using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzBoard.RegionBoards
{
    /// <summary>
    /// 区域与产线的关联关系
    /// </summary>
    [RootEntity, Serializable]
    [Label("区域与产线的关联关系")]
    [CriteriaQuery]
    public class RegionBoard : DataEntity
    {
        #region 看报区域 Region
        /// <summary>
        /// 看报区域
        /// </summary>
        [Label("看报区域")]
        public static readonly Property<string> RegionProperty = P<RegionBoard>.Register(e => e.Region);

        /// <summary>
        /// 看报区域
        /// </summary>
        public string Region
        {
            get { return this.GetProperty(RegionProperty); }
            set { this.SetProperty(RegionProperty, value); }
        }
        #endregion

        #region 看板类型 RegionBoardType
        /// <summary>
        /// 看板类型
        /// </summary>
        [Label("看板类型")]
        public static readonly Property<RegionBoardType> RegionBoardTypeProperty = P<RegionBoard>.Register(e => e.RegionBoardType);

        /// <summary>
        /// 看板类型
        /// </summary>
        public RegionBoardType RegionBoardType
        {
            get { return this.GetProperty(RegionBoardTypeProperty); }
            set { this.SetProperty(RegionBoardTypeProperty, value); }
        }
        #endregion

        //#region 设备状态区域 DeviceStatus
        ///// <summary>
        ///// 设备状态
        ///// </summary>
        //[Label("设备状态")]
        //public static readonly Property<string> DeviceStatusProperty = P<RegionBoard>.Register(e => e.DeviceStatus);

        ///// <summary>
        ///// 设备状态
        ///// </summary>
        //public string DeviceStatus
        //{
        //    get { return this.GetProperty(DeviceStatusProperty); }
        //    set { this.SetProperty(DeviceStatusProperty, value); }
        //}
        //#endregion

        //#region 热处理区域 HeatTreatment
        ///// <summary>
        ///// 热处理
        ///// </summary>
        //[Label("热处理")]
        //public static readonly Property<string> HeatTreatmentProperty = P<RegionBoard>.Register(e => e.HeatTreatment);

        ///// <summary>
        ///// 热处理
        ///// </summary>
        //public string HeatTreatment
        //{
        //    get { return this.GetProperty(HeatTreatmentProperty); }
        //    set { this.SetProperty(HeatTreatmentProperty, value); }
        //}
        //#endregion

        //#region 生产产出区域 ProductionOutput
        ///// <summary>
        ///// 生产产出
        ///// </summary>
        //[Label("生产产出")]
        //public static readonly Property<string> ProductionOutputProperty = P<RegionBoard>.Register(e => e.ProductionOutput);

        ///// <summary>
        ///// 生产产出
        ///// </summary>
        //public string ProductionOutput
        //{
        //    get { return this.GetProperty(ProductionOutputProperty); }
        //    set { this.SetProperty(ProductionOutputProperty, value); }
        //}
        //#endregion

        #region 产线明细 RegionBoardDetailList
        /// <summary>
        /// 产线明细
        /// </summary>
        [Label("产线明细")]
        public static readonly ListProperty<EntityList<RegionBoardDetail>> RegionBoardDetailListProperty = P<RegionBoard>.RegisterList(e => e.RegionBoardDetailList);

        /// <summary>
        /// 产线明细
        /// </summary>
        public EntityList<RegionBoardDetail> RegionBoardDetailList
        {
            get { return this.GetLazyList(RegionBoardDetailListProperty); }
        }
        #endregion

        #region MRP控制者 RegionBoardMRBListProperty
        /// <summary>
        /// MRB控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly ListProperty<EntityList<RegionBoardMRB>> RegionBoardMRBListProperty = P<RegionBoard>.RegisterList(e => e.RegionBoardMRBList);

        /// <summary>
        /// 产线明细
        /// </summary>
        public EntityList<RegionBoardMRB> RegionBoardMRBList
        {
            get { return this.GetLazyList(RegionBoardMRBListProperty); }
        }
        #endregion

    }

    internal class RegionBoardConfig : EntityConfig<RegionBoard>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(RegionBoard.RegionProperty, new NotDuplicateRule());
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("REGION_BOARD").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
