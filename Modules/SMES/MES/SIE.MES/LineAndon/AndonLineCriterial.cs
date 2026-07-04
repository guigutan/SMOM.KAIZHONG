using SIE.Domain;
using SIE.MES.Andon;
using SIE.MES.EmpWork;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WorkCenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.LineAndon
{
    /// <summary>
    /// 产线与安灯区域查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("产线与安灯区域查询实体")]
    public class AndonLineCriterial : Criteria
    {
        #region 产线/机台编码 MachineCode
        /// <summary>
        /// 产线/机台编码
        /// </summary>
        [Required]
        [Label("产线/机台编码")]
        public static readonly Property<string> MachineCodeProperty = P<AndonLineCriterial>.Register(e => e.MachineCode);

        /// <summary>
        /// 产线/机台编码
        /// </summary>
        public string MachineCode
        {
            get { return this.GetProperty(MachineCodeProperty); }
            set { this.SetProperty(MachineCodeProperty, value); }
        }
        #endregion

        #region 产线/机台名称 MachineName
        /// <summary>
        /// 产线/机台名称
        /// </summary>
        [Required]
        [Label("产线/机台名称")]
        public static readonly Property<string> MachineNameProperty = P<AndonLineCriterial>.Register(e => e.MachineName);

        /// <summary>
        /// 产线/机台名称
        /// </summary>
        public string MachineName
        {
            get { return this.GetProperty(MachineNameProperty); }
            set { this.SetProperty(MachineNameProperty, value); }
        }
        #endregion

        #region 主设备号 EquipmentNo
        /// <summary>
        /// 主设备号
        /// </summary>
        [Required]
        [Label("主设备号")]
        public static readonly Property<string> EquipmentNoProperty = P<AndonLineCriterial>.Register(e => e.EquipmentNo);

        /// <summary>
        /// 主设备号
        /// </summary>
        public string EquipmentNo
        {
            get { return this.GetProperty(EquipmentNoProperty); }
            set { this.SetProperty(EquipmentNoProperty, value); }
        }
        #endregion

        #region 工作中心编码 WorkCenter
        /// <summary>
        /// 工作中心编码Id
        /// </summary>
        [Label("工作中心编码")]
        public static readonly IRefIdProperty WorkCenterIdProperty = P<AndonLineCriterial>.RegisterRefId(e => e.WorkCenterId, ReferenceType.Normal);

        /// <summary>
        /// 工作中心编码Id
        /// </summary>
        public double? WorkCenterId
        {
            get { return (double?)GetRefNullableId(WorkCenterIdProperty); }
            set { SetRefNullableId(WorkCenterIdProperty, value); }
        }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public static readonly RefEntityProperty<WorkCenter> WorkCenterProperty = P<AndonLineCriterial>.RegisterRef(e => e.WorkCenter, WorkCenterIdProperty);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public WorkCenter WorkCenter
        {
            get { return GetRefEntity(WorkCenterProperty); }
            set { SetRefEntity(WorkCenterProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<AndonLineCriterial>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<AndonLineCriterial>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<AndonLineCriterial>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<AndonLineCriterial>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 区域描述 AndonUphold
        /// <summary>
        /// 区域描述Id
        /// </summary>
        [Label("区域描述")]
        public static readonly IRefIdProperty AndonUpholdIdProperty =
            P<AndonLineCriterial>.RegisterRefId(e => e.AndonUpholdId, ReferenceType.Normal);

        /// <summary>
        /// 区域描述Id
        /// </summary>
        public double? AndonUpholdId
        {
            get { return (double?)this.GetRefNullableId(AndonUpholdIdProperty); }
            set { this.SetRefNullableId(AndonUpholdIdProperty, value); }
        }

        /// <summary>
        /// 区域描述
        /// </summary>
        public static readonly RefEntityProperty<AndonUphold> AndonUpholdProperty =
            P<AndonLineCriterial>.RegisterRef(e => e.AndonUphold, AndonUpholdIdProperty);

        /// <summary>
        /// 区域描述
        /// </summary>
        public AndonUphold AndonUphold
        {
            get { return this.GetRefEntity(AndonUpholdProperty); }
            set { this.SetRefEntity(AndonUpholdProperty, value); }
        }
        #endregion

        #region 安灯编码 AndonCode
        /// <summary>
        /// 安灯编码
        /// </summary>
        [Label("安灯编码")]
        public static readonly Property<string> AndonCodeProperty = P<AndonLineCriterial>.Register(e => e.AndonCode);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonCode
        {
            get { return this.GetProperty(AndonCodeProperty); }
            set { this.SetProperty(AndonCodeProperty, value); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<AndonLineCriterial>.Register(e => e.WorkShopCode);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
            set { this.SetProperty(WorkShopCodeProperty, value); }
        }
        #endregion
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AndonLineController>().CriterialAndonLine(this);
        }
    }
}
