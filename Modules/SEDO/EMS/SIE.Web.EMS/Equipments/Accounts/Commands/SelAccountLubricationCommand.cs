using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.EquipModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Equipments.Accounts.Commands
{
    /// <summary>
    /// 添加校验项目
    /// </summary>
    public class SelAccountLubricationCommand : ViewCommand
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
            {
                throw new ValidationException("润滑项目列表不能为空".L10N());
            }
            var projectDetailIds = lubricationInfos.Select(m=>m.ProjectDetailId).ToList();
            var projectDetails = RT.Service.Resolve<ProjectDetailController>().GetProjectDetails(projectDetailIds);
            var detailSparePartItemDic = RT.Service.Resolve<ProjectDetailController>().GetSparePartItem(projectDetailIds).GroupBy(p=>p.ProjectDetailId).ToDictionary(p=>p.Key,p=>p.ToList());

            foreach (var item in lubricationInfos)
            {
                var detail = projectDetails.FirstOrDefault(m => m.Id == item.ProjectDetailId);
                
                if (!detail.CycleType.HasValue)
                {
                    throw new ValidationException("润滑项目【{0}】的周期类型为空".L10nFormat(detail.Name));
                }

                var checkProject = new EquipAccountLubricationProject();
                checkProject.EquipAccountId = item.SourceId;
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

                List<SparePartItem> SparePartItemList = null;
                if (detailSparePartItemDic.TryGetValue(item.ProjectDetailId, out SparePartItemList))
                {
                    foreach (var sp in SparePartItemList)
                    {
                        EquipAccountLubricaSparePart esp = new EquipAccountLubricaSparePart();
                        esp.Qty = sp.Qty;
                        esp.LubricationProjectId = sp.ProjectDetailId;
                        esp.SparePartId = sp.SparePartId;
                        checkProject.LubricaSparePartList.Add(esp);
                    }
                }
                savedData.Add(checkProject);
            }
            RF.Save(savedData);
            return true;
        }
    }

    /// <summary>
    /// 润滑信息
    /// </summary>
    public class LubricationInfo
    {
        /// <summary>
        /// 源Id
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public double ProjectDetailId { get; set; }


        /// <summary>
        /// 设备型号是否为新增
        /// </summary>
        public bool EquipModelIsNew { get; set; }
    }
}
