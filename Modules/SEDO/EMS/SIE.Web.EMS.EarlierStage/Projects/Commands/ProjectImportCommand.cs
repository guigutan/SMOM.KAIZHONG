using SIE.Common.Import;
using SIE.Domain.Validation;
using SIE.EMS.DevicePurs;
using SIE.EMS.EarlierStage.Projects;
using SIE.EMS.Projects.Enums;
using SIE.Equipments.Enums;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 导入项目
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.ProjectImportCommand")]
    public class ProjectImportCommand : ImportExcelCommand
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

            var project = data.Entity as Project;
            GetImportInfo(project);

            //验证工厂与部门的上下级关系          
            if (!cache.CustomData.Contains(ConstrantDepartmentAndFactory))
            {
                //Key 是 "部门ID,工厂ID"
                cache.CustomData.Add(ConstrantDepartmentAndFactory, new Dictionary<string, bool>());
            }

            var departmentAndFactoryDictionary = cache.CustomData[ConstrantDepartmentAndFactory]
                as Dictionary<string, bool>;

            string key = string.Format("{0},{1}", project.DepartmentId, project.FactoryId);

            if (!departmentAndFactoryDictionary.ContainsKey(key))
            {
                if (project.Factory.Level.Type != EnterpriseType.Plant)
                {
                    throw new ValidationException("工厂【{0}】的企业层级不是【工厂】".L10nFormat(project.Factory.Name));
                }

                if (project.Department != null && project.Department.Level.Type != EnterpriseType.Department)
                {
                    throw new ValidationException("部门【{0}】的企业层级不是【部门】".L10nFormat(project.Department.Name));
                }

                if (project.Department != null && project.Department.TreePId != project.FactoryId)
                {
                    throw new ValidationException("部门【{0}】不属于工厂【{1}】".L10nFormat(project.Department.Name, project.Factory.Name));
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

            if (!employeeEnterpriseDictionary.ContainsKey(project.FactoryId))
            {
                var isEmployeeHasEnterprise = RT.Service.Resolve<EmployeeEnterpriseSelectController>()
                    .EmployeeHasEnterprise(RT.IdentityId, project.FactoryId);

                if (!isEmployeeHasEnterprise)
                {
                    throw new ValidationException("您没有工厂【{0}】的权限".L10nFormat(project.Factory.Name));
                }
                else
                {
                    employeeEnterpriseDictionary.Add(project.FactoryId, true);
                }
            }

            // 验证部门是否当前操作用户有权限操作
            //if (project.DepartmentId != null)
            //{
            if (!cache.CustomData.Contains(ConstrantDeviceBudgetDepartment))
            {
                cache.CustomData.Add(ConstrantDeviceBudgetDepartment, new Dictionary<double, bool>());
            }

            var deviceBudgetDepartmentDictionary = cache.CustomData[ConstrantDeviceBudgetDepartment]
                as Dictionary<double, bool>;

            if (!deviceBudgetDepartmentDictionary.ContainsKey(project.DepartmentId))
            {
                var deviceBudgetDepartment = RT.Service.Resolve<DevicePurController>()
                    .GetBudgetDepartment(RT.Identity.UserId, project.DepartmentId);

                if (deviceBudgetDepartment == null)
                {
                    throw new ValidationException("您没有部门【{0}】的预算权限".L10nFormat(project.Department.Name));
                }
                else
                {
                    deviceBudgetDepartmentDictionary.Add(project.DepartmentId, true);
                }
            }
            //}

            //当前年
            var currentYear = DateTime.Now.Year;
            if (project.ProjectYear < currentYear)
            {
                throw new ValidationException("年度小于当前年份".L10N());
            }
            var parentProject = project.ParentProject;
            if (parentProject != null && (parentProject.FactoryId != project.FactoryId || parentProject.DepartmentId != project.DepartmentId))
            {
                throw new ValidationException("父项目编码的工厂部门信息与项目不符".L10N());
            }

            //年度是对应年份的1月1日
            project.Year = new DateTime(project.ProjectYear, 1, 1);
            project.ProjectStatus = ProjectStatus.NotStarted;
            project.ApprovalStatus = ApprovalStatus.Draft;
        }

        /// <summary>
        /// 获取导入信息
        /// </summary>
        /// <param name="project"></param>
        private static void GetImportInfo(Project project)
        {
            if (project.PrincipalCodeImport.IsNullOrEmpty())
            {
                throw new ValidationException("项目负责人不能为空".L10N());
            }
            if (project.DepartmentNameImport.IsNullOrEmpty())
            {
                throw new ValidationException("部门不能为空".L10N());
            }
            if (project.FactoryNameImport.IsNullOrEmpty())
            {
                throw new ValidationException("工厂不能为空".L10N());
            }

            var principal = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(project.PrincipalCodeImport);
            if (principal == null)
            {
                throw new ValidationException("项目负责人[{0}]在系统中不存在".L10nFormat(project.PrincipalCodeImport));
            }
            project.Principal = principal;

            var factory = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(project.FactoryNameImport);
            if (factory == null)
            {
                throw new ValidationException("工厂[{0}]在系统中不存在".L10nFormat(project.FactoryNameImport));
            }
            project.Factory = factory;


            var department = RT.Service.Resolve<EnterpriseController>().GetDepartments(null, project.DepartmentNameImport).FirstOrDefault();
            if (department == null)
            {
                throw new ValidationException("部门[{0}]在系统中不存在".L10nFormat(project.DepartmentNameImport));
            }
            project.Department = department;
        }
    }
}
