using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.RunStandards;
using SIE.Web.Command;
using SIE.Web.EMS.Equipments.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.RunStandards.Commands
{
    /// <summary>
    /// 添加点检项目
    /// </summary>
    [JsCommand("SIE.Web.EMS.RunStandards.Commands.SelRunStandardProjectCommand")]
    public class SelRunStandardProjectCommand : ViewCommand
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

            var lubricationInfos = args.Data.ToJsonObject<List<LubricationInfo>>();

            Check.NotNullOrEmpty(lubricationInfos, nameof(lubricationInfos));

            if (null == lubricationInfos || lubricationInfos.Count == 0)
                throw new ValidationException("维修项目列表不能为空".L10N());

            var projectDetailIds = lubricationInfos.Select(m => m.ProjectDetailId).ToList();
            var projectDetails = RT.Service.Resolve<EquipController>().GetEquipModelRepairProjectsByIds(projectDetailIds);

            foreach (var item in projectDetailIds)
            {
                var detail = projectDetails.FirstOrDefault(m => m.Id == item);
                if (detail != null)
                {
                    var runStandardProject = new RunStandardProject();
                    runStandardProject.RunStandardId = lubricationInfos.FirstOrDefault().SourceId;
                    runStandardProject.DepartmentId = detail.DepartmentId;
                    runStandardProject.ProjectDetailId = detail.ProjectDetailId;
                    runStandardProject.Consumable = detail.Consumable;
                    runStandardProject.MaxValue = detail.MaxValue;
                    runStandardProject.Method = detail.Method;
                    runStandardProject.MinValue = detail.MinValue;
                    runStandardProject.Part = detail.Part;
                    runStandardProject.Standard = detail.Standard;
                    runStandardProject.Unit = detail.Unit;
                    runStandardProject.UseTime = detail.UseTime;
                    runStandardProject.CreateBy = RT.IdentityId;
                    runStandardProject.CreateDate = DateTime.Now;
                    runStandardProject.UpdateBy = RT.IdentityId;
                    runStandardProject.UpdateDate = DateTime.Now;
                    savedData.Add(runStandardProject);
                }
            }

            RF.Save(savedData);
            return savedData;
        }
    }
}
