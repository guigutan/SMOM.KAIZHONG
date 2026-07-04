using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工视图 （只为做选择视图）
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EmployeeSelectCriteria))]
    [Label("员工")]
    [DisplayMember(nameof(Name))]
    public partial class EmployeeSelect : Employee
    {

    }

    /// <summary>
    /// 工厂实体配置
    /// </summary>
    internal class EmployeeSelectConfig : EntityConfig<EmployeeSelect>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_EMP").MapAllProperties();
            Meta.Property(Employee.UserIdProperty).MapColumn().IgnoreFK();
            Meta.Property(Employee.SexProperty).MapColumn().IsNullable();//SIE.Resources.Employee没有此字段，数据库需要可空
            Meta.Property(Employee.RemarkProperty).MapColumn().HasLength(1200);
            Meta.DisableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}