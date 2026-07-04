using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments;
using SIE.EMS.EquipRepair.PlanRepairs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EquipRepair.PlanRepairs.Commands
{
    /// <summary>
    /// 添加点检项目
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.PlanRepairs.Commands.SelPlanRepairsProjectCommand")]
    public class SelPlanRepairsProjectCommand : ViewCommand
    {
        /// <summary>
        /// 执行添加
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();

            var projectInfos = args.Data.ToJsonObject<List<SelProjectInfo>>();

            Check.NotNullOrEmpty(projectInfos, nameof(projectInfos));

            if (null == projectInfos || projectInfos.Count == 0)
                throw new ValidationException("维修项目列表不能为空".L10N());

            var projectDetailIds = projectInfos.Select(m => m.ProjectDetailId).ToList();
            var projectDetails = RT.Service.Resolve<EquipController>().GetEquipModelRepairProjectsByIds(projectDetailIds);

            foreach (var item in projectDetailIds)
            {
                var detail = projectDetails.FirstOrDefault(m => m.Id == item);
                if (detail != null)
                {
                    var planRepairProject = new PlanRepairProject();
                    planRepairProject.PlanRepairId = projectInfos.FirstOrDefault().SourceId;
                    planRepairProject.DepartmentId = detail.DepartmentId;
                    planRepairProject.ProjectDetailId = detail.ProjectDetailId;
                    planRepairProject.Consumable = detail.Consumable;
                    planRepairProject.MaxValue = detail.MaxValue;
                    planRepairProject.Method = detail.Method;
                    planRepairProject.MinValue = detail.MinValue;
                    planRepairProject.Part = detail.Part;
                    planRepairProject.Standard = detail.Standard;
                    planRepairProject.Unit = detail.Unit;
                    planRepairProject.UseTime = detail.UseTime;                    
                    planRepairProject.CreateBy = RT.IdentityId;
                    planRepairProject.CreateDate = DateTime.Now;
                    planRepairProject.UpdateBy = RT.IdentityId;
                    planRepairProject.UpdateDate = DateTime.Now;
                    savedData.Add(planRepairProject);
                }
            }

            RF.Save(savedData);
            return savedData;
        }
    }

    /// <summary>
    /// 维修项目信息
    /// </summary>
    public class SelProjectInfo
    {
        /// <summary>
        /// 源Id
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public double ProjectDetailId { get; set; }

    }
}
