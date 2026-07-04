using SIE.Domain;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Common.Users;
using SIE.ObjectModel;

namespace SIE.Wpf.Resources.Employees.ViewModels
{
    /// <summary>
    /// 关联账户的弹框界面的查询实体
    /// </summary>
    [QueryEntity]
    [Label("关联账号")]
    public class EmployeeLinkUserlCriteria : Criteria
    {
        #region 构造函数
        /// <summary>
        /// 构造方法
        /// </summary>
        public EmployeeLinkUserlCriteria()
        {
            base.PagingInfo = null;
        }
        #endregion

        #region 代码 Code
        /// <summary>
        /// 代码
        /// </summary>
        [Label("账号")]
        public static readonly Property<string> CodeProperty = P<EmployeeLinkUserlCriteria>.Register(e => e.Code);

        /// <summary>
        /// 代码
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
		[Label("姓名")]
        public static readonly Property<string> NameProperty = P<EmployeeLinkUserlCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }

        #endregion

        /// <summary>
        /// 排除用户ID
        /// </summary>
        private IEnumerable<double> ExclusionUserIds
        {
            get { return new List<double>(); }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns>实体列表</returns>
        protected override EntityList Fetch()
        {
            //根据编码和名称去查用户表，返回用户列表
            var users = RT.Service.Resolve<EmployeeController>().GetUsersByCodeOrName(Code, Name);

            //筛选出不关联员工的账户
            var data = users.OfType<User>().Where(p => !ExclusionUserIds.Contains((double)p.GetId()) && p.State == State.Enable).ToList();

            //创建实体
            var result = Activator.CreateInstance(users.GetType()) as EntityList;
            result.AddRange(data);
            return result;
        }
    }

    /// <summary>
    /// EmployeeLinkUserlCriteria视图配置
    /// </summary>
    class EmployeeLinkUserViewModelViewConfig : WPFViewConfig<EmployeeLinkUserlCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
            }
        }
    }
}
