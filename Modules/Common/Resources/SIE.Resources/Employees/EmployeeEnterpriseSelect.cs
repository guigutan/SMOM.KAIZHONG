using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 工厂视图 （只为做选择视图）
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EmployeeEnterpriseSelectCriteria))]
    [Label("工厂")]
    [DisplayMember(nameof(Name))]
    public partial class EmployeeEnterpriseSelect : Enterprise
    {
    }

    /// <summary>
    /// 工厂实体配置
    /// </summary>
    internal class WipResourceSelectConfig : EntityConfig<EmployeeEnterpriseSelect>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_ENTERPRISE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}