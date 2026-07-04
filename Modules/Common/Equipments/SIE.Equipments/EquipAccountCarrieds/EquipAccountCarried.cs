using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.Equipments.EquipAccountCarrieds
{
    /// <summary>
    /// 生产资源载位信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产资源载位信息")]
    [CriteriaQuery]
    [DisplayMember(nameof(Code))]
    public class EquipAccountCarried : DataEntity
    {
        #region 生产资源 WipResouce
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty WipResouceIdProperty =
            P<EquipAccountCarried>.RegisterRefId(e => e.WipResouceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double WipResouceId
        {
            get { return (double)this.GetRefId(WipResouceIdProperty); }
            set { this.SetRefId(WipResouceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResouceProperty =
            P<EquipAccountCarried>.RegisterRef(e => e.WipResouce, WipResouceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource WipResouce
        {
            get { return this.GetRefEntity(WipResouceProperty); }
            set { this.SetRefEntity(WipResouceProperty, value); }
        }
        #endregion


        #region 载位编码 Code
        /// <summary>
        /// 载位编码
        /// </summary>
        [Label("载位编码")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> CodeProperty
            = P<EquipAccountCarried>.Register(e => e.Code);

        /// <summary>
        /// 载位编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 载位类型 CarriedType
        /// <summary>
        /// 载位类型
        /// </summary>
        [Label("载位类型")]
        public static readonly Property<CarriedType> CarriedTypeProperty
            = P<EquipAccountCarried>.Register(e => e.CarriedType);

        /// <summary>
        /// 载位类型
        /// </summary>
        public CarriedType CarriedType
        {
            get { return this.GetProperty(CarriedTypeProperty); }
            set { this.SetProperty(CarriedTypeProperty, value); }
        }
        #endregion

        #region 当前载具 CurCarried
        /// <summary>
        /// 当前载具
        /// </summary>
        [Label("当前载具")]
        public static readonly Property<string> CurCarriedProperty
            = P<EquipAccountCarried>.Register(e => e.CurCarried);

        /// <summary>
        /// 当前载具
        /// </summary>
        public string CurCarried
        {
            get { return this.GetProperty(CurCarriedProperty); }
            set { this.SetProperty(CurCarriedProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 生产资源 WipResouceName
        /// <summary>
        /// 生产资源
        /// </summary>
        [Label("生产资源名称")]
        public static readonly Property<string> WipResouceNameProperty
            = P<EquipAccountCarried>.RegisterView(e => e.WipResouceName, p => p.WipResouce.Name);

        /// <summary>
        /// 生产资源
        /// </summary>
        public string WipResouceName
        {
            get { return this.GetProperty(WipResouceNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class EquipAccountCarriedConfig : EntityConfig<EquipAccountCarried>
    {
        /// <summary>
        /// 实体元配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_CARRIED").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }


}
