using SIE.Common.Catalogs;
using SIE.Core.Common.Models;
using SIE.Domain;
using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目查询器
    /// </summary>
    public class ProjectDataQueryer : DataQueryer
    {
        /// <summary>
        /// 创建一个新的项目
        /// </summary>
        /// <returns>新的项目</returns>
        public Project GetNewProject()
        {
            return RT.Service.Resolve<ProjectController>().GetNewProject();
        }

        /// <summary>
        /// 导出项目
        /// </summary>
        /// <param name="criter">查询实体</param>
        /// <returns>项目信息</returns>
        public virtual ExportDataTable ExportProject(ProjectCriteria criter)
        {
            var projects = RT.Service.Resolve<ProjectController>().CriteriaProjects(criter);
            var projectIds = projects.Select(p => p.Id).ToList();
            ExportDataTable exportDataTable = new ExportDataTable();
            ExportProjectHeadInfo(projects, exportDataTable);

            //关键事项
            ExportProjectKeyItem(projectIds, exportDataTable);

            //项目成员
            var members = RT.Service.Resolve<ProjectController>().GetMembersByProjectIds(projectIds);
            DataTable dataTableMem = new DataTable();
            List<string> arrMem = new List<string>() { "项目编码", "工号", "姓名", "职位", "联系方式", "备注", "状态" };
            string[] columnsMem = new string[arrMem.Count];
            for (int i = 0; i < columnsMem.Length; i++)
            {
                columnsMem[i] = arrMem[i].L10N();
                dataTableMem.Columns.Add(columnsMem[i]);
            }
            members.ForEach(member =>
            {
                var row = dataTableMem.NewRow();
                row[0] = member.ProjectCode;
                row[1] = member.EmployeeCode;
                row[2] = member.EmployeeName;
                row[3] = member.Position;
                row[4] = member.Phone;
                row[5] = member.Remark;
                row[6] = member.MemberStatus.ToLabel();
                dataTableMem.Rows.Add(row);
            });
            exportDataTable.SheetNames.Add("项目成员");
            exportDataTable.Tables.Add(dataTableMem);
            exportDataTable.Columns.Add(columnsMem);

            //项目计划
            var workItems = RT.Service.Resolve<ProjectController>().GetWorkItemsByProjectIds(projectIds);
            DataTable dataTableWo = new DataTable();
            List<string> arrWo = new List<string>() { "项目编码", "计划节点", "计划开始时间", "计划结束时间", "实际开始时间", "实际结束时间", "状态", "责任人" };
            string[] columnsWo = new string[arrWo.Count];
            for (int i = 0; i < columnsWo.Length; i++)
            {
                columnsWo[i] = arrWo[i].L10N();
                dataTableWo.Columns.Add(columnsWo[i]);
            }
            workItems.ForEach(workItem =>
            {
                var row = dataTableWo.NewRow();
                row[0] = workItem.ProjectCode;
                row[1] = workItem.WorkItem;
                row[2] = workItem.PlanStart;
                row[3] = workItem.PlantEnd;
                row[4] = workItem.ActualStart;
                row[5] = workItem.ActaulEnd;
                row[6] = workItem.WorkStatus.ToLabel();
                row[7] = workItem.PrincipalName;
                dataTableWo.Rows.Add(row);
            });
            exportDataTable.SheetNames.Add("项目计划");
            exportDataTable.Tables.Add(dataTableWo);
            exportDataTable.Columns.Add(columnsWo);
            return exportDataTable;
        }

        /// <summary>
        /// 导出关键事项
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="exportDataTable"></param>
        private void ExportProjectKeyItem(List<double> projectIds, ExportDataTable exportDataTable)
        {
            var allKeyItems = RT.Service.Resolve<ProjectController>().GetKeyItemsByProjectIds(projectIds);
            DataTable dataTableKey = new DataTable();
            List<string> arrKey = new List<string>() { "项目编码", "事项说明", "预算编码", "预算名称", "预算可使用金额", "事项预算", "事项成本", "事项状态", "工时成本", "备注" };
            string[] columnsKey = new string[arrKey.Count];
            for (int i = 0; i < columnsKey.Length; i++)
            {
                columnsKey[i] = arrKey[i].L10N();
                dataTableKey.Columns.Add(columnsKey[i]);
            }
            allKeyItems.ForEach(keyItem =>
            {
                var row = dataTableKey.NewRow();
                row[0] = keyItem.ProjectCode;
                row[1] = keyItem.Description;
                row[2] = keyItem.BudgetNo;
                row[3] = keyItem.BudgetName;
                row[4] = keyItem.CanUseAmount;
                row[5] = keyItem.BudgetAmount;
                row[6] = keyItem.ActualCost;
                row[7] = keyItem.WorkStatus.ToLabel();
                row[8] = keyItem.LaborCost;
                row[9] = keyItem.Remark;
                dataTableKey.Rows.Add(row);
            });
            exportDataTable.SheetNames.Add("关键事项");
            exportDataTable.Tables.Add(dataTableKey);
            exportDataTable.Columns.Add(columnsKey);
        }

        /// <summary>
        /// 导出项目管理主表
        /// </summary>
        /// <param name="projects"></param>
        /// <param name="exportDataTable"></param>
        private void ExportProjectHeadInfo(EntityList<Project> projects, ExportDataTable exportDataTable)
        {
            DataTable dataTable = new DataTable();
            List<string> arr = new List<string>(){ "工厂", "部门", "项目编码","项目名称", "项目状态","审核状态","计划类型", "年度", "项目类别","项目负责人","项目预算",
                "中标金额","工时成本","立项日期","父项目编码","父项目名称","项目内容及立项依据","预期目标及综合经济效益","备注","创建人","创建时间",
                "修改人","修改时间"};
            string[] columns = new string[arr.Count];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = arr[i].L10N();
                dataTable.Columns.Add(columns[i]);
            }
            var catalogList = RT.Service.Resolve<CatalogController>().GetCatalogList(Project.ProjectClassify);
            projects.ForEach(exportDate =>
            {
                var row = dataTable.NewRow();
                row[0] = exportDate.FactoryName;
                row[1] = exportDate.DepartmentName;
                row[2] = exportDate.Code;
                row[3] = exportDate.Name;
                row[4] = exportDate.ProjectStatus.ToLabel();
                row[5] = exportDate.ApprovalStatus.ToLabel();
                row[6] = exportDate.PlanType.ToLabel();
                row[7] = exportDate.Year.Year;
                row[8] = (catalogList.Count == 0 || exportDate.ProjectType == "") ? "" : catalogList.FirstOrDefault(p => p.Code == exportDate.ProjectType)?.Name;
                row[9] = exportDate.PrincipalName;
                row[10] = exportDate.Amount;
                row[11] = exportDate.ActualAmount;
                row[12] = exportDate.LaborCost;
                row[13] = exportDate.ProjectDate;
                row[14] = exportDate.ParentProjectCode;
                row[15] = exportDate.ParentProjectName;
                row[16] = exportDate.ContentAndBasis;
                row[17] = exportDate.GoalAndBenefit;
                row[18] = exportDate.Remark;
                row[19] = exportDate.CreateByName;
                row[20] = exportDate.CreateDate;
                row[21] = exportDate.UpdateByName;
                row[22] = exportDate.UpdateDate;
                dataTable.Rows.Add(row);
            });
            exportDataTable.SheetNames.Add("项目管理");
            exportDataTable.Tables.Add(dataTable);
            exportDataTable.Columns.Add(columns);
        }

        /// <summary>
        /// 根据项目id获取项目子列表信息
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <returns>项目子列表信息</returns>
        public Tuple<EntityList<ProjectChangeKeyItem>, EntityList<ProjectChangeMember>, EntityList<ProjectChangeWorkItem>> GetChildInfoByProjectId(double projectId)
        {
            return RT.Service.Resolve<ProjectChangeController>().GetChildInfoByProjectId(projectId);
        }
    }
}
