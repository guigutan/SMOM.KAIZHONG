using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Defects.InspectionItems
{
    /// <summary>
    /// 检验方式
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("检验方式")]
    [DisplayMember(nameof(Name))]
    public class InspectionMode : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        [MaxLength(80)]
        public static readonly Property<string> CodeProperty = P<InspectionMode>.Register(e => e.Code);

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
        [MaxLength(80)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<InspectionMode>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 检验方式配置
    /// </summary>
    internal class InspectionModeConfig : EntityConfig<InspectionMode>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);

            //测试要求添加时检验类型默认为空，添加检验类型null验证规则
        }

        /// <summary>
        /// 数据课检验方式表名
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BD_INSP_MODE").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}