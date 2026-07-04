using SIE.Common.Import;
using SIE.Domain.Validation;
using SIE.EMS.DevicePurs;
using SIE.EMS.EarlierStage.Budgets;
using SIE.Equipments.Enums;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EarlierStage.Budgets.Commands
{
    /// <summary>
    /// 导入预算
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Budgets.Commands.BudgetImportCommand")]
    public class BudgetImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 用户与预算部门关系
        /// </summary>
        private const string ConstrantDeviceBudgetDepartment = "DeviceBudgetDepartment";

        /// <summary>
        /// 员工与工厂关系
        /// </summary>
        private const string ConstrantEmployeeEnterprise = "EmployeeEnterprise";

        /// <summary>
        /// 员工与工厂关系
        /// </summary>
        private const string ConstrantDepartmentAndFactory = "DepartmentAndFactory";

        /// <summary>
        /// 行数据读取, 默认按视图配置把数据读取后，还可以进行自定义处理
        /// </summary>
        /// <param name="data">行数据</param>
        /// <param name="cache">缓存数据</param>
        protected override void OnRowDataRead(RowData data, CacheData cache)
        {
            base.OnRowDataRead(data, cache);

            var budget = data.Entity as Budget;
            if (budget.Factory == null)
                throw new ValidationException("第{0}行的工厂不能为空".L10nFormat(data.RowIndex));

            //验证工厂与部门的上下级关系          
            if (!cache.CustomData.Contains(ConstrantDepartmentAndFactory))
            {
                //Key 是 "部门ID,工厂ID"
                cache.CustomData.Add(ConstrantDepartmentAndFactory, new Dictionary<string, bool>());
            }

            var departmentAndFactoryDictionary = cache.CustomData[ConstrantDepartmentAndFactory]
                as Dictionary<string, bool>;

            string key = string.Format("{0},{1}", budget.DepartmentId, budget.FactoryId);

            if (!departmentAndFactoryDictionary.ContainsKey(key))
            {
                if (budget.Factory.Level.Type != EnterpriseType.Plant)
                {
                    throw new ValidationException("第{0}行的工厂【{1}】的企业层不是的【工厂】"
                        .L10nFormat(data.RowIndex, budget.Factory.Name));
                }

                if (budget.Department != null && budget.Department.Level.Type != EnterpriseType.Department)
                {
                    throw new ValidationException("第{0}行的部门【{1}】的企业层不是的【部门】"
                        .L10nFormat(data.RowIndex, budget.Department.Name));
                }

                if (budget.Department != null && budget.Department.TreePId != budget.FactoryId)
                {
                    throw new ValidationException("第{0}行的部门【{1}】不属于工厂【{2}】"
                        .L10nFormat(data.RowIndex, budget.Department.Name, budget.Factory.Name));
                }

                departmentAndFactoryDictionary.Add(key, true);
            }

            // 验证工厂是否当前操作用户有权限操作的            
            if (!cache.CustomData.Contains(ConstrantEmployeeEnterprise))
            {
                cache.CustomData.Add(ConstrantEmployeeEnterprise, new Dictionary<double, bool>());
            }

            var employeeEnterpriseDictionary = cache.CustomData[ConstrantEmployeeEnterprise]
                as Dictionary<double, bool>;

            if (!employeeEnterpriseDictionary.ContainsKey(budget.FactoryId))
            {
                var isEmployeeHasEnterprise = RT.Service.Resolve<EmployeeEnterpriseSelectController>()
                    .EmployeeHasEnterprise(RT.IdentityId, budget.FactoryId);

                if (!isEmployeeHasEnterprise)
                {
                    throw new ValidationException("您没有第{0}行的工厂【{1}】的权限"
                        .L10nFormat(data.RowIndex, budget.Factory.Name));
                }
                else
                {
                    employeeEnterpriseDictionary.Add(budget.FactoryId, true);
                }
            }

            // 验证部门是否当前操作用户有权限操作
            //if (budget.DepartmentId != null)
            //{
                if (!cache.CustomData.Contains(ConstrantDeviceBudgetDepartment))
                {
                    cache.CustomData.Add(ConstrantDeviceBudgetDepartment, new Dictionary<double, bool>());
                }

                var deviceBudgetDepartmentDictionary = cache.CustomData[ConstrantDeviceBudgetDepartment]
                    as Dictionary<double, bool>;

                if (!deviceBudgetDepartmentDictionary.ContainsKey(budget.DepartmentId))
                {
                    var deviceBudgetDepartment = RT.Service.Resolve<DevicePurController>()
                        .GetBudgetDepartment(RT.Identity.UserId, budget.DepartmentId);

                    if (deviceBudgetDepartment == null)
                    {
                        throw new ValidationException("您没有第{0}行的部门【{1}】的预算权限"
                            .L10nFormat(data.RowIndex, budget.Department.Name));
                    }
                    else
                    {
                        deviceBudgetDepartmentDictionary.Add(budget.DepartmentId, true);
                    }
                }
            //}

            //当前年
            var currentYear = DateTime.Now.Year;

            if (budget.BudgetYear < currentYear)
            {
                throw new ValidationException("第{0}行的年度小于当前年份".L10nFormat(data.RowIndex));
            }

            //年度是对应年份的1月1日
            budget.Year = new DateTime(budget.BudgetYear, 1, 1);

            //导入的表格没有这个字段，生成数据时默认为【待提交】
            budget.ApprovalStatus = ApprovalStatus.Draft;
        }
    }
}
