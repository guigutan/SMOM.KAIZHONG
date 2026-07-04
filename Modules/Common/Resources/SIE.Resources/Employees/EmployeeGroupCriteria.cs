using SIE.Domain;
using System;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工组查询
    /// </summary>
    [QueryEntity, Serializable]
    public partial class EmployeeGroupCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        public static readonly Property<string> CodeProperty = P<EmployeeGroupCriteria>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<EmployeeGroupCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 执行查询操作
        /// </summary>
        /// <returns>EntityList</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EmployeeController>().GetEmployeeGroups(this);
        }
    }
}
