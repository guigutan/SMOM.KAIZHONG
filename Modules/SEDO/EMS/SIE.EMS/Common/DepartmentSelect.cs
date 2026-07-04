using SIE.Domain;
using SIE.Domain.Query;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Common
{
    /// <summary>
    /// 部门
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("部门")]
    [DisplayMember(nameof(Name))]
    public partial class DepartmentSelect : DataEntity
    {
        #region 库存组织ID InvOrgId
        /// <summary>
        /// 库存组织ID
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int?> InvOrgIdProperty = P<DepartmentSelect>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织ID
        /// </summary>
        public int? InvOrgId
        {
            get { return GetProperty(InvOrgIdProperty); }
            set { SetProperty(InvOrgIdProperty, value); }
        }
        #endregion

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<DepartmentSelect>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<DepartmentSelect>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region ERP存储的组织ID ErpOrgId
        /// <summary>
        /// ERP存储的组织ID
        /// </summary>
        [Label("ERP存储的组织ID")]
        public static readonly Property<int> ErpOrgIdProperty = P<DepartmentSelect>.Register(e => e.ErpOrgId);

        /// <summary>
        /// ERP存储的组织ID
        /// </summary>
        public int ErpOrgId
        {
            get { return GetProperty(ErpOrgIdProperty); }
            set { SetProperty(ErpOrgIdProperty, value); }
        }
        #endregion

        #region 是否资源 IsResource
        /// <summary>
        /// 是否资源
        /// </summary>
        [Label("是否资源")]
        public static readonly Property<bool> IsResourceProperty = P<DepartmentSelect>.Register(e => e.IsResource);

        /// <summary>
        /// 是否资源
        /// </summary>
        public bool IsResource
        {
            get { return GetProperty(IsResourceProperty); }
            set { SetProperty(IsResourceProperty, value); }
        }
        #endregion

        #region 是否手工录入 IsByHand
        /// <summary>
        /// 是否手工录入
        /// </summary>
        [Label("是否手工录入")]
        public static readonly Property<YesNo> IsByHandProperty = P<DepartmentSelect>.Register(e => e.IsByHand);

        /// <summary>
        /// 是否手工录入
        /// </summary>
        public YesNo IsByHand
        {
            get { return GetProperty(IsByHandProperty); }
            set { SetProperty(IsByHandProperty, value); }
        }
        #endregion

        #region 企业层级 Level
        /// <summary>
        /// 企业层级Id
        /// </summary>
        [Label("层级")]
        public static readonly IRefIdProperty LevelIdProperty = P<DepartmentSelect>.RegisterRefId(e => e.LevelId, ReferenceType.Normal);

        /// <summary>
        /// 企业层级Id
        /// </summary>
        public double LevelId
        {
            get { return (double)GetRefId(LevelIdProperty); }
            set { SetRefId(LevelIdProperty, value); }
        }

        /// <summary>
        /// 企业层级
        /// </summary>
        [Label("层级")]
        public static readonly RefEntityProperty<EnterpriseLevel> LevelProperty = P<DepartmentSelect>.RegisterRef(e => e.Level, LevelIdProperty);

        /// <summary>
        /// 企业层级
        /// </summary>
        public EnterpriseLevel Level
        {
            get { return GetRefEntity(LevelProperty); }
            set { SetRefEntity(LevelProperty, value); }
        }
        #endregion

        #region 企业类型 LevelType
        /// <summary>
        /// 企业类型
        /// </summary>
        [Label("企业类型")]
        public static readonly Property<EnterpriseType> LevelTypeProperty = P<DepartmentSelect>.RegisterView(e => e.LevelType, p => p.Level.Type);

        /// <summary>
        /// 企业类型
        /// </summary>
        public EnterpriseType LevelType
        {
            get { return this.GetProperty(LevelTypeProperty); }
            set { this.SetProperty(LevelTypeProperty, value); }
        }
        #endregion

        #region 是否资源 LevelIsResource
        /// <summary>
        /// 是否资源
        /// </summary>
        [Label("是否资源")]
        public static readonly Property<bool> LevelIsResourceProperty = P<DepartmentSelect>.RegisterView(e => e.LevelIsResource, p => p.Level.IsResource);

        /// <summary>
        /// 是否资源
        /// </summary>
        public bool LevelIsResource
        {
            get { return this.GetProperty(LevelIsResourceProperty); }
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class DepartmentSelectConfig : EntityConfig<DepartmentSelect>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<Enterprise>()
                .Where(p => p.InvOrgId == RT.InvOrg || p.InvOrgId == 0)
                .Exists<EnterpriseLevel>((x, y) => y.Where(z => z.Id == x.LevelId && z.Type == EnterpriseType.Department))
                .ToQuery();
            
            Meta.MapView(view).MapAllProperties();

            Meta.DisableInvOrg();            
            Meta.EnablePhantoms();
        }
    }
}