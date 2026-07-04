using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常信息分类
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("异常信息分类")]
    [DisplayMember(nameof(Code))]
    public partial class AbnormalInfoCategory : DataEntity
    {
        #region 异常分类编码 Code
        /// <summary>
        /// 异常分类编码
        /// </summary>
        [Label("异常分类编码")]
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        public static readonly Property<string> CodeProperty = P<AbnormalInfoCategory>.Register(e => e.Code);

        /// <summary>
        /// 异常分类编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 异常分类描述 Desc
        /// <summary>
        /// 异常分类描述
        /// </summary>
        [Label("异常分类描述")]
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        public static readonly Property<string> DescProperty = P<AbnormalInfoCategory>.Register(e => e.Desc);

        /// <summary>
        /// 异常分类描述
        /// </summary>
        public string Desc
        {
            get { return GetProperty(DescProperty); }
            set { SetProperty(DescProperty, value); }
        }
        #endregion

        #region 推送升级设置 SendUpgradeSetList
        /// <summary>
        /// 推送升级设置
        /// </summary>
        [Label("推送升级设置")]
        public static readonly ListProperty<EntityList<SenderUpgradeSettings>> SendUpgradeSetListProperty = P<AbnormalInfoCategory>.RegisterList(e => e.SendUpgradeSetList);

        /// <summary>
        /// 推送升级设置
        /// </summary>
        public EntityList<SenderUpgradeSettings> SendUpgradeSetList
        {
            get { return GetLazyList(SendUpgradeSetListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 异常信息分类 实体配置
    /// </summary>
    internal class AbnormalInfoCategoryConfig : EntityConfig<AbnormalInfoCategory>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QMS_AbnormalInfoCat").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
