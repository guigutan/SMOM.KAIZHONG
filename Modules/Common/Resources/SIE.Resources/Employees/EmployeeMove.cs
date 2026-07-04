using SIE.Domain;
using SIE.Domain.Query;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 过站人员
    /// </summary>
    [RootEntity, Serializable]
    [Label("过站人员")]
    public partial class EmployeeMove : Entity<double>
    {
        #region 工号 Code
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> CodeProperty = P<EmployeeMove>.Register(e => e.Code);

        /// <summary>
        /// 工号
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
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<EmployeeMove>.Register(e => e.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 过站人员 实体配置
    /// </summary>
    internal class EmployeeMoveConfig : EntityConfig<EmployeeMove>
    {
        /// <summary>
        /// 实体元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<Employee>().Select((a) => new
            {
                a.Id,
                a.Code,
                a.Name
            }).ToQuery();
            Meta.MapView(view).MapAllProperties();
        }
    }
}
