using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工组
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("员工组")]
    [DisplayMember(nameof(Name))]
    public partial class EmployeeGroup : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<EmployeeGroup>.Register(e => e.Code);

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
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<EmployeeGroup>.Register(e => e.Name);

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
    ///  实体配置
    /// </summary>
    internal class EmployeeGroupConfig : EntityConfig<EmployeeGroup>
    {
        /// <summary>
        /// 实体数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("RES_EMP_GROUP").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.DisableInvOrg();
        }
    }
}