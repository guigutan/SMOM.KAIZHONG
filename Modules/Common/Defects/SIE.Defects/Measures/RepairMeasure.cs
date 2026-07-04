using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Defects.Measures
{
    /// <summary>
    /// 维修措施
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("维修措施")]
    [DisplayMember(nameof(Name))]
    public class RepairMeasure : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(20)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<RepairMeasure>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
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
        public static readonly Property<string> NameProperty = P<RepairMeasure>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        [MaxLength(1000)]
        public static readonly Property<string> DescriptionProperty = P<RepairMeasure>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return this.GetProperty(DescriptionProperty); }
            set { this.SetProperty(DescriptionProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 维修措施 实体配置
    /// </summary>
    internal class ReapirMeasureConfig : EntityConfig<RepairMeasure>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_REP_MEA").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}