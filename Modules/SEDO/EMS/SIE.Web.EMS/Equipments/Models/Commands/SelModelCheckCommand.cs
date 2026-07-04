using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Common.Entity;
using SIE.EMS.Equipments.Models;
using SIE.EMS.MainenanceProjects;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Equipments.Models.Commands
{
    /// <summary>
    /// 添加点检项目
    /// </summary>
    [JsCommand("SIE.Web.EMS.Equipments.Models.Commands.SelModelCheckCommand")]
    public class SelModelCheckCommand : ViewCommand
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
            var checkProjectInfos = args.Data.ToJsonObject<List<CheckProjectInfo>>();
            Check.NotNullOrEmpty(checkProjectInfos, nameof(checkProjectInfos));
            if (null == checkProjectInfos || checkProjectInfos.Count == 0)
                throw new ValidationException("点检项目列表不能为空".L10N());
            var projectDetailIds = checkProjectInfos.Select(m => m.ProjectDetailId).ToList();
            var projectDetails = RT.Service.Resolve<ProjectDetailController>().GetProjectDetails(projectDetailIds);

            foreach (var item in checkProjectInfos)
            {
                var detail = projectDetails.FirstOrDefault(m => m.Id == item.ProjectDetailId);

                if (!detail.CycleType.HasValue)
                {
                    throw new ValidationException("点检项目【{0}】的周期类型为空".L10nFormat(detail.Name));
                }

                var checkProject = new EquipModelCheckProject();
                checkProject.EquipModelId = item.SourceId;
                checkProject.ProjectDetailId = item.ProjectDetailId;
                checkProject.ProjectType = detail.ProjectType;
                checkProject.Consumable = detail.Consumable;
                checkProject.CycleType = detail.CycleType.Value;
                checkProject.MaxValue = detail.MaxValue;
                checkProject.Method = detail.Method;
                checkProject.MinValue = detail.MinValue;
                checkProject.Part = detail.Part;
                checkProject.Standard = detail.Standard;
                checkProject.Unit = detail.Unit;
                checkProject.UseTime = detail.UseTime;
                savedData.Add(checkProject);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
