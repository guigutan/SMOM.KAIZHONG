using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常信息定义
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("异常信息定义")]
    [DisplayMember(nameof(Code))]
    public partial class AbnormalInfoDefinition : DataEntity
    {
        /// <summary>
        /// 缺陷等级快码
        /// </summary>
        public const string LevelCatalog = "DEFECT_LEVEL";

        #region 异常来源 AbnormalSource
        /// <summary>
        /// 异常来源
        /// </summary>
        [Label("异常来源")]
        public static readonly Property<AbnormalSource> AbnormalSourceProperty = P<AbnormalInfoDefinition>.Register(e => e.AbnormalSource);

        /// <summary>
        /// 异常来源
        /// </summary>
        public AbnormalSource AbnormalSource
        {
            get { return this.GetProperty(AbnormalSourceProperty); }
            set { this.SetProperty(AbnormalSourceProperty, value); }
        }
        #endregion

        #region 异常编码 Code
        /// <summary>
        /// 异常编码
        /// </summary>
        [Label("异常编码")]
        public static readonly Property<string> CodeProperty = P<AbnormalInfoDefinition>.Register(e => e.Code);

        /// <summary>
        /// 异常编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 异常描述 Desc
        /// <summary>
        /// 异常描述
        /// </summary>
        [Label("异常描述")]
        public static readonly Property<string> DescProperty = P<AbnormalInfoDefinition>.Register(e => e.Desc);

        /// <summary>
        /// 异常描述
        /// </summary>
        public string Desc
        {
            get { return this.GetProperty(DescProperty); }
            set { this.SetProperty(DescProperty, value); }
        }
        #endregion

        #region 预警配置id AlerterId
        /// <summary>
        /// 预警配置id
        /// </summary>
        [Label("预警配置id")]
        public static readonly Property<double?> AlerterIdProperty = P<AbnormalInfoDefinition>.Register(e => e.AlerterId);

        /// <summary>
        /// 预警配置id
        /// </summary>
        public double? AlerterId
        {
            get { return this.GetProperty(AlerterIdProperty); }
            set { this.SetProperty(AlerterIdProperty, value); }
        }
        #endregion

        #region 异常信息分类 AbnormalCategory
        /// <summary>
        /// 异常信息分类Id
        /// </summary>
        [Label("异常信息分类")]
        public static readonly IRefIdProperty AbnormalCategoryIdProperty =
            P<AbnormalInfoDefinition>.RegisterRefId(e => e.AbnormalCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 异常信息分类Id
        /// </summary>
        public double AbnormalCategoryId
        {
            get { return (double)this.GetRefId(AbnormalCategoryIdProperty); }
            set { this.SetRefId(AbnormalCategoryIdProperty, value); }
        }

        /// <summary>
        /// 异常信息分类
        /// </summary>
        public static readonly RefEntityProperty<AbnormalInfoCategory> AbnormalCategoryProperty =
            P<AbnormalInfoDefinition>.RegisterRef(e => e.AbnormalCategory, AbnormalCategoryIdProperty);

        /// <summary>
        /// 异常信息分类
        /// </summary>
        public AbnormalInfoCategory AbnormalCategory
        {
            get { return this.GetRefEntity(AbnormalCategoryProperty); }
            set { this.SetRefEntity(AbnormalCategoryProperty, value); }
        }
        #endregion

        #region 异常分类描述 AbnormalCategoryDesc
        /// <summary>
        /// 异常分类描述
        /// </summary>
        [Label("异常分类描述")]
        public static readonly Property<string> AbnormalCategoryDescProperty = P<AbnormalInfoDefinition>.RegisterView(e => e.AbnormalCategoryDesc, p => p.AbnormalCategory.Desc);

        /// <summary>
        /// 异常分类描述
        /// </summary>
        public string AbnormalCategoryDesc
        {
            get { return this.GetProperty(AbnormalCategoryDescProperty); }
        }
        #endregion

        //#region 缺陷等级 DefectLevel
        ///// <summary>
        ///// 缺陷等级
        ///// </summary>
        //[Required]
        //[Label("缺陷等级")]
        //public static readonly Property<string> DefectLevelProperty = P<AbnormalInfoDefinition>.Register(e => e.DefectLevel);

        ///// <summary>
        ///// 缺陷等级
        ///// </summary>
        //public string DefectLevel
        //{
        //    get { return GetProperty(DefectLevelProperty); }
        //    set { SetProperty(DefectLevelProperty, value); }
        //}
        //#endregion

        #region 推送升级设置 SendUpgradeSetList
        /// <summary>
        /// 推送升级设置
        /// </summary>
        [Label("推送升级设置")]
        public static readonly ListProperty<EntityList<DefinitionSenderSettings>> SendUpgradeSetListProperty = P<AbnormalInfoDefinition>.RegisterList(e => e.SendUpgradeSetList);

        /// <summary>
        /// 推送升级设置
        /// </summary>
        public EntityList<DefinitionSenderSettings> SendUpgradeSetList
        {
            get { return this.GetLazyList(SendUpgradeSetListProperty); }
        }
        #endregion

        #region 异常处理人 HandlerList
        /// <summary>
        /// 异常处理人
        /// </summary>
        [Label("异常处理人")]
        public static readonly ListProperty<EntityList<AbnormalInfoDefinitionEmployee>> HandlerListProperty = P<AbnormalInfoDefinition>.RegisterList(e => e.HandlerList);
        /// <summary>
        /// 异常处理人
        /// </summary>
        public EntityList<AbnormalInfoDefinitionEmployee> HandlerList
        {
            get { return this.GetLazyList(HandlerListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 异常信息定义 实体配置
    /// </summary>
    internal class AbnormalInfoDefinitionConfig : EntityConfig<AbnormalInfoDefinition>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QMS_AbnormalInfoDef").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
