using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.SmomControl
{
    /// <summary>
    /// SMOM总控配置
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("SMOM总控配置")]
    [DisplayMember(nameof(FactoryName)), QueryMembers(new string[] { nameof(FactoryName), nameof(FactoryCode) })]
    public class SmomControlSetting: DataEntity
    {
        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<SmomControlSetting>.Register(e => e.FactoryName);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
            set { this.SetProperty(FactoryNameProperty, value); }
        }
        #endregion

        #region 工厂编码 FactoryCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<SmomControlSetting>.Register(e => e.FactoryCode);

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode
        {
            get { return this.GetProperty(FactoryCodeProperty); }
            set { this.SetProperty(FactoryCodeProperty, value); }
        }
        #endregion

        #region 工厂Url FactoryUrl
        /// <summary>
        /// 工厂Url
        /// </summary>
        [Required]
        [Label("工厂Url")]
        public static readonly Property<string> FactoryUrlProperty = P<SmomControlSetting>.Register(e => e.FactoryUrl);

        /// <summary>
        /// 工厂Url
        /// </summary>
        public string FactoryUrl
        {
            get { return this.GetProperty(FactoryUrlProperty); }
            set { this.SetProperty(FactoryUrlProperty, value); }
        }
        #endregion

        #region 主数据是否交互 IsMain
        /// <summary>
        /// 主数据是否交互
        /// </summary>
        [Label("主数据是否交互")]
        public static readonly Property<YesNo> IsMainProperty = P<SmomControlSetting>.Register(e => e.IsMain);

        /// <summary>
        /// 主数据是否交互
        /// </summary>
        public YesNo IsMain
        {
            get { return this.GetProperty(IsMainProperty); }
            set { this.SetProperty(IsMainProperty, value); }
        }
        #endregion

        #region Qrqc是否交互 IsQrqc
        /// <summary>
        /// Qrqc是否交互
        /// </summary>
        [Label("Qrqc是否交互")]
        public static readonly Property<YesNo> IsQrqcProperty = P<SmomControlSetting>.Register(e => e.IsQrqc);

        /// <summary>
        /// Qrqc是否交互
        /// </summary>
        public YesNo IsQrqc
        {
            get { return this.GetProperty(IsQrqcProperty); }
            set { this.SetProperty(IsQrqcProperty, value); }
        }
        #endregion

        #region 工厂明细 FactoryDetail
        /// <summary>
        /// 工厂明细
        /// </summary>
        [Label("工厂明细")]
        public static readonly ListProperty<EntityList<FactoryDetail>> FactoryDetailProperty = P<SmomControlSetting>.RegisterList(e => e.FactoryDetail);

        /// <summary>
        /// 工厂明细
        /// </summary>
        public EntityList<FactoryDetail> FactoryDetail
        {
            get { return this.GetLazyList(FactoryDetailProperty); }
        }
        #endregion

        #region 类型参数 TypeParamDetail
        /// <summary>
        /// 类型参数
        /// </summary>
        [Label("类型参数")]
        public static readonly ListProperty<EntityList<TypeParamDetail>> TypeParamDetailProperty = P<SmomControlSetting>.RegisterList(e => e.TypeParamDetail);

        /// <summary>
        /// 类型参数
        /// </summary>
        public EntityList<TypeParamDetail> TypeParamDetail
        {
            get { return this.GetLazyList(TypeParamDetailProperty); }
        }
        #endregion

    }

    /// <summary>
    /// SMOM总控配置 实体配置
    /// </summary>
    internal class SmomControlSettingConfig : EntityConfig<SmomControlSetting>
    {

        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(SmomControlSetting.FactoryCodeProperty, new RequiredRule());
            rules.AddRule(SmomControlSetting.FactoryCodeProperty, new NotDuplicateRule());
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("SMOM_CONTROL_SET").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

}
