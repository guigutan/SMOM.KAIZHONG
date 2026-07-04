using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccountLocations;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipAccounts.TabBases
{
    /// <summary>
    /// 台账位置基类
    /// </summary>
    [RootEntity, Serializable]
    [Label("台账位置基类")]
    public class LocationBase : DataEntity
    {
        #region 分区 Subarea
        /// <summary>
        /// 分区
        /// </summary>
        [Label("分区")]
        public static readonly Property<string> SubareaProperty = P<LocationBase>.Register(e => e.Subarea);

        /// <summary>
        /// 分区
        /// </summary>
        public string Subarea
        {
            get { return GetProperty(SubareaProperty); }
            set { SetProperty(SubareaProperty, value); }
        }
        #endregion

        #region 大站位 BigStance
        /// <summary>
        /// 大站位
        /// </summary>
        [Required]
        [Label("大站位")]
        public static readonly Property<string> BigStanceProperty = P<LocationBase>.Register(e => e.BigStance);

        /// <summary>
        /// 大站位
        /// </summary>
        public string BigStance
        {
            get { return GetProperty(BigStanceProperty); }
            set { SetProperty(BigStanceProperty, value); }
        }
        #endregion

        #region 站位 Stance
        /// <summary>
        /// 站位
        /// </summary>
        [Label("站位")]
        public static readonly Property<string> StanceProperty = P<LocationBase>.Register(e => e.Stance);

        /// <summary>
        /// 站位
        /// </summary>
        public string Stance
        {
            get { return GetProperty(StanceProperty); }
            set { SetProperty(StanceProperty, value); }
        }
        #endregion

        #region 站位类型 StanceType
        /// <summary>
        /// 站位类型
        /// </summary>
        [Label("站位类型")]
        public static readonly Property<StanceType> StanceTypeProperty = P<LocationBase>.Register(e => e.StanceType);

        /// <summary>
        /// 站位类型
        /// </summary>
        public StanceType StanceType
        {
            get { return GetProperty(StanceTypeProperty); }
            set { SetProperty(StanceTypeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public class LocationBaseConfig : EntityConfig<LocationBase>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_ACCOUNT_LOC").MapAllProperties();
            Meta.EnableSort();
            Meta.EnablePhantoms();
        }
    }
}
