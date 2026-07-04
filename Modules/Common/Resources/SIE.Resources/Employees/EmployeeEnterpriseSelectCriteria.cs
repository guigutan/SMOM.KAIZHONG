using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Employees
{

    /// <summary>
    /// 生产资源组选择生产资源查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("生产资源组选择生产资源查询实体")]
    public class EmployeeEnterpriseSelectCriteria : Criteria
    {

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<EmployeeEnterpriseSelectCriteria>.Register(e => e.Code);

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
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<EmployeeEnterpriseSelectCriteria>.Register(e => e.Name);

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
        /// 查询方法
        /// </summary>
        /// <returns>资源组列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EmployeeEnterpriseSelectController>().GetEmployeeEnterprise(this);
        }
    }
}
