using SIE.Domain;
using SIE.Domain.Validation;
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
    [JsCommand("SIE.Web.EMS.Equipments.Models.Commands.SelRepairProjectCommand")]
    public class SelRepairProjectCommand : ViewCommand
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
            var projectDetails = RT.Service.Resolve<ProjectDetailController>().GetProjectDetails(projectDetailIds);
            var detailSparePartItemDic = RT.Service.Resolve<ProjectDetailController>().GetSparePartItem(projectDetailIds).GroupBy(p => p.ProjectDetailId)
                .ToDictionary(p => p.Key, p => p.ToList());

            foreach (var item in lubricationInfos)
            {
                var detail = projectDetails.FirstOrDefault(m => m.Id == item.ProjectDetailId);
                var lubricationProject = new EquipModelRepairProject();
                lubricationProject.EquipModelId = item.SourceId;
                lubricationProject.ProjectDetailId = item.ProjectDetailId;
                lubricationProject.Consumable = detail.Consumable;
                lubricationProject.MaxValue = detail.MaxValue;
                lubricationProject.Method = detail.Method;
                lubricationProject.MinValue = detail.MinValue;
                lubricationProject.Part = detail.Part;
                lubricationProject.Standard = detail.Standard;
                lubricationProject.Unit = detail.Unit;
                lubricationProject.UseTime = detail.UseTime.ToString();
                lubricationProject.ProjectName = detail.Name;
                lubricationProject.DepartmentName = "";
                lubricationProject.CreateBy = RT.IdentityId;
                lubricationProject.CreateDate = DateTime.Now;
                lubricationProject.UpdateBy = RT.IdentityId;
                lubricationProject.UpdateDate = DateTime.Now;

                List<SparePartItem> SparePartItemList = null;
                if (detailSparePartItemDic.TryGetValue(item.ProjectDetailId, out SparePartItemList))
                {
                    foreach (var sp in SparePartItemList)
                    {
                        EquipModelRepairSparePart esp = new EquipModelRepairSparePart();
                        esp.Qty = sp.Qty;
                        esp.SparePartId = sp.SparePartId;
                        lubricationProject.EquipModelRepairSparePartList.Add(esp);
                    }
                }

                savedData.Add(lubricationProject);
            }

            RF.Save(savedData);
            return savedData;
        }
    }
}
