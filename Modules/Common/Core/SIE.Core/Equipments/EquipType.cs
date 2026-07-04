using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Equipments
{
    /// <summary>
    /// 设备类型
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(TypeCode))]
    [Label("设备类型")]
    public partial class EquipType : DataEntity
    {
        /// <summary>
        /// 设备类别快码组
        /// </summary>
        public const string EquipTypeCatalogType = "EQUIP_TYPE";

        #region 类型编码 TypeCode 
        /// <summary>
        /// 类型编码
        /// </summary>
        [Label("类型编码")]
        [Required]
        public static readonly Property<string> TypeCodeProperty = P<EquipType>.Register(e => e.TypeCode);
        /// <summary>
        /// 类型编码
        /// </summary>

        public string TypeCode
        {
            get { return GetProperty(TypeCodeProperty); }
            set { SetProperty(TypeCodeProperty, value); }
        }

        #endregion

        #region 类型名称 TypeName

        /// <summary>
        /// 类型名称
        /// </summary>
        [Label("类型名称")]
        [Required]
        public static readonly Property<string> TypeNameProperty = P<EquipType>.Register(e => e.TypeName);
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName
        {
            get { return GetProperty(TypeNameProperty); }
            set { SetProperty(TypeNameProperty, value); }
        }

        #endregion

        #region 台账数量 Num
        /// <summary>
        /// 台账数量
        /// </summary>
        [Label("台账数量")]
        public static readonly Property<int> NumProperty = P<EquipType>.Register(e => e.Num);

        /// <summary>
        /// 台账数量
        /// </summary>
        public int Num
        {
            get { return GetProperty(NumProperty); }
            set { SetProperty(NumProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备类型 实体配置
    /// </summary>
    internal class EquipTypeConfig : EntityConfig<EquipType>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_TYPE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}