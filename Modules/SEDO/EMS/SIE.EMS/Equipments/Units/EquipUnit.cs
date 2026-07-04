using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Units
{
    /// <summary>
	/// 设备单元
	/// </summary>
	[RootEntity, Serializable]
    [CriteriaQuery]
    [Label("设备单元")]
    [DisplayMember(nameof(EquipUnit.Code))]
    public partial class EquipUnit : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<EquipUnit>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<EquipUnit>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备单元 实体配置
    /// </summary>
    internal class EquipUnitConfig : EntityConfig<EquipUnit>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.SupportTree();
            Meta.MapTable("EMS_EQUIP_UNIT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
