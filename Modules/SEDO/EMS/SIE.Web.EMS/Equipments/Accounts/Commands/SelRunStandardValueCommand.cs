using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.RunStandards;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Equipments.Accounts.Commands
{
    /// <summary>
    /// 添加点检项目
    /// </summary>
    [JsCommand("SIE.Web.EMS.Equipments.Accounts.Commands.SelRunStandardValueCommand")]
    public class SelRunStandardValueCommand : ViewCommand
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
                throw new ValidationException("维修定标列表不能为空".L10N());

            var runStandardValueIds = lubricationInfos.Select(m => m.ProjectDetailId).ToList();
            var runStandardValues = RT.Service.Resolve<RunStandardsController>().GetRunStandardValueByIds(runStandardValueIds);

            foreach (var item in runStandardValueIds)
            {
                var detail = runStandardValues.FirstOrDefault(m => m.Id == item);
                if (detail != null)
                {
                    var runStandardProject = new EquipAccountRepairStandard();
                    runStandardProject.EquipAccountId= lubricationInfos.FirstOrDefault().SourceId;
                    runStandardProject.StandardType = detail.StandardType;
                    runStandardProject.StandardUnit = detail.StandardUnit;
                    runStandardProject.LeadTime = detail.LeadTime;
                    runStandardProject.NextExecuteDate = detail.NextExecuteDate;
                    runStandardProject.LastExecuteDate = detail.LastExecuteDate;
                    runStandardProject.RoundAmount = detail.AmountOfRound;
                    runStandardProject.Amount = detail.Amount;
                    runStandardProject.RunStandardValueId = detail.Id;
                    runStandardProject.TotalAmount = detail.TotalAmount;
                    runStandardProject.CreateBy = RT.IdentityId;
                    runStandardProject.CreateDate = DateTime.Now;
                    runStandardProject.UpdateBy = RT.IdentityId;
                    runStandardProject.UpdateDate = DateTime.Now;
                    savedData.Add(runStandardProject);
                }
            }

            //RF.Save(savedData);
            return savedData;
        }
    }
}
