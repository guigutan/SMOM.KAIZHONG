using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料原因
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("叫料原因")]
    [DisplayMember(nameof(Name))]
    public partial class CallMaterialReason : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(20)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<CallMaterialReason>.Register(e => e.Code);

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
        [NotDuplicate]
        [MaxLength(20)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<CallMaterialReason>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(100)]
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<CallMaterialReason>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 叫料原因 实体配置
    /// </summary>
    internal class CallMaterialReasonConfig : EntityConfig<CallMaterialReason>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_CALL_REASON").MapAllProperties();
            Meta.Property(CallMaterialReason.DescriptionProperty).ColumnMeta.HasLength(2400);
            Meta.EnablePhantoms();
        }
    }
}